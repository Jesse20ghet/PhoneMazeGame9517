using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleValidationRules
{
	public static class HintChecker
	{
		public static List<String> CheckForHints(List<String> pathsToCheck)
		{
			var returnList = new List<String>();

			foreach(var path in pathsToCheck)
			{
				var fileContents = File.ReadAllText(path);

				if (!fileContents.Contains("CorrectPath"))
					returnList.Add(path);
			}

			return returnList;
		}
	}
}
