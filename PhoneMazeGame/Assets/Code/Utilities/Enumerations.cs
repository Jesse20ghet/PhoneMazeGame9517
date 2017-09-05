using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Code.Utilities
{
	public class Enumerations
	{
		public enum OctagonType { Empty, Normal, Endpoint, CorrectPath }
		public enum OctagonAction { Turn, Swap }
		public enum OctagonColor { Locked, Default, Green, Red, Blue, Yellow, Correct }
		public enum Wall { N, NE, E, SE, S, SW, W, NW }
		public enum Setting { OctagonTurnSpeed, OctagonSwapSpeed, OctagonShowIcons, Audio }
	}
}
