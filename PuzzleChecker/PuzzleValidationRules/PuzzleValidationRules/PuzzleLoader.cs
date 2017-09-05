using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleValidationRules
{
	public static class PuzzleLoader
	{
		public static List<String> LoadPuzzles(string directory)
		{
			return Directory.GetFiles(directory).ToList();
		}
	}
}
