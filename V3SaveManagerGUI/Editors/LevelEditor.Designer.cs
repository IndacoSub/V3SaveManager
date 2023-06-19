namespace V3SaveManagerGUI.Editors
{
	partial class LevelEditor
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
			CurrentLevelLabel = new Label();
			CurrentTotalEXPLabel = new Label();
			NewLevelLabel = new Label();
			NewLevelTextbox = new TextBox();
			NewTotalEXPLabel = new Label();
			NewTotalEXPTextbox = new TextBox();
			SetButton = new Button();
			DesiredLevelLabel = new Label();
			DesiredLevelTextbox = new TextBox();
			CalculateButton = new Button();
			ResultTextbox = new TextBox();
			MinimumEXPLabel = new Label();
			SuspendLayout();
			// 
			// CurrentLevelLabel
			// 
			CurrentLevelLabel.AutoSize = true;
			CurrentLevelLabel.Location = new Point(12, 9);
			CurrentLevelLabel.Name = "CurrentLevelLabel";
			CurrentLevelLabel.Size = new Size(80, 15);
			CurrentLevelLabel.TabIndex = 0;
			CurrentLevelLabel.Text = "Current Level:";
			// 
			// CurrentTotalEXPLabel
			// 
			CurrentTotalEXPLabel.AutoSize = true;
			CurrentTotalEXPLabel.Location = new Point(12, 24);
			CurrentTotalEXPLabel.Name = "CurrentTotalEXPLabel";
			CurrentTotalEXPLabel.Size = new Size(101, 15);
			CurrentTotalEXPLabel.TabIndex = 1;
			CurrentTotalEXPLabel.Text = "Current Total EXP:";
			// 
			// NewLevelLabel
			// 
			NewLevelLabel.AutoSize = true;
			NewLevelLabel.Location = new Point(12, 61);
			NewLevelLabel.Name = "NewLevelLabel";
			NewLevelLabel.Size = new Size(64, 15);
			NewLevelLabel.TabIndex = 2;
			NewLevelLabel.Text = "New Level:";
			// 
			// NewLevelTextbox
			// 
			NewLevelTextbox.Location = new Point(121, 58);
			NewLevelTextbox.MaxLength = 3;
			NewLevelTextbox.Name = "NewLevelTextbox";
			NewLevelTextbox.Size = new Size(100, 23);
			NewLevelTextbox.TabIndex = 3;
			// 
			// NewTotalEXPLabel
			// 
			NewTotalEXPLabel.AutoSize = true;
			NewTotalEXPLabel.Location = new Point(12, 93);
			NewTotalEXPLabel.Name = "NewTotalEXPLabel";
			NewTotalEXPLabel.Size = new Size(85, 15);
			NewTotalEXPLabel.TabIndex = 4;
			NewTotalEXPLabel.Text = "New Total EXP:";
			// 
			// NewTotalEXPTextbox
			// 
			NewTotalEXPTextbox.Location = new Point(121, 90);
			NewTotalEXPTextbox.MaxLength = 6;
			NewTotalEXPTextbox.Name = "NewTotalEXPTextbox";
			NewTotalEXPTextbox.Size = new Size(100, 23);
			NewTotalEXPTextbox.TabIndex = 5;
			// 
			// SetButton
			// 
			SetButton.Location = new Point(12, 124);
			SetButton.Name = "SetButton";
			SetButton.Size = new Size(209, 23);
			SetButton.TabIndex = 6;
			SetButton.Text = "Set";
			SetButton.UseVisualStyleBackColor = true;
			SetButton.Click += SetButton_Click;
			// 
			// DesiredLevelLabel
			// 
			DesiredLevelLabel.AutoSize = true;
			DesiredLevelLabel.Location = new Point(237, 61);
			DesiredLevelLabel.Name = "DesiredLevelLabel";
			DesiredLevelLabel.Size = new Size(79, 15);
			DesiredLevelLabel.TabIndex = 7;
			DesiredLevelLabel.Text = "Desired Level:";
			// 
			// DesiredLevelTextbox
			// 
			DesiredLevelTextbox.Location = new Point(331, 58);
			DesiredLevelTextbox.MaxLength = 2;
			DesiredLevelTextbox.Name = "DesiredLevelTextbox";
			DesiredLevelTextbox.Size = new Size(100, 23);
			DesiredLevelTextbox.TabIndex = 8;
			// 
			// CalculateButton
			// 
			CalculateButton.Location = new Point(237, 124);
			CalculateButton.Name = "CalculateButton";
			CalculateButton.Size = new Size(194, 23);
			CalculateButton.TabIndex = 9;
			CalculateButton.Text = "Calculate";
			CalculateButton.UseVisualStyleBackColor = true;
			CalculateButton.Click += CalculateButton_Click;
			// 
			// ResultTextbox
			// 
			ResultTextbox.Location = new Point(331, 90);
			ResultTextbox.Name = "ResultTextbox";
			ResultTextbox.ReadOnly = true;
			ResultTextbox.Size = new Size(100, 23);
			ResultTextbox.TabIndex = 10;
			// 
			// MinimumEXPLabel
			// 
			MinimumEXPLabel.AutoSize = true;
			MinimumEXPLabel.Location = new Point(237, 93);
			MinimumEXPLabel.Name = "MinimumEXPLabel";
			MinimumEXPLabel.Size = new Size(86, 15);
			MinimumEXPLabel.TabIndex = 11;
			MinimumEXPLabel.Text = "Minimum EXP:";
			// 
			// LevelEditor
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(444, 159);
			Controls.Add(MinimumEXPLabel);
			Controls.Add(ResultTextbox);
			Controls.Add(CalculateButton);
			Controls.Add(DesiredLevelTextbox);
			Controls.Add(DesiredLevelLabel);
			Controls.Add(SetButton);
			Controls.Add(NewTotalEXPTextbox);
			Controls.Add(NewTotalEXPLabel);
			Controls.Add(NewLevelTextbox);
			Controls.Add(NewLevelLabel);
			Controls.Add(CurrentTotalEXPLabel);
			Controls.Add(CurrentLevelLabel);
			Name = "LevelEditor";
			Text = "LevelEditor";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		public Label CurrentLevelLabel;
		public Label CurrentTotalEXPLabel;
		public Label NewLevelLabel;
		public TextBox NewLevelTextbox;
		public Label NewTotalEXPLabel;
		public TextBox NewTotalEXPTextbox;
		public Button SetButton;
		public Label DesiredLevelLabel;
		public TextBox DesiredLevelTextbox;
		public Button CalculateButton;
		public TextBox ResultTextbox;
		public Label MinimumEXPLabel;
	}
}