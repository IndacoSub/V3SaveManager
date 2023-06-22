using System.Diagnostics;
using System.Globalization;
using V3SaveManagerGUI.Editors;

namespace V3SaveManagerGUI
{
	public partial class Main : Form
	{
		string FilePath = "DefaultFilePath";
		bool LoadedFile = false;
		V3SaveManager.Savefile CurrentSaveFile = null;

		enum Editable
		{
			None,
			Coins,
			Playtime,
			Flag,
			Level_and_EXP,
			Map,
			Character_Statuses,
			Files,
			Audio,
			Num,
		}

		public Main()
		{
			InitializeComponent();
			CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
			LoadCombobox();
		}

		private void LoadCombobox()
		{
			this.EditListComboBox.Items.Clear();

			for (int i = (int)Editable.None + 0; i < (int)Editable.Num; i++)
			{
				string item = ((Editable)i).ToString();
				item = item.Replace("_", " ");
				this.EditListComboBox.Items.Add(item);
			}
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
				case Editable.Coins:
					EditMonocoins();
					break;
				case Editable.Playtime:
					EditPlaytime();
					break;
				case Editable.Flag:
					EditFlags();
					break;
				case Editable.Level_and_EXP:
					EditLevelAndEXP();
					break;
				case Editable.Map:
					EditMap();
					break;
				case Editable.Character_Statuses:
					EditDeaths();
					break;
				case Editable.Files:
					EditFiles();
					break;
				case Editable.Audio:
					EditAudio();
					break;
				default:
					break;
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
			OpenReminderLabel.Text = "File: " + Path.GetFileName(openFileDialog.FileName);
		}
	}
}