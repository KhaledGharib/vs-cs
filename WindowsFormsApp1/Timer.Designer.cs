namespace WindowsFormsApp1
{
    partial class Timer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.nudPomodoroDuration = new System.Windows.Forms.NumericUpDown();
            this.lblTimer = new System.Windows.Forms.Label();
            this.btnStartTimer = new System.Windows.Forms.Button();
            this.btnStopTimer = new System.Windows.Forms.Button();
            this.cmbTasks = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudPomodoroDuration)).BeginInit();
            this.SuspendLayout();
            // 
            // nudPomodoroDuration
            // 
            this.nudPomodoroDuration.Location = new System.Drawing.Point(124, 86);
            this.nudPomodoroDuration.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nudPomodoroDuration.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudPomodoroDuration.Name = "nudPomodoroDuration";
            this.nudPomodoroDuration.Size = new System.Drawing.Size(120, 22);
            this.nudPomodoroDuration.TabIndex = 0;
            this.nudPomodoroDuration.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblTimer
            // 
            this.lblTimer.AutoSize = true;
            this.lblTimer.Location = new System.Drawing.Point(48, 88);
            this.lblTimer.Name = "lblTimer";
            this.lblTimer.Size = new System.Drawing.Size(38, 16);
            this.lblTimer.TabIndex = 1;
            this.lblTimer.Text = "00:00";
            // 
            // btnStartTimer
            // 
            this.btnStartTimer.Location = new System.Drawing.Point(21, 196);
            this.btnStartTimer.Name = "btnStartTimer";
            this.btnStartTimer.Size = new System.Drawing.Size(75, 23);
            this.btnStartTimer.TabIndex = 2;
            this.btnStartTimer.Text = "Sstart";
            this.btnStartTimer.UseVisualStyleBackColor = true;
            this.btnStartTimer.Click += new System.EventHandler(this.btnStartTimer_Click);
            // 
            // btnStopTimer
            // 
            this.btnStopTimer.Location = new System.Drawing.Point(109, 196);
            this.btnStopTimer.Name = "btnStopTimer";
            this.btnStopTimer.Size = new System.Drawing.Size(75, 23);
            this.btnStopTimer.TabIndex = 3;
            this.btnStopTimer.Text = "Stop";
            this.btnStopTimer.UseVisualStyleBackColor = true;
            this.btnStopTimer.Click += new System.EventHandler(this.btnStopTimer_Click);
            // 
            // cmbTasks
            // 
            this.cmbTasks.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTasks.FormattingEnabled = true;
            this.cmbTasks.Location = new System.Drawing.Point(124, 114);
            this.cmbTasks.Name = "cmbTasks";
            this.cmbTasks.Size = new System.Drawing.Size(121, 24);
            this.cmbTasks.TabIndex = 4;
            // 
            // Timer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 289);
            this.Controls.Add(this.cmbTasks);
            this.Controls.Add(this.btnStopTimer);
            this.Controls.Add(this.btnStartTimer);
            this.Controls.Add(this.lblTimer);
            this.Controls.Add(this.nudPomodoroDuration);
            this.Name = "Timer";
            this.Text = "Timer";
            this.Load += new System.EventHandler(this.Timer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudPomodoroDuration)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nudPomodoroDuration;
        private System.Windows.Forms.Label lblTimer;
        private System.Windows.Forms.Button btnStartTimer;
        private System.Windows.Forms.Button btnStopTimer;
        private System.Windows.Forms.ComboBox cmbTasks;
    }
}