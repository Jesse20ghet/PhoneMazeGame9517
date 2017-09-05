using UnityEngine;
using System.Collections;

public class OctagonRotationResetScript : MonoBehaviour {

	public float m_Speed = SettingsPersistence.GetValueBySetting(Assets.Code.Utilities.Enumerations.Setting.OctagonTurnSpeed);

	private Quaternion targetRotation;

	// Use this for initialization
	void Start ()
	{
		targetRotation = Quaternion.Euler(new Vector3(0, 0, 0));
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, m_Speed * Time.deltaTime);
	}

	void LateUpdate()
	{
		if (Mathf.Approximately(transform.rotation.z, targetRotation.z))
		{
			Destroy(this);
		}
	}
}
