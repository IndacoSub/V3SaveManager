using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using V3SaveManagerGUI.Editors;

namespace V3SaveManagerGUI
{
	public partial class Main : Form
	{
		private void EditMap()
		{
			string current_map = BitConverter.ToInt16(CurrentSaveFile.CurrentMapID).ToString();
			string current_map_again = BitConverter.ToInt16(CurrentSaveFile.CurrentMapID_Again).ToString();
			if (current_map != current_map_again)
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
			if (res == DialogResult.OK)
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

		private void EditDeaths()
		{
			DeathEditor de = new DeathEditor();
			DialogResult res = DialogResult.No;
			do
			{
				de = new DeathEditor();

				byte[] alive_bits_bin = CurrentSaveFile.IsStatusAlive;
				byte[] unknown_bits_bin = CurrentSaveFile.IsStatusUnknown;

				string alive_bits = "";
				string unknown_bits = "";

				int alivev_int = BitConverter.ToInt32(alive_bits_bin);
				alive_bits = Convert.ToString(alivev_int, 2);

				int unknownv_int = BitConverter.ToInt32(unknown_bits_bin);
				unknown_bits = Convert.ToString(unknownv_int, 2);

				while (unknown_bits.Length < 32)
				{
					unknown_bits = unknown_bits.Insert(0, "0");
				}

				Debug.WriteLine("Alive bits: " + alive_bits);
				Debug.WriteLine("Unknown bits: " + unknown_bits);

				de.LoadInfo(alive_bits, unknown_bits);
				res = de.ShowDialog();
			} while(res == DialogResult.No);

			if (res == DialogResult.OK)
			{
				string new_alive_str = de.GetAliveString();
				string new_unknown_str = de.GetUnknownString();

				int new_alive_int = Convert.ToInt32(new_alive_str, 2);
				int new_unknown_int = Convert.ToInt32(new_unknown_str, 2);

				CurrentSaveFile.IsStatusAlive = BitConverter.GetBytes((int)new_alive_int);
				CurrentSaveFile.IsStatusUnknown = BitConverter.GetBytes((int)new_unknown_int);
			}
		}
	}
}
