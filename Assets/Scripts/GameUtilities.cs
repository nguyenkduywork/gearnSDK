using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine.UI;

public class GameUtilities
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct Currencies
	{
		private static string[] format = new string[]
		{
			"K",
			"M",
			"B",
			"T",
			"aa",
			"ab",
			"ac",
			"ad",
			"ae",
			"af",
			"ag",
			"ah",
			"ai",
			"aj",
			"ak",
			"al",
			"am",
			"an",
			"ao",
			"ap",
			"aq",
			"ar",
			"as",
			"at",
			"au",
			"av",
			"aw",
			"ax",
			"ay",
			"az"
		};

		public static string Convert(double input)
		{
			if (input < 1000.0)
			{
				return Math.Round(input).ToString();
			}
			double num = 0.0;
			for (int i = 0; i < GameUtilities.Currencies.format.Length; i++)
			{
				num = input / Math.Pow(1000.0, (double)(i + 1));
				if (num < 1000.0)
				{
					return Math.Round(num, (num >= 100.0) ? 0 : 1).ToString() + GameUtilities.Currencies.format[i];
				}
			}
			return num.ToString();
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct DateTime
	{
		public static string Convert(int second)
		{
			int num = second / 86400;
			int num2 = second % 86400 / 3600;
			int num3 = second % 3600 / 60;
			int num4 = second % 60;
			if (num > 0)
			{
				return num.ToString() + "d" + ((num2 <= 0) ? string.Empty : (num2.ToString() + "h"));
			}
			if (num2 > 0)
			{
				return num2.ToString() + "h" + ((num3 <= 0) ? string.Empty : (num3.ToString() + "m"));
			}
			if (num3 > 0)
			{
				return num3.ToString() + "m" + ((num4 <= 0) ? string.Empty : (num4.ToString() + "s"));
			}
			return num4.ToString() + "s";
		}

		public static int Offline(string dateTime)
		{
			if (dateTime == string.Empty)
			{
				return 0;
			}
			return (int)Math.Round(System.DateTime.Now.Subtract(System.Convert.ToDateTime(dateTime)).TotalSeconds);
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct String
	{
		private static StringBuilder stringBuilder = new StringBuilder();

		public static void ToText(Text text, string content)
		{
			text.text = GameUtilities.String.stringBuilder.Append(content).ToString();
			GameUtilities.String.stringBuilder.Remove(0, GameUtilities.String.stringBuilder.Length);
		}
	}
}
