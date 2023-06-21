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
		public void GenerateAllView(string file)
		{
			List<string> blacklist = new List<string>()
			{
				"beforetext",
			};

			//Savefile sv2 = ReadSave(file);
			Savefile sv2 = new Savefile();
			var members = sv2.GetType().GetMembers();
			foreach (var member in members)
			{
				if (member.MemberType != MemberTypes.Field)
				{
					continue;
				}
				if (member.Name.ToLowerInvariant().StartsWith("unk") || member.Name.ToLowerInvariant().StartsWith("todo"))
				{
					continue;
				}
				if (blacklist.Contains(member.Name.ToLowerInvariant()))
				{
					continue;
				}
				string type = GetTypeByName(member.Name);
				if (type.ToLowerInvariant() == "unknown")
				{
					type = GetTypeByLength(member);
				}
				GenerateStringView(member.Name, member.Name, type);
			}
		}

		private void GenerateStringView(string var_name, string read_name, string type)
		{
			Console.WriteLine("ViewValue(this." + var_name + ", \"" + read_name + "\", \"" + type + "\");");
		}
	}
}
