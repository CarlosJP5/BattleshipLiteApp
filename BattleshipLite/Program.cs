﻿using BattleshipLiteLibrary;
using BattleshipLiteLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipLite
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WelcomeMessage();

            PlayerInfoModel activePlayer = CreatePlayer("Player 1");
            PlayerInfoModel opponent = CreatePlayer("Player 2");

            PlayerInfoModel winner = null;

            do
            {
                // Display grid from activePlayer on where they fired.
                DisplayShotGrid(activePlayer);

                // Ask activePlayer for a shot.
                // Determine if it is a valid shot
                // Determine shot results
                RecordPlayerShot(activePlayer, opponent);

                // Determine if game should continue
                bool doesGameContinue = GameLogic.PlayerStillActive(opponent);

                // If over, set activePlayer as the winner
                // else, swap positions (activePlatey to opponent)
                if (doesGameContinue)
                {
                    // Swap player using Tuple
                    (activePlayer, opponent) = (opponent, activePlayer);
                }
                else
                {
                    winner = activePlayer;
                }

                IdentifyWinner(winner);

            } while (winner == null);

            Console.ReadLine();
        }

        private static void IdentifyWinner(PlayerInfoModel winner)
        {
            Console.WriteLine($"Congratularions to {winner.UserName} for winning!");
            Console.WriteLine($"{winner.UserName} took {GameLogic.GetShotCount(winner)} shots");
        }

        private static void RecordPlayerShot(PlayerInfoModel activePlayer, PlayerInfoModel opponent)
        {
            bool isValidShot = false;
            string row = "";
            int column = 0;

            do
            {
                // Ask for a shot (we ask for "B2")
                string shot = AskForShot();

                // Determine what row an column that is - split it apart
                (row, column) = GameLogic.SplitShotIntoRowAndColumn(shot);
                
                // Determine if that is a valid shot
                isValidShot = GameLogic.ValidateShot(activePlayer, row, column);

                // Go back to beginning if not a valid shot
                if (!isValidShot)
                {
                    Console.WriteLine("Invalid shot location. Please try again.");
                }
            } while (!isValidShot);

            // Determine shot results
            bool isAHit = GameLogic.IdentifyShotResult(opponent, row, column);

            // Record result
            GameLogic.MarkShotResult(activePlayer, row, column, isAHit);
        }

        private static string AskForShot()
        {
            Console.Write("Pleace enter your shot selection: ");
            string output = Console.ReadLine();
            return output;
        }

        private static void DisplayShotGrid(PlayerInfoModel activePlayer)
        {
            string currentRow = activePlayer.ShotGrid[0].SpotLetter;

            foreach (var gridSpot in activePlayer.ShotGrid)
            {
                if (gridSpot.SpotLetter != currentRow)
                {
                    Console.WriteLine();
                    currentRow = gridSpot.SpotLetter;
                }

                if (gridSpot.Status == GridSpotStatus.Empty)
                {
                    Console.Write($" {gridSpot.SpotLetter}{gridSpot.SpotNumber} ");
                }
                else if (gridSpot.Status == GridSpotStatus.Hit)
                {
                    Console.Write(" X ");
                }
                else if (gridSpot.Status == GridSpotStatus.Miss)
                {
                    Console.Write(" O ");
                }
                else
                {
                    Console.WriteLine(" ? ");
                }
            }
        }

        private static void WelcomeMessage()
        {
            Console.WriteLine("Welcome to Battleship Lite");
            Console.WriteLine("create by Carlos.");
            Console.WriteLine();
        }

        private static PlayerInfoModel CreatePlayer(string playerTitle)
        {
            PlayerInfoModel output = new PlayerInfoModel();

            Console.WriteLine($"Player information for {playerTitle}");

            // Ask User for the name
            output.UserName = AskForUsersName();

            // Load up the shot grid
            GameLogic.InitializeGrid(output);

            // Ask the user for their 5 ship placements
            PlaceShips(output);

            // clear
            Console.Clear();

            return output;
        }

        private static string AskForUsersName()
        {
            Console.Write("What's your name? ");
            string output = Console.ReadLine();
            return output;
        }

        private static void PlaceShips(PlayerInfoModel model)
        {
            do
            {
                Console.Write($"Where do you want to place ship number {model.ShipLocations.Count + 1}: ");
                string location = Console.ReadLine();

                bool isValidLocation = GameLogic.PlaceShip(model, location);

                if (!isValidLocation)
                {
                    Console.WriteLine("That was not a valid location. Pleace try again.");
                }
            } while (model.ShipLocations.Count < 5);
        }
    }
}
