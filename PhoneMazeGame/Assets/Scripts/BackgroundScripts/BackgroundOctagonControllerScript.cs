using UnityEngine;
using System.Collections.Generic;

public class BackgroundOctagonControllerScript : MonoBehaviour {

	private float intervalBetweenRotations = .2F;
	private float nextRotationTime;

	private List<GameObject> OctagonList = new List<GameObject>();

	// Use this for initialization
	void Start ()
	{
		var children = gameObject.GetComponentsInChildren<Transform>();

		foreach(var child in children)
		{
			if(child.transform.name.Contains("Octagon"))
				OctagonList.Add(child.gameObject);
		}

		nextRotationTime = Time.time + intervalBetweenRotations;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.time > nextRotationTime)
		{
			var octagonToRotate = Random.Range(0, OctagonList.Count - 1);
			while (OctagonList[octagonToRotate].GetComponent<RotatePieceScript>() != null)
				octagonToRotate = Random.Range(0, OctagonList.Count - 1);

			var scriptRef = OctagonList[octagonToRotate].AddComponent<RotatePieceScript>();
			scriptRef.m_Speed = 40;
			nextRotationTime = Time.time + intervalBetweenRotations;
		}
	}
}
