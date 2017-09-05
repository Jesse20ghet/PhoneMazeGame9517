using UnityEngine;
using System.Collections;

public class OctagonSwapResetScript : MonoBehaviour {

	private float m_speed = SettingsPersistence.GetValueBySetting(Assets.Code.Utilities.Enumerations.Setting.OctagonSwapSpeed);

	public Vector3 positionToResetTo;

	// Update is called once per frame
	void Update ()
	{
		this.transform.position = Vector3.MoveTowards(transform.position, positionToResetTo, m_speed * Time.deltaTime);

		if (transform.position == positionToResetTo)
			Destroy(this);
	}
}
