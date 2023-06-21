using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace V3SaveManager
{
	public partial class Savefile
	{
		private long GetArrayLength(MemberInfo member)
		{
			if (member.MemberType == MemberTypes.Field && ((FieldInfo)member).FieldType.IsArray)
			{
				FieldInfo myFieldInfo = (FieldInfo)member;
				// Get value of myArray
				object myArrayObject = myFieldInfo.GetValue(this);
				// Cast value to an array
				byte[] myArray = (byte[])myArrayObject;
				return myArray.Length;
			}

			return 0;
		}

		private static string GetTypeByName(string name)
		{
			Dictionary<string, string> dict = new Dictionary<string, string>() {
				{ "MovementBob", "Boolean" },
				{ "Controls", "Boolean" },
				{ "LookInvert", "Boolean" },
				{ "MapPositionX", "Float" },
				{ "MapPositionY", "Float" },
				{ "BacklogText", "BeginString" },
				{ "VoiceLanguageEnglish", "Boolean" },
				{ "IsStatusAlive", "ByteString" },
				{ "IsStatusUnknown", "ByteString" },
			};

			var first = dict.FirstOrDefault(x => x.Key == name);
			if (first.Key == null || first.Value == null)
			{
				return "Unknown";
			}
			return first.Value;
		}

		private string GetTypeByLength(MemberInfo member)
		{
			// This is just guessing!

			long length = GetArrayLength(member);

			switch (length)
			{
				case 0:
					Assert(false, "Array length for " + member.Name + " is zero!");
					break;
				case 2:
					return "short";
				case 4:
					return "int";
				case 8:
					return "long";
				default:
					return "string";
			}

			return "unknown";
		}
	}
}
