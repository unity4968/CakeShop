using UnityEngine;
using System.Collections;
 
using UnityEngine.EventSystems;

public class DecorationMenuButton : MonoBehaviour ,IPointerDownHandler // , IPointerEnterHandler 
{
	public bool bDecorationUnlocked = true;
	string name = "";
	Vector3 downPos;
	Vector3 upPos;
	bool bDown = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonUp(0) && bDown)
	   {
			bDown = false;
			upPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
			if(Mathf.Abs(upPos.y-downPos.y) <0.08f)
			{
				//		Input.mousePosition = 
				if(bDecorationUnlocked)
				{
					Camera.main.SendMessage( "ClickItem",transform.name, SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					Camera.main.SendMessage( "ShowBuyDecorationMenu",transform.name, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}

	public void  OnPointerDown( PointerEventData poinrterData)
	{
		 
		bDown = true;
		downPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		Debug.Log(downPos);
	}
}
