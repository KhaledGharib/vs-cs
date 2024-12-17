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
    public partial class BarChart : Form
    {
        public BarChart()
        {
            InitializeComponent();
        }

        private void Chart_Load(object sender, EventArgs e)
        {

            LoadSessionDurationChart();

        }

        private void LoadSessionDurationChart()
        {
            try
            {
                using (var conn = new SQLiteConnection("Data Source=TaskOrganizer.db;Version=3;"))
                {
                    conn.Open();

                    // Query to get total session duration for each task
                    string query = @"
                        SELECT 
                            T.TaskDescription, 
                            IFNULL(SUM(TL.Duration), 0) AS TotalDuration
                        FROM Tasks T
                        LEFT JOIN TimerLogs TL ON T.TaskID = TL.TaskID
                        GROUP BY T.TaskID, T.TaskDescription
                    ";

                    using (var cmd = new SQLiteCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        // Clear existing chart data
                        chart1.Series.Clear();
                        chart1.ChartAreas.Clear();

                        // Configure the chart area
                        ChartArea chartArea = new ChartArea("SessionDuration");
                        chart1.ChartAreas.Add(chartArea);

                        // Create the series
                        Series series = new Series("Sessions");
                        series.ChartType = SeriesChartType.Bar;

                        // Add data points to the series
                        while (reader.Read())
                        {
                            string taskDescription = reader.IsDBNull(0) ? "Unknown Task" : reader.GetString(0);
                            int totalDuration = reader.IsDBNull(1) ? 0 : Convert.ToInt32(reader.GetValue(1));
                            series.Points.AddXY(taskDescription, totalDuration);
                        }


                        // Add the series to the chart
                        chart1.Series.Add(series);

                        // Set the title
                        chart1.Titles.Clear();
                        chart1.Titles.Add("Session Duration Per Task");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading session duration chart: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



    }

}