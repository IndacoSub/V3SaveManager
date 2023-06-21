using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V3SaveManager
{
	public partial class Savefile
	{
		public byte[] GameName = new byte[0x80];                // Localized? game-name at 0x0, variable length?
		public byte[] ChapterName = new byte[0x80];             // Localized chapter name at 0x80, variable length?
		public byte[] SaveDetails = new byte[0x400];            // Localized monocoins, number and gametime at 0x100, variable length?
		public byte[] NewSaveData = new byte[0x80];             // Localized "new save data" at 0x500
		public byte[] Unk1 = new byte[4];                       // 0x580
		public byte[] Unk2 = new byte[4];                       // 0x584
		public byte[] Unk3 = new byte[4];                       // 0x588
		public byte[] Unk4 = new byte[4];                       // 0x58C
		public byte[] Monocoins = new byte[2];                  // 0x590
		public byte[] Unk5 = new byte[2];                       // 0x592
		public byte[] Unk6 = new byte[4];                       // 0x594
		public byte[] Unk7 = new byte[4];                       // 0x598
		public byte[] Unk8 = new byte[4];                       // 0x59C
		public byte[] PlayTime = new byte[8];                   // Playtime in frames -- 0x5A0 -> 0x5A7
		public byte[] Unk9 = new byte[8];                       // 0x5A8 -> 0x5AF

		public byte[] ToDo1 = new byte[0x28];                   // 0x5B0 -> 0x5D7

		public byte[] BGMVolume = new byte[2];                  // 0x5D8
		public byte[] SFXVolume = new byte[2];                  // 0x5DA
		public byte[] VoicesVolume = new byte[2];               // 0x5DC
		public byte[] ReticleSpeed = new byte[2];               // 0x5DE
		public byte[] MovementBob = new byte[2];                // 0x5E0
		public byte[] Controls = new byte[2];                   // 0x5E2
		public byte[] LookInvert = new byte[2];                 // 0x5E4
		public byte[] TextSpeed = new byte[2];                  // 0x5E6
		public byte[] MapDisplay = new byte[2];                 // 0x5E8
		public byte[] Setting1 = new byte[2];                   // 0x5EA
		public byte[] LogicDifficulty = new byte[2];            // 0x5EC
		public byte[] ActionDifficulty = new byte[2];           // 0x5EE
		public byte[] IsContinueEnabled = new byte[2];          // 0x5F0
		public byte[] GameClearFlg = new byte[2];               // 0x5F2

		public byte[] ToDo2 = new byte[0x238C];                 // 0x5F4 -> 0x2980

		public byte[] Unk10 = new byte[4];                      // 0x2980
		public byte[] CurrentLevel = new byte[4];               // 0x2984
		public byte[] TotalEXP = new byte[4];                   // 0x2988
		public byte[] Unk11 = new byte[4];                      // 0x298C
		public byte[] Unk12 = new byte[4];                      // 0x2990
		public byte[] CurrentLevelShort = new byte[2];          // 0x2994
		public byte[] Unk13 = new byte[2];                      // 0x2996
		public byte[] Unk14 = new byte[0x21C];                  // 0x2998 -> 0x2BB3
		public byte[] MapPositionX = new byte[4];               // 0x2BB4 -- float?
		public byte[] MapPositionY = new byte[4];               // 0x2BB8 -- float?
		public byte[] Unk15 = new byte[4];                      // 0x2BBC

		public byte[] ToDo3 = new byte[0x784];                  // 0x2BC0 -> 0x3443

		public byte[] IsStatusAlive = new byte[4];              // 0x3344, 1 bit per character, 16 remaining bits for future-proofing
		public byte[] IsStatusUnknown = new byte[4];            // 0x3348, 1 bit per character, 16 remaining bits for future-proofing

		public byte[] ToDo4 = new byte[0xF0];                   // 0x334C -> 343B

		public byte[] MonopadTheme = new byte[4];               // 0x343C
		public byte[] BeforeText = new byte[0xC];               // 0x3440?
		public byte[] BacklogText = new byte[0x4000];           // 0x344C -> 0x744B

		public byte[] ToDo5 = new byte[0x400C];                 // 0x744C

		public byte[] CasinoCoins = new byte[4];                // 0xB458
		public byte[] Unk16 = new byte[0xE0];                   // 0xB45C -> 0xB53B
		public byte[] GDeathCardMachine = new byte[4];          // 0xB53C
		public byte[] Unk17 = new byte[0x20];                   // 0xB53D -> 0xB560
		public byte[] Unk18 = new byte[0x5DD0];                 // 0xB560 -> 0x11330
		public byte[] Unk19 = new byte[0x8CD0];                 // 0x11330 -> 0x1A000
		public byte[] Unk20 = new byte[0x3F0];                  // 0x1A000 -> 0x1A3F0
		public byte[] Unk21 = new byte[0x1410];                 // 0x1A3F0 -> 0x1B800
		public byte[] Unk22 = new byte[0xFD30];                 // 0x1B800 -> 0x2B530
		public byte[] Unk23 = new byte[0x180];                  // 0x2B530 -> 0x2B6B0
		public byte[] Unk24 = new byte[0xE90];                  // 0x2B6B0 -> 0x2C540
		public byte[] Unk25 = new byte[0x1AE0];                 // 0x2C540 -> 0x2E020
		public byte[] Unk26 = new byte[0x190];                  // 0x2E020 -> 0x2E1B0
		public byte[] Unk27 = new byte[0x240];                  // 0x2E1B0 -> 0x2E3F0
		public byte[] Unk28 = new byte[0x2140];                 // 0x2E3F0 -> 0x30530
		public byte[] Unk29 = new byte[4];                      // 0x30530
		public byte[] Unk30 = new byte[4];                      // 0x30534
		public byte[] Unk31 = new byte[4];                      // 0x30538
		public byte[] WRDArchive = new byte[0x40];              // 0x3053C -> 0x3057B
		public byte[] SubroutineArchive = new byte[0x40];       // 0x3057C -> 0x305BB
		public byte[] Unk32 = new byte[0x40];                   // 0x305BC -> 0x305FB
		public byte[] Unk33 = new byte[0x40];                   // 0x305FC -> 0x3063B
		public byte[] Unk34 = new byte[0x40];                   // 0x3063C -> 0x3067B
		public byte[] WRDFile = new byte[0x40];                 // 0x3067C -> 0x306BB
		public byte[] SubroutineWRD = new byte[0x40];           // 0x306BC -> 0x306FB
		public byte[] Unk35 = new byte[0x40];                   // 0x306FC -> 0x3073B
		public byte[] Unk36 = new byte[0x40];                   // 0x3073C -> 0x3077B
		public byte[] Unk37 = new byte[0x40];                   // 0x3077C -> 0x307BB
		public byte[] TextArchive = new byte[0x40];             // 0x307BC -> 0x307FB
		public byte[] SubroutineTextArchive = new byte[0x40];   // 0x307FC -> 0x3083B
		public byte[] Unk38 = new byte[0x40];                   // 0x3083C -> 0x3087B
		public byte[] Unk39 = new byte[0x40];                   // 0x3087C -> 0x308BB
		public byte[] Unk40 = new byte[0x40];                   // 0x308BC -> 0x308FB
		public byte[] TextSTX = new byte[0x40];                 // 0x308FC -> 0x3093B
		public byte[] SubroutineSTX = new byte[0x40];           // 0x3093C -> 0x3097B
		public byte[] Unk41 = new byte[0x40];                   // 0x3097C -> 0x309BB
		public byte[] Unk42 = new byte[0x40];                   // 0x309BC -> 0x309FB
		public byte[] Unk43 = new byte[0x40];                   // 0x309FC -> 0x30A3B
		public byte[] WRDMap = new byte[0x40];                  // 0x30A3C -> 0x30A7B
		public byte[] Unk44 = new byte[0x100];                  // 0x30A7C -> 0x30B7B
		public byte[] Unk45 = new byte[0x80];                   // 0x30B7C -> 0x30BFB
		public byte[] Unk46 = new byte[0x1004];                 // 0x30BFC -> 0x31BFF
		public byte[] CurrentMapID = new byte[2];				// 0x31C00
		public byte[] Unk47 = new byte[0xE2];                   // 0x31C02 -> 0x31CE3
		public byte[] BGMIndex = new byte[4];                   // 0x31CE4
		public byte[] Unk48 = new byte[2];                      // 0x31CE8
		public byte[] Unk49 = new byte[2];                      // 0x31CEA
		public byte[] CurrentBGM = new byte[0x10];              // 0x31CEC -> 0x31CFB
		public byte[] Unk50 = new byte[0x18];                   // 0x31CFC -> 0x31D13
		public byte[] CurrentSFX = new byte[0x10];              // 0x31D14 -> 0x31D23
		public byte[] Unk51 = new byte[0x30];                   // 0x31D24 -> 0x31D53
		public byte[] Unk52 = new byte[0x10];                   // 0x31D54 -> 0x31D63
		public byte[] Unk53 = new byte[0x28];                   // 0x31D64 -> 0x31D8B
		public byte[] CurrentLine = new byte[2];                // 0x31D8C
		public byte[] Unk54 = new byte[0x2E2];                  // 0x31D8E -> 0x3206F
		public byte[] Unk55 = new byte[4];                      // 0x32070
		public byte[] Unk56 = new byte[4];                      // 0x32074
		public byte[] Unk57 = new byte[4];                      // 0x32078
		public byte[] Unk58 = new byte[4];                      // 0x3207C
		public byte[] Unk59 = new byte[4];                      // 0x32080
		public byte[] Unk60 = new byte[4];                      // 0x32084
		public byte[] Unk61 = new byte[0xC];                    // 0x32088 -> 0x32093
		public byte[] Voiceline1 = new byte[0x40];              // 0x32094
		public byte[] Voiceline2 = new byte[0x40];              //
		public byte[] Voiceline3 = new byte[0x40];              //
		public byte[] Voiceline4 = new byte[0x40];              //
		public byte[] Voiceline5 = new byte[0x40];              //
		public byte[] Voiceline6 = new byte[0x40];              //
		public byte[] Voiceline7 = new byte[0x40];              //
		public byte[] Voiceline8 = new byte[0x40];              //
		public byte[] Voiceline9 = new byte[0x40];              //
		public byte[] Voiceline10 = new byte[0x40];             //
		public byte[] Voiceline11 = new byte[0x40];             //
		public byte[] Voiceline12 = new byte[0x40];             //
		public byte[] Voiceline13 = new byte[0x40];             //
		public byte[] Voiceline14 = new byte[0x40];             //
		public byte[] Voiceline15 = new byte[0x40];             //
		public byte[] Voiceline16 = new byte[0x40];             // 0x32454
		public byte[] Unk62 = new byte[0x4C];                   // 0x32494 -> 0x324DF
		public byte[] Unk63 = new byte[0x10D0];                 // 0x324E0 -> 0x335AF
		public byte[] Unk64 = new byte[0x2DB0];                 // 0x335B0 -> 0x3635F
		public byte[] CurrentMapID_Again = new byte[2];			// 0x36360
		public byte[] Unk65 = new byte[0x4E];                   // 0x36362 -> 0x363AF
		public byte[] Unk66 = new byte[0x270];                  // 0x363B0 -> 0x3661F
		public byte[] Unk67 = new byte[0xCE58];                 // 0x36620 -> 0x43478
		public byte[] VoiceLanguageEnglish = new byte[4];		// 0x43478
		public byte[] Setting2 = new byte[4];                   // 0x4347C
		public byte[] InvertX = new byte[4];                    // 0x43480
		public byte[] DateFormat = new byte[4];                 // 0x43484
		public byte[] Unk68 = new byte[4];                      // 0x43488
		public byte[] Unk69 = new byte[4];                      // 0x4348C
		public byte[] Unk70 = new byte[0xF54];                  // 0x43490 -> 0x443E3
		public byte[] LastBytes = new byte[4];                  // 0x443E4
	};
}
