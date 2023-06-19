namespace V3SaveManagerGUI.Editors
{
	partial class FlagEditor
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
			CurrentGameClearFlgLabel = new Label();
			GameClearFlgCheckbox = new CheckBox();
			NewGameClearFlgLabel = new Label();
			SetButton = new Button();
			SuspendLayout();
			// 
			// CurrentGameClearFlgLabel
			// 
			CurrentGameClearFlgLabel.AutoSize = true;
			CurrentGameClearFlgLabel.Location = new Point(12, 9);
			CurrentGameClearFlgLabel.Name = "CurrentGameClearFlgLabel";
			CurrentGameClearFlgLabel.Size = new Size(127, 15);
			CurrentGameClearFlgLabel.TabIndex = 0;
			CurrentGameClearFlgLabel.Text = "Current GameClearFlg:";
			// 
			// GameClearFlgCheckbox
			// 
			GameClearFlgCheckbox.AutoSize = true;
			GameClearFlgCheckbox.Location = new Point(129, 32);
			GameClearFlgCheckbox.Name = "GameClearFlgCheckbox";
			GameClearFlgCheckbox.Size = new Size(59, 19);
			GameClearFlgCheckbox.TabIndex = 1;
			GameClearFlgCheckbox.Text = "Active";
			GameClearFlgCheckbox.UseVisualStyleBackColor = true;
			// 
			// NewGameClearFlgLabel
			// 
			NewGameClearFlgLabel.AutoSize = true;
			NewGameClearFlgLabel.Location = new Point(12, 33);
			NewGameClearFlgLabel.Name = "NewGameClearFlgLabel";
			NewGameClearFlgLabel.Size = new Size(111, 15);
			NewGameClearFlgLabel.TabIndex = 2;
			NewGameClearFlgLabel.Text = "New GameClearFlg:";
			// 
			// SetButton
			// 
			SetButton.Location = new Point(12, 72);
			SetButton.Name = "SetButton";
			SetButton.Size = new Size(176, 23);
			SetButton.TabIndex = 3;
			SetButton.Text = "Set";
			SetButton.UseVisualStyleBackColor = true;
			SetButton.Click += SetButton_Click;
			// 
			// FlagEditor
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(372, 110);
			Controls.Add(SetButton);
			Controls.Add(NewGameClearFlgLabel);
			Controls.Add(GameClearFlgCheckbox);
			Controls.Add(CurrentGameClearFlgLabel);
			Name = "FlagEditor";
			Text = "FlagEditor";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		public Label CurrentGameClearFlgLabel;
		public CheckBox GameClearFlgCheckbox;
		public Label NewGameClearFlgLabel;
		public Button SetButton;
	}
}