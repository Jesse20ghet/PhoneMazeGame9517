using UnityEngine;
using System.Collections;

public class DisableParentCanvasGroupScript : MonoBehaviour {

	public void OnClick()
	{
		transform.parent.gameObject.GetComponent<CanvasGroup>().gameObject.SetActive(false);
	}
}
