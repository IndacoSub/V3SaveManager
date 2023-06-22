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
		public static bool IsValidNumber(string str, bool allow_negative, bool allow_decimal)
		{
			string accepted = "0123456789";
			if (allow_negative)
			{
				accepted += "-";
			}
			if (allow_decimal)
			{
				accepted += ".";
			}

			bool only_accepted = str.All(x => accepted.Contains(x));

			if (!only_accepted)
			{
				return false;
			}

			if (allow_negative)
			{
				bool contains_minus = str.Contains('-');

				if (contains_minus)
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

			if (allow_decimal)
			{
				bool contains_decimal = str.Contains(".");

				if (contains_decimal)
				{
					bool decimal_in_the_middle = !str.StartsWith(".") && !str.EndsWith(".");
					bool one_decimal = str.IndexOf(".") == str.LastIndexOf(".");

					if (!decimal_in_the_middle)
					{
						return false;
					}

					if (!one_decimal)
					{
						return false;
					}
				}
			}

			return true;
		}

		static public string MakeDouble(string str)
		{

			int find_dot = str.IndexOf(".");
			if (find_dot > -1)
			{
				return str;
			}
			str += ".0";
			return str;
		}

		static public string ReverseString(string str)
		{
			char[] arr = str.ToCharArray();
			Array.Reverse(arr);
			string ret = new string(arr);
			return ret;
		}
	}
}
