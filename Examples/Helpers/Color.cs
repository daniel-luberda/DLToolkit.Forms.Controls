using System;

namespace Examples.Helpers
{
	public static class Color
	{
		static readonly Random random = new Random();

		static Color()
		{
			colors = "#370900, #584500, #35618f, #0069ed, #495362, #211b0c, #3b3b3b, #06121f, #003783, #001439, #6c6555, #463800, #7a6400, #0073ff, #48688f, #004095, #353940"
				.Split(new string[] {","," "},
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

