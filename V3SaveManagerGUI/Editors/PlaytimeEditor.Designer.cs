namespace V3SaveManagerGUI.Editors
{
	partial class PlaytimeEditor
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
			CurrentPlaytimeLabel = new Label();
			NewPlaytimeLabel = new Label();
			NewPlaytimeTextbox = new TextBox();
			HoursLabel = new Label();
			MinutesLabel = new Label();
			SecondsLabel = new Label();
			FramesLabel = new Label();
			HoursTextbox = new TextBox();
			MinutesTextbox = new TextBox();
			SecondsTextbox = new TextBox();
			FramesTextbox = new TextBox();
			ResultTextbox = new TextBox();
			SetButton = new Button();
			CalculateButton = new Button();
			NoteLabel = new Label();
			SuspendLayout();
			// 
			// CurrentPlaytimeLabel
			// 
			CurrentPlaytimeLabel.AutoSize = true;
			CurrentPlaytimeLabel.Location = new Point(12, 9);
			CurrentPlaytimeLabel.Name = "CurrentPlaytimeLabel";
			CurrentPlaytimeLabel.Size = new Size(159, 15);
			CurrentPlaytimeLabel.TabIndex = 0;
			CurrentPlaytimeLabel.Text = "Current playtime (in frames):";
			// 
			// NewPlaytimeLabel
			// 
			NewPlaytimeLabel.AutoSize = true;
			NewPlaytimeLabel.Location = new Point(12, 101);
			NewPlaytimeLabel.Name = "NewPlaytimeLabel";
			NewPlaytimeLabel.Size = new Size(140, 15);
			NewPlaytimeLabel.TabIndex = 1;
			NewPlaytimeLabel.Text = "New playtime (in frames)";
			// 
			// NewPlaytimeTextbox
			// 
			NewPlaytimeTextbox.Location = new Point(12, 123);
			NewPlaytimeTextbox.Name = "NewPlaytimeTextbox";
			NewPlaytimeTextbox.Size = new Size(140, 23);
			NewPlaytimeTextbox.TabIndex = 2;
			// 
			// HoursLabel
			// 
			HoursLabel.AutoSize = true;
			HoursLabel.Location = new Point(315, 9);
			HoursLabel.Name = "HoursLabel";
			HoursLabel.Size = new Size(42, 15);
			HoursLabel.TabIndex = 4;
			HoursLabel.Text = "Hours:";
			// 
			// MinutesLabel
			// 
			MinutesLabel.AutoSize = true;
			MinutesLabel.Location = new Point(315, 38);
			MinutesLabel.Name = "MinutesLabel";
			MinutesLabel.Size = new Size(53, 15);
			MinutesLabel.TabIndex = 5;
			MinutesLabel.Text = "Minutes:";
			// 
			// SecondsLabel
			// 
			SecondsLabel.AutoSize = true;
			SecondsLabel.Location = new Point(315, 67);
			SecondsLabel.Name = "SecondsLabel";
			SecondsLabel.Size = new Size(54, 15);
			SecondsLabel.TabIndex = 6;
			SecondsLabel.Text = "Seconds:";
			// 
			// FramesLabel
			// 
			FramesLabel.AutoSize = true;
			FramesLabel.Location = new Point(315, 96);
			FramesLabel.Name = "FramesLabel";
			FramesLabel.Size = new Size(48, 15);
			FramesLabel.TabIndex = 7;
			FramesLabel.Text = "Frames:";
			// 
			// HoursTextbox
			// 
			HoursTextbox.Location = new Point(410, 6);
			HoursTextbox.MaxLength = 4;
			HoursTextbox.Name = "HoursTextbox";
			HoursTextbox.Size = new Size(138, 23);
			HoursTextbox.TabIndex = 8;
			// 
			// MinutesTextbox
			// 
			MinutesTextbox.Location = new Point(410, 35);
			MinutesTextbox.MaxLength = 2;
			MinutesTextbox.Name = "MinutesTextbox";
			MinutesTextbox.Size = new Size(138, 23);
			MinutesTextbox.TabIndex = 9;
			// 
			// SecondsTextbox
			// 
			SecondsTextbox.Location = new Point(410, 64);
			SecondsTextbox.MaxLength = 2;
			SecondsTextbox.Name = "SecondsTextbox";
			SecondsTextbox.Size = new Size(138, 23);
			SecondsTextbox.TabIndex = 10;
			// 
			// FramesTextbox
			// 
			FramesTextbox.Location = new Point(410, 93);
			FramesTextbox.MaxLength = 3;
			FramesTextbox.Name = "FramesTextbox";
			FramesTextbox.Size = new Size(138, 23);
			FramesTextbox.TabIndex = 11;
			// 
			// ResultTextbox
			// 
			ResultTextbox.Location = new Point(410, 123);
			ResultTextbox.Name = "ResultTextbox";
			ResultTextbox.ReadOnly = true;
			ResultTextbox.Size = new Size(138, 23);
			ResultTextbox.TabIndex = 12;
			// 
			// SetButton
			// 
			SetButton.Location = new Point(158, 122);
			SetButton.Name = "SetButton";
			SetButton.Size = new Size(142, 23);
			SetButton.TabIndex = 13;
			SetButton.Text = "Set";
			SetButton.UseVisualStyleBackColor = true;
			SetButton.Click += SetButton_Click;
			// 
			// CalculateButton
			// 
			CalculateButton.Location = new Point(315, 122);
			CalculateButton.Name = "CalculateButton";
			CalculateButton.Size = new Size(89, 23);
			CalculateButton.TabIndex = 14;
			CalculateButton.Text = "Calculate";
			CalculateButton.UseVisualStyleBackColor = true;
			CalculateButton.Click += CalculateButton_Click;
			// 
			// NoteLabel
			// 
			NoteLabel.Location = new Point(12, 38);
			NoteLabel.Name = "NoteLabel";
			NoteLabel.Size = new Size(297, 63);
			NoteLabel.TabIndex = 15;
			NoteLabel.Text = "Note: due to a static string located in the first bytes the beginning of the savefile, you won't be able to *see* the newly-inserted playtime until you save the game (but this still works).";
			// 
			// PlaytimeEditor
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(560, 155);
			Controls.Add(NoteLabel);
			Controls.Add(CalculateButton);
			Controls.Add(SetButton);
			Controls.Add(ResultTextbox);
			Controls.Add(FramesTextbox);
			Controls.Add(SecondsTextbox);
			Controls.Add(MinutesTextbox);
			Controls.Add(HoursTextbox);
			Controls.Add(FramesLabel);
			Controls.Add(SecondsLabel);
			Controls.Add(MinutesLabel);
			Controls.Add(HoursLabel);
			Controls.Add(NewPlaytimeTextbox);
			Controls.Add(NewPlaytimeLabel);
			Controls.Add(CurrentPlaytimeLabel);
			Name = "PlaytimeEditor";
			Text = "PlaytimeEditor";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		public Label CurrentPlaytimeLabel;
		public Label NewPlaytimeLabel;
		public TextBox NewPlaytimeTextbox;
		public Label HoursLabel;
		public Label MinutesLabel;
		public Label SecondsLabel;
		public Label FramesLabel;
		public TextBox HoursTextbox;
		public TextBox MinutesTextbox;
		public TextBox SecondsTextbox;
		public TextBox FramesTextbox;
		public TextBox ResultTextbox;
		public Button SetButton;
		public Button CalculateButton;
		public Label NoteLabel;
	}
}