namespace Metronome
{
	partial class MainWindow
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
			this.txtTempo = new System.Windows.Forms.TextBox();
			this.txtBeat = new System.Windows.Forms.TextBox();
			this.txtTime = new System.Windows.Forms.TextBox();
			this.cmdClose = new System.Windows.Forms.Button();
			this.cmdStartStop = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.lblBPM = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtBeatsPerMeasure = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// txtTempo
			// 
			this.txtTempo.Location = new System.Drawing.Point(61, 12);
			this.txtTempo.Name = "txtTempo";
			this.txtTempo.Size = new System.Drawing.Size(100, 20);
			this.txtTempo.TabIndex = 0;
			// 
			// txtBeat
			// 
			this.txtBeat.BackColor = System.Drawing.Color.Black;
			this.txtBeat.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtBeat.ForeColor = System.Drawing.Color.Lime;
			this.txtBeat.Location = new System.Drawing.Point(231, 38);
			this.txtBeat.Name = "txtBeat";
			this.txtBeat.Size = new System.Drawing.Size(172, 44);
			this.txtBeat.TabIndex = 0;
			this.txtBeat.Text = "0|0|000";
			// 
			// txtTime
			// 
			this.txtTime.BackColor = System.Drawing.Color.Black;
			this.txtTime.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtTime.ForeColor = System.Drawing.Color.Lime;
			this.txtTime.Location = new System.Drawing.Point(12, 38);
			this.txtTime.Name = "txtTime";
			this.txtTime.Size = new System.Drawing.Size(213, 44);
			this.txtTime.TabIndex = 0;
			this.txtTime.Text = "00:00:00.00";
			// 
			// cmdClose
			// 
			this.cmdClose.Location = new System.Drawing.Point(328, 88);
			this.cmdClose.Name = "cmdClose";
			this.cmdClose.Size = new System.Drawing.Size(75, 23);
			this.cmdClose.TabIndex = 1;
			this.cmdClose.Text = "Close";
			this.cmdClose.UseVisualStyleBackColor = true;
			// 
			// cmdStartStop
			// 
			this.cmdStartStop.Location = new System.Drawing.Point(247, 88);
			this.cmdStartStop.Name = "cmdStartStop";
			this.cmdStartStop.Size = new System.Drawing.Size(75, 23);
			this.cmdStartStop.TabIndex = 1;
			this.cmdStartStop.Text = "&Start";
			this.cmdStartStop.UseVisualStyleBackColor = true;
			this.cmdStartStop.Click += new System.EventHandler(this.cmdStartStop_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(12, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(43, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "&Tempo:";
			// 
			// lblBPM
			// 
			this.lblBPM.AutoSize = true;
			this.lblBPM.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lblBPM.Location = new System.Drawing.Point(167, 15);
			this.lblBPM.Name = "lblBPM";
			this.lblBPM.Size = new System.Drawing.Size(30, 13);
			this.lblBPM.TabIndex = 2;
			this.lblBPM.Text = "BPM";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(228, 15);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(98, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "&Beats per measure:";
			// 
			// txtBeatsPerMeasure
			// 
			this.txtBeatsPerMeasure.Location = new System.Drawing.Point(332, 12);
			this.txtBeatsPerMeasure.Name = "txtBeatsPerMeasure";
			this.txtBeatsPerMeasure.Size = new System.Drawing.Size(71, 20);
			this.txtBeatsPerMeasure.TabIndex = 4;
			this.txtBeatsPerMeasure.Text = "4";
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(415, 126);
			this.Controls.Add(this.txtBeatsPerMeasure);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lblBPM);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cmdStartStop);
			this.Controls.Add(this.cmdClose);
			this.Controls.Add(this.txtTime);
			this.Controls.Add(this.txtBeat);
			this.Controls.Add(this.txtTempo);
			this.Name = "MainWindow";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtTempo;
		private System.Windows.Forms.TextBox txtBeat;
		private System.Windows.Forms.TextBox txtTime;
		private System.Windows.Forms.Button cmdClose;
		private System.Windows.Forms.Button cmdStartStop;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblBPM;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtBeatsPerMeasure;
	}
}

