using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	
	public static GameManager Instance = null;
	public static bool  bSplashScreen = false;
	
	public static bool bEnableNavigation = false;


	public static void InitGameManager()
	{
		if(Instance == null) { 	

			GameObject container = new GameObject();
			container.name = "GAME_MANAGER";
			Instance = container.AddComponent(typeof(GameManager)) as GameManager;

			DontDestroyOnLoad(container);

			Instance.Init();

		}
		Instance.LoadFirstSplashScreen();

	}

	void Start () {
	
	}
	
	 
	void Update () {
	
	}

   void Init () {
		GetSound();
		 
	}

	void LoadFirstSplashScreen()
	{
		Instance.StartCoroutine(WLoadFirstSplashScreen());
	}

	//***************************************************************************************************
	//PLAYER PREFS
	
	public bool bSound = true;

	public bool GetSound() {
		bSound = (PlayerPrefs.GetInt("SOUND",1) == 1) ? true: false;
		return bSound;
	}

	public void SetSound(bool value)
	{
		int sound =value?1:0;
		PlayerPrefs.SetInt("SOUND", sound);
		bSound = value;
	}
 
		

	  



	//***************************************************************************
	//NAVIGACIJA

	IEnumerator WLoadFirstSplashScreen()
	{

		if(!bSplashScreen) 
		{
			yield return null;
			bSplashScreen = true;
			bEnableNavigation = false;
			LevelsStack.Push("SplashScreen" );
			//Application.LoadLevel("SplashScreen" );
			LevelTransition.Instance.HideScene("SplashScreen");
		}
		else
		{
			yield return new WaitForSeconds(0.5f);
			bEnableNavigation = true;
		}
	}

 

	static Stack<string> LevelsStack = new Stack<string>();

	public void LoadNext()
	{
		if(bEnableNavigation)
		{
			switch(LevelsStack.Peek())
			{

			case "SplashScreen":
				LevelsStack.Push("HomeScreen" );
				break;
			case "HomeScreen":
				LevelsStack.Push("Gameplay 1" );
				break;
			case "Gameplay 1":
				LevelsStack.Push("Gameplay 2" );
				break;
			case "Gameplay 2":
				LevelsStack.Push("Gameplay 3" );
				break;
			case "Gameplay 3":
				LevelsStack.Push("Gameplay 4" );
				break;
			}

			StartCoroutine(WaitLoadNext());
		}
	}

	IEnumerator WaitLoadNext()
	{
		bEnableNavigation = false;
		yield return  new WaitForSeconds(0.5f);
		Application.LoadLevel(LevelsStack.Peek() );
	}


	public void LoadPrevious()
	{
		if(bEnableNavigation)
		{
			string pop = LevelsStack.Pop();
			if(pop== Application.loadedLevelName)
				StartCoroutine(WaitLoadPrevious());
		}
	}
	
	IEnumerator WaitLoadPrevious()
	{
		bEnableNavigation = false;
		string peep = LevelsStack.Peek();
		Debug.Log(peep);
		yield return  new WaitForSeconds(0.5f);
		Application.LoadLevel( peep );
	}




	public void LoadHomeScreen()
	{
		LevelsStack.Clear();
		LevelsStack.Push("SplashScreen" );
		LevelsStack.Push("HomeScreen" );

		//Application.LoadLevel("HomeScreen" );
		LevelTransition.Instance.HideScene("HomeScreen");
	}

	public void LoadSplashScreen()
	{
		LevelsStack.Clear();
		LevelsStack.Push("SplashScreen" );

		//Application.LoadLevel("SplashScreen" );
		LevelTransition.Instance.HideScene("SplashScreen");
	}


}
