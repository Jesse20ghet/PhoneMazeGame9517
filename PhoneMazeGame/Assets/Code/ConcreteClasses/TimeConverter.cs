using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.ConcreteClasses
{
	public static class TimeConverter
	{
		public static string ConvertSecondsToTimeString(float secondsParam)
		{
			float minutes = Mathf.Floor(secondsParam / 60);
			float seconds = Mathf.RoundToInt(secondsParam % 60);

			string minutesAsString = minutes.ToString();
			string secondsAsString = seconds.ToString();

			if (minutes < 10)
				minutesAsString = "0" + minutes.ToString();
			if (seconds < 10)
				secondsAsString = "0" + Mathf.RoundToInt(seconds).ToString();

			return minutesAsString + ":" + secondsAsString;
		}
	}
}
