using Assets.Code.ConcreteClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.Utilities
{
	public static class Constants
	{
		public static PuzzleCategoryDescriptor[] PuzzleCategories =
		{
			new PuzzleCategoryDescriptor() {Difficulty = 1, CategoryName = "2x2" },
			new PuzzleCategoryDescriptor() {Difficulty = 1, CategoryName = "3x3" },
			new PuzzleCategoryDescriptor() {Difficulty = 1, CategoryName = "4x4"},
			new PuzzleCategoryDescriptor() {Difficulty = 1, CategoryName = "5x5"},
			new PuzzleCategoryDescriptor() {Difficulty = 2, CategoryName = "6x6"},
			new PuzzleCategoryDescriptor() {Difficulty = 3, CategoryName = "7x7"},
			new PuzzleCategoryDescriptor() {Difficulty = 4, CategoryName = "8x8"},
			new PuzzleCategoryDescriptor() {Difficulty = 5, CategoryName = "9x9"},
			new PuzzleCategoryDescriptor() {Difficulty = 5, CategoryName = "10x10"}
		};

		public static string[] WonPuzzlePhraseList_Encouraging =
		{
			"You did it!",
			"Way to go!",
			"Easy peasy",
			"Great!",
			"Wow!",
			"I'm impressed",
			"Wooooo!",
			"Awesome!",
			"ooooohhh baby!",
			"Drinks on me!"
		};

		public static string[] WonPuzzlePhraseList_Demeaning =
		{
			"That took a long time...",
			"Its about time!",
			"Good thing I didn't hold my breath"
		};
	}
}
