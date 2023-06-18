namespace V3SaveManagerGUI
{
	partial class Main
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			EditListComboBox = new ComboBox();
			WhatEditLabel = new Label();
			EditButton = new Button();
			SaveDatAsButton = new Button();
			OpenFileButton = new Button();
			OpenReminderLabel = new Label();
			SuspendLayout();
			// 
			// EditListComboBox
			// 
			EditListComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			EditListComboBox.FormattingEnabled = true;
			EditListComboBox.Location = new Point(192, 26);
			EditListComboBox.Name = "EditListComboBox";
			EditListComboBox.Size = new Size(157, 23);
			EditListComboBox.TabIndex = 0;
			EditListComboBox.SelectedIndexChanged += EditListComboBox_SelectedIndexChanged;
			// 
			// WhatEditLabel
			// 
			WhatEditLabel.AutoSize = true;
			WhatEditLabel.Location = new Point(192, 8);
			WhatEditLabel.Name = "WhatEditLabel";
			WhatEditLabel.Size = new Size(157, 15);
			WhatEditLabel.TabIndex = 1;
			WhatEditLabel.Text = "What would you like to edit?";
			// 
			// EditButton
			// 
			EditButton.Location = new Point(368, 26);
			EditButton.Name = "EditButton";
			EditButton.Size = new Size(75, 23);
			EditButton.TabIndex = 2;
			EditButton.Text = "Edit";
			EditButton.UseVisualStyleBackColor = true;
			EditButton.Click += EditButton_Click;
			// 
			// SaveDatAsButton
			// 
			SaveDatAsButton.Location = new Point(456, 26);
			SaveDatAsButton.Name = "SaveDatAsButton";
			SaveDatAsButton.Size = new Size(96, 23);
			SaveDatAsButton.TabIndex = 3;
			SaveDatAsButton.Text = "Save .dat as...";
			SaveDatAsButton.UseVisualStyleBackColor = true;
			SaveDatAsButton.Click += SaveDatAsButton_Click;
			// 
			// OpenFileButton
			// 
			OpenFileButton.Location = new Point(6, 25);
			OpenFileButton.Name = "OpenFileButton";
			OpenFileButton.Size = new Size(163, 23);
			OpenFileButton.TabIndex = 4;
			OpenFileButton.Text = "Open .dat file";
			OpenFileButton.UseVisualStyleBackColor = true;
			OpenFileButton.Click += OpenFileButton_Click;
			// 
			// OpenReminderLabel
			// 
			OpenReminderLabel.AutoSize = true;
			OpenReminderLabel.Location = new Point(9, 8);
			OpenReminderLabel.Name = "OpenReminderLabel";
			OpenReminderLabel.Size = new Size(160, 15);
			OpenReminderLabel.TabIndex = 5;
			OpenReminderLabel.Text = "Remember to open a .dat file";
			// 
			// Main
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(564, 61);
			Controls.Add(OpenReminderLabel);
			Controls.Add(OpenFileButton);
			Controls.Add(SaveDatAsButton);
			Controls.Add(EditButton);
			Controls.Add(WhatEditLabel);
			Controls.Add(EditListComboBox);
			MaximumSize = new Size(580, 100);
			MinimumSize = new Size(580, 100);
			Name = "Main";
			Text = "V3SaveManagerGUI";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private ComboBox EditListComboBox;
		private Label WhatEditLabel;
		private Button EditButton;
		private Button SaveDatAsButton;
		private Button OpenFileButton;
		private Label OpenReminderLabel;
	}
}