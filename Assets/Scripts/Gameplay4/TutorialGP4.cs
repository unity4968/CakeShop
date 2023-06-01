using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class TutorialGP4 : MonoBehaviour {

	public static int TutorialPhase =0;

	public GameObject  HandPointer;
	int NextPositionNo = 0;
	public Transform[] handPointerPositions;
	public Text txtTutorial;
	public Animator animTutorial;

	public ScrollRect MenuItemsHolder;
	float timePauseDialog = 2.5f;
	void Awake()
	{
		txtTutorial.text = "";
		iTween.FadeTo(HandPointer,0,0f);
	}

	// Use this for initialization
	void Start () {
	 



	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TutorialSteps(int tutStep)
	{
		switch (tutStep)
		{
			
			case 1:

			StartCoroutine(WaitTutorialSteps(1));
			break;
		}
	}


	IEnumerator   WaitTutorialSteps(int tutStep)
	{
		switch (tutStep)
		{

			case 1:
				animTutorial.SetTrigger("tShow");
				MenuItemsHolder.vertical = false;
				//yield return new WaitForSeconds(.3f);
		//	txtTutorial.text = "SO, THIS IS YOUR FIRST DAY ON THE JOB?";
		//		yield return new WaitForSeconds(timePauseDialog);
			txtTutorial.text = "LET ME HELP YOU\nWITH THE FIRST ORDER";
				yield return new WaitForSeconds(timePauseDialog);
				ShowPointer();
				txtTutorial.text = "CLICK ON THE BUTTON\nTO SEE THE ORDER";
				HandPointer.transform.position = handPointerPositions[0].position;
				yield return new WaitForSeconds(0.3f);
				iTween.MoveTo( HandPointer , handPointerPositions[1].position,1f);
				TutorialPhase= 1;
				break;
		 
		case 2:
			HidePointer();
			yield return new WaitForSeconds(.5f);
			 
			txtTutorial.text = "SOMEBODY ORDERED A CHOCOLATE CAKE";
			yield return new WaitForSeconds(timePauseDialog);
			ShowPointer();
			txtTutorial.text = "CHOOSE A CAKE BASE \nFROM THE LEFT MENU";

			yield return new WaitForSeconds(0.3f);
			iTween.MoveTo( HandPointer , handPointerPositions[2].position,1f);
			TutorialPhase= 4;
			break;


		case 3:
			HidePointer();
			yield return new WaitForSeconds(.5f);
			
			txtTutorial.text = "NOW, LET'S MAKE THE CREAM\nFOR OUR DELICIOUS CAKE\nCLICK ON THIS BUTTON";
//			yield return new WaitForSeconds(timePauseDialog);
//			txtTutorial.text = "CLICK ON THIS BUTTON";
			ShowPointer();
			yield return new WaitForSeconds(0.3f);
			iTween.MoveTo( HandPointer , handPointerPositions[3].position,1f);
			TutorialPhase= 6; 
			break;
		
		case 4:
			//HidePointer();
			yield return new WaitForSeconds(.5f);
			
			txtTutorial.text = "AND SELECT THE CHOCOLATE CREAM";

			yield return new WaitForSeconds(0.3f);
			iTween.MoveTo( HandPointer , handPointerPositions[4].position,1f);
			TutorialPhase= 8; 
			break;

		case 5:
			HidePointer();
			yield return new WaitForSeconds(.5f);

			txtTutorial.text = "ONE LAST THING AND WE ARE DONE!\nDECORATION ON TOP!";
			yield return new WaitForSeconds(timePauseDialog);
			txtTutorial.text = "CLICK ON THE FRUIT\nDECORATIONS BUTTON";
			ShowPointer();
			yield return new WaitForSeconds(0.3f);
			iTween.MoveTo( HandPointer , handPointerPositions[5].position,1f);
			TutorialPhase= 10; 
			break;

		case 6:
			//HidePointer();
			yield return new WaitForSeconds(.5f);
			
			txtTutorial.text = "AND SELECT DECORATION";
			
			yield return new WaitForSeconds(0.3f);
			iTween.MoveTo( HandPointer , handPointerPositions[6].position,1f);
			TutorialPhase= 12; 
			break;


		 

		case 7:
			 HidePointer();
			yield return new WaitForSeconds(.5f);
			
			txtTutorial.text = "EXCELLENT! YOU WILL BECOME A VERY FAMOUS CHEF";
			yield return new WaitForSeconds(timePauseDialog);
			txtTutorial.text = "YOU CAN MOVE, SCALE AND ROTATE THE DECORATION!";
			yield return new WaitForSeconds(timePauseDialog);

			txtTutorial.text = "THIS DELICIOUS CAKE IS READY!! \nCLICK THE FINISH BUTTON AND SERVE IT.";
			ShowPointer();
			yield return new WaitForSeconds(0.3f);
			iTween.MoveTo( HandPointer , handPointerPositions[7].position,1f);
			TutorialPhase= 14; 

			DataManager.Instance.Tutorial = 4;
			DataManager.Instance.SaveTutorialProgress();

			yield return new WaitForSeconds(4f);
			animTutorial.SetTrigger("tHide");
			break;

		case 8:
			HidePointer();
			break;
		}

	

	}

	public void ShowHelpCloseCustomerOrder()
	{
		Debug.Log("GSG");
		iTween.MoveTo( HandPointer    ,   iTween.Hash("position",  handPointerPositions[8].position,   "time", 1f,     "delay", 2 ));
	}

	public void NextPhase()
	{
		TutorialPhase++;
		if(TutorialPhase ==2)
			StartCoroutine(WaitTutorialSteps(2));
		if(TutorialPhase ==5)
			StartCoroutine(WaitTutorialSteps(3));
		if(TutorialPhase ==7)
			StartCoroutine(WaitTutorialSteps(4));
		if(TutorialPhase ==9)
			StartCoroutine(WaitTutorialSteps(5));
		if(TutorialPhase ==11)
			StartCoroutine(WaitTutorialSteps(6));
		if(TutorialPhase ==13)
			StartCoroutine(WaitTutorialSteps(7));
		if(TutorialPhase ==15)
			StartCoroutine(WaitTutorialSteps(8));

	}

	
	//******************************************************
//	public void GoalPointer()
//	{
//		 
//		if(DataManager.Instance.Tutorial >=3 && bShowHelp) 
//			StartCoroutine(WaitToHidePointer (2.5f));
//		iTween.MoveTo( HandPointer , OvenPointerPosition.position,1f);
//		
//	}

	IEnumerator WaitToShowPointer(float timeW)
	{
		yield return new WaitForSeconds(timeW);
		ShowPointer();
		
	}

	IEnumerator WaitToHidePointer(float timeW)
	{
		yield return new WaitForSeconds(timeW);
		HidePointer();
		
	}
	public void HidePointer()
	{
		 
		iTween.FadeTo(HandPointer,0,0.3f);
	}
	
	public void ShowPointer()
	{
		//if(Gameplay4.bTutorial) //ako nije prikazan treci tutorijal
		//{
			 
			HandPointer.SetActive(true);
			iTween.FadeTo(HandPointer,1,0.3f);
			
		//}
	}
	
	public void NextPointer( )
	{
		NextPositionNo++;
		iTween.MoveTo( HandPointer , handPointerPositions[1].position,1f);
	}
	
	public void HelpNextPointer( )
	{
		
		ShowPointer();
		
//		if(NextPositionNo == 0) 
//		{
//			if(tepsija.bDrag) GoalPointer();
//			else iTween.MoveTo( HandPointer , handPointerPositions[0].position,1f);
//		}
		 
	}



}
