using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class InterstitialAdScript : MonoBehaviour {

	private static InterstitialAdScript instance = null;

	public static InterstitialAdScript Instance
	{
		get
		{
			return instance;
		}
	}

#if UNITY_ANDROID
		string adUnitId = "ca-app-pub-6972440739761219/8470262487";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-6972440739761219/8470262487";
#else
        string adUnitId = "ca-app-pub-6972440739761219/8470262487";
#endif

	private InterstitialAd interstitial;

	void Awake()
	{
		// if the singleton hasn't been initialized yet
		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
		}

		instance = this;
		DontDestroyOnLoad(this.gameObject);
	}

	public void PrepareInterstitalAd()
	{
		// Initialize an InterstitialAd.
		interstitial = new InterstitialAd(adUnitId);
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the interstitial with the request.
		interstitial.LoadAd(request);
	}

	public void LaunchInterstitalAd()
	{
		if (interstitial.IsLoaded())
		{
			interstitial.Show();
		}
	}
}
