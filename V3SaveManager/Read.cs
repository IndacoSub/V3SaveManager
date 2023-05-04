using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V3SaveManager
{
	public partial class Savefile
	{
		public static Savefile ReadSave(string datfile)
		{
			Savefile sv = new Savefile();

			using (FileStream fs = new FileStream(datfile, FileMode.Open))
			{
				using (BinaryReader br = new BinaryReader(fs))
				{
					sv.GameName = br.ReadBytes(sv.GameName.Length);
					sv.ChapterName = br.ReadBytes(sv.ChapterName.Length);
					sv.SaveDetails = br.ReadBytes(sv.SaveDetails.Length);
					sv.NewSaveData = br.ReadBytes(sv.NewSaveData.Length);
					sv.Unk1 = br.ReadBytes(sv.Unk1.Length);
					sv.Unk2 = br.ReadBytes(sv.Unk2.Length);
					sv.Unk3 = br.ReadBytes(sv.Unk3.Length);
					sv.Unk4 = br.ReadBytes(sv.Unk4.Length);
					sv.Monocoins = br.ReadBytes(sv.Monocoins.Length);
					sv.Unk5 = br.ReadBytes(sv.Unk5.Length);
					sv.Unk6 = br.ReadBytes(sv.Unk6.Length);
					sv.Unk7 = br.ReadBytes(sv.Unk7.Length);
					sv.Unk8 = br.ReadBytes(sv.Unk8.Length);
					sv.PlayTime = br.ReadBytes(sv.PlayTime.Length);
					sv.Unk9 = br.ReadBytes(sv.Unk9.Length);
					sv.ToDo1 = br.ReadBytes(sv.ToDo1.Length);
					sv.BGMVolume = br.ReadBytes(sv.BGMVolume.Length);
					sv.SFXVolume = br.ReadBytes(sv.SFXVolume.Length);
					sv.VoicesVolume = br.ReadBytes(sv.VoicesVolume.Length);
					sv.ReticleSpeed = br.ReadBytes(sv.ReticleSpeed.Length);
					sv.MovementBob = br.ReadBytes(sv.MovementBob.Length);
					sv.Controls = br.ReadBytes(sv.Controls.Length);
					sv.LookInvert = br.ReadBytes(sv.LookInvert.Length);
					sv.TextSpeed = br.ReadBytes(sv.TextSpeed.Length);
					sv.MapDisplay = br.ReadBytes(sv.MapDisplay.Length);
					sv.Setting1 = br.ReadBytes(sv.Setting1.Length);
					sv.LogicDifficulty = br.ReadBytes(sv.LogicDifficulty.Length);
					sv.ActionDifficulty = br.ReadBytes(sv.ActionDifficulty.Length);
					sv.IsContinueEnabled = br.ReadBytes(sv.IsContinueEnabled.Length);
					sv.GameClearFlg = br.ReadBytes(sv.GameClearFlg.Length);
					sv.ToDo2 = br.ReadBytes(sv.ToDo2.Length);
					sv.Unk10 = br.ReadBytes(sv.Unk10.Length);
					sv.CurrentLevel = br.ReadBytes(sv.CurrentLevel.Length);
					sv.TotalEXP = br.ReadBytes(sv.TotalEXP.Length);
					sv.Unk11 = br.ReadBytes(sv.Unk11.Length);
					sv.Unk12 = br.ReadBytes(sv.Unk12.Length);
					sv.CurrentLevelShort = br.ReadBytes(sv.CurrentLevelShort.Length);
					sv.Unk13 = br.ReadBytes(sv.Unk13.Length);
					sv.Unk14 = br.ReadBytes(sv.Unk14.Length);
					sv.MapPositionX = br.ReadBytes(sv.MapPositionX.Length);
					sv.MapPositionY = br.ReadBytes(sv.MapPositionY.Length);
					sv.Unk15 = br.ReadBytes(sv.Unk15.Length);
					sv.ToDo3 = br.ReadBytes(sv.ToDo3.Length);
					sv.IsStatusAlive = br.ReadBytes(sv.IsStatusAlive.Length);
					sv.IsStatusUnknown = br.ReadBytes(sv.IsStatusUnknown.Length);
					sv.ToDo4 = br.ReadBytes(sv.ToDo4.Length);
					sv.MonopadTheme = br.ReadBytes(sv.MonopadTheme.Length);
					sv.BeforeText = br.ReadBytes(sv.BeforeText.Length);
					sv.BacklogText = br.ReadBytes(sv.BacklogText.Length);
					sv.ToDo5 = br.ReadBytes(sv.ToDo5.Length);
					sv.CasinoCoins = br.ReadBytes(sv.CasinoCoins.Length);
					sv.Unk16 = br.ReadBytes(sv.Unk16.Length);
					sv.GDeathCardMachine = br.ReadBytes(sv.GDeathCardMachine.Length);
					sv.Unk17 = br.ReadBytes(sv.Unk17.Length);
					sv.Unk18 = br.ReadBytes(sv.Unk18.Length);
					sv.Unk19 = br.ReadBytes(sv.Unk19.Length);
					sv.Unk20 = br.ReadBytes(sv.Unk20.Length);
					sv.Unk21 = br.ReadBytes(sv.Unk21.Length);
					sv.Unk22 = br.ReadBytes(sv.Unk22.Length);
					sv.Unk23 = br.ReadBytes(sv.Unk23.Length);
					sv.Unk24 = br.ReadBytes(sv.Unk24.Length);
					sv.Unk25 = br.ReadBytes(sv.Unk25.Length);
					sv.Unk26 = br.ReadBytes(sv.Unk26.Length);
					sv.Unk27 = br.ReadBytes(sv.Unk27.Length);
					sv.Unk28 = br.ReadBytes(sv.Unk28.Length);
					sv.Unk29 = br.ReadBytes(sv.Unk29.Length);
					sv.Unk30 = br.ReadBytes(sv.Unk30.Length);
					sv.Unk31 = br.ReadBytes(sv.Unk31.Length);
					sv.WRDArchive = br.ReadBytes(sv.WRDArchive.Length);
					sv.SubroutineArchive = br.ReadBytes(sv.SubroutineArchive.Length);
					sv.Unk32 = br.ReadBytes(sv.Unk32.Length);
					sv.Unk33 = br.ReadBytes(sv.Unk33.Length);
					sv.Unk34 = br.ReadBytes(sv.Unk34.Length);
					sv.WRDFile = br.ReadBytes(sv.WRDFile.Length);
					sv.SubroutineWRD = br.ReadBytes(sv.SubroutineWRD.Length);
					sv.Unk35 = br.ReadBytes(sv.Unk35.Length);
					sv.Unk36 = br.ReadBytes(sv.Unk36.Length);
					sv.Unk37 = br.ReadBytes(sv.Unk37.Length);
					sv.TextArchive = br.ReadBytes(sv.TextArchive.Length);
					sv.SubroutineTextArchive = br.ReadBytes(sv.SubroutineTextArchive.Length);
					sv.Unk38 = br.ReadBytes(sv.Unk38.Length);
					sv.Unk39 = br.ReadBytes(sv.Unk39.Length);
					sv.Unk40 = br.ReadBytes(sv.Unk40.Length);
					sv.TextSTX = br.ReadBytes(sv.TextSTX.Length);
					sv.SubroutineSTX = br.ReadBytes(sv.SubroutineSTX.Length);
					sv.Unk41 = br.ReadBytes(sv.Unk41.Length);
					sv.Unk42 = br.ReadBytes(sv.Unk42.Length);
					sv.Unk43 = br.ReadBytes(sv.Unk43.Length);
					sv.WRDMap = br.ReadBytes(sv.WRDMap.Length);
					sv.Unk44 = br.ReadBytes(sv.Unk44.Length);
					sv.Unk45 = br.ReadBytes(sv.Unk45.Length);
					sv.Unk46 = br.ReadBytes(sv.Unk46.Length);
					sv.Unk47 = br.ReadBytes(sv.Unk47.Length);
					sv.BGMIndex = br.ReadBytes(sv.BGMIndex.Length);
					sv.Unk48 = br.ReadBytes(sv.Unk48.Length);
					sv.Unk49 = br.ReadBytes(sv.Unk49.Length);
					sv.CurrentBGM = br.ReadBytes(sv.CurrentBGM.Length);
					sv.Unk50 = br.ReadBytes(sv.Unk50.Length);
					sv.CurrentSFX = br.ReadBytes(sv.CurrentSFX.Length);
					sv.Unk51 = br.ReadBytes(sv.Unk51.Length);
					sv.Unk52 = br.ReadBytes(sv.Unk52.Length);
					sv.Unk53 = br.ReadBytes(sv.Unk53.Length);
					sv.CurrentLine = br.ReadBytes(sv.CurrentLine.Length);
					sv.Unk54 = br.ReadBytes(sv.Unk54.Length);
					sv.Unk55 = br.ReadBytes(sv.Unk55.Length);
					sv.Unk56 = br.ReadBytes(sv.Unk56.Length);
					sv.Unk57 = br.ReadBytes(sv.Unk57.Length);
					sv.Unk58 = br.ReadBytes(sv.Unk58.Length);
					sv.Unk59 = br.ReadBytes(sv.Unk59.Length);
					sv.Unk60 = br.ReadBytes(sv.Unk60.Length);
					sv.Unk61 = br.ReadBytes(sv.Unk61.Length);
					sv.Voiceline1 = br.ReadBytes(sv.Voiceline1.Length);
					sv.Voiceline2 = br.ReadBytes(sv.Voiceline2.Length);
					sv.Voiceline3 = br.ReadBytes(sv.Voiceline3.Length);
					sv.Voiceline4 = br.ReadBytes(sv.Voiceline4.Length);
					sv.Voiceline5 = br.ReadBytes(sv.Voiceline5.Length);
					sv.Voiceline6 = br.ReadBytes(sv.Voiceline6.Length);
					sv.Voiceline7 = br.ReadBytes(sv.Voiceline7.Length);
					sv.Voiceline8 = br.ReadBytes(sv.Voiceline8.Length);
					sv.Voiceline9 = br.ReadBytes(sv.Voiceline9.Length);
					sv.Voiceline10 = br.ReadBytes(sv.Voiceline10.Length);
					sv.Voiceline11 = br.ReadBytes(sv.Voiceline11.Length);
					sv.Voiceline12 = br.ReadBytes(sv.Voiceline12.Length);
					sv.Voiceline13 = br.ReadBytes(sv.Voiceline13.Length);
					sv.Voiceline14 = br.ReadBytes(sv.Voiceline14.Length);
					sv.Voiceline15 = br.ReadBytes(sv.Voiceline15.Length);
					sv.Voiceline16 = br.ReadBytes(sv.Voiceline16.Length);
#if DEBUG
					//sv.DEBUG_VoiceLine17 = br.ReadBytes(sv.DEBUG_VoiceLine17.Length);
					//sv.DEBUG_VoiceLine18 = br.ReadBytes(sv.DEBUG_VoiceLine18.Length);
					//sv.DEBUG_VoiceLine19 = br.ReadBytes(sv.DEBUG_VoiceLine19.Length);
#endif
					sv.Unk62 = br.ReadBytes(sv.Unk62.Length);
					sv.Unk63 = br.ReadBytes(sv.Unk63.Length);
					sv.Unk64 = br.ReadBytes(sv.Unk64.Length);
					sv.Unk65 = br.ReadBytes(sv.Unk65.Length);
					sv.Unk66 = br.ReadBytes(sv.Unk66.Length);
					sv.Unk67 = br.ReadBytes(sv.Unk67.Length);
					sv.VoiceLanguage = br.ReadBytes(sv.VoiceLanguage.Length);
					sv.Setting2 = br.ReadBytes(sv.Setting2.Length);
					sv.InvertX = br.ReadBytes(sv.InvertX.Length);
					sv.DateFormat = br.ReadBytes(sv.DateFormat.Length);
					sv.Unk68 = br.ReadBytes(sv.Unk68.Length);
					sv.Unk69 = br.ReadBytes(sv.Unk69.Length);
					sv.Unk70 = br.ReadBytes(sv.Unk70.Length);
					sv.LastBytes = br.ReadBytes(sv.LastBytes.Length);
				}
			}

			return sv;
		}
	}
}
