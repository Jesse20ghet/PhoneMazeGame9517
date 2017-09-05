using UnityEngine;
using Assets.Code.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class OctagonControllerScript : MonoBehaviour 
{
	public Enumerations.OctagonAction octagonAction;
	public Enumerations.OctagonColor octagonColor;
	public Enumerations.OctagonType octagonType;

	public int XCoordinate;
	public int YCoordinate;
	public int OctagonId;

	public float CorrectRotation;

	public bool IsCorrect;

	public Vector3 startingPositionInWorldSpace = new Vector3(-1, -1, -1);

	public List<GameObject> linkedOctagons = new List<GameObject>();

	public Sprite CorrectSprite;

	private ActionControllerScript actionControllerScript;
	private AudioSource audioSource;
	private Vector3 startingScale;

	// Child references
	private List<GameObject> Walls = new List<GameObject>();
	private GameObject image;
	private GameObject background;
	private GameObject light;

	void Start()
	{
		actionControllerScript = GameObject.Find("ActionController").GetComponent<ActionControllerScript>();
		audioSource = GetComponent<AudioSource>();
		startingScale = transform.localScale;
		IsCorrect = false;

		// Get child references
		foreach (Transform child in transform)
		{
			if(child.tag == "Wall")
				Walls.Add(child.gameObject);

			if (child.name == "Image")
				image = child.gameObject;

			if (child.name == "Background")
				background = child.gameObject;

			if (child.name == "Light")
				light = child.gameObject;
		}

		if (light != null)
		{
			light.GetComponent<Light>().range = 2.6F;
			Debug.Log("Light Range: " + light.GetComponent<Light>().range);
			Debug.Log("Rect Width: " + gameObject.GetComponent<RectTransform>().rect.width);
		}
	}
	
	public void OnClick()
	{
		if (octagonColor == Enumerations.OctagonColor.Locked || IsCorrect)
			return;

		switch(octagonAction)
		{
			case Enumerations.OctagonAction.Turn:
				RotateOctagon();
				break;

			case Enumerations.OctagonAction.Swap:
				SwapOctagon();
				break;

			default:
				break;
		}
	}

	public void PlaySound()
	{
		audioSource.Play();
	}

	public void OnMouseDown()
	{
		transform.localScale = new Vector3(transform.localScale.x * .95F, transform.localScale.y * .95F, transform.localScale.z);
	}

	public void OnMouseUp()
	{
		transform.localScale = startingScale;
	}

	public void CorrectRotationFromHint()
	{
		if (!IsCorrect)
		{
			// Remove reference to this octagon in all other linked octagons
			foreach(var linkedOctagon in linkedOctagons)
				linkedOctagon.GetComponent<OctagonControllerScript>().linkedOctagons.Remove(gameObject);

			image.GetComponent<Image>().sprite = CorrectSprite;
			var color = image.GetComponent<Image>().color;
			image.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 1);
			background.GetComponent<Image>().color = EnumToColorConverter.SwapBackgroundColor(Enumerations.OctagonColor.Correct);
			foreach (var wall in Walls)
				wall.GetComponent<Image>().color = EnumToColorConverter.SwapBackgroundColor(Enumerations.OctagonColor.Correct);


			gameObject.AddComponent<RotateToSpecificZAngleScript>();
			gameObject.GetComponent<RotateToSpecificZAngleScript>().targetRotation = new Quaternion(
																									this.transform.rotation.x,
																									transform.rotation.y,
																									CorrectRotation,
																									transform.rotation.w);

			gameObject.AddComponent<LightIntensifyThenFadeScript>();
			gameObject.GetComponent<LightIntensifyThenFadeScript>().light = light.GetComponent<Light>();
			IsCorrect = true;
		}
	}

	private void RotateOctagon()
	{
		// Get this octagon with and linked octagons
		var octagonList = new List<GameObject>();
		foreach (var linkedOctagon in linkedOctagons)
			octagonList.Add(linkedOctagon);

		octagonList.Add(gameObject);

		actionControllerScript.PerformAction(Enumerations.OctagonAction.Turn, octagonList);
	}

	private void SwapOctagon()
	{
		// Get this octagon with and linked octagons
		var octagonList = new List<GameObject>();
		foreach (var linkedOctagon in linkedOctagons)
			octagonList.Add(linkedOctagon);

		octagonList.Add(gameObject);
		octagonList = octagonList.OrderBy(x => x.GetComponent<OctagonControllerScript>().OctagonId).ToList();

		actionControllerScript.PerformAction(Enumerations.OctagonAction.Swap, octagonList);
	}
}
