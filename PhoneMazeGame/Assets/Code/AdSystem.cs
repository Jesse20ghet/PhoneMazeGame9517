using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Code
{
	public static class AdSystem
	{
		public static bool ShouldWeShowAd(string currentCategory)
		{
			var showAdd = false;
			int num;
			switch (currentCategory)
			{
				case "2x2":
				case "3x3":
					num = UnityEngine.Random.Range(0, 8);
					if (num == 0)
						showAdd = true;
					break;

				case "4x4":
				case "5x5":
					num = UnityEngine.Random.Range(0, 2);
					if (num == 0)
						showAdd = true;
					break;

				case "6x6":
				case "7x7":
				case "8x8":
				case "9x9":
				case "10x10":
					num = UnityEngine.Random.Range(0, 10);
					if (num > 3)
						showAdd = true;
					break;

				default:
					num = UnityEngine.Random.Range(0, 10);
					if (num > 4)
						showAdd = true;
					break;
			}

			return showAdd;
		}
	}
}