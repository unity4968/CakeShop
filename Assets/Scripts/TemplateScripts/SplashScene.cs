﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
  * Scene:Splash
  * Object:Main Camera
  * Description: F-ja zaduzena za ucitavanje MainScene-e, kao i vizuelni prikaz inicijalizaije CrossPromotion-e i ucitavanja scene
  **/
public class SplashScene : MonoBehaviour {
	
	int appStartedNumber;
	AsyncOperation progress = null;
	Image progressBar;
	float myProgress=0;
	string sceneToLoad;
	// Use this for initialization

	void Start ()
	{
		StatusBar.BannerPixelOffset = Screen.height*0.084f; 

		sceneToLoad = "MainScene";

		progressBar = GameObject.Find("ProgressBar").GetComponent<Image>();
		if(PlayerPrefs.HasKey("appStartedNumber"))
		{
			appStartedNumber = PlayerPrefs.GetInt("appStartedNumber");
		}
		else
		{
			appStartedNumber = 0;
		}
		appStartedNumber++;
		PlayerPrefs.SetInt("appStartedNumber",appStartedNumber);

	 

		DataManager.InitDataManager();
		DataManager.Instance.GetLocalData(); 
		Shop.InitShop();
		
		DataManager.Instance.GetTutorialProgress();
		DataManager.Instance.PopulateGameplay1Data();
		DataManager.Instance.GetPriceData();



		StartCoroutine(LoadScene());
	}
	
	/// <summary>
	/// Coroutine koja ceka dok se ne inicijalizuje CrossPromotion, menja progres ucitavanja CrossPromotion-a, kao i progres ucitavanje scene, i taj progres se prikazuje u Update-u
	/// </summary>
	IEnumerator LoadScene()
	{
		while(myProgress < 0.4)
		{
			myProgress += 0.05f;
			progressBar.fillAmount=myProgress;
			yield return new WaitForSeconds(0.05f);
		}

		while(myProgress < 1)
		{
			myProgress += 0.05f;
			progressBar.fillAmount=myProgress;
			yield return new WaitForSeconds(0.05f);
		}
		 
		Application.LoadLevel(sceneToLoad);

       
    }
}
