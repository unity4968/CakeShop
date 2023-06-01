using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayCustomerOrder : MonoBehaviour {

	public GameObject CustomerOrderWindow;
	
	public Image CakeBase;
	public Image Cream;
	public Image Decoration1;
	public Image Decoration2;
	public Image Decoration3;
	public Image Decoration4;
	public Image Decoration5;
	public Image Decoration6;

	Vector2[] orderMenuDecorationsPositions = new Vector2[8];
	Sprite[] ItemiSprites ;
 
	public static bool bVisible= false;
	bool bEnableClick= true;


	void Start () {

		if(Application.loadedLevelName != "CustmerScene")
		{
			float scale = 1.5f + 2.66f * (    (float) Screen.height/ (float) Screen.width - .5625f  ); 
			CustomerOrderWindow.transform.localScale  = Vector3.one*scale;
		}

		bVisible= false;
		
		orderMenuDecorationsPositions[0] =  CakeBase.rectTransform.anchoredPosition;
		orderMenuDecorationsPositions[1] =  Cream.rectTransform.anchoredPosition;
		orderMenuDecorationsPositions[2] =  Decoration1.rectTransform.anchoredPosition;
		orderMenuDecorationsPositions[3] =  Decoration2.rectTransform.anchoredPosition;
		orderMenuDecorationsPositions[4] =  Decoration3.rectTransform.anchoredPosition;
		orderMenuDecorationsPositions[5] =  Decoration4.rectTransform.anchoredPosition;
		orderMenuDecorationsPositions[6] =  Decoration5.rectTransform.anchoredPosition;
		orderMenuDecorationsPositions[7] =  Decoration6.rectTransform.anchoredPosition;
		
		
		
		
		CustomerOrderWindow.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}





	bool noCream = false;
	
	void SetOrderImages()
	{ 
		
		string[] items = DataManager.Instance.CustomerOrder.Split(';');
		Cream.enabled = false;
		
		if(!items[1].StartsWith("m2") )noCream = true;
		else noCream = false;


		for(int i = 0; i<items.Length;i++)
		{
			
			string key = items[i];
			
			if(MenuItems.menuItemsDictionary.ContainsKey(key) )//&& MenuItems.menuItemsDictionary[key].Locked == false)
			{
				ItemiSprites =(Sprite[]) Resources.LoadAll<Sprite>( "Decorations/"+MenuItems.menuItemsDictionary[key].Atlas);
				foreach(Sprite sp in ItemiSprites)
				{
					if(noCream)
					{
						if(sp.name ==  key ) 
						{
							if(i == 0) CakeBase.sprite = sp;
							if(i == 1 ) Decoration1.sprite = sp;
							if(i == 2) Decoration2.sprite = sp;
							if(i == 3) Decoration3.sprite = sp;
							if(i == 4) Decoration4.sprite = sp;
							if(i == 5) Decoration5.sprite = sp;
							if(i == 6) Decoration6.sprite = sp;
						}
					}
					else
					{
						if(sp.name ==  key ) 
						{
							if(i == 0) CakeBase.sprite = sp;
							if(i == 1  ) {  Cream.sprite = sp; Cream.enabled = true;}
							if(i == 2) Decoration1.sprite = sp;
							if(i == 3) Decoration2.sprite = sp;
							if(i == 4) Decoration3.sprite = sp;
							if(i == 5) Decoration4.sprite = sp;
							if(i == 6) Decoration5.sprite = sp;
							if(i == 7) Decoration6.sprite = sp;
						}
					}
				}
			}
		}
		
	}
	
	
	IEnumerator ShowCustomerOrder()
	{
		
		CakeBase.rectTransform.anchoredPosition = orderMenuDecorationsPositions[0];
		Cream.rectTransform.anchoredPosition = orderMenuDecorationsPositions[1] ;
		Decoration1.rectTransform.anchoredPosition = orderMenuDecorationsPositions[2];
		Decoration2.rectTransform.anchoredPosition = orderMenuDecorationsPositions[3];
		Decoration3.rectTransform.anchoredPosition = orderMenuDecorationsPositions[4];
		Decoration4.rectTransform.anchoredPosition = orderMenuDecorationsPositions[5];
		Decoration5.rectTransform.anchoredPosition = orderMenuDecorationsPositions[6];
		Decoration6.rectTransform.anchoredPosition = orderMenuDecorationsPositions[7];
		
		SetOrderImages();
		if(!noCream )
		{
			if(   DataManager.Instance.SelectedLevelData.MaxIngredientsPerCustomer == 2)
			{
				Decoration1.enabled = true;
				Decoration2.enabled = false;
				Decoration3.enabled = false;
				Decoration4.enabled = false;
				Decoration5.enabled = false;
				Decoration6.enabled = false;
				
				Decoration1.rectTransform.anchoredPosition+=new Vector2( -Decoration1.rectTransform.anchoredPosition.x,80);
				
				CakeBase.rectTransform.anchoredPosition+=new Vector2( 0,40);
				Cream.rectTransform.anchoredPosition+=new Vector2( 0,65);
			}
			else if (DataManager.Instance.SelectedLevelData.MaxIngredientsPerCustomer == 3)
			{
				Decoration1.enabled = true;
				Decoration2.enabled = true;
				Decoration3.enabled = false;
				Decoration4.enabled = false;
				Decoration5.enabled = false;
				Decoration6.enabled = false;
				
				Decoration1.rectTransform.anchoredPosition+=new Vector2( -Decoration1.rectTransform.anchoredPosition.x,35);
				Decoration2.rectTransform.anchoredPosition+=new Vector2( -Decoration2.rectTransform.anchoredPosition.x,35);
				CakeBase.rectTransform.anchoredPosition+=new Vector2( 0,25);
				Cream.rectTransform.anchoredPosition+=new Vector2( 0,30);
			}
			
			if(     DataManager.Instance.SelectedLevelData.MaxIngredientsPerCustomer == 4)
			{
				Decoration1.enabled = true;
				Decoration2.enabled = true;
				Decoration3.enabled = true;
				Decoration4.enabled = false;
				Decoration5.enabled = false;
				Decoration6.enabled = false;
				
				Decoration1.rectTransform.anchoredPosition+=new Vector2(- Decoration1.rectTransform.anchoredPosition.x,0);
				Decoration2.rectTransform.anchoredPosition+=new Vector2(-Decoration2.rectTransform.anchoredPosition.x,0);
				Decoration3.rectTransform.anchoredPosition+=new Vector2(-Decoration3.rectTransform.anchoredPosition.x,0);
			}
			
			if(   DataManager.Instance.SelectedLevelData.MaxIngredientsPerCustomer == 5)
			{
				Decoration1.enabled = true;
				Decoration2.enabled = true;
				Decoration3.enabled = false;
				Decoration4.enabled = true;
				Decoration5.enabled = true;
				Decoration6.enabled = false;
				
				Decoration1.rectTransform.anchoredPosition+=new Vector2( 0,35);
				Decoration2.rectTransform.anchoredPosition+=new Vector2(0,35);
				Decoration4.rectTransform.anchoredPosition+=new Vector2( 0,35);
				Decoration5.rectTransform.anchoredPosition+=new Vector2(0,35);
				
				CakeBase.rectTransform.anchoredPosition+=new Vector2( 0,25);
				Cream.rectTransform.anchoredPosition+=new Vector2( 0,30);
			}
			
			if(   DataManager.Instance.SelectedLevelData.MaxIngredientsPerCustomer == 6)
			{
				Decoration1.enabled = true;
				Decoration2.enabled = true;
				Decoration3.enabled = true;
				Decoration4.enabled = true;
				Decoration5.enabled = true;
				Decoration6.enabled = false;
				
				Decoration3.rectTransform.anchoredPosition+=new Vector2( -Decoration3.rectTransform.anchoredPosition.x,0);
			}
			
			//*************************************************************************
		}
		else
		{
			if(  DataManager.Instance.SelectedLevelData.MaxIngredientsPerCustomer == 2)
			{
				Decoration1.enabled = true;
				Decoration2.enabled = true;
				Decoration3.enabled = false;
				Decoration4.enabled = false;
				Decoration5.enabled = false;
				Decoration6.enabled = false;
				
				Decoration1.rectTransform.anchoredPosition+=new Vector2( -Decoration1.rectTransform.anchoredPosition.x,15);
				Decoration2.rectTransform.anchoredPosition+=new Vector2( -Decoration2.rectTransform.anchoredPosition.x,25);
				CakeBase.rectTransform.anchoredPosition+=new Vector2( 0,45);
				
			}
			
			
			if (DataManager.Instance.SelectedLevelData.MaxIngredientsPerCustomer == 3)
			{
				Decoration1.enabled = true;
				Decoration2.enabled = true;
				Decoration3.enabled = true;
				Decoration4.enabled = false;
				Decoration5.enabled = false;
				Decoration6.enabled = false;
				
				Decoration1.rectTransform.anchoredPosition+=new Vector2(- Decoration1.rectTransform.anchoredPosition.x,-10);
				Decoration2.rectTransform.anchoredPosition+=new Vector2(-Decoration2.rectTransform.anchoredPosition.x,-10);
				Decoration3.rectTransform.anchoredPosition+=new Vector2(-Decoration3.rectTransform.anchoredPosition.x,-10);
				CakeBase.rectTransform.anchoredPosition+=new Vector2( 0,25);
			}
			
			
			
			if(     DataManager.Instance.SelectedLevelData.MaxIngredientsPerCustomer == 4)
			{
				Decoration1.enabled = true;
				Decoration2.enabled = true;
				Decoration3.enabled = false;
				Decoration4.enabled = true;
				Decoration5.enabled = true;
				Decoration6.enabled = false;
				
				Decoration1.rectTransform.anchoredPosition+=new Vector2( 0,15);
				Decoration2.rectTransform.anchoredPosition+=new Vector2(0,25);
				Decoration4.rectTransform.anchoredPosition+=new Vector2( 0,15);
				Decoration5.rectTransform.anchoredPosition+=new Vector2(0,25);
				
				CakeBase.rectTransform.anchoredPosition+=new Vector2( 0,45);
				
			}
			
			
			
			if(   DataManager.Instance.SelectedLevelData.MaxIngredientsPerCustomer == 5)
			{
				Decoration1.enabled = true;
				Decoration2.enabled = true;
				Decoration3.enabled = true;
				Decoration4.enabled = true;
				Decoration5.enabled = true;
				Decoration6.enabled = false;
				
				Decoration1.rectTransform.anchoredPosition+=new Vector2( 0,-10);
				Decoration2.rectTransform.anchoredPosition+=new Vector2(0,-10);
				Decoration3.rectTransform.anchoredPosition+=new Vector2( -Decoration3.rectTransform.anchoredPosition.x,-10);
				Decoration4.rectTransform.anchoredPosition+=new Vector2(0,-10);
				Decoration5.rectTransform.anchoredPosition+=new Vector2( 0,-10);
				
				CakeBase.rectTransform.anchoredPosition+=new Vector2( 0,25);
			}
			
			
			if(   DataManager.Instance.SelectedLevelData.MaxIngredientsPerCustomer == 6)
			{
				Decoration1.enabled = true;
				Decoration2.enabled = true;
				Decoration3.enabled = true;
				Decoration4.enabled = true;
				Decoration5.enabled = true;
				Decoration6.enabled = true;
				
				Decoration1.rectTransform.anchoredPosition+=new Vector2( 0,-10);
				Decoration2.rectTransform.anchoredPosition+=new Vector2(0,-10);
				Decoration3.rectTransform.anchoredPosition+=new Vector2( 0,-10);
				Decoration4.rectTransform.anchoredPosition+=new Vector2(0,-10);
				Decoration5.rectTransform.anchoredPosition+=new Vector2( 0,-10);
				Decoration6.rectTransform.anchoredPosition+=new Vector2(0,-10);
				
				CakeBase.rectTransform.anchoredPosition+=new Vector2( 0,25);
				
				
			}
			
		}

		yield return new WaitForSeconds(.018f);
		CustomerOrderWindow.SetActive(true);
	}
	





	public void btnCustomerOrderClicked()
	{
		if(Application.loadedLevelName =="Gameplay4" && TutorialGP4.TutorialPhase != 1 && Gameplay4.bTutorial)
		{
			return;
		}
		if(bEnableClick)
		{
			SoundManager.Instance.Play_ButtonClick();
			bEnableClick = false;
			if(!bVisible)
			{
				StartCoroutine(ShowCustomerOrder());	
				bVisible = true;
				EnableMoveObjects(false);
				if(Application.loadedLevelName =="Gameplay4" && TutorialGP4.TutorialPhase == 1)
				{
//					Camera.main.SendMessage("NextPhase");
					Debug.Log("GSG");
				Camera.main.SendMessage("ShowHelpCloseCustomerOrder");
 				}

			}
			else
			{
				bVisible = false;
				CustomerOrderWindow.SetActive(false);
				EnableMoveObjects(true);
				if(Application.loadedLevelName =="Gameplay4" && TutorialGP4.TutorialPhase == 1)
				{
					Camera.main.SendMessage("NextPhase");
				}
			}
			StartCoroutine(EnableClick());
		}

	}

	IEnumerator EnableClick()
	{
		yield return new WaitForSeconds(.5f);
		bEnableClick = true;
	}


	void EnableMoveObjects(bool value)
	{
		if(Application.loadedLevelName == "Gameplay1")  Camera.main.GetComponent<Gameplay1>().bMenuVisible = !value;
		if(Application.loadedLevelName == "Gameplay2a")  Camera.main.GetComponent<Gameplay2a>().bEnableMove = value;
		if(Application.loadedLevelName == "Gameplay3")  Camera.main.GetComponent<Gameplay3>().bEnableMove = value;
	}

}
