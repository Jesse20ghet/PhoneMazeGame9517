using UnityEngine;
using System.Collections;
using Assets.Code.Save;
using System;
using Assets.Code;

public class ApplicationStartUp : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		PuzzlePersistence.Load();
		SettingsPersistence.Load();
		HintSystem.InitializeFirstTimeHintSystem();
	}
}
