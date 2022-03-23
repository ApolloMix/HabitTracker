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
        static int quantity = 0;
        static void Main(string[] args)
        {
            if (sqlite != null)
            {
                //If the connection is a success, create the Habits table, if the habit table already exists,
                // delete and recreate it. 
                try
                {
                    CreateTable();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Database already exists! Would you like to clear the cached database?");
                    Console.WriteLine("Press 'Y' for yes, or 'N' to continue");
                    string choice = GetMenuInput();
                    if(choice == "Y")
                    {
                        ClearPreviousTable();
                        CreateTable();
                    }
                }
            }

            while(true)
            {
                //Intro menu, gets player input
                Console.Clear();
                MainMenu();
                string input = GetMenuInput();


                switch (input)
                {
                    case "A":
                        GetAddition();
                        break;
                    case "R":
                        RemoveItem();
                        break;
                    case "U":
                        StartUpdate();
                        break;
                    case "D":
                        ShowTable();
                        string userInput = GetMenuInput();
                        userInput = null;
                        break;
                }

            }
        }
        static SQLiteConnection InitializeDatabase()
        {
            string databaseFile= @"c:\users\meowm\github\repos\habittracker\db.sqlite";
            if (!File.Exists(databaseFile))
            {
                File.Create(databaseFile);
            }

            SQLiteConnection connection = new SQLiteConnection($"Data Source= {databaseFile}");

            try
            {
                connection.Open();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Connection failed :(");
                Console.WriteLine(ex.Message);
                sqlite = null;
                Environment.Exit(0);
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

        static void ClearPreviousTable()
        {
            SQLiteCommand command;
            string clearTable = @"DROP TABLE Habits";
            command = sqlite.CreateCommand();
            command.CommandText = clearTable;
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
            Console.WriteLine("Type 'U' to update an entry\n");
            Console.WriteLine("Type 'D' to display all Habits");
        }

        static string GetUserInput()
        {
            string userInput = Console.ReadLine();
            return userInput;
        }

        static string GetMenuInput()
        {
            string userInput = Console.ReadLine().ToUpper();
            return userInput;
        }
        static void GetAddition()
        {
            // Get habit first
            Console.WriteLine("What would you like to add to the Habit Tracker?\n");
            Console.WriteLine("Your answer can only contain 50 characters");
            string input = GetUserInput();

            // Then get quantity 
            Console.WriteLine("How many times a day do you do this habit?\n");
            Console.WriteLine("Enter only a whole number");
            quantity = Int32.Parse(GetUserInput());

            // Add to database
            if(input != "")
            {
                SQLiteCommand command;
                string addToTable= $"INSERT INTO Habits " +
                    $"VALUES('{input}', {quantity});";
                command = sqlite.CreateCommand();
                command.CommandText = addToTable;
                command.ExecuteNonQuery();
            }
        }
        static void RemoveItem()
        {
            Console.WriteLine("What would you like to remove from the habit tracker?\n");

            ShowTable();

            string input = GetUserInput();

            SQLiteCommand command;
            string removeFromTable = $@"DELETE FROM Habits WHERE [Habit] = '{input}';";
            command = sqlite.CreateCommand();
            command.CommandText = removeFromTable;
            command.ExecuteNonQuery();
        }

        static void StartUpdate()
        {
            Console.WriteLine("What would you like to update from the habit tracker?\n");
            Console.WriteLine("First input the Habit you'd like to edit");

            ShowTable();
            string habitID = GetUserInput();
            
            
            Console.WriteLine("\nNow, what would you like to edit, the habit, or the quantity?");
            
            string choice = GetUserInput();

            if(choice == "habit")
            {
                UpdateHabit(habitID);
            }
            if(choice == "quantity")
            {
                UpdateQuantity(habitID);
            }
            
        }

        static void UpdateHabit(string Id)
        {
            Console.WriteLine("What would is the new value?");
            string newValue = GetUserInput();
            SQLiteCommand command;
            string updateHabit = $@"UPDATE Habits 
                                    SET Habit = '{newValue}'
                                    WHERE Habit = '{Id}';";
            command = sqlite.CreateCommand();
            command.CommandText = updateHabit;
            command.ExecuteNonQuery();
        }

        static void UpdateQuantity(string Id)
        {
            string newValue = GetUserInput();
            SQLiteCommand command;
            string updateHabit = $@"UPDATE Habits 
                                    SET Quantity = '{newValue}'
                                    WHERE Habit = '{Id}';";
            command = sqlite.CreateCommand();
            command.CommandText = updateHabit;
            command.ExecuteNonQuery();
        }

        static void ShowTable()
        {
            using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM Habits", sqlite))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        for(int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.WriteLine(reader.GetValue(i));
                        }
                        Console.WriteLine();
                    }
                }
            }
        }
    }
}
