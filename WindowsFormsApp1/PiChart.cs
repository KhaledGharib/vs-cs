using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1
{
    public partial class PiChart : Form
    {
        public PiChart()
        {
            InitializeComponent();
        }

        private void PiChart_Load(object sender, EventArgs e)
        {
            LoadTaskCompletionPieChart();
        }

        private void LoadTaskCompletionPieChart()
        {
            try
            {
                using (var conn = new SQLiteConnection("Data Source=TaskOrganizer.db;Version=3;"))
                {
                    conn.Open();

                    // Query to get completed and incomplete tasks
                    string query = @"
                SELECT 
                    (SELECT COUNT(*) FROM Tasks WHERE IsCompleted = 1) AS Completed,
                    (SELECT COUNT(*) FROM Tasks WHERE IsCompleted = 0) AS Incomplete";

                    using (var cmd = new SQLiteCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int completedTasks = reader.GetInt32(0);
                            int incompleteTasks = reader.GetInt32(1);

                            // Clear existing data from the chart
                            chart1.Series.Clear();
                            chart1.ChartAreas.Clear();

                            // Add a ChartArea for the pie chart
                            ChartArea chartArea = new ChartArea("TaskCompletionArea");
                            chart1.ChartAreas.Add(chartArea);

                            // Create a Series for the pie chart
                            Series series = new Series("TaskCompletion");
                            series.ChartType = SeriesChartType.Pie;
                            series.Points.AddXY("Completed", completedTasks);
                            series.Points.AddXY("Incomplete", incompleteTasks);

                            // Add the series to the chart
                            chart1.Series.Add(series);

                            // Customize the pie chart
                            series.IsValueShownAsLabel = true; // Show values on the chart
                            chart1.Titles.Clear();
                            chart1.Titles.Add("Task Completion (Pie Chart)");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading pie chart: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
