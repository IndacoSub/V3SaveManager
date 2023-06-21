namespace V3SaveManagerGUI.Editors
{
	partial class FilesEditor
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
			FileInfoWRDArchive = new Templates.FileForm();
			FileInfoWRDFile = new Templates.FileForm();
			FileInfoTextArchive = new Templates.FileForm();
			FileInfoTextSTX = new Templates.FileForm();
			FileInfoWRDMap = new Templates.FileForm();
			FileInfoSubroutineArchive = new Templates.FileForm();
			FileInfoSubroutineWRD = new Templates.FileForm();
			FileInfoSubroutineTextArchive = new Templates.FileForm();
			FileInfoSubroutineSTX = new Templates.FileForm();
			SetButton = new Button();
			ResetButton = new Button();
			CurrentLine = new Templates.FileForm();
			SuspendLayout();
			// 
			// FileInfoWRDArchive
			// 
			FileInfoWRDArchive.Location = new Point(12, 12);
			FileInfoWRDArchive.Name = "FileInfoWRDArchive";
			FileInfoWRDArchive.Size = new Size(304, 71);
			FileInfoWRDArchive.TabIndex = 0;
			// 
			// FileInfoWRDFile
			// 
			FileInfoWRDFile.Location = new Point(632, 12);
			FileInfoWRDFile.Name = "FileInfoWRDFile";
			FileInfoWRDFile.Size = new Size(304, 71);
			FileInfoWRDFile.TabIndex = 1;
			// 
			// FileInfoTextArchive
			// 
			FileInfoTextArchive.Location = new Point(324, 89);
			FileInfoTextArchive.Name = "FileInfoTextArchive";
			FileInfoTextArchive.Size = new Size(304, 71);
			FileInfoTextArchive.TabIndex = 2;
			// 
			// FileInfoTextSTX
			// 
			FileInfoTextSTX.Location = new Point(12, 166);
			FileInfoTextSTX.Name = "FileInfoTextSTX";
			FileInfoTextSTX.Size = new Size(304, 71);
			FileInfoTextSTX.TabIndex = 3;
			// 
			// FileInfoWRDMap
			// 
			FileInfoWRDMap.Location = new Point(632, 166);
			FileInfoWRDMap.Name = "FileInfoWRDMap";
			FileInfoWRDMap.Size = new Size(304, 71);
			FileInfoWRDMap.TabIndex = 4;
			// 
			// FileInfoSubroutineArchive
			// 
			FileInfoSubroutineArchive.Location = new Point(322, 12);
			FileInfoSubroutineArchive.Name = "FileInfoSubroutineArchive";
			FileInfoSubroutineArchive.Size = new Size(304, 71);
			FileInfoSubroutineArchive.TabIndex = 5;
			// 
			// FileInfoSubroutineWRD
			// 
			FileInfoSubroutineWRD.Location = new Point(12, 89);
			FileInfoSubroutineWRD.Name = "FileInfoSubroutineWRD";
			FileInfoSubroutineWRD.Size = new Size(304, 71);
			FileInfoSubroutineWRD.TabIndex = 6;
			// 
			// FileInfoSubroutineTextArchive
			// 
			FileInfoSubroutineTextArchive.Location = new Point(634, 89);
			FileInfoSubroutineTextArchive.Name = "FileInfoSubroutineTextArchive";
			FileInfoSubroutineTextArchive.Size = new Size(304, 71);
			FileInfoSubroutineTextArchive.TabIndex = 7;
			// 
			// FileInfoSubroutineSTX
			// 
			FileInfoSubroutineSTX.Location = new Point(322, 166);
			FileInfoSubroutineSTX.Name = "FileInfoSubroutineSTX";
			FileInfoSubroutineSTX.Size = new Size(304, 71);
			FileInfoSubroutineSTX.TabIndex = 8;
			// 
			// SetButton
			// 
			SetButton.Location = new Point(788, 291);
			SetButton.Name = "SetButton";
			SetButton.Size = new Size(150, 23);
			SetButton.TabIndex = 9;
			SetButton.Text = "Set";
			SetButton.UseVisualStyleBackColor = true;
			SetButton.Click += SetButton_Click;
			// 
			// ResetButton
			// 
			ResetButton.Location = new Point(632, 291);
			ResetButton.Name = "ResetButton";
			ResetButton.Size = new Size(150, 23);
			ResetButton.TabIndex = 10;
			ResetButton.Text = "Reset";
			ResetButton.UseVisualStyleBackColor = true;
			ResetButton.Click += ResetButton_Click;
			// 
			// CurrentLine
			// 
			CurrentLine.Location = new Point(12, 243);
			CurrentLine.Name = "CurrentLine";
			CurrentLine.Size = new Size(304, 71);
			CurrentLine.TabIndex = 11;
			// 
			// FilesEditor
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(944, 320);
			Controls.Add(CurrentLine);
			Controls.Add(ResetButton);
			Controls.Add(SetButton);
			Controls.Add(FileInfoSubroutineSTX);
			Controls.Add(FileInfoSubroutineTextArchive);
			Controls.Add(FileInfoSubroutineWRD);
			Controls.Add(FileInfoSubroutineArchive);
			Controls.Add(FileInfoWRDMap);
			Controls.Add(FileInfoTextSTX);
			Controls.Add(FileInfoTextArchive);
			Controls.Add(FileInfoWRDFile);
			Controls.Add(FileInfoWRDArchive);
			Name = "FilesEditor";
			Text = "FilesEditor";
			ResumeLayout(false);
		}

		#endregion

		public Templates.FileForm FileInfoWRDArchive;
		public Templates.FileForm FileInfoWRDFile;
		public Templates.FileForm FileInfoTextArchive;
		public Templates.FileForm FileInfoTextSTX;
		public Templates.FileForm FileInfoWRDMap;
		public Templates.FileForm FileInfoSubroutineArchive;
		public Templates.FileForm FileInfoSubroutineWRD;
		public Templates.FileForm FileInfoSubroutineTextArchive;
		public Templates.FileForm FileInfoSubroutineSTX;
		public Button SetButton;
		public Button ResetButton;
		public Templates.FileForm CurrentLine;
	}
}