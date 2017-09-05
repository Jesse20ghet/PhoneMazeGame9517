using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Code.ConcreteClasses.DTOs
{
	public class BeatenLevelDTO
	{
		public string UniqueID { get; set; }
		public int Moves { get; set; }
		public float TimeTaken { get; set; }
		public DateTime TimeRecorded { get; set; }
	}
}
