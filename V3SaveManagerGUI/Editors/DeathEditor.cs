using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using V3SaveManagerGUI.Editors.Templates;

namespace V3SaveManagerGUI.Editors
{
	public partial class DeathEditor : Form
	{
		public DeathEditor()
		{
			InitializeComponent();
		}

		private List<string> GetCharacterNames()
		{
			List<string> ret = new List<string>()
			{
				"Shuichi Saihara",		// 0
				"Kaito Momota",			// 1
				"Ryoma Hoshi",			// 2
				"Rantaro Amami",		// 3
				"Gonta Gokuhara",		// 4
				"Kokichi Oma",			// 5
				"Korekiyo Shinguji",	// 6
				"K1-B0",				// 7
				"Kirumi Tojo",			// 8
				"Himiko Yumeno",		// 9
				"Maki Harukawa",		// 10
				"Tenko Chabashira",		// 11
				"Tsumugi Shirogane",	// 12
				"Angie Yonaga",			// 13
				"Miu Iruma",			// 14
				"Kaede Akamatsu",		// 15
			};

			return ret;
		}

		private List<CharacterStatus> GetCharacterStatuses()
		{
			List<CharacterStatus> ret = new List<CharacterStatus>()
			{

				StatusShuichi,	// 0
				StatusKaito,	// 1
				StatusRyoma,	// 2
				StatusRantaro,	// 3
				StatusGonta,	// 4
				StatusKokichi,	// 5
				StatusKorekiyo,	// 6
				StatusKeebo,	// 7
				StatusKirumi,	// 8
				StatusHimiko,	// 9
				StatusMaki,		// 10
				StatusTenko,	// 11
				StatusTsumugi,	// 12
				StatusAngie,	// 13
				StatusMiu,		// 14
				StatusKaede,	// 15
			};

			return ret;
		}

		public void LoadInfo(string alive_bits, string unknown_bits)
		{
			List<CharacterStatus> statuses = GetCharacterStatuses();
			List<string> names = GetCharacterNames();
			for (int i = 0; i < statuses.Count; i++)
			{
				if (statuses[i] == null)
				{
					statuses[i] = new CharacterStatus();
				}

				statuses[i].CharacterNameLabel.Text = names[i] + ":";
				statuses[i].CurrentStatusLabel.Text = "Current Status: ";

				bool is_dead = alive_bits[(statuses.Count * 2) - 1 - i] == '0';
				if (is_dead)
				{
					statuses[i].CurrentStatusLabel.Text += "Dead";
					statuses[i].NewStatusCombobox.SelectedIndex = statuses[i].NewStatusCombobox.FindString("Dead");
				}
				else
				{
					bool is_missing = unknown_bits[(statuses.Count * 2) - 1 - i] == '1';
					if (is_missing)
					{
						statuses[i].CurrentStatusLabel.Text += "Unknown";
						statuses[i].NewStatusCombobox.SelectedIndex = statuses[i].NewStatusCombobox.FindString("Unknown");
					}
					else
					{
						statuses[i].CurrentStatusLabel.Text += "Alive";
						statuses[i].NewStatusCombobox.SelectedIndex = statuses[i].NewStatusCombobox.FindString("Alive");
					}
				}
			}
		}

		private void SetButton_Click(object sender, EventArgs e)
		{

			DialogResult = DialogResult.OK;
			this.Close();
		}

		private void ResetButton_Click(object sender, EventArgs e)
		{

			DialogResult = DialogResult.No;
			this.Close();
		}

		private string GetStatusString()
		{
			List<CharacterStatus> statuses = GetCharacterStatuses();
			string ret = "";
			foreach (CharacterStatus stat in statuses)
			{
				if (stat.NewStatusCombobox.SelectedIndex == -1)
				{
					Debug.WriteLine("Selected index == -1");
					continue;
				}
				object item = stat.NewStatusCombobox.SelectedItem;
				if (item == null)
				{
					Debug.WriteLine("Item is null!");
					continue;
				}
				string? current_status = item.ToString();
				if (current_status == null)
				{
					Debug.WriteLine("Null item name!");
					continue;
				}

				switch (current_status.ToLowerInvariant())
				{
					case "unknown":
						ret += "2";
						break;
					case "dead":
						ret += "0";
						break;
					case "alive":
						ret += "1";
						break;
					default:
						ret += "3";
						break;
				}
			}

			return ret;
		}

		public string GetAliveString()
		{
			string ret = GetStatusString();

			ret = ret.Replace("3", "0");
			ret = ret.Replace("2", "1"); // Treat unknown as alive? Otherwise replace with "0"

			Debug.WriteLine("Before reverse (alive): " + ret);

			ret = Utils.ReverseString(ret);

			while(ret.Length < 32)
			{
				ret = ret.Insert(0, "1");
			}

			Debug.WriteLine("New alive string: " + ret);

			return ret;
		}

		public string GetUnknownString()
		{
			string ret = GetStatusString();

			ret = ret.Replace("3", "0");
			ret = ret.Replace("1", "0");
			ret = ret.Replace("2", "1");

			Debug.WriteLine("Before reverse (unknown): " + ret);

			ret = Utils.ReverseString(ret);

			while (ret.Length < 32)
			{
				ret = ret.Insert(0, "0");
			}

			Debug.WriteLine("New unknown string: " + ret);

			return ret;
		}
	}
}
