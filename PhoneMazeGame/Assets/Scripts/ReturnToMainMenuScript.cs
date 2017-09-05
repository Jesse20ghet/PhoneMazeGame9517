using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ReturnToMainMenuScript : MonoBehaviour {

	public void OnClick()
	{
		SceneManager.LoadScene("Main");
	}
}
