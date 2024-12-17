using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class MainForm : Form
    {
        private int _userId;
        private string _userName;
        public MainForm(int userId, string userName)
        {
            InitializeComponent();
            _userId = userId;
            _userName = userName;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            label1.Text = $"Welcome, {_userName}!";
            LoadTasks();
           
        }
        private void LoadTasks()
        {
            dataGridViewTasks.Rows.Clear();
            DisplayTaskSummary();
            try
            {
                using (var conn = new SQLiteConnection("Data Source=TaskOrganizer.db;Version=3;"))
                {
                    conn.Open();

                    string query = "SELECT TaskID, TaskDescription, IsCompleted FROM Tasks WHERE UserID = @UserID";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", _userId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int taskId = reader.GetInt32(0);
                                string taskDescription = reader.GetString(1);
                                bool isCompleted = reader.GetBoolean(2);

                                // Add rows to DataGridView
                                dataGridViewTasks.Rows.Add(taskId, taskDescription, isCompleted ? 1 : 0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddTask_Click(object sender, EventArgs e)
        {
            string newTaskDescription = Prompt.ShowDialog("Enter Task Description:", "Add Task");
            if (string.IsNullOrEmpty(newTaskDescription)) return;

            try
            {
                using (var conn = new SQLiteConnection("Data Source=TaskOrganizer.db;Version=3;"))
                {
                    conn.Open();

                    string query = "INSERT INTO Tasks (UserID, TaskDescription, IsCompleted) VALUES (@UserID, @TaskDescription, 0)";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", _userId);
                        cmd.Parameters.AddWithValue("@TaskDescription", newTaskDescription);
                        cmd.ExecuteNonQuery();
                    }
                }

                LoadTasks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditTask_Click(object sender, EventArgs e)
        {
            if (dataGridViewTasks.SelectedRows.Count == 0) return;

            int taskId = Convert.ToInt32(dataGridViewTasks.SelectedRows[0].Cells[0].Value);
            string currentDescription = dataGridViewTasks.SelectedRows[0].Cells[1].Value.ToString();

            string newDescription = Prompt.ShowDialog("Edit Task Description:", "Edit Task", currentDescription);
            if (string.IsNullOrEmpty(newDescription)) return;

            try
            {
                using (var conn = new SQLiteConnection("Data Source=TaskOrganizer.db;Version=3;"))
                {
                    conn.Open();

                    string query = "UPDATE Tasks SET TaskDescription = @TaskDescription WHERE TaskID = @TaskID";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TaskDescription", newDescription);
                        cmd.Parameters.AddWithValue("@TaskID", taskId);
                        cmd.ExecuteNonQuery();
                    }
                }

                LoadTasks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteTask_Click(object sender, EventArgs e)
        {
            if (dataGridViewTasks.SelectedRows.Count == 0) return;

            int taskId = Convert.ToInt32(dataGridViewTasks.SelectedRows[0].Cells[0].Value);

            try
            {
                using (var conn = new SQLiteConnection("Data Source=TaskOrganizer.db;Version=3;"))
                {
                    conn.Open();

                    string query = "DELETE FROM Tasks WHERE TaskID = @TaskID";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TaskID", taskId);
                        cmd.ExecuteNonQuery();
                    }
                }

                LoadTasks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Auth loginForm = new Auth();
            loginForm.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toggleStatus(object sender, DataGridViewCellEventArgs e)
        {
            int taskId = Convert.ToInt32(dataGridViewTasks.SelectedRows[0].Cells[0].Value);
            int taskStatus = Convert.ToInt32(dataGridViewTasks.SelectedRows[0].Cells[2].Value);
            int newStatus = taskStatus == 0 ? 1 : 0;

            try
            {
                using (var conn = new SQLiteConnection("Data Source=TaskOrganizer.db;Version=3;"))
                {
                    conn.Open();
                    string query = "UPDATE Tasks SET IsCompleted = @IsCompleted  WHERE TaskID = @TaskID";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IsCompleted", newStatus);
                        cmd.Parameters.AddWithValue("@TaskID", taskId);
                        cmd.ExecuteNonQuery();
                    }
                }

                LoadTasks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DisplayTaskSummary()
        {
            try
            {
                using (var conn = new SQLiteConnection("Data Source=TaskOrganizer.db;Version=3;"))
                {
                    conn.Open();
                    string queryTotal = "SELECT COUNT(*) FROM Tasks";
                    string queryCompleted = "SELECT COUNT(*) FROM Tasks WHERE IsCompleted = 1";

                    int totalTasks = 0;
                    int completedTasks = 0;

                    using (var cmdTotal = new SQLiteCommand(queryTotal, conn))
                    {
                        totalTasks = Convert.ToInt32(cmdTotal.ExecuteScalar());
                    }

                    using (var cmdCompleted = new SQLiteCommand(queryCompleted, conn))
                    {
                        completedTasks = Convert.ToInt32(cmdCompleted.ExecuteScalar());
                    }

                    // Display the summary
                    string message = $"Tasks Completed: {completedTasks} / {totalTasks}";
                    lblTaskSummary.Text = message; // Assuming you have a Label control named lblTaskSummary
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Timer timerForm = new Timer();
            timerForm.Show();
        }

        private void sesstionsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            BarChart chartFrom = new BarChart();
            chartFrom.Show();
        }

        private void tasksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PiChart piChart = new PiChart();
            piChart.Show();
        }
    }

}

