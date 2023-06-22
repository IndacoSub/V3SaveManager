namespace V3SaveManagerGUI.Editors
{
	partial class MonocoinEditor
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
			SetButton = new Button();
			CurrentMonocoinsLabel = new Label();
			NewMonocoinsTextbox = new TextBox();
			NewMonocoinsLabel = new Label();
			CurrentCasinoCoinsLabel = new Label();
			NewCasinoCoinsLabel = new Label();
			NewCasinoCoinsTextbox = new TextBox();
			SuspendLayout();
			// 
			// SetButton
			// 
			SetButton.Location = new Point(12, 114);
			SetButton.Name = "SetButton";
			SetButton.Size = new Size(267, 23);
			SetButton.TabIndex = 0;
			SetButton.Text = "Set";
			SetButton.UseVisualStyleBackColor = true;
			SetButton.Click += SetButton_Click;
			// 
			// CurrentMonocoinsLabel
			// 
			CurrentMonocoinsLabel.AutoSize = true;
			CurrentMonocoinsLabel.Location = new Point(12, 9);
			CurrentMonocoinsLabel.Name = "CurrentMonocoinsLabel";
			CurrentMonocoinsLabel.Size = new Size(113, 15);
			CurrentMonocoinsLabel.TabIndex = 1;
			CurrentMonocoinsLabel.Text = "Current Monocoins:";
			// 
			// NewMonocoinsTextbox
			// 
			NewMonocoinsTextbox.Location = new Point(152, 85);
			NewMonocoinsTextbox.MaxLength = 3;
			NewMonocoinsTextbox.Name = "NewMonocoinsTextbox";
			NewMonocoinsTextbox.Size = new Size(127, 23);
			NewMonocoinsTextbox.TabIndex = 2;
			// 
			// NewMonocoinsLabel
			// 
			NewMonocoinsLabel.AutoSize = true;
			NewMonocoinsLabel.Location = new Point(12, 88);
			NewMonocoinsLabel.Name = "NewMonocoinsLabel";
			NewMonocoinsLabel.Size = new Size(127, 15);
			NewMonocoinsLabel.TabIndex = 3;
			NewMonocoinsLabel.Text = "Insert new monocoins:";
			// 
			// CurrentCasinoCoinsLabel
			// 
			CurrentCasinoCoinsLabel.AutoSize = true;
			CurrentCasinoCoinsLabel.Location = new Point(12, 24);
			CurrentCasinoCoinsLabel.Name = "CurrentCasinoCoinsLabel";
			CurrentCasinoCoinsLabel.Size = new Size(118, 15);
			CurrentCasinoCoinsLabel.TabIndex = 4;
			CurrentCasinoCoinsLabel.Text = "Current casino coins:";
			// 
			// NewCasinoCoinsLabel
			// 
			NewCasinoCoinsLabel.AutoSize = true;
			NewCasinoCoinsLabel.Location = new Point(12, 62);
			NewCasinoCoinsLabel.Name = "NewCasinoCoinsLabel";
			NewCasinoCoinsLabel.Size = new Size(132, 15);
			NewCasinoCoinsLabel.TabIndex = 5;
			NewCasinoCoinsLabel.Text = "Insert new casino coins:";
			// 
			// NewCasinoCoinsTextbox
			// 
			NewCasinoCoinsTextbox.Location = new Point(152, 59);
			NewCasinoCoinsTextbox.MaxLength = 7;
			NewCasinoCoinsTextbox.Name = "NewCasinoCoinsTextbox";
			NewCasinoCoinsTextbox.Size = new Size(127, 23);
			NewCasinoCoinsTextbox.TabIndex = 6;
			// 
			// MonocoinEditor
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(291, 149);
			Controls.Add(NewCasinoCoinsTextbox);
			Controls.Add(NewCasinoCoinsLabel);
			Controls.Add(CurrentCasinoCoinsLabel);
			Controls.Add(NewMonocoinsLabel);
			Controls.Add(NewMonocoinsTextbox);
			Controls.Add(CurrentMonocoinsLabel);
			Controls.Add(SetButton);
			MaximumSize = new Size(307, 188);
			MinimumSize = new Size(307, 188);
			Name = "MonocoinEditor";
			Text = "MonocoinEditor";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		public Button SetButton;
		public Label CurrentMonocoinsLabel;
		public TextBox NewMonocoinsTextbox;
		public Label NewMonocoinsLabel;
		public Label CurrentCasinoCoinsLabel;
		public Label NewCasinoCoinsLabel;
		public TextBox NewCasinoCoinsTextbox;
	}
}