using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using V3SaveManagerGUI.Editors.Templates;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace V3SaveManagerGUI.Editors
{
	public partial class FilesEditor : Form
	{
		public FilesEditor()
		{
			InitializeComponent();
		}

		private List<FileForm> GetAllFiles()
		{
			List<FileForm> ret = new List<FileForm>()
			{
				FileInfoWRDArchive,
				FileInfoSubroutineArchive,
				FileInfoWRDFile,
				FileInfoSubroutineWRD,
				FileInfoTextArchive,
				FileInfoSubroutineTextArchive,
				FileInfoTextSTX,
				FileInfoSubroutineSTX,
				FileInfoWRDMap,
			};

			return ret;
		}

		private List<string> GetFileNames()
		{
			List<string> ret = new List<string>()
			{
				"WRD Archive",
				"Subroutine Archive",
				"WRD File",
				"Subroutine WRD",
				"Text Archive",
				"Subroutine Text Archive",
				"Text STX",
				"Subroutine STX",
				"WRD Map",
			};

			return ret;
		}

		public void LoadInfo(List<string> values)
		{
			var files = GetAllFiles();
			var names = GetFileNames();

			for(int i = 0; i < files.Count; i++)
			{
				files[i].FieldNameLabel.Text = names[i];
				files[i].NewValueTextbox.Text = values[i];
				files[i].CurrentValueLabel.Text = "Current Value: " + values[i];
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
	}
}
