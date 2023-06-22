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
	public partial class AudioEditor : Form
	{
		public AudioEditor()
		{
			InitializeComponent();
		}

		private void SetButton_Click(object sender, EventArgs e)
		{
			bool index_is_number = Utils.IsValidNumber(this.BGMIndex.NewValueTextbox.Text, false, false);

			if (index_is_number)
			{
				DialogResult = DialogResult.OK;
				this.Close();
			}
		}
	}
}
