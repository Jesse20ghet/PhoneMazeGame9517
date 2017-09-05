using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleValidationRules
{
	public class PuzzleOrganizer
	{
		private List<String> filesToOrganize;
		private String outputDirectory;

		private Dictionary<String, int> createdFoldersDict = new Dictionary<String, int>();

		public PuzzleOrganizer(List<String> filesToOrganize, String outputDirectory)
		{
			this.filesToOrganize = filesToOrganize;
			this.outputDirectory = outputDirectory;
		}

		public void OrganizePuzzles()
		{
			CreateFolders();
			OrganizePuzzlesIntoAppropriateFolder();
		}

		private void CreateFolders()
		{
			// Go through all the directories and get the file names then get the prefix to them to know what folders to create
			foreach(var x in filesToOrganize)
			{
				var filesSplit = x.Split('\\');

				var fileName = filesSplit[filesSplit.Length - 1];

				var folderToCreate = fileName.Split('-')[0];

				if (!createdFoldersDict.ContainsKey(folderToCreate))
				{
					createdFoldersDict.Add(folderToCreate, 1);
				}
			}

			foreach (var folderToCreate in createdFoldersDict)
				Directory.CreateDirectory(outputDirectory + "/" + folderToCreate.Key);
		}

		private void OrganizePuzzlesIntoAppropriateFolder()
		{
			foreach(var file in filesToOrganize)
			{
				var fileContents = File.ReadAllBytes(file);
				var fileSplit = file.Split('\\');
				var fileName = fileSplit[fileSplit.Length - 1];

				//for(int i = createdFoldersDict.Count - 1; i >= 0; i--)
				String keyToUseAgain = "x";
				foreach(var x in createdFoldersDict.Keys)
				{
					if (file.Contains(x))
					{
						var fullPath = outputDirectory + "\\" + x + "\\Level " + createdFoldersDict[x] + ".xml";
						
						var fileStream = File.Create(fullPath);
						fileStream.Close();

						File.WriteAllBytes(fullPath, fileContents);

						keyToUseAgain = x;
					}
				}
				createdFoldersDict[keyToUseAgain]++;
			}
		}
	}
}
