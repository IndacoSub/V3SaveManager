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
	public partial class FlagEditor : Form
	{
		public FlagEditor()
		{
			InitializeComponent();
		}

		private void SetButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
