using System;
using System.Data;
using System.Data.SQLite; 
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Timer : Form
    {
        private System.Windows.Forms.Timer pomodoroTimer; // Timer for countdown
        private DateTime startTime; // Start time of the session
        private DateTime endTime; // End time of the session
        private int durationInMinutes; // Pomodoro duration
        private int timeLeftInSeconds; // Time left in seconds

        public Timer()
        {
            InitializeComponent();

            // Initialize the Pomodoro timer
            pomodoroTimer = new System.Windows.Forms.Timer();
            pomodoroTimer.Interval = 1000; // Timer ticks every second
            pomodoroTimer.Tick += PomodoroTimer_Tick;
        }

        private void Timer_Load(object sender, EventArgs e)
        {
            LoadTasksIntoComboBox();
        }
        private void LoadTasksIntoComboBox()
        {
            string dbPath = "TaskOrganizer.db"; // Ensure the path is correct
            string connectionString = $"Data Source={dbPath};Version=3;";

            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT TaskID, TaskDescription FROM Tasks WHERE IsCompleted = 0"; // Only show incomplete tasks
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            var tasks = new DataTable();
                            tasks.Load(reader);

                            // Bind the DataTable to the ComboBox
                            cmbTasks.DataSource = tasks;
                            cmbTasks.DisplayMember = "TaskDescription"; // What the user sees
                            cmbTasks.ValueMember = "TaskID"; // The actual value
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tasks: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    

    // Start Timer Button Click
    private void btnStartTimer_Click(object sender, EventArgs e)
        {
            durationInMinutes = (int)nudPomodoroDuration.Value; // Get duration from NumericUpDown
            timeLeftInSeconds = durationInMinutes * 60; // Convert minutes to seconds
            startTime = DateTime.Now;

            pomodoroTimer.Start(); // Start the countdown timer

            lblTimer.Text = $"{durationInMinutes:D2}:00"; // Display the initial time
            btnStartTimer.Enabled = false; // Disable Start button
            btnStopTimer.Enabled = true; // Enable Stop button
        }

        // Timer Tick Event - Updates Timer Countdown
        private void PomodoroTimer_Tick(object sender, EventArgs e)
        {
            if (timeLeftInSeconds > 0)
            {
                timeLeftInSeconds--;

                int minutes = timeLeftInSeconds / 60;
                int seconds = timeLeftInSeconds % 60;

                lblTimer.Text = $"{minutes:D2}:{seconds:D2}"; // Update timer label
            }
            else
            {
                pomodoroTimer.Stop();
                endTime = DateTime.Now; // Record end time

                MessageBox.Show("Pomodoro session completed!", "Timer Finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SaveSession();
                btnStartTimer.Enabled = true;
                btnStopTimer.Enabled = false;
            }
        }

        // Stop Timer Button Click
        private void btnStopTimer_Click(object sender, EventArgs e)
        {
            pomodoroTimer.Stop();
            endTime = DateTime.Now; // Record end time

            MessageBox.Show("Timer stopped.", "Timer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            SaveSession();
            btnStartTimer.Enabled = true;
            btnStopTimer.Enabled = false;
        }

     
        private void SaveSession()
        {
            try
            {
                int selectedTaskId = cmbTasks.SelectedValue != null ? Convert.ToInt32(cmbTasks.SelectedValue) : 0;
                //if (selectedTaskId == 0)
                //{
                //    MessageBox.Show("Please select a task before saving the session.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}

                int actualDuration = (int)(endTime - startTime).TotalMinutes;

                using (var conn = new SQLiteConnection("Data Source=TaskOrganizer.db;Version=3;"))
                {
                    conn.Open();

                    string query = @"
                INSERT INTO TimerLogs (TaskID, StartTime, EndTime, Duration) 
                VALUES (@TaskID, @StartTime, @EndTime, @Duration)";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TaskID", selectedTaskId);
                        cmd.Parameters.AddWithValue("@StartTime", startTime);
                        cmd.Parameters.AddWithValue("@EndTime", endTime);
                        cmd.Parameters.AddWithValue("@Duration", actualDuration);
                        cmd.ExecuteNonQuery();
                    }
                }

                //MessageBox.Show("Pomodoro session saved successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the session: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
