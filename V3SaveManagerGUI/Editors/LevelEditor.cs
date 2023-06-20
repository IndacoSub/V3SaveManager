using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace V3SaveManagerGUI.Editors
{
	public partial class LevelEditor : Form
	{
		public LevelEditor()
		{
			InitializeComponent();
		}

		private void SetButton_Click(object sender, EventArgs e)
		{
			bool valid_level = Utils.IsValidNumber(this.NewLevelTextbox.Text, true, false);
			bool valid_exp = Utils.IsValidNumber(this.NewTotalEXPTextbox.Text, true, false);

			if (!valid_level || !valid_exp)
			{
				return;
			}

			DialogResult = DialogResult.OK;
			this.Close();
		}

		private void CalculateButton_Click(object sender, EventArgs e)
		{
			CalculateEXP();
		}

		public void CalculateEXP()
		{
			bool valid_number = Utils.IsValidNumber(DesiredLevelTextbox.Text, false, false);
			if (!valid_number)
			{
				DesiredLevelTextbox.Text = "";
				return;
			}

			int value = int.Parse(DesiredLevelTextbox.Text);

			if (value <= 0)
			{
				DesiredLevelTextbox.Text = "";
				return;
			}

			var list = GetNecessaryEXPForLevels();
			int exp = list[value];
			this.ResultTextbox.Text = exp.ToString();
		}

		private List<int> GetNecessaryEXPForLevels()
		{
			// From game_resident/Level.dat
			// From 0 to 99
			return new List<int>
			{
				0,			// 0
				0,
				500,
				1000,
				1500,
				2000,
				2500,
				3500,
				4500,
				5500,
				6500,
				7500,
				8500,
				9500,
				10500,
				11500,
				12500,
				13500,
				14500,
				15500,
				16500,
				17500,
				18500,
				19500,
				20500,
				21500,
				22500,
				23500,
				24500,
				25500,
				26500,
				27500,
				28500,
				29500,
				30500,
				31500,
				32500,
				33500,
				34500,
				35500,
				36500,
				37500,
				38500,
				39500,
				40500,
				41500,
				42500,
				43500,
				44500,
				45500,
				46500,
				48500,
				50500,
				52500,
				54500,
				56500,
				58500,
				60500,
				62500,
				64500,
				66500,
				68500,
				70500,
				72500,
				74500,
				76500,
				78500,
				80500,
				82500,
				84500,
				86500,
				88500,
				90500,
				92500,
				94500,
				96500,
				99500,
				102500,
				105500,
				108500,
				111500,
				114500,
				117500,
				120500,
				123500,
				126500,
				129500,
				132500,
				135500,
				138500,
				141500,
				144500,
				147500,
				150500,
				153500,
				156500,
				159500,
				162500,
				165500,
				168500		// 99
			};
		}
	}
}
