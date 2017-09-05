using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Code.Save
{
	[Serializable]
	public class PuzzleSaveClass
	{
		public int TurnCount { get; set; }
		public float TimeSpent { get; set; }
		public string PuzzleName { get; set; }
		public string CategoryName { get; set; }
	}
}
