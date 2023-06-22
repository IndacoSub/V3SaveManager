using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace V3SaveManagerGUI.Editors.Templates
{
	public partial class FileForm : UserControl
	{
		public FileForm()
		{
			InitializeComponent();
		}

		public void LoadInfo(string name, string value)
		{
			this.FieldNameLabel.Text = name;
			this.CurrentValueLabel.Text = "Current value: " + value;
			this.NewValueTextbox.Text = value;
		}
	}
}
