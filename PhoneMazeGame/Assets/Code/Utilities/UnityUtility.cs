using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.Utilities
{
	public static class UnityUtility
	{
		public static GameObject FindInactiveGameObject(string name)
		{
			GameObject[] all = GameObject.FindObjectsOfType<GameObject>();
			foreach (var go in all)
			{
				if (go.transform.name == name)
					return go;
			}

			return null;
		}
	}
}
