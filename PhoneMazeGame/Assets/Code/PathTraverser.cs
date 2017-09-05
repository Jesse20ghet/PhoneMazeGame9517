using Assets.Code.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code
{
	public class PathTraverser
	{
		private GameObject endPointOne;
		private GameObject endPointTwo;
		private List<GameObject> nodes;
		List<Enumerations.Wall> wallValues;

		private List<GameObject> visitedNodes;
		private List<GameObject> correctPath = new List<GameObject>();
		private bool FoundEndPoint = false;

        public Dictionary<GameObject, List<Vector3>> connectedPathways;

		public PathTraverser(GameObject endPointOne, GameObject endPointTwo, List<GameObject> nodes)
		{
			this.endPointOne = endPointOne;
			this.endPointTwo = endPointTwo;
			this.nodes = nodes;
			visitedNodes = new List<GameObject>();
			wallValues = Enum.GetValues(typeof(Enumerations.Wall)).Cast<Enumerations.Wall>().ToList();
            connectedPathways = new Dictionary<GameObject, List<Vector3>>();
		}

		public bool CanTraverse()
		{
			Traverse(endPointOne);

			return FoundEndPoint;
		}

        public Dictionary<GameObject, List<Vector3>> GetConnectedPathways()
        {
            return connectedPathways;
        }

		public List<GameObject> GetCorrectPath()
		{
			return correctPath;
		}

		private GameObject Traverse(GameObject octagon)
		{
			if (octagon == endPointTwo || FoundEndPoint)
			{
				FoundEndPoint = true;
				correctPath.Add(octagon);
				return null;
			}

			visitedNodes.Add(octagon);

			foreach(var direction in wallValues)
			{
				if (CanTravelDirection(octagon, direction))
				{
					var nodeToTravelTo = GetNodeToTravelTo(octagon, direction);

                    // Build dictionary of connection vectors (for line drawing) for each visited node
                    if (!connectedPathways.ContainsKey(octagon))
                        connectedPathways.Add(octagon, new List<Vector3>());

                    connectedPathways[octagon].Add(nodeToTravelTo.GetComponent<Renderer>().bounds.center);
                    //
                        
                    if (visitedNodes.Contains(nodeToTravelTo))
                        continue;

                    Traverse(GetNodeToTravelTo(octagon, direction));
					if (octagon == endPointTwo || FoundEndPoint)
					{
						FoundEndPoint = true;
						correctPath.Add(octagon);
						return null;
					}
				}
			}

			return null;
		}

		private bool CanTravelDirection(GameObject node, Enumerations.Wall direction)
		{
			// You want to travel a direction but if the node is rotated, you need to get the appropriate wall
			var wallToCheck = AccountForRotation(node, direction);
			var wallGameObject = node.transform.Find(wallToCheck.ToString() + "-Wall").gameObject;

			// If the wall in the direction we want to travel isn't active.. That means we can try to go that direction
			if (!wallGameObject.GetComponent<WallScript>().active)
			{
				// Now that we can travel this direction from this node, we need to check 
				// the next node to make sure it has the appropriate wall turned off
				var nodeToTravelTo = GetNodeToTravelTo(node, direction);
				if (nodeToTravelTo == null)
					return false;

				var oppositeWall = GetOppositeWall(direction);
				var oppositeWallWithRotation = AccountForRotation(nodeToTravelTo, oppositeWall);

				var nextNodeWallGameObject = nodeToTravelTo.transform.Find(oppositeWallWithRotation.ToString() + "-Wall").gameObject;

				if (!nextNodeWallGameObject.GetComponent<WallScript>().active)
					return true;
			}
			
			return false;
		}

		private GameObject GetNodeToTravelTo(GameObject node, Enumerations.Wall direction)
		{ 
			var scriptRef = node.GetComponent<OctagonControllerScript>();
			int xValueToTravel = 0;
			int yValueToTravel = 0;
			
			switch(direction)
			{
				case Enumerations.Wall.N:
					yValueToTravel = -1;
					break;

				case Enumerations.Wall.NE:
					yValueToTravel = -1;
					xValueToTravel = 1;
					break;

				case Enumerations.Wall.E:
					xValueToTravel = 1;
					break;

				case Enumerations.Wall.SE:
					yValueToTravel = 1;
					xValueToTravel = 1;
					break;

				case Enumerations.Wall.S:
					yValueToTravel = 1;
					break;

				case Enumerations.Wall.SW:
					yValueToTravel = 1;
					xValueToTravel = -1;
					break;

				case Enumerations.Wall.W:
					xValueToTravel = -1;
					break;

				case Enumerations.Wall.NW:
					xValueToTravel = -1;
					yValueToTravel = -1;
					break;
			}

			var xCoordinate = scriptRef.XCoordinate + xValueToTravel;
			var yCoordinate = scriptRef.YCoordinate + yValueToTravel;

			var nodeToReturn = nodes.FirstOrDefault(x => x.GetComponent<OctagonControllerScript>().XCoordinate == xCoordinate &&
												x.GetComponent<OctagonControllerScript>().YCoordinate == yCoordinate);

			return nodeToReturn;
		}

		private Enumerations.Wall GetOppositeWall(Enumerations.Wall direction)
		{
			switch (direction)
			{
				case Enumerations.Wall.N:
					return Enumerations.Wall.S;

				case Enumerations.Wall.NE:
					return Enumerations.Wall.SW;

				case Enumerations.Wall.E:
					return Enumerations.Wall.W;

				case Enumerations.Wall.SE:
					return Enumerations.Wall.NW;

				case Enumerations.Wall.S:
					return Enumerations.Wall.N;

				case Enumerations.Wall.SW:
					return Enumerations.Wall.NE;

				case Enumerations.Wall.W:
					return Enumerations.Wall.E;

				case Enumerations.Wall.NW:
					return Enumerations.Wall.SE;

				default:
					throw new ArgumentException("Passed in direction not accounted for");
			}
		}
		private Enumerations.Wall AccountForRotation(GameObject node, Enumerations.Wall direction)
		{
			int incrementAmount = 0;

			int angle = (int)node.transform.rotation.eulerAngles.z;
			switch (angle)
			{
				case 0:
					break;

				case 44:
				case 45:
				case 46:
					incrementAmount = 1;
					break;

				case 89:
				case 90:
				case 91:
					incrementAmount = 2;
					break;

				case 134:
				case 135:
				case 136:
					incrementAmount = 3;
					break;

				case 179:
				case 180:
				case 181:
					incrementAmount = 4;
					break;

				case 224:
				case 225:
				case 226:
					incrementAmount = 5;
					break;

				case 269:
				case 270:
				case 271:
					incrementAmount = 6;
					break;

				case 314:
				case 315:
				case 316:
					incrementAmount = 7;
					break;
			}

			Enumerations.Wall newDirection = direction;
			for(int i = 0; i < incrementAmount; i++)
			{
				newDirection++;
				if (newDirection > Enumerations.Wall.NW)
					newDirection = Enumerations.Wall.N;
			}

			return newDirection;
		}
	}
}
