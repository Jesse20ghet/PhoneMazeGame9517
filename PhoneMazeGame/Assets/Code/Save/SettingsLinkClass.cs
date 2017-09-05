using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Save
{
	public class SettingsLinkClass : MonoBehaviour
	{
		public GameObject MainMenuRef;

		public Slider OctagonRotateSpeed;
		public Slider OctagonSwapSpeed;
		public Toggle OctagonShowIcons;
		public Toggle Audio;

		void Start()
		{
			OctagonRotateSpeed.value = SettingsPersistence.GetValueBySetting(Utilities.Enumerations.Setting.OctagonTurnSpeed);
			OctagonSwapSpeed.value = SettingsPersistence.GetValueBySetting(Utilities.Enumerations.Setting.OctagonSwapSpeed);
			OctagonShowIcons.isOn = SettingsPersistence.GetValueBySetting(Utilities.Enumerations.Setting.OctagonShowIcons) != 0;
			Audio.isOn = SettingsPersistence.GetValueBySetting(Utilities.Enumerations.Setting.Audio) != 0;
			SetAudioListeners(Audio.isOn);
		}

		public void SaveSettings()
		{
			SettingsPersistence.SetValue(Utilities.Enumerations.Setting.OctagonTurnSpeed, (int)OctagonRotateSpeed.value);
			SettingsPersistence.SetValue(Utilities.Enumerations.Setting.OctagonSwapSpeed, (int)OctagonSwapSpeed.value);
			SettingsPersistence.SetValue(Utilities.Enumerations.Setting.OctagonShowIcons, Convert.ToInt32(OctagonShowIcons.isOn));

			SettingsPersistence.SetValue(Utilities.Enumerations.Setting.Audio, Convert.ToInt32(Audio.isOn));
			SetAudioListeners(Audio.isOn);

			SettingsPersistence.Save();

			GameObject.Find("ScreenManager").GetComponent<MainMenuNavigtationScript>().ShowMenu(MainMenuRef);
		}

		public void SetAudioListeners(bool state)
		{
			var audioListenerRef = FindObjectOfType<AudioListener>();
			audioListenerRef.enabled = state;
		}
	}
}
