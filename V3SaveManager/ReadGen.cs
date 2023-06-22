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
		public void GenerateAllRead()
		{
			var members = this.GetType().GetMembers();
			foreach (var member in members)
			{
				if (member.MemberType != MemberTypes.Field)
				{
					continue;
				}
				GenerateStringRead(member.Name);
			}
		}

		private void GenerateStringRead(string var_name)
		{
			Console.WriteLine("sv." + var_name + " = br.ReadBytes(sv." + var_name + ".Length);");
		}
	}
}
