using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using V3SaveManager;

namespace V3SaveManagerGUI.Editors
{
	public partial class MapEditor : Form
	{
		public MapEditor()
		{
			InitializeComponent();
		}

		private List<string> GetMapList()
		{
			// From game_resident/Map.dat

			List<string> mapList = new List<string>()
			{
				"ID000_dummy",
				"ID001_sch_1F",
				"ID002_sch_2F",
				"ID003_sch_3F",
				"ID004_sch_B1",
				"ID005_sch_4F",
				"ID006_sch_5F",
				"ID007_gym",
				"ID008_gym_magic",
				"ID010_cafe",
				"ID014_shop",
				"ID015_warehousea",
				"ID016_wc",
				"ID017_classRoom_A",
				"ID018_classRoom_B",
				"ID019_libA",
				"ID020_libB",
				"ID021_libA",
				"ID025_gam",
				"ID026_av",
				"ID027_secretRoom_0",
				"ID028_secretRoom_1",
				"ID029_secret",
				"ID030_classRoom_C",
				"ID031_classRoom_C",
				"ID032_cold",
				"ID034_wc",
				"ID036_vacancy_00",
				"ID037_vacancy_01",
				"ID038_vacancy_02",
				"ID040_vacancy_01",
				"ID041_vacancy_01",
				"ID042_sch_1F",
				"ID043_sch_1F",
				"ID044_sch_2F",
				"ID045_sch_6F",
				"ID050_sch_B1",
				"ID051_sch_1F",
				"ID052_sch_1F",
				"ID053_sch_2F",
				"ID054_sch_3F",
				"ID055_sch_4F",
				"ID056_sch_5F",
				"ID058_cave",
				"ID060_warehousea",
				"ID061_classRoom_A",
				"ID062_classRoom_B",
				"ID063_classRoom_C",
				"ID070_sch_1F",
				"ID071_sch_1F",
				"ID072_sch_2F",
				"ID073_sch_3F",
				"ID074_sch_B1",
				"ID076_wc",
				"ID100_residence",
				"ID101_Private_00",
				"ID103_Private_02",
				"ID106_Private_05",
				"ID107_Private_06",
				"ID110_Private_09",
				"ID114_Private_13",
				"ID116_Private_15",
				"ID130_lab_saihara",
				"ID131_lab_momota",
				"ID132_lab_hoshi",
				"ID134_lab_hoshi_shower",
				"ID135_lab_amami",
				"ID136_lab_gokuhara",
				"ID137_lab_oma",
				"ID138_lab_shinguji",
				"ID146_lab_Ki-Bo",
				"ID147_lab_tojo",
				"ID148_lab_yumeno",
				"ID149_lab_yumeno",
				"ID150_lab_harukawa",
				"ID151_lab_chabasira",
				"ID152_lab_shirogane",
				"ID153_lab_yonaga",
				"ID154_lab_iruma",
				"ID155_lab_akamatu",
				"ID156_lab_yonaga_af",
				"ID190_pool",
				"ID191_pool",
				"ID200_computer_A",
				"ID201_computer_B",
				"ID220_hangar",
				"ID222_hangar_wc",
				"ID240_casino_1f",
				"ID241_casino_B1",
				"ID247_sch_side_p",
				"ID248_elv_side_p",
				"ID249_sch_side_0",
				"ID251_elv_side_0",
				"ID252_backyard_c1",
				"ID253_elevator_c1",
				"ID255_sch_side_1",
				"ID256_elv_side_1",
				"ID257_backyard_c2",
				"ID258_sch_side_2",
				"ID259_elv_side_2",
				"ID262_sch_side_3",
				"ID263_elv_side_3",
				"ID266_sch_side_4",
				"ID267_elv_side_4",
				"ID270_sch_side_5",
				"ID271_elv_side_5",
				"ID272_backyard_c6",
				"ID273_elevator_c1",
				"ID281_casino_area_0",
				"ID287_cyber_area_0",
				"ID300_LC_01",
				"ID301_LC_01s",
				"ID308_LC_05",
				"ID309_LC_05s",
				"ID310_LC_06",
				"ID311_LC_06_nico",
				"ID330_LC_01_after",
				"ID334_LC_05_after",
				"ID335_LC_06_before",
				"ID336_LC_06_after",
				"ID353_under",
				"ID354_escapedeck",
				"ID400_loveapart",
				"ID501_sch_1F",
				"ID502_sch_2F",
				"ID504_sch_B1",
				"ID510_cafe",
				"ID515_warehousea",
				"ID516_wc",
				"ID517_classRoom_A",
				"ID542_sch_1F",
				"ID570_sch_1F",
				"ID571_sch_1F",
				"ID572_sch_2F",
				"ID573_sch_3F",
				"ID600_residence",
				"ID601_Private_00",
				"ID616_Private_15",
				"ID655_lab_akamatu",
				"ID749_sch_side_0",
				"ID751_elv_side_0",
				"ID752_backyard_c1",
				"ID753_elevator_c1",
				"ID755_sch_side_1",
				"ID756_elv_side_1",
				"ID757_elevator_c2",
				"ID758_sch_side_2",
				"ID759_elv_side_2",
				"ID762_sch_side_3",
				"ID766_sch_side_4",
				"ID770_sch_side_6",
				"ID771_elv_side_6",
				"ID950_1F",
				"ID951_residence",
				"ID952_classRoom_B",
				"ID953_gym",
				"ID954_Private_15",
				"ID955_showerroom",
				"ID956_cafe",
				"ID957_warehousea",
				"ID958_wc",
				"ID999_dummy",
				"ID960_LC_01_after",
				"ID961_LC_01",
			};

			return mapList;
		}

		private string GetMapByID(int id)
		{
			List<string> maps = GetMapList();
			foreach (string map in maps)
			{
				string str = "ID" + Savefile.PadString(id.ToString(), 3, '0');
				if (map.StartsWith(str))
				{
					return "map";
				}
			}

			return "Unknown";
		}

		public void LoadListbox()
		{
			List<string> blacklist = new List<string>()
			{
				"ID300", // crash
				"ID301", // crash
				"ID308", // crash
				"ID309", // crash
				"ID310", // crash
				"ID311", // crash
				"ID950", // infinite loading
				"ID960", // infinite loading
				"ID961", // crash
			};

			bool ignore_blacklist = false;
			int current_map = int.Parse(this.CurrentMapIDNoLabel.Text);
			string current_id = GetMapByID(current_map);
			PossibleMapsListbox.Items.Clear();
			List<string> maps = GetMapList();
			foreach (string map in maps)
			{
				bool blacklisted = blacklist.Any(x => map.Contains(x));
				if (!blacklisted || ignore_blacklist)
				{
					// Unfortunately listboxes do not support
					// colored text, so it's better to outright
					// remove the "dangerous" maps
					PossibleMapsListbox.Items.Add(map);
				}
			}
			PossibleMapsListbox.SelectedIndex = PossibleMapsListbox.FindString(current_id);
			PossibleMapsListbox.Refresh();
		}

		private void SetButton_Click(object sender, EventArgs e)
		{
			NewPositionXTextbox.Text = Utils.MakeDouble(NewPositionXTextbox.Text);
			NewPositionYTextbox.Text = Utils.MakeDouble(NewPositionYTextbox.Text);

			bool valid_position_x = Utils.IsValidNumber(NewPositionXTextbox.Text, true, true);
			bool valid_position_y = Utils.IsValidNumber(NewPositionYTextbox.Text, true, true);

			if (!valid_position_x || !valid_position_y)
			{
				return;
			}

			DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
