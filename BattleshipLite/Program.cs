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


            Console.ReadLine();
        }

        private static void WelcomeMessage()
        {
            Console.WriteLine("Welcome to Battleship Lite");
            Console.WriteLine("create by Carlos.");
            Console.WriteLine();
        }

        private static PlayerInfoModel CreatePlayer()
        {

        }
    }
}
