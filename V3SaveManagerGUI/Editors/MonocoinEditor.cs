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
	public partial class MonocoinEditor : Form
	{
		public MonocoinEditor()
		{
			InitializeComponent();
		}

		private void SetButton_Click(object sender, EventArgs e)
		{
			string accepted = "0123456789";
			while (this.NewMonocoinsTextbox.Text.StartsWith("0") && this.NewMonocoinsTextbox.Text.Length > 1)
			{
				this.NewMonocoinsTextbox.Text = this.NewMonocoinsTextbox.Text.Remove(0);
			}

			if (this.NewMonocoinsTextbox.Text.Length > 0 && this.NewMonocoinsTextbox.Text.All(x => accepted.Contains(x)))
			{
				DialogResult = DialogResult.OK;
				this.Close();
			}
		}
	}
}
