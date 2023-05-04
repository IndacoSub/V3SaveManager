using System;
using System.Text;

namespace V3SaveManager // Note: actual namespace depends on the project name.
{

	// Many thanks to the Danganronpa "Spiral Framework" modding community
	// Special thanks to Ziella

	public partial class Savefile
	{
		// Make a backup before using this function
		public void EditSave()
		{
			// Insert your changes here!
			// Examples below: 
			// this.Monocoins = BitConverter.GetBytes((short)999);
			// this.ChapterName = StringToBytes(InsertBeginning(BytesToString(this.ChapterName), "[E] ", false), this.ChapterName.Length);
			// this.TextSTX = StringToBytes("c05_001_018.stx", this.TextSTX.Length);
			// this.CurrentLevel = BitConverter.GetBytes(99);
			// this.CurrentLevelShort = BitConverter.GetBytes((short)99);
			// this.TotalEXP = BitConverter.GetBytes(200000);
			// this.CurrentLine = BitConverter.GetBytes(6);
			// this.MapPositionX = BitConverter.GetBytes((float)100.0);
			// this.MapPositionY = BitConverter.GetBytes((float)100.0);
		}
	}

	internal class Program
    {

        static void Main(string[] args)
        {
			string file = "";
#if DEBUG
			file = ""; /* Put .dat path and .dat file, ex. C:\Games\V3\Savedata\SAVE-DATA00.dat */
#else
			if(args == null || args.Length <= 0)
            {
                Console.WriteLine("No arguments found!");
                return;
            }

			file = args[0];
#endif

			Console.WriteLine("Reading: " + file);

            string path = Directory.GetParent(file).FullName;
            string newfile = Path.Combine(path, "mysave.dat");

            Savefile sv = Savefile.ReadSave(file);

            Console.WriteLine("File red!");
            Console.WriteLine();

            sv.ViewSave();
            sv.EditSave();
            sv.WriteSave(newfile);
        }
	}
}