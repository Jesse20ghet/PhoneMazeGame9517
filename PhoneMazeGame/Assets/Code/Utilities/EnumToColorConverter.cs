using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.Utilities
{
	public static class EnumToColorConverter
	{
		private static Color BackgroundYellow = new Color(.3F, .3F, 0);
		private static Color WallYellow = new Color(1, 1, 0);

		private static Color BackgroundBlue = new Color(0, 0, .6F);
		private static Color WallBlue = new Color(0, 0, 1);

		private static Color BackgroundRed = new Color(.5F, 0, 0);
		private static Color WallRed = new Color(1, 0, 0);

		private static Color BackgroundGreen = new Color(0, .5F, 0);
		private static Color WallGreen = new Color(0, 1, 0);

		private static Color BackgroundLocked = new Color(.38F, .38F, .38F);
		private static Color WallLocked = new Color(0, 0, 0);

		private static Color BackgroundDefault = new Color(.49F, .49F, .49F);
		private static Color WallDefault = new Color(1, 1, 1);

		private static Color BackgroundCorrect = new Color(.81F, .71F, .23F);
		private static Color WallCorrect = new Color(1F, 1F, 1F);

		public static Color SwapBackgroundColor(Enumerations.OctagonColor colorToSwap)
		{
			switch (colorToSwap)
			{
				case Enumerations.OctagonColor.Locked:
					return BackgroundLocked;

				case Enumerations.OctagonColor.Default:
					return BackgroundDefault;

				case Enumerations.OctagonColor.Blue:
					return BackgroundBlue;

				case Enumerations.OctagonColor.Red:
					return BackgroundRed;

				case Enumerations.OctagonColor.Green:
					return BackgroundGreen;

				case Enumerations.OctagonColor.Yellow:
					return BackgroundYellow;

				case Enumerations.OctagonColor.Correct:
					return BackgroundCorrect;

				default:
					return BackgroundDefault;
			}
		}

		public static Color SwapWallColor(Enumerations.OctagonColor colorToSwap)
		{
			switch (colorToSwap)
			{
				case Enumerations.OctagonColor.Locked:
					return WallLocked;

				case Enumerations.OctagonColor.Default:
					return WallDefault;

				case Enumerations.OctagonColor.Blue:
					return WallBlue;

				case Enumerations.OctagonColor.Red:
					return WallRed;

				case Enumerations.OctagonColor.Green:
					return WallGreen;

				case Enumerations.OctagonColor.Yellow:
					return WallYellow;

				case Enumerations.OctagonColor.Correct:
					return WallCorrect;

				default:
					return WallDefault;
			}
		}
	}
}
