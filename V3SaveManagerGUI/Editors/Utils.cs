using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V3SaveManagerGUI.Editors
{
	internal class Utils
	{
		public static bool IsValidNumber(string str, bool allow_negative)
		{
			string accepted = "0123456789";
			if (allow_negative)
			{
				accepted += "-";
			}

			bool only_accepted = str.All(x => accepted.Contains(x));

			if (!only_accepted)
			{
				return false;
			}

			if (allow_negative)
			{
				bool contains_minus = str.Contains('-');

				if(contains_minus)
				{
					bool minus_in_front = str.StartsWith('-');
					bool one_minus = str.IndexOf('-') == str.LastIndexOf('-');

					if (!minus_in_front)
					{
						return false;
					}

					if (!one_minus)
					{
						return false;
					}
				}
			}

			return true;
		}
	}
}
