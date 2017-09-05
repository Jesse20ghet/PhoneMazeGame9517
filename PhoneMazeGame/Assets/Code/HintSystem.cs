using Assets.Code.Save;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code
{
	public static class HintSystem
	{
		private const string HINTS_KEY = "x123asdfasdfsdfa!";
		private const int FIRST_TIME_INSTALL_HINTS = 10;

		private const string BEATEN_LEVELS_COUNTER_KEY = "bc12w3dddA!";
		private static int beatenLevelsCounter = 0;

		private const int HINT_REWARD_SPACER = 4;

		public static void InitializeFirstTimeHintSystem()
		{
			if (!PlayerPrefs.HasKey(HINTS_KEY))
				PlayerPrefs.SetInt(HINTS_KEY, FIRST_TIME_INSTALL_HINTS);

			if (!PlayerPrefs.HasKey(BEATEN_LEVELS_COUNTER_KEY))
				PlayerPrefs.SetInt(BEATEN_LEVELS_COUNTER_KEY, 0);
			else
				beatenLevelsCounter = PlayerPrefs.GetInt(BEATEN_LEVELS_COUNTER_KEY);
		}

		public static int GetAvailableHints()
		{
#if UNITY_EDITOR
			return 1;
#endif
			return PlayerPrefs.GetInt(HINTS_KEY);
		}

		public static void SetAvailableHints(int amount)
		{
			PlayerPrefs.SetInt(HINTS_KEY, amount);
		}

		public static void IncrementHints(int amount)
		{
			var currentHints = GetAvailableHints();
			SetAvailableHints(currentHints + amount);
		}

		public static void DecrementHints(int amount)
		{
			var currentHints = GetAvailableHints();
			SetAvailableHints(currentHints - amount);
		}

		internal static bool UserGetsAnotherHint(string currentPuzzleName, string currentPuzzleCategory)
		{
			if (PuzzlePersistence.IsPuzzleFinished(currentPuzzleCategory, currentPuzzleName))
				return false;

			beatenLevelsCounter++;

			if (beatenLevelsCounter == HINT_REWARD_SPACER)
			{
				beatenLevelsCounter = 0;
				SaveBeatenLevels();
				return true;
			}

			SaveBeatenLevels();
			return false;
		}

		private static void SaveBeatenLevels()
		{
			PlayerPrefs.SetInt(BEATEN_LEVELS_COUNTER_KEY, beatenLevelsCounter);
		}
	}
}
