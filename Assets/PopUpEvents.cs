using UnityEngine;
using System.Collections;

public class PopUpEvents : MonoBehaviour {
	public static bool bMenuVisible = false;

	Animator anim;
	RectTransform rect;
	public void Start()
	{
		bMenuVisible = false;
		anim = gameObject.GetComponent<Animator>();
		rect = gameObject.GetComponent<RectTransform> ();
	}

	public void PopUpClosed()
	{
		Camera.main.SendMessage("PopUpClosed",gameObject);
		bMenuVisible = false;
	}



	public void PopUpOpened()
	{
		bMenuVisible = true;
		Camera.main.SendMessage("PopUpOpened",gameObject);
	}


	public void Open()
	{
		if(!bMenuVisible)
		{
			if(rect == null) rect = gameObject.GetComponent<RectTransform> ();
			rect.offsetMax = rect.offsetMin = new Vector2 (0, 0);

			if(anim == null)  anim = gameObject.GetComponent<Animator>();
			bMenuVisible = true;
			anim.SetTrigger("tOpen");
			anim.ResetTrigger("tClose");
		}
	}	
	public void Close()
	{
		if(anim == null)  anim = gameObject.GetComponent<Animator>();

		anim.ResetTrigger("tOpen");
		anim.SetTrigger("tClose");
	}
}
