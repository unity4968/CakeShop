using UnityEngine;
using System.Collections;
using UnityEngine.UI;

///<summary>
///<para>Scene:All/NameOfScene/NameOfScene1,NameOfScene2,NameOfScene3...</para>
///<para>Object:N/A</para>
///<para>Description: Sample Description </para>
///</summary>

public class GlobalVariables : MonoBehaviour {

	//********************************************************

	//PAUZIRANJE IGRE
	public static bool bPauseGame = false;
	public static bool bPauseUI;
	public static bool bPauseFakeLoading;

	public enum PauseSource
	{
		UI,
		FakeLoading
	}

	public static void PauseGame( PauseSource pauseSource)
	{
		switch(pauseSource)
		{
		case PauseSource.UI: bPauseUI = true; break;
		case PauseSource.FakeLoading: bPauseFakeLoading = true; break;
		}

		if(!bPauseGame)
		{
			bPauseGame = true;
			if(OnPauseGame!=null) OnPauseGame();
		}

	}

	public static void UnpauseGame( PauseSource pauseSource)
	{
		switch(pauseSource)
		{
		case PauseSource.UI: bPauseUI = false; break;
		case PauseSource.FakeLoading: bPauseFakeLoading = false; break;
		}

		if(bPauseUI || bPauseFakeLoading) bPauseGame = true;
		else bPauseGame = false;

		if( !bPauseGame && pauseSource == PauseSource.FakeLoading  && OnFLContinueGame!=null) OnFLContinueGame();
		if( !bPauseGame && pauseSource == PauseSource.UI  && OnUIContinueGame!=null) OnUIContinueGame();
	}

	public static bool IsGamePaused()
	{
		if(bPauseUI || bPauseFakeLoading) bPauseGame = true;
		else bPauseGame = false;
		return bPauseGame;
	}



	public delegate void PauseGameAction();
	public static event PauseGameAction OnPauseGame;

	public delegate void  UIContinueGameAction();
	public static event UIContinueGameAction OnUIContinueGame;

	public delegate void  FLContinueGameAction();
	public static event FLContinueGameAction OnFLContinueGame;


	public static void ClearAllPauseEvents()
	{
		OnPauseGame = null;
		OnFLContinueGame = null;
		OnUIContinueGame = null;
	}


	//***************************************************************
 
	public static string applicationID;
	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad(gameObject);
		#if UNITY_ANDROID || UNITY_EDITOR_WIN
        applicationID =  "com.Test.Package.Name"; //   "package.name";
		#elif UNITY_IOS
		applicationID = "bundle.ID";
		#endif
	}
	 

}
