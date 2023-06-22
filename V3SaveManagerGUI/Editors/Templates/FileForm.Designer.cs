namespace V3SaveManagerGUI.Editors.Templates
{
	partial class FileForm
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
			FieldNameLabel = new Label();
			NewValueLabel = new Label();
			NewValueTextbox = new TextBox();
			CurrentValueLabel = new Label();
			SuspendLayout();
			// 
			// FieldNameLabel
			// 
			FieldNameLabel.AutoSize = true;
			FieldNameLabel.Location = new Point(3, 0);
			FieldNameLabel.Name = "FieldNameLabel";
			FieldNameLabel.Size = new Size(42, 15);
			FieldNameLabel.TabIndex = 0;
			FieldNameLabel.Text = "Name:";
			// 
			// NewValueLabel
			// 
			NewValueLabel.AutoSize = true;
			NewValueLabel.Location = new Point(7, 45);
			NewValueLabel.Name = "NewValueLabel";
			NewValueLabel.Size = new Size(65, 15);
			NewValueLabel.TabIndex = 1;
			NewValueLabel.Text = "New Value:";
			// 
			// NewValueTextbox
			// 
			NewValueTextbox.Location = new Point(86, 42);
			NewValueTextbox.Name = "NewValueTextbox";
			NewValueTextbox.Size = new Size(212, 23);
			NewValueTextbox.TabIndex = 2;
			// 
			// CurrentValueLabel
			// 
			CurrentValueLabel.AutoSize = true;
			CurrentValueLabel.Location = new Point(3, 24);
			CurrentValueLabel.Name = "CurrentValueLabel";
			CurrentValueLabel.Size = new Size(81, 15);
			CurrentValueLabel.TabIndex = 3;
			CurrentValueLabel.Text = "Current Value:";
			// 
			// FileForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(CurrentValueLabel);
			Controls.Add(NewValueTextbox);
			Controls.Add(NewValueLabel);
			Controls.Add(FieldNameLabel);
			Name = "FileForm";
			Size = new Size(304, 71);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		public Label FieldNameLabel;
		public Label NewValueLabel;
		public TextBox NewValueTextbox;
		public Label CurrentValueLabel;
	}
}
