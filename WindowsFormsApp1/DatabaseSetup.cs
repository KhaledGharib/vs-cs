using System;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public static class DatabaseSetup
    {
        public static void InitializeDatabase()
        {
            try
            {
                // Use a consistent database path (e.g., in AppData)
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string appDirectory = Path.Combine(appDataPath, "TaskOrganizer");
                if (!Directory.Exists(appDirectory))
                {
                    Directory.CreateDirectory(appDirectory);
                }

                string dbPath = Path.Combine(appDirectory, "TaskOrganizer.db");

                // Check if the database file exists, if not create it
                if (!File.Exists(dbPath))
                {
                    SQLiteConnection.CreateFile(dbPath);
                    MessageBox.Show("Database created successfully.");
                }

                // Create tables in the database
                using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();

                    // Create Users table
                    string createUsersTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Users (
                            UserID INTEGER PRIMARY KEY AUTOINCREMENT,
                            Username TEXT NOT NULL UNIQUE,
                            Password TEXT NOT NULL
                        );";

                    // Create Tasks table
                    string createTasksTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Tasks (
                            TaskID INTEGER PRIMARY KEY AUTOINCREMENT,
                            UserID INTEGER NOT NULL,
                            TaskDescription TEXT NOT NULL,
                            IsCompleted INTEGER DEFAULT 0,
                            FOREIGN KEY (UserID) REFERENCES Users (UserID)
                        );";

                    // Create TimerLogs table
                    string createTimerTableQuery = @"
                        CREATE TABLE IF NOT EXISTS TimerLogs (
                            LogID INTEGER PRIMARY KEY AUTOINCREMENT,
                            TaskID INTEGER, 
                            StartTime DATETIME,
                            EndTime DATETIME,
                            Duration INTEGER,
                            FOREIGN KEY (TaskID) REFERENCES Tasks (TaskID)
                        );";

                    // Execute the table creation queries
                    ExecuteNonQuery(conn, createUsersTableQuery);
                    ExecuteNonQuery(conn, createTasksTableQuery);
                    ExecuteNonQuery(conn, createTimerTableQuery);

                    MessageBox.Show("Tables created successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while initializing the database: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper method to execute a non-query command
        private static void ExecuteNonQuery(SQLiteConnection conn, string query)
        {
            using (var cmd = new SQLiteCommand(query, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}
