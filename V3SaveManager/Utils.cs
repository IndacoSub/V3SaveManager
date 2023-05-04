using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V3SaveManager
{
	public partial class Savefile
	{

		// Old code, pretty much unmaintained
		// Optimizations and fixes are welcome

		private static int GetLastCharacterIndex(string str)
		{
			int[] whitelist = new int[]
			{
				(int)' ',
				0x00,
				0x09,
				0x0A,
				0x0B,
				0x0C,
				0x0D,
				0x20,
				0x3000
			};
			int i = 0;
			foreach (char c in str)
			{
				if (char.IsWhiteSpace(c) && !whitelist.Contains(c))
				{
					Console.WriteLine("Unknown whitespace character: " + (int)c);
					return i;
				}
				i++;
			}
			return i;
		}

		private static string BytesToString(byte[] arr)
		{
			string ret = Encoding.UTF8.GetString(arr, 0, arr.Length);
			int lastcharindex = GetLastCharacterIndex(ret);
			ret = ret.Substring(0, lastcharindex);
			ret = ret.Normalize();
			ret = ret.Trim();
			return ret;
		}

		private static byte[] StringToBytes(string str, int size)
		{
			string str2 = str;
			byte[] ret = Encoding.UTF8.GetBytes(str2);
			while (ret.Length > size)
			{
				str2 = str2.Substring(0, str2.Length - 1);
				ret = Encoding.UTF8.GetBytes(str2);
			}
			while (ret.Length < size)
			{
				str2 = str2 + (char)0x00;
				ret = Encoding.UTF8.GetBytes(str2);
			}
			return ret;
		}

		private static string InsertBeginning(string str, string front, bool force)
		{
			string ret = str;
			if (ret.StartsWith(front) && !force)
			{
				return ret;
			}
			ret = ret.Insert(0, front);
			return ret;
		}

		private static string GetBeginningOfString(string str)
		{
			return str.Substring(0, (str.Length / 50));
		}

		private static string GetEndOfString(string str)
		{
			return str.Substring(str.Length - (str.Length / 50));
		}
	}
}
