using UnityEngine;
using System.Collections;
using UnityEngine.UI;

///<summary>
///<para>Scene:All/NameOfScene/NameOfScene1,NameOfScene2,NameOfScene3...</para>
///<para>Object:N/A</para>
///<para>Description: Sample Description </para>
///</summary>

public class AnimEvents : MonoBehaviour {
	public static bool startShowned = false;
//	void Start()
//	{
//		StartCoroutine("StartingPopUp");
//	}
//
//	void LoadGamePlayScene()
//	{
//		Application.LoadLevel("LoadingGamePlayScene");
//	}
//
//	void LoadMainScene()
//	{
//		Application.LoadLevel("MainScene");
//	}
//
//	public void LoadAdequateScene()
//	{
//		if(GlobalVariables.SceneToLoad==0)
//		{
//			LoadMainScene();
//		}
//		else
//		{
//			LoadGamePlayScene();
//		}
//	}
//
//	IEnumerator StartingPopUp()
//	{
//		if(!startShowned)
//		{
//			if(Application.loadedLevelName.Equals("GamePlay") && !LevelGenerator.gameActive && !LevelGenerator.timeFreezeActive)
//			{
//				SoundManager.Instance.Play_LoadingDeparting();
//				yield return new WaitForSeconds(1);
//				if(GlobalVariables.GameplayMode==1)
//				{
//					if(LevelsParser.ListOfLevels[GlobalVariables.currentLevel-1].levelUnlockedItem==0)
//					{
//						GameObject.Find("Canvas").GetComponent<MenuManager>().ShowPopUpMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("PopUpGoal").gameObject);
//					}
//					else
//					{
//						SoundManager.Instance.Play_UnlockNewItem();
//						GameObject.Find("Canvas").GetComponent<MenuManager>().ShowPopUpMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("PopUpUnlocked").gameObject);
//					}
//				}
//				else if(GlobalVariables.GameplayMode==0)
//				{
//					//Chef Championship MODE 
//					GameObject.Find("Canvas").GetComponent<MenuManager>().ShowPopUpMessage("Chef Championship", "Make a unique burger and compete with other chefs!");
//				}
//				else if(GlobalVariables.GameplayMode==2)
//				{
//					//TimeAttack MODE
//					GameObject.Find("Canvas").GetComponent<MenuManager>().ShowPopUpMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("PopUpMessage").gameObject);
//					GameObject.Find("Canvas").GetComponent<MenuManager>().ShowPopUpMessage("Time Attack", "Follow the recipes, be fast and win great bonuses!");
//				}
//				startShowned = true;
//			}
//		}
//
//	}
//
//	public void TurnOfLoading()
//	{
//		transform.parent.parent.gameObject.SetActive(false);
//	}
//
//	public void StartWorldChooser()
//	{
//		GameObject.Find("Canvas").GetComponent<MenuManager>().ShowPopUpMenu(GameObject.Find("Canvas/Menus").transform.FindChild("WorldChooserMenu").gameObject);
//		GameObject.Find("Canvas").GetComponent<MenuManager>().SetArrivingStateOfWorldChooser();
//	}
//
//	public void AddTips()
//	{
////		StartCoroutine(GlobalVariables.Instance.moneyCounter(transform.parent.GetComponent<TipsCollect>().tipsAmount, GameObject.Find("CoinsText").GetComponent<Text>()));
//		GameObject.Find("LevelGenerator").GetComponent<LevelGenerator>().AddPointsToScore(transform.parent.GetComponent<TipsCollect>().tipsAmount);
//		LevelGenerator.tipsEarningsFinal += transform.parent.GetComponent<TipsCollect>().tipsAmount;
//	}
//
//	public void WinSparkles()
//	{
//		if(GlobalVariables.GameplayMode==1)
//		{
//			GameObject.Find("WinHolder/AnimationHolder").transform.FindChild("SparklesMain").gameObject.SetActive(true);
//		}
//	}
//
//	public void ChampionshipLeaderboard()
//	{
//		MenuManager.popupType = 4;
//		GameObject.Find("Canvas/PopUps").transform.FindChild("ChefChampionshipMenu/AnimationHolder/ChampionshipLeaderBoard").GetComponent<Animator>().Play("ChampionshipLeaderBoardArriving");
//	}

	public void CustomerArrivalSound()
	{
//		if(Application.loadedLevelName == "GamePlay")
//			SoundManager.Instance.Play_CustomerArrival();
	}

//	public void TimeUpDisableInteraction()
//	{
//		for(int i=0;i<14;i++)
//		{
//			GameObject.Find("IngredientsHolder").transform.GetChild(i).GetChild(0).GetComponent<Button>().interactable = false;
//		}
//
//		for(int i=0;i<4;i++)
//		{
//			GameObject.Find("SideFoodsHolder").transform.GetChild(i).GetChild(0).GetComponent<Button>().interactable = false;
//		}
//
//		GameObject.Find("AddTimeImage").GetComponent<Button>().interactable = false;
//		GameObject.Find("StopTimeImage").GetComponent<Button>().interactable = false;
//
//	}
//
//	public void StartTipsDeparting()
//	{
//		if(LevelGenerator.tipsTutorial)
//			transform.GetComponent<Animator>().Play("TipsDeparting");
//	}
}
