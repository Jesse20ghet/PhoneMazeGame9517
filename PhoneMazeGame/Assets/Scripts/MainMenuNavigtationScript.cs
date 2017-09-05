using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using Assets.Code.Utilities;
using System.Collections.Generic;

public class MainMenuNavigtationScript : MonoBehaviour {

	public GameObject MenuToShow;

	private List<GameObject> MenuList = new List<GameObject>();

	void Start()
	{
		MenuList.Add(GameObject.Find("MainMenuCanvas"));
		MenuList.Add(GameObject.Find("PuzzleCategorySelectorCanvas"));
		MenuList.Add(GameObject.Find("PuzzleMenuCanvas"));
		MenuList.Add(GameObject.Find("SettingsCanvas"));
		MenuList.Add(GameObject.Find("CreditsCanvas"));

		// If this key exists, that means that we wanted to control the first menu to show that was set in a different scene
		if (PlayerPrefs.HasKey("MenuToShow"))
		{
			MenuToShow = GameObject.Find(PlayerPrefs.GetString("MenuToShow"));
			PlayerPrefs.DeleteKey("MenuToShow");
		}


		foreach(var menu in MenuList)
		{
			if(menu == null)
				throw new Exception("Didn't populate the public canvas gameobject with this script");
		}

		ShowMenu(MenuToShow);
	}

	public void ShowMenu(GameObject menuToShow)
	{
		foreach(var menu in MenuList)
		{
			var animator = menu.GetComponent<Animator>();

			if (menu == menuToShow)
				animator.SetBool("Show", true);
			else
				animator.SetBool("Show", false);
		}
	}

	//public void ShowMainMenu()
	//{
	//	MainMenuCanvas.GetComponent<Animator>().SetBool("Show", true);
	//	CategoryCanvas.GetComponent<Animator>().SetBool("Show", false);
	//	SettingsCanvas.GetComponent<Animator>().SetBool("Show", false);
	//	CreditsCanvas.GetComponent<Animator>().SetBool("Show", false);
	//}

	//public void ShowCategoriesMenu()
	//{
	//	MainMenuCanvas.GetComponent<Animator>().SetBool("Show", false);
	//	CategoryCanvas.GetComponent<Animator>().SetBool("Show", true);
	//	SelectPuzzleCanvas.GetComponent<Animator>().SetBool("Show", false);
	//}

	//public void ShowPuzzleMenu()
	//{
	//	CategoryCanvas.GetComponent<Animator>().SetBool("Show", false);
	//	SelectPuzzleCanvas.GetComponent<Animator>().SetBool("Show", true);
	//}

	//public void ShowSettingsMenu()
	//{
	//	MainMenuCanvas.GetComponent<Animator>().SetBool("Show", false);
	//	SettingsCanvas.GetComponent<Animator>().SetBool("Show", true);
	//}

	//public void ShowCreditsMenu()
	//{
	//	MainMenuCanvas.GetComponent<Animator>().SetBool("Show", false);
	//	CreditsCanvas.GetComponent<Animator>().SetBool("Show", true);
	//}
}
