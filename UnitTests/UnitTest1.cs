using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assets.Code;

namespace UnitTests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void PuzzleParser_ThrowAwayTest()
		{
			TextPuzzleParser puzzleParser = new TextPuzzleParser();

			puzzleParser.ParsePuzzles("Puzzles");
		}
	}
}
