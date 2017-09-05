using UnityEngine;
using System.Collections;

public class FloatScript : MonoBehaviour {

	float startingYPosition;
	float startingXPosition;
	float startingZPosition;
	float speed = .5F;

	// Use this for initialization
	void Start ()
	{
		startingXPosition = transform.position.x;
		startingYPosition = transform.position.y;
		startingZPosition = transform.position.z;
	}
	
	// Update is called once per frame
	void Update ()
	{
		var y = startingYPosition + (Mathf.Sin(speed * Time.time) * .1F);
		var newVector3 = new Vector3(startingXPosition, y, startingZPosition);

		transform.position = newVector3;
	}
}
