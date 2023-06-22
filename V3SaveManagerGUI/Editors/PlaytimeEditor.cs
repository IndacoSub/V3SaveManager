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
	public partial class PlaytimeEditor : Form
	{
		public PlaytimeEditor()
		{
			InitializeComponent();
		}

		private void SetButton_Click(object sender, EventArgs e)
		{
			bool valid_new = Utils.IsValidNumber(this.NewPlaytimeTextbox.Text, true, false);
			if (!valid_new)
			{
				return;
			}

			DialogResult = DialogResult.OK;
			this.Close();
		}

		private void CalculateButton_Click(object sender, EventArgs e)
		{

			if (this.HoursTextbox.Text.Length == 0)
			{
				this.HoursTextbox.Text = "0";
			}

			if (this.MinutesTextbox.Text.Length == 0)
			{
				this.MinutesTextbox.Text = "0";
			}

			if (this.SecondsTextbox.Text.Length == 0)
			{
				this.SecondsTextbox.Text = "0";
			}

			if (this.FramesTextbox.Text.Length == 0)
			{
				this.FramesTextbox.Text = "0";
			}

			bool valid_hours = Utils.IsValidNumber(this.HoursTextbox.Text, false, false);
			bool valid_minutes = Utils.IsValidNumber(this.MinutesTextbox.Text, false, false);
			bool valid_seconds = Utils.IsValidNumber(this.SecondsTextbox.Text, false, false);
			bool valid_frames = Utils.IsValidNumber(this.FramesTextbox.Text, false, false);

			if (!valid_hours || !valid_minutes || !valid_seconds || !valid_frames)
			{
				return;
			}

			const int framerate = 60;
			long total_seconds = 0;
			total_seconds += long.Parse(this.SecondsTextbox.Text);
			total_seconds += long.Parse(this.MinutesTextbox.Text) * 60;
			total_seconds += long.Parse(this.HoursTextbox.Text) * 3600;
			long total_frames = long.Parse(this.FramesTextbox.Text);
			total_frames += (total_seconds * framerate);
			this.ResultTextbox.Text = total_frames.ToString();
		}
	}
}
