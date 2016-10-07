using System;

namespace DLToolkitControlsSamples
{
	public static class Colors
	{
		static readonly Random random = new Random();

		static Colors()
		{
			colors = "#D32F2F, #B71C1C, #C2185B, #880E4F, #7B1FA2, #4A148C, #512DA8, #311B92, #303F9F, #1A237E, #1976D2, #0D47A1, #0288D1, #01579B, #0097A7, #006064, #00796B, #004D40, #388E3C, #1B5E20, #689F38, #33691E, #AFB42B, #827717, #FBC02D, #F57F17, #FFA000, #FF6F00, #F57C00, #E65100, #E64A19, #BF360C, #5D4037, #3E2723, #616161, #212121, #455A64, #263238"
				.Split(new string[] { ",", " " },
					StringSplitOptions.RemoveEmptyEntries);
		}

		static readonly string[] colors;

		public static Xamarin.Forms.Color RandomColor
		{
			get
			{
				return Xamarin.Forms.Color.FromHex(colors[random.Next(colors.Length)]);
			}
		}
	}
}
