using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Assets.Code.ConcreteClasses
{
	[Serializable]
	[System.Xml.Serialization.XmlRoot("PuzzleLevel")]
	public class Puzzle
	{
		[XmlElement("Width")]
		public int Width { get; set; }

		[XmlElement("Height")]
		public int Height { get; set; }

		[XmlElement("DesignerId")]
		public int DesignerId { get; set; }

		[XmlElement("Url")]
		public string Url { get; set; }

		[XmlArray("Octagons")]
		[XmlArrayItem("Octagon", typeof(Octagon))]
		public Octagon[] Octagons { get; set; }

		[System.Xml.Serialization.XmlElement("PuzzleName")]
		public string PuzzleName { get; set; }
	}
}
