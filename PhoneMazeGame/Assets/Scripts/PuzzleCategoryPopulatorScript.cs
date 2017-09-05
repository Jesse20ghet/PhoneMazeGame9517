using UnityEngine;
using UnityEngine.UI;
using Assets.Code.Utilities;

public class PuzzleCategoryPopulatorScript : MonoBehaviour {

	public GameObject grid;
	private GameObject categoryUIObject;

	// Use this for initialization
	void Start ()
	{
		categoryUIObject = (GameObject)Resources.Load("UI/PuzzleCategoryUIElement");

		LoadPuzzleCategories();
	}
	
	public void LoadPuzzleCategories()
	{
		foreach(var category in Constants.PuzzleCategories)
		{
			var goRef = (GameObject)GameObject.Instantiate(categoryUIObject, Vector3.zero, new Quaternion(0, 0, 0, 0));
			goRef.transform.SetParent(grid.transform, false);
			goRef.transform.Find("CategoryNameText").GetComponent<Text>().text = category.CategoryName + " Puzzles";
			goRef.transform.Find("PuzzlesCompletedText").GetComponent<Text>().text = category.LevelsCompleted + " / " + category.LevelsAvailable;
			goRef.GetComponent<PuzzleCategoryInfoScript>().CategoryName = category.CategoryName;

			var starContainer = goRef.transform.Find("StarContainer");
			for(int i = 1; i <= category.Difficulty; i++)
			{
				starContainer.transform.Find("Star" + i.ToString()).GetComponent<Image>().enabled = true;
			}

			goRef.transform.localScale = Vector3.one;
		}
	}
}
