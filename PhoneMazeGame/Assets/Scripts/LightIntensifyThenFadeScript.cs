using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIntensifyThenFadeScript : MonoBehaviour {

	public Light light;

	private float maxIntensity = 16f;
	private float minIntensity = 0f;

	private float increaseIntensityStepValue = .8F;
	private float decreaseIntensityStepValue = .15F;

	private float timeBetween = .02F;

	private int currentState = 0;

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update()
	{
		switch (currentState)
		{
			case 0:
				light.intensity += increaseIntensityStepValue;
				if (light.intensity > maxIntensity)
					currentState++;
				break;
			case 1:
				light.intensity -= decreaseIntensityStepValue;
				if (light.intensity < minIntensity)
					currentState++;
				break;
			case 2:
				Destroy(this);
				break;
		}
	}
}
