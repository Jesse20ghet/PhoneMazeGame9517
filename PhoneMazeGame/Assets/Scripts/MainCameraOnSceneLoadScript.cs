using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraOnSceneLoadScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		var audioListener = GetComponent<AudioListener>();
		var x = SettingsPersistence.GetValueBySetting(Assets.Code.Utilities.Enumerations.Setting.Audio);

		audioListener.enabled = x != 0;
	}
	
}
