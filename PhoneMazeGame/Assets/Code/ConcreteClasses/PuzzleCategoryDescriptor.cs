using Assets.Code.Save;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.ConcreteClasses
{
	public class PuzzleCategoryDescriptor
	{
		private int _difficulty;

		public string CategoryName { get; set; }

		/// <summary>
		/// Difficulty is an integer from 1 - 5. Used to display the number of stars on canvas
		/// </summary>
		public int Difficulty
		{
			get { return _difficulty; }
			set
			{
				if (value > 5)
				{
					_difficulty = 5;
				}
				else if (value < 1)
					_difficulty = 1;
				else
					_difficulty = value;
			}
		}
		
		public int LevelsCompleted
		{
			get
			{
				return GetCompletedLevels();
			}
		}

		private int _levelsAvailable = -1;
		public int LevelsAvailable
		{
			get
			{
				if (_levelsAvailable == -1)
				{
					_levelsAvailable = GetAvailableLevels();
				}

				return _levelsAvailable;
			}
		}

		private int GetAvailableLevels()
		{
			var levels = Resources.LoadAll("Puzzles/" + CategoryName);

			return levels.Count();
		}

		private int GetCompletedLevels()
		{
			var levels = PuzzlePersistence.GetPuzzlesFinishedInCategory(CategoryName);

			return levels.Count();
		}
	}
}
