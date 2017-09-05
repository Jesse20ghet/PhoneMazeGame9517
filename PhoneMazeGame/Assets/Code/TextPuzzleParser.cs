using Assets.Code.ConcreteClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace Assets.Code
{
	public class TextPuzzleParser
	{
		/// <summary>
		/// Creates a list of puzzles in specified directory
		/// </summary>
		/// <returns>Puzzle from XML file, throws error if unparsable</returns>
		public List<Puzzle> ParsePuzzles(string directory)
		{
			var puzzles = Resources.LoadAll(directory);

			var puzzleList = new List<Puzzle>();
			foreach (var puzzle in puzzles)
			{ 
				var parsedPuzzle = ParsePuzzle(puzzle.ToString());
				puzzleList.Add(parsedPuzzle);
			}

			puzzleList = OrderPuzzlesNumerically(puzzleList);

			return puzzleList;
		}

		public Puzzle ParsePuzzle(string puzzleXML)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(Puzzle));
			var reader = new StringReader(puzzleXML);
			return (Puzzle)serializer.Deserialize(reader);
		}

		private List<Puzzle> OrderPuzzlesNumerically(List<Puzzle> puzzleList)
		{
			var puzzleListToReturn = new List<Puzzle>();

			int puzzleSearchingFor = 1;
			foreach(var puzzle in puzzleList)
			{
				var puzzlee = puzzleList.First(x => x.PuzzleName.Split(' ')[1] == puzzleSearchingFor.ToString());
				puzzleListToReturn.Add(puzzlee);

				puzzleSearchingFor++;
			}

			return puzzleListToReturn;
		}
	}
}
