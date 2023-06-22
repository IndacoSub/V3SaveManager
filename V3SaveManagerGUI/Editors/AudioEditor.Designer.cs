namespace V3SaveManagerGUI.Editors
{
	partial class AudioEditor
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
			BGMIndex = new Templates.FileForm();
			CurrentBGM = new Templates.FileForm();
			CurrentSFX = new Templates.FileForm();
			SetButton = new Button();
			CurrentSFXAgain = new Templates.FileForm();
			SuspendLayout();
			// 
			// BGMIndex
			// 
			BGMIndex.Location = new Point(12, 12);
			BGMIndex.Name = "BGMIndex";
			BGMIndex.Size = new Size(304, 71);
			BGMIndex.TabIndex = 0;
			// 
			// CurrentBGM
			// 
			CurrentBGM.Location = new Point(12, 89);
			CurrentBGM.Name = "CurrentBGM";
			CurrentBGM.Size = new Size(304, 71);
			CurrentBGM.TabIndex = 1;
			// 
			// CurrentSFX
			// 
			CurrentSFX.Location = new Point(12, 166);
			CurrentSFX.Name = "CurrentSFX";
			CurrentSFX.Size = new Size(304, 71);
			CurrentSFX.TabIndex = 2;
			// 
			// SetButton
			// 
			SetButton.Location = new Point(12, 320);
			SetButton.Name = "SetButton";
			SetButton.Size = new Size(304, 23);
			SetButton.TabIndex = 3;
			SetButton.Text = "Set";
			SetButton.UseVisualStyleBackColor = true;
			SetButton.Click += SetButton_Click;
			// 
			// CurrentSFXAgain
			// 
			CurrentSFXAgain.Location = new Point(12, 243);
			CurrentSFXAgain.Name = "CurrentSFXAgain";
			CurrentSFXAgain.Size = new Size(304, 71);
			CurrentSFXAgain.TabIndex = 4;
			// 
			// AudioEditor
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(328, 350);
			Controls.Add(CurrentSFXAgain);
			Controls.Add(SetButton);
			Controls.Add(CurrentSFX);
			Controls.Add(CurrentBGM);
			Controls.Add(BGMIndex);
			Name = "AudioEditor";
			Text = "AudioEditor";
			ResumeLayout(false);
		}

		#endregion

		public Templates.FileForm BGMIndex;
		public Templates.FileForm CurrentBGM;
		public Templates.FileForm CurrentSFX;
		public Button SetButton;
		public Templates.FileForm CurrentSFXAgain;
	}
}