using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleValidationRules
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			string directory = String.Empty;
			using (var fileBrowser = new FolderBrowserDialog())
			{
				fileBrowser.Description = "Choose puzzle directory";

				var dialogResult = DialogResult.No;
				while (dialogResult != DialogResult.OK)
				{
					dialogResult = fileBrowser.ShowDialog();
					directory = fileBrowser.SelectedPath;
				}
			}

			var files = PuzzleLoader.LoadPuzzles(directory);

			Console.WriteLine("Found " + files.Count + " puzzles");
			Console.WriteLine("");

			var listOfFilesWithoutHints = HintChecker.CheckForHints(files);

			Console.WriteLine("Files without Correct Path tags in the puzzles");
			Console.WriteLine("----------------------------------------------");
			foreach (var file in listOfFilesWithoutHints)
				Console.WriteLine(file);

			if (listOfFilesWithoutHints.Count > 0)
			{
				Console.ReadLine();
				return;
			}

			Console.Write("Reorganize files into appropriate folders?(y/n): ");
			var x = Console.ReadKey();

			if(Char.ToLower(x.KeyChar) == 'y')
			{
				var puzzleOrganizer = new PuzzleOrganizer(files, directory);
				puzzleOrganizer.OrganizePuzzles();
			}
		}
	}
}
