using V3SaveManagerGUI.Editors;

namespace V3SaveManagerGUI
{
	public partial class Main : Form
	{
		string FilePath = "DefaultFilePath";
		bool LoadedFile = false;
		V3SaveManager.Savefile CurrentSaveFile = null;

		public Main()
		{
			InitializeComponent();
			LoadCombobox();
		}

		enum Editable
		{
			None,
			Monocoins,
		}

		private void LoadCombobox()
		{
			this.EditListComboBox.Items.Clear();

			this.EditListComboBox.Items.Add(Editable.None.ToString());
			this.EditListComboBox.Items.Add(Editable.Monocoins.ToString());
		}

		private void EditListComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			// TODO?
		}

		private void EditButton_Click(object sender, EventArgs e)
		{
			if (!LoadedFile)
			{
				return;
			}

			switch ((Editable)this.EditListComboBox.SelectedIndex)
			{
				case Editable.None:
					break;
				case Editable.Monocoins:
					EditMonocoins();
					break;
			}
		}

		private void EditMonocoins()
		{
			string current = BitConverter.ToInt16(CurrentSaveFile.Monocoins).ToString();
			MonocoinEditor me = new MonocoinEditor();
			me.CurrentMonocoinsLabel.Text = "Current Monocoins: " + current;
			me.NewMonocoinsTextbox.Text = current;
			var res = me.ShowDialog();
			if (res == DialogResult.OK)
			{
				CurrentSaveFile.Monocoins = BitConverter.GetBytes(short.Parse(me.NewMonocoinsTextbox.Text));
			}
		}

		private void SaveDatAsButton_Click(object sender, EventArgs e)
		{
			if (!LoadedFile)
			{
				return;
			}

			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "DAT File|*.dat|All files|*.*";
			var res = saveFileDialog.ShowDialog();
			if (res != DialogResult.OK)
			{
				return;
			}
			string filename = saveFileDialog.FileName;
			string filepath = new FileInfo(saveFileDialog.FileName).FullName;

			CurrentSaveFile.WriteSave(filepath);
		}

		private void OpenFileButton_Click(object sender, EventArgs e)
		{

			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "DAT File|*.dat|All files|*.*";
			var res = openFileDialog.ShowDialog();
			if (res != DialogResult.OK)
			{
				return;
			}

			FilePath = new FileInfo(openFileDialog.FileName).FullName;
			CurrentSaveFile = V3SaveManager.Savefile.ReadSave(FilePath);
			LoadedFile = true;
		}
	}
}