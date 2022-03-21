using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;

namespace HabitTracker
{
    internal class Program
    {
        static SQLiteConnection sqlite = InitializeDatabase();
        static void Main(string[] args)
        {
            if (sqlite == null)
            {
                CreateTable();
            }
            MainMenu();
            string input = GetMainMenuInput();

            switch(input)
            {
                case "A":
                    GetAddition();
                    break;

            }
                
        }
        static SQLiteConnection InitializeDatabase()
        {
            string databaseFile= @"c:\users\meowm\github\repos\habittracker\db.sqlite";
            if (!File.Exists(databaseFile))
            {
                File.Create(databaseFile);
            }

            SQLiteConnection connection = new SQLiteConnection($"Data Source= {databaseFile}; Version=3;New=True;Compress=True;");

            try
            {
                connection.Open();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Connection failed :(");
                sqlite = null;
            }
            return connection;
        }

        static void CreateTable()
        {
            SQLiteCommand command;
            string createTable = "CREATE TABLE Habits (" +
                "Habit varchar(50), Quantity int);";
            command = sqlite.CreateCommand();
            command.CommandText = createTable;
            command.ExecuteNonQuery();
        }

        static void MainMenu()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Welcome to the Habit Tracker App!\n");
            Console.WriteLine("Choose from the menu below to use the app");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Type 'A' to add a habit\n");
            Console.WriteLine("Type 'R' to remove habit\n");
            Console.WriteLine("Type 'D' to display all Habits");
        }

        static string GetUserInput()
        {
            string userInput = Console.ReadLine();
            return userInput;
        }

        static string GetMainMenuInput()
        {
            string userInput = Console.ReadLine().ToUpper();
            return userInput;
        }

        static string GetQuantity()
        {
            string input = Console.ReadLine();
            return input;
        }

        static void GetAddition()
        {
            Console.WriteLine("What would you like to add to the Habit Tracker?\n");
            Console.WriteLine("Your answer can only contain 50 characters");

            string input = GetUserInput();
            //string quantity = GetQuantity();
            if(input != "")
            {
                SQLiteCommand command;
                command = sqlite.CreateCommand();
                command.CommandText = $"INSERT INTO Habits " +
                    $"VALUES('{input}', {null});";
                command.ExecuteNonQuery();
            }
        }
    }

   
}
