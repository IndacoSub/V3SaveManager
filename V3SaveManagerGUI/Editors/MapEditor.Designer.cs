namespace V3SaveManagerGUI.Editors
{
	partial class MapEditor
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
			CurrentMapLabel = new Label();
			CurrentPositionXLabel = new Label();
			CurrentPositionYLabel = new Label();
			SetButton = new Button();
			PossibleMapsListbox = new ListBox();
			NewMapLabel = new Label();
			NewPositionXLabel = new Label();
			NewPositionYLabel = new Label();
			NewPositionXTextbox = new TextBox();
			NewPositionYTextbox = new TextBox();
			CurrentMapIDNoLabel = new Label();
			SuspendLayout();
			// 
			// CurrentMapLabel
			// 
			CurrentMapLabel.AutoSize = true;
			CurrentMapLabel.Location = new Point(12, 9);
			CurrentMapLabel.Name = "CurrentMapLabel";
			CurrentMapLabel.Size = new Size(91, 15);
			CurrentMapLabel.TabIndex = 0;
			CurrentMapLabel.Text = "Current Map: ID";
			// 
			// CurrentPositionXLabel
			// 
			CurrentPositionXLabel.AutoSize = true;
			CurrentPositionXLabel.Location = new Point(12, 33);
			CurrentPositionXLabel.Name = "CurrentPositionXLabel";
			CurrentPositionXLabel.Size = new Size(106, 15);
			CurrentPositionXLabel.TabIndex = 1;
			CurrentPositionXLabel.Text = "Current Position X:";
			// 
			// CurrentPositionYLabel
			// 
			CurrentPositionYLabel.AutoSize = true;
			CurrentPositionYLabel.Location = new Point(12, 57);
			CurrentPositionYLabel.Name = "CurrentPositionYLabel";
			CurrentPositionYLabel.Size = new Size(106, 15);
			CurrentPositionYLabel.TabIndex = 2;
			CurrentPositionYLabel.Text = "Current Position Y:";
			// 
			// SetButton
			// 
			SetButton.Location = new Point(12, 354);
			SetButton.Name = "SetButton";
			SetButton.Size = new Size(240, 23);
			SetButton.TabIndex = 3;
			SetButton.Text = "Set";
			SetButton.UseVisualStyleBackColor = true;
			SetButton.Click += SetButton_Click;
			// 
			// PossibleMapsListbox
			// 
			PossibleMapsListbox.FormattingEnabled = true;
			PossibleMapsListbox.ItemHeight = 15;
			PossibleMapsListbox.Location = new Point(12, 103);
			PossibleMapsListbox.Name = "PossibleMapsListbox";
			PossibleMapsListbox.ScrollAlwaysVisible = true;
			PossibleMapsListbox.Size = new Size(240, 184);
			PossibleMapsListbox.TabIndex = 5;
			// 
			// NewMapLabel
			// 
			NewMapLabel.AutoSize = true;
			NewMapLabel.Location = new Point(12, 85);
			NewMapLabel.Name = "NewMapLabel";
			NewMapLabel.Size = new Size(61, 15);
			NewMapLabel.TabIndex = 6;
			NewMapLabel.Text = "New Map:";
			// 
			// NewPositionXLabel
			// 
			NewPositionXLabel.AutoSize = true;
			NewPositionXLabel.Location = new Point(12, 300);
			NewPositionXLabel.Name = "NewPositionXLabel";
			NewPositionXLabel.Size = new Size(90, 15);
			NewPositionXLabel.TabIndex = 7;
			NewPositionXLabel.Text = "New Position X:";
			// 
			// NewPositionYLabel
			// 
			NewPositionYLabel.AutoSize = true;
			NewPositionYLabel.Location = new Point(12, 328);
			NewPositionYLabel.Name = "NewPositionYLabel";
			NewPositionYLabel.Size = new Size(90, 15);
			NewPositionYLabel.TabIndex = 8;
			NewPositionYLabel.Text = "New Position Y:";
			// 
			// NewPositionXTextbox
			// 
			NewPositionXTextbox.Location = new Point(108, 297);
			NewPositionXTextbox.Name = "NewPositionXTextbox";
			NewPositionXTextbox.Size = new Size(144, 23);
			NewPositionXTextbox.TabIndex = 9;
			NewPositionXTextbox.TextAlign = HorizontalAlignment.Center;
			// 
			// NewPositionYTextbox
			// 
			NewPositionYTextbox.Location = new Point(108, 325);
			NewPositionYTextbox.Name = "NewPositionYTextbox";
			NewPositionYTextbox.Size = new Size(144, 23);
			NewPositionYTextbox.TabIndex = 10;
			NewPositionYTextbox.TextAlign = HorizontalAlignment.Center;
			// 
			// CurrentMapIDNoLabel
			// 
			CurrentMapIDNoLabel.AutoSize = true;
			CurrentMapIDNoLabel.Location = new Point(105, 9);
			CurrentMapIDNoLabel.Name = "CurrentMapIDNoLabel";
			CurrentMapIDNoLabel.Size = new Size(13, 15);
			CurrentMapIDNoLabel.TabIndex = 11;
			CurrentMapIDNoLabel.Text = "0";
			// 
			// MapEditor
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(264, 385);
			Controls.Add(CurrentMapIDNoLabel);
			Controls.Add(NewPositionYTextbox);
			Controls.Add(NewPositionXTextbox);
			Controls.Add(NewPositionYLabel);
			Controls.Add(NewPositionXLabel);
			Controls.Add(NewMapLabel);
			Controls.Add(PossibleMapsListbox);
			Controls.Add(SetButton);
			Controls.Add(CurrentPositionYLabel);
			Controls.Add(CurrentPositionXLabel);
			Controls.Add(CurrentMapLabel);
			Name = "MapEditor";
			Text = "MapEditor";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
		public Label CurrentMapLabel;
		public Label CurrentPositionXLabel;
		public Label CurrentPositionYLabel;
		public Button SetButton;
		public ListBox PossibleMapsListbox;
		public Label NewMapLabel;
		public Label NewPositionXLabel;
		public Label NewPositionYLabel;
		public TextBox NewPositionXTextbox;
		public TextBox NewPositionYTextbox;
		public Label CurrentMapIDNoLabel;
	}
}