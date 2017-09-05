using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		var x = GameObject.Find(this.transform.name);

		if (x != this.gameObject)
			Destroy(this.gameObject);
		else
			DontDestroyOnLoad(transform.gameObject);

	}
}
