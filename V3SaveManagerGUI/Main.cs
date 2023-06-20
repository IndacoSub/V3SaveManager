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

		public Main()
		{
			InitializeComponent();
			CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
			LoadCombobox();
		}

		enum Editable
		{
			None,
			Monocoins,
			Playtime,
			Flags,
			Level_and_EXP,
			Map,
			Num,
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
				case Editable.Monocoins:
					EditMonocoins();
					break;
				case Editable.Playtime:
					EditPlaytime();
					break;
				case Editable.Flags:
					EditFlags();
					break;
				case Editable.Level_and_EXP:
					EditLevelAndEXP();
					break;
				case Editable.Map:
					EditMap();
					break;
				default:
					break;
			}
		}

		private void EditMap()
		{
			/*
			 * CurrentSaveFile.CurrentMapID = BitConverter.GetBytes((short)131);
			 * CurrentSaveFile.CurrentMapID_Again = BitConverter.GetBytes((short)131);
			*/

			string current_map = BitConverter.ToInt16(CurrentSaveFile.CurrentMapID).ToString();
			string current_map_again = BitConverter.ToInt16(CurrentSaveFile.CurrentMapID_Again).ToString();
			if(current_map != current_map_again)
			{
				Debug.WriteLine("...Invalid map id?");
			}
			string current_position_x = BitConverter.ToSingle(CurrentSaveFile.MapPositionX).ToString();
			current_position_x = Utils.MakeDouble(current_position_x);
			string current_position_y = BitConverter.ToSingle(CurrentSaveFile.MapPositionY).ToString();
			current_position_y = Utils.MakeDouble(current_position_y);
			MapEditor me = new MapEditor();
			me.CurrentMapIDNoLabel.Text = current_map;
			me.CurrentPositionXLabel.Text = "Current Position X: " + current_position_x;
			me.CurrentPositionYLabel.Text = "Current Position Y: " + current_position_y;
			me.NewPositionXTextbox.Text = current_position_x;
			me.NewPositionYTextbox.Text = current_position_y;
			me.LoadListbox();
			var res = me.ShowDialog();
			if(res == DialogResult.OK)
			{
				int selected_map_index = me.PossibleMapsListbox.SelectedIndex;
				if (me.PossibleMapsListbox != null && me.PossibleMapsListbox.Items != null && selected_map_index > -1)
				{
					string selected_entry = me.PossibleMapsListbox.Items[selected_map_index].ToString();
					selected_entry = selected_entry.Substring(0, 5); // ID123 (removes whatever is after the ID)
					selected_entry = selected_entry.Substring(2); // 123 (removes ID)
					CurrentSaveFile.CurrentMapID = BitConverter.GetBytes(short.Parse(selected_entry));
					CurrentSaveFile.CurrentMapID_Again = BitConverter.GetBytes(short.Parse(selected_entry));
				}
				Debug.WriteLine("PosX: " + me.NewPositionXTextbox.Text);
				float posx = float.Parse(me.NewPositionXTextbox.Text);
				float posy = float.Parse(me.NewPositionYTextbox.Text);
				Debug.WriteLine("Parsed PosX: " + posx.ToString());
				CurrentSaveFile.MapPositionX = BitConverter.GetBytes((float)posx);
				CurrentSaveFile.MapPositionY = BitConverter.GetBytes((float)posy);
			}
		}

		private void EditLevelAndEXP()
		{
			string current_level = BitConverter.ToInt32(CurrentSaveFile.CurrentLevel).ToString();
			string current_level_short = BitConverter.ToInt16(CurrentSaveFile.CurrentLevelShort).ToString();
			if (current_level != current_level_short)
			{
				Debug.WriteLine("...Invalid current level?");
			}
			string total_exp = BitConverter.ToInt32(CurrentSaveFile.TotalEXP).ToString();
			LevelEditor le = new LevelEditor();
			le.CurrentLevelLabel.Text = "Current Level: " + current_level;
			le.CurrentTotalEXPLabel.Text = "Current Total EXP: " + total_exp;
			le.NewLevelTextbox.Text = current_level;
			le.NewTotalEXPTextbox.Text = total_exp;
			le.DesiredLevelTextbox.Text = current_level;
			le.CalculateEXP();
			var res = le.ShowDialog();
			if (res == DialogResult.OK)
			{
				CurrentSaveFile.CurrentLevel = BitConverter.GetBytes(int.Parse(le.NewLevelTextbox.Text));
				CurrentSaveFile.CurrentLevelShort = BitConverter.GetBytes(short.Parse(le.NewLevelTextbox.Text));
				CurrentSaveFile.TotalEXP = BitConverter.GetBytes(int.Parse(le.NewTotalEXPTextbox.Text));
			}
		}

		private void EditFlags()
		{
			string clear_game = BitConverter.ToInt16(CurrentSaveFile.GameClearFlg).ToString();
			FlagEditor fe = new FlagEditor();
			fe.CurrentGameClearFlgLabel.Text = "Current GameClearFlg: " + (clear_game == "1" ? "On" : "Off");
			fe.GameClearFlgCheckbox.Checked = clear_game == "1";
			var res = fe.ShowDialog();
			if (res == DialogResult.OK)
			{
				CurrentSaveFile.GameClearFlg = BitConverter.GetBytes((short)(fe.GameClearFlgCheckbox.Checked ? 1 : 0));
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

		private void EditPlaytime()
		{
			const int framerate = 60;
			long frames_count = BitConverter.ToInt64(CurrentSaveFile.PlayTime);
			long seconds = 0;
			long hours = 0;
			long minutes = 0;
			long frames = frames_count;
			string current = frames_count.ToString();
			PlaytimeEditor pe = new PlaytimeEditor();
			pe.CurrentPlaytimeLabel.Text = "Current playtime (in frames):\n" + current;
			pe.NewPlaytimeTextbox.Text = current;
			while (frames >= framerate)
			{
				seconds++;
				frames -= framerate;
			}
			while (seconds >= 60)
			{
				minutes++;
				seconds -= 60;
			}
			while (minutes >= 60)
			{
				hours++;
				minutes -= 60;
			}

			pe.HoursTextbox.Text = hours.ToString();
			pe.MinutesTextbox.Text = minutes.ToString();
			pe.SecondsTextbox.Text = seconds.ToString();
			pe.FramesTextbox.Text = frames.ToString();

			var res = pe.ShowDialog();
			if (res == DialogResult.OK)
			{
				CurrentSaveFile.PlayTime = BitConverter.GetBytes(long.Parse(pe.NewPlaytimeTextbox.Text));
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