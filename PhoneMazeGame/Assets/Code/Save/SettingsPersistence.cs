using UnityEngine;
using System.Collections;
using Assets.Code.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

public static class SettingsPersistence  {

	const string fileSaveLocation = "settings.gd";
	
	static List<SettingsSaveClass> settings = new List<SettingsSaveClass>()
	{
		new SettingsSaveClass {Setting = Enumerations.Setting.OctagonTurnSpeed, Value=640 },
		new SettingsSaveClass {Setting = Enumerations.Setting.OctagonSwapSpeed, Value=2 },
		new SettingsSaveClass {Setting = Enumerations.Setting.OctagonShowIcons, Value=0 },
		new SettingsSaveClass {Setting = Enumerations.Setting.Audio, Value = 1 }
	};

	public static int GetValueBySetting(Enumerations.Setting setting)
	{
		return settings.Single(x => x.Setting == setting).Value;
	}

	public static void SetValue(Enumerations.Setting setting, int value)
	{
		settings.First(x => x.Setting == setting).Value = value;
	}

	public static void Load()
	{
		if (File.Exists(Application.persistentDataPath + "/" + fileSaveLocation))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/" + fileSaveLocation, FileMode.Open);
			var settingsFromFile = (List<SettingsSaveClass>)bf.Deserialize(file);
			file.Close();

			// Loop through every file taken from settings file just incase we add new ones, things dont get broken.
			foreach(var settingFromFile in settingsFromFile)
			{
				settings.First(x => x.Setting == settingFromFile.Setting).Value = settingFromFile.Value;
			}
		}
	}

	public static void Save()
	{
		try
		{
			var bf = new BinaryFormatter();
			FileStream file = File.Create(Application.persistentDataPath + "/" + fileSaveLocation);
			bf.Serialize(file, settings);
			file.Close();
		}
		catch
		{

		}
	}
}
