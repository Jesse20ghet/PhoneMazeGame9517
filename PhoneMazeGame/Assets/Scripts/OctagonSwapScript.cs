using UnityEngine;
using System.Collections;

public class OctagonSwapScript : MonoBehaviour {

	public Vector3 positionToMoveTowards;
	public float distanceBetween;
	float speed = SettingsPersistence.GetValueBySetting(Assets.Code.Utilities.Enumerations.Setting.OctagonSwapSpeed);

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		transform.position = Vector3.MoveTowards(transform.position, positionToMoveTowards, Time.deltaTime * speed * distanceBetween);
	}

	void LateUpdate()
	{
		if (Mathf.Approximately(transform.position.x, positionToMoveTowards.x) &&
			Mathf.Approximately(transform.position.z, positionToMoveTowards.z))
		{
			GameObject.Find("ActionController").GetComponent<ActionControllerScript>().Completed(gameObject);
			Destroy(this);
		}

	}
}
