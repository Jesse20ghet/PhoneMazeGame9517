using UnityEngine;
using System.Collections;

public class RotatePieceScript : MonoBehaviour {

	private float m_rotationAngle = -45f;
	public float m_Speed = SettingsPersistence.GetValueBySetting(Assets.Code.Utilities.Enumerations.Setting.OctagonTurnSpeed);
	private Vector3 m_RotationDirection = Vector3.forward;

	private Quaternion targetRotation;

	// Use this for initialization
	void Start ()
	{
		targetRotation = transform.rotation;
		targetRotation *= Quaternion.AngleAxis(m_rotationAngle, m_RotationDirection);
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, m_Speed * Time.deltaTime);
	}

	void LateUpdate()
	{
		if (Mathf.Approximately(transform.rotation.z, targetRotation.z))
		{
			var actionController = GameObject.Find("ActionController");

			if(actionController != null)
				actionController.GetComponent<ActionControllerScript>().Completed(gameObject);
			Destroy(this);
		}
	}
}
