using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour {
	MenuManager menuManager;
	public GameObject popUpShop;
	 
	CanvasGroup BlockAll;

	public static bool bShowShop = false;

	void Start () {

        bShowShop = false;
		menuManager = GameObject.Find("Canvas").GetComponent<MenuManager>();
		 
		BlockAll = GameObject.Find("ForegroundBlockAll").GetComponent<CanvasGroup>();

		if(Application.loadedLevelName == "CustomerScene")  transform.localScale =  (0.82f-( 1.7777f -(float) Screen.width/ (float) Screen.height )*0.45f) *Vector3.one;
		if(Application.loadedLevelName == "MapScene")  transform.localScale =  (0.80f-( 1.7777f -(float) Screen.width/ (float) Screen.height )*0.19f) *Vector3.one;
		if(Shop.RemoveAds !=2 && DataManager.Instance.Tutorial>=4 ) GameObject.Find("PopUps").GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-30);
	}

	public void btnBuyClick(string btnID)
	{
	
		BlockAll.blocksRaycasts = true;
		
		if(btnID  == "WatchVideo" ) 
		{
			Shop.Instance.bShopWatchVideo = true;
			Shop.Instance.WatchVideo( );
		}

		StartCoroutine(SetBlockAll(1f,false));
		SoundManager.Instance.Play_ButtonClick();
	}

	public void ShowPopUpShop()
	{
		if(BlockAll == null) BlockAll = GameObject.Find("ForegroundBlockAll").GetComponent<CanvasGroup>();
		BlockAll.blocksRaycasts = true;
//		InitPrices();
		if(menuManager == null) menuManager = GameObject.Find("Canvas").GetComponent<MenuManager>();
 
		menuManager.ShowPopUpMenu (popUpShop);
		StartCoroutine(SetBlockAll(1f,false));

		Camera.main.SendMessage("SetPauseOn",SendMessageOptions.DontRequireReceiver);
		menuManager.EscapeButonFunctionStack.Push("ClosePopUpShop");
		bShowShop = true;
	}

	IEnumerator SetBlockAll(float time, bool blockRays)
	{
		if(BlockAll == null) BlockAll = GameObject.Find("ForegroundBlockAll").GetComponent<CanvasGroup>();
		yield return new WaitForSeconds(time);
		BlockAll.blocksRaycasts = blockRays;
		 
	}

    //ovo je funkcija koja sluzi samo da pusti zvuk
	public void btnClicked_PlaySound()
	{
		SoundManager.Instance.Play_ButtonClick();
	}
}
