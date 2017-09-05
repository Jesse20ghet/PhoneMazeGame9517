using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToSpecificZAngleScript : MonoBehaviour
{

	public Quaternion targetRotation;
	public float m_Speed = SettingsPersistence.GetValueBySetting(Assets.Code.Utilities.Enumerations.Setting.OctagonTurnSpeed);

	// Use this for initialization
	void Start()
	{

	}

	void Update()
	{
		if (Mathf.Approximately(transform.rotation.eulerAngles.z, targetRotation.z))
		{
			Destroy(this);
		}
		else // This needs to be here or else it'll add the rotate piece script even if this is "Destroyed", it doesn't happen until next frame.
		{
			if (gameObject.GetComponent<RotatePieceScript>() == null)
				gameObject.AddComponent<RotatePieceScript>();
		}


	}
}