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
			SuspendLayout();
			// 
			// SetButton
			// 
			SetButton.Location = new Point(145, 58);
			SetButton.Name = "SetButton";
			SetButton.Size = new Size(134, 23);
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
			NewMonocoinsTextbox.Location = new Point(12, 57);
			NewMonocoinsTextbox.MaxLength = 3;
			NewMonocoinsTextbox.Name = "NewMonocoinsTextbox";
			NewMonocoinsTextbox.Size = new Size(127, 23);
			NewMonocoinsTextbox.TabIndex = 2;
			// 
			// NewMonocoinsLabel
			// 
			NewMonocoinsLabel.AutoSize = true;
			NewMonocoinsLabel.Location = new Point(12, 39);
			NewMonocoinsLabel.Name = "NewMonocoinsLabel";
			NewMonocoinsLabel.Size = new Size(127, 15);
			NewMonocoinsLabel.TabIndex = 3;
			NewMonocoinsLabel.Text = "Insert new monocoins:";
			// 
			// MonocoinEditor
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(291, 93);
			Controls.Add(NewMonocoinsLabel);
			Controls.Add(NewMonocoinsTextbox);
			Controls.Add(CurrentMonocoinsLabel);
			Controls.Add(SetButton);
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
	}
}