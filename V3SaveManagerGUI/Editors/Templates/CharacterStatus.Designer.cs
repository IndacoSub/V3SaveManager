namespace V3SaveManagerGUI.Editors.Templates
{
	partial class CharacterStatus
	{
		/// <summary> 
		/// Variabile di progettazione necessaria.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Pulire le risorse in uso.
		/// </summary>
		/// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Codice generato da Progettazione componenti

		/// <summary> 
		/// Metodo necessario per il supporto della finestra di progettazione. Non modificare 
		/// il contenuto del metodo con l'editor di codice.
		/// </summary>
		private void InitializeComponent()
		{
			CharacterNameLabel = new Label();
			CurrentStatusLabel = new Label();
			NewStatusCombobox = new ComboBox();
			NewStatusLabel = new Label();
			SuspendLayout();
			// 
			// CharacterNameLabel
			// 
			CharacterNameLabel.AutoSize = true;
			CharacterNameLabel.Location = new Point(3, 11);
			CharacterNameLabel.Name = "CharacterNameLabel";
			CharacterNameLabel.Size = new Size(61, 15);
			CharacterNameLabel.TabIndex = 0;
			CharacterNameLabel.Text = "Character:";
			// 
			// CurrentStatusLabel
			// 
			CurrentStatusLabel.AutoSize = true;
			CurrentStatusLabel.Location = new Point(3, 35);
			CurrentStatusLabel.Name = "CurrentStatusLabel";
			CurrentStatusLabel.Size = new Size(85, 15);
			CurrentStatusLabel.TabIndex = 1;
			CurrentStatusLabel.Text = "Current Status:";
			// 
			// NewStatusCombobox
			// 
			NewStatusCombobox.DropDownStyle = ComboBoxStyle.DropDownList;
			NewStatusCombobox.FormattingEnabled = true;
			NewStatusCombobox.Items.AddRange(new object[] { "Alive", "Dead", "Unknown" });
			NewStatusCombobox.Location = new Point(78, 61);
			NewStatusCombobox.Name = "NewStatusCombobox";
			NewStatusCombobox.Size = new Size(121, 23);
			NewStatusCombobox.TabIndex = 2;
			// 
			// NewStatusLabel
			// 
			NewStatusLabel.AutoSize = true;
			NewStatusLabel.Location = new Point(3, 64);
			NewStatusLabel.Name = "NewStatusLabel";
			NewStatusLabel.Size = new Size(69, 15);
			NewStatusLabel.TabIndex = 3;
			NewStatusLabel.Text = "New Status:";
			// 
			// CharacterStatus
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(NewStatusLabel);
			Controls.Add(NewStatusCombobox);
			Controls.Add(CurrentStatusLabel);
			Controls.Add(CharacterNameLabel);
			Name = "CharacterStatus";
			Size = new Size(205, 89);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		public Label CharacterNameLabel;
		public Label CurrentStatusLabel;
		public ComboBox NewStatusCombobox;
		public Label NewStatusLabel;
	}
}
