using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class BottomBannerScript : MonoBehaviour {

	private static BottomBannerScript instance = null;

	public static BottomBannerScript Instance
	{
		get
		{
			return instance;
		}
	}

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

	void Start()
	{
		RequestBanner();
	}

	private void RequestBanner()
	{
		#if UNITY_EDITOR
				string adUnitId = "ca-app-pub-6972440739761219/3389418083";
		#elif UNITY_ANDROID
						string adUnitId = "ca-app-pub-6972440739761219/3389418083";
		#elif UNITY_IPHONE
						string adUnitId = "ca-app-pub-6972440739761219/3389418083";
		#else
						string adUnitId = "ca-app-pub-6972440739761219/3389418083";
		#endif

		try
		{
			BannerView bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
			AdRequest request = new AdRequest.Builder().Build();
			bannerView.LoadAd(request);
		}
		catch
		{ }
	}

	void OnLevelWasLoaded()
	{
		RequestBanner();
	}
}
