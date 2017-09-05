using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

namespace Assets.Code.Save
{
	public static class PuzzlePersistence
	{
		const string fileSaveLocation = "savedPuzzles.gd";

		static List<PuzzleSaveClass> finishedPuzzles = new List<PuzzleSaveClass>();

		public static bool IsPuzzleFinished(string category, string puzzleName)
		{
			return finishedPuzzles.Where(x => x.CategoryName == category && x.PuzzleName == puzzleName).Count() > 0;
		}

		/// <summary>
		/// Returns completed puzzle if it exists, if it doesn't, returns null.
		/// </summary>
		/// <param name="category"></param>
		/// <param name="puzzleName"></param>
		/// <returns></returns>
		public static PuzzleSaveClass GetBestPuzzle(string category, string puzzleName)
		{
			return finishedPuzzles.FirstOrDefault(x => x.CategoryName == category && x.PuzzleName == puzzleName);
		}

		public static List<PuzzleSaveClass> GetPuzzlesFinishedInCategory(string category)
		{
			return finishedPuzzles.Where(x => x.CategoryName == category).ToList();
		}

		public static void Load()
		{
			if (File.Exists(Application.persistentDataPath + "/" + fileSaveLocation))
			{
				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Open(Application.persistentDataPath + "/" + fileSaveLocation, FileMode.Open);
				finishedPuzzles = (List<PuzzleSaveClass>)bf.Deserialize(file);
				file.Close();
			}
		}

		public static void Save(PuzzleSaveClass classToSave)
		{
			try
			{
				var oldSavePuzzle = finishedPuzzles.FirstOrDefault(x => x.PuzzleName == classToSave.PuzzleName && x.CategoryName == classToSave.CategoryName);

				var beatTime = true;
				var beatMoves = true;
				// If this puzzle has never been beaten
				if (oldSavePuzzle == null)
				{
					finishedPuzzles.Add(classToSave);
					var bf = new BinaryFormatter();
					FileStream file = File.Create(Application.persistentDataPath + "/" + fileSaveLocation);
					bf.Serialize(file, finishedPuzzles);
					file.Close();
				}
				else
				{
					// We save the better time and the better turn count
					if (oldSavePuzzle.TimeSpent < classToSave.TimeSpent)
					{
						beatTime = false;
						classToSave.TimeSpent = oldSavePuzzle.TimeSpent;
					}

					if (oldSavePuzzle.TurnCount < classToSave.TurnCount)
					{
						beatMoves = false;
						classToSave.TurnCount = oldSavePuzzle.TurnCount;
					}
				}

				// If this puzzle has been beaten but the time is better
				if (oldSavePuzzle != null && (beatTime || beatMoves))
				{
					finishedPuzzles.Remove(oldSavePuzzle);
					finishedPuzzles.Add(classToSave);
					var bf = new BinaryFormatter();
					FileStream file = File.Create(Application.persistentDataPath + "/" + fileSaveLocation);
					bf.Serialize(file, finishedPuzzles);
					file.Close();
				}
			}
			catch
			{

			}
		}
	}
}
