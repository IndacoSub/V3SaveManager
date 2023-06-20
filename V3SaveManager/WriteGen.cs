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
		public void GenerateAllWrite()
		{
			var members = this.GetType().GetMembers();
			foreach (var member in members)
			{
				if (member.MemberType != MemberTypes.Field)
				{
					continue;
				}
				GenerateStringWrite(member.Name);
			}
		}

		private void GenerateStringWrite(string var_name)
		{
			Console.WriteLine("bw.Write(this." + var_name + ");");
		}
	}
}
