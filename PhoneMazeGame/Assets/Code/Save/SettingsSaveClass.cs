using UnityEngine;
using System.Collections;
using Assets.Code.Utilities;
using System;

[Serializable]
public class SettingsSaveClass
{
	public Enumerations.Setting Setting { get; set; }
	public int Value { get; set; }
}
