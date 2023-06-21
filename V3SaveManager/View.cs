using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace V3SaveManager
{
	public partial class Savefile
	{
		public void NewViewSave()
		{
			// NOTE: MIGHT NOT BE ACCURATE
			// STICK WITH THE "DOCUMENTATION" IN SAVEFILE.CS

			ViewValue(this.GameName, "GameName", "string");
			ViewValue(this.ChapterName, "ChapterName", "string");
			ViewValue(this.SaveDetails, "SaveDetails", "string");
			ViewValue(this.NewSaveData, "NewSaveData", "string");
			ViewValue(this.Monocoins, "Monocoins", "short");
			ViewValue(this.PlayTime, "PlayTime", "long");
			ViewValue(this.BGMVolume, "BGMVolume", "short");
			ViewValue(this.SFXVolume, "SFXVolume", "short");
			ViewValue(this.VoicesVolume, "VoicesVolume", "short");
			ViewValue(this.ReticleSpeed, "ReticleSpeed", "short");
			ViewValue(this.MovementBob, "MovementBob", "Boolean");
			ViewValue(this.Controls, "Controls", "Boolean");
			ViewValue(this.LookInvert, "LookInvert", "Boolean");
			ViewValue(this.TextSpeed, "TextSpeed", "short");
			ViewValue(this.MapDisplay, "MapDisplay", "short");
			ViewValue(this.Setting1, "Setting1", "short");
			ViewValue(this.LogicDifficulty, "LogicDifficulty", "short");
			ViewValue(this.ActionDifficulty, "ActionDifficulty", "short");
			ViewValue(this.IsContinueEnabled, "IsContinueEnabled", "short");
			ViewValue(this.GameClearFlg, "GameClearFlg", "short");
			ViewValue(this.CurrentLevel, "CurrentLevel", "int");
			ViewValue(this.TotalEXP, "TotalEXP", "int");
			ViewValue(this.CurrentLevelShort, "CurrentLevelShort", "short");
			ViewValue(this.MapPositionX, "MapPositionX", "Float");
			ViewValue(this.MapPositionY, "MapPositionY", "Float");
			ViewValue(this.IsStatusAlive, "IsStatusAlive", "ByteString");
			ViewValue(this.IsStatusUnknown, "IsStatusUnknown", "ByteString");
			ViewValue(this.MonopadTheme, "MonopadTheme", "int");
			ViewValue(this.BacklogText, "BacklogText", "BeginString");
			ViewValue(this.CasinoCoins, "CasinoCoins", "int");
			ViewValue(this.GDeathCardMachine, "GDeathCardMachine", "int");
			ViewValue(this.WRDArchive, "WRDArchive", "string");
			ViewValue(this.SubroutineArchive, "SubroutineArchive", "string");
			ViewValue(this.WRDFile, "WRDFile", "string");
			ViewValue(this.SubroutineWRD, "SubroutineWRD", "string");
			ViewValue(this.TextArchive, "TextArchive", "string");
			ViewValue(this.SubroutineTextArchive, "SubroutineTextArchive", "string");
			ViewValue(this.TextSTX, "TextSTX", "string");
			ViewValue(this.SubroutineSTX, "SubroutineSTX", "string");
			ViewValue(this.WRDMap, "WRDMap", "string");
			ViewValue(this.CurrentMapID, "CurrentMapID", "short");
			ViewValue(this.BGMIndex, "BGMIndex", "int");
			ViewValue(this.CurrentBGM, "CurrentBGM", "string");
			ViewValue(this.CurrentSFX, "CurrentSFX", "string");
			ViewValue(this.CurrentLine, "CurrentLine", "int");
			ViewValue(this.Voiceline1, "Voiceline1", "string");
			ViewValue(this.Voiceline2, "Voiceline2", "string");
			ViewValue(this.Voiceline3, "Voiceline3", "string");
			ViewValue(this.Voiceline4, "Voiceline4", "string");
			ViewValue(this.Voiceline5, "Voiceline5", "string");
			ViewValue(this.Voiceline6, "Voiceline6", "string");
			ViewValue(this.Voiceline7, "Voiceline7", "string");
			ViewValue(this.Voiceline8, "Voiceline8", "string");
			ViewValue(this.Voiceline9, "Voiceline9", "string");
			ViewValue(this.Voiceline10, "Voiceline10", "string");
			ViewValue(this.Voiceline11, "Voiceline11", "string");
			ViewValue(this.Voiceline12, "Voiceline12", "string");
			ViewValue(this.Voiceline13, "Voiceline13", "string");
			ViewValue(this.Voiceline14, "Voiceline14", "string");
			ViewValue(this.Voiceline15, "Voiceline15", "string");
			ViewValue(this.Voiceline16, "Voiceline16", "string");
			ViewValue(this.CurrentMapID_Again, "CurrentMapID_Again", "short");
			ViewValue(this.VoiceLanguageEnglish, "VoiceLanguageEnglish", "Boolean");
			ViewValue(this.Setting2, "Setting2", "int");
			ViewValue(this.InvertX, "InvertX", "int");
			ViewValue(this.DateFormat, "DateFormat", "int");
			ViewValue(this.LastBytes, "LastBytes", "int");
		}

		private void ViewValue(byte[] value, string name, string type)
		{
			if(type == null)
			{
				Console.WriteLine("Type is null!");
				return;
			}

			const bool include_info = false;
			if (include_info)
			{
				Console.Write("[" + type + " | " + value.Length + " | 0x" + value.Length.ToString("X") + "] ");
			}

			switch(type.ToLowerInvariant())
			{
				case "boolean":
				case "bool":
					Assert(value.Length == sizeof(Boolean) && value.Length == sizeof(bool), name + " is NOT a bool");
					Console.WriteLine(name + ": " + BitConverter.ToBoolean(value));
					break;
				case "int16":
				case "short":
					Assert(value.Length == sizeof(Int16) && value.Length == sizeof(short), name + " is NOT an int16");
					Console.WriteLine(name + ": " + BitConverter.ToInt16(value));
					break;
				case "int32":
				case "int":
					Assert(value.Length == sizeof(Int32) && value.Length == sizeof(int), name + " is NOT an int32");
					Console.WriteLine(name + ": " + BitConverter.ToInt32(value));
					break;
				case "long":
				case "int64":
					Assert(value.Length == sizeof(Int64) && value.Length == sizeof(long), name + " is NOT a long");
					Console.WriteLine(name + ": " + BitConverter.ToInt64(value));
					break;
				case "bytestring":
					{
						int v_int = BitConverter.ToInt32(value);
						string v = Convert.ToString(v_int, 2);
						Console.WriteLine(name + ": " + v);
					}
					break;
				case "beginstring":
					Console.WriteLine(name + ": " + GetBeginningOfString(BytesToString(value)));
					break;
				case "endstring":
					Console.WriteLine(name + ": " + GetEndOfString(BytesToString(value)));
					break;
				case "string":
					Console.WriteLine(name + ": " + BytesToString(value, true, true));
					break;
				case "float":
					Console.WriteLine(name + ": " + BitConverter.ToSingle(value));
					break;
				case "double":
					Console.WriteLine(name + ": " + BitConverter.ToDouble(value));
					break;
				default:
					Assert(false, "Unknown type: " + type);
					break;
			}
		}
	}
}
