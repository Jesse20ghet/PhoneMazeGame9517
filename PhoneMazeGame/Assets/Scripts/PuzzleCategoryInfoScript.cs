using UnityEngine;
using System.Collections;

public class PuzzleCategoryInfoScript : MonoBehaviour {

	public string CategoryName;
	public GameObject PuzzleMenuRef;

	void Start()
	{
		PuzzleMenuRef = GameObject.Find("PuzzleMenuCanvas");
	}

	public void ShowCategories()
	{
		PlayerPrefs.SetString("PuzzleCategory", CategoryName);
		GameObject.Find("PuzzleMenuCanvas").GetComponent<PuzzleMenuPopulatorScript>().PopulatePuzzleMenu();
		GameObject.Find("ScreenManager").GetComponent<MainMenuNavigtationScript>().ShowMenu(PuzzleMenuRef);
	}
}
