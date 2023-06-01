using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class LevelTransition : MonoBehaviour {

	//TransitionDepartingStart
	static bool bFirstStart = true;
	static LevelTransition instance;

	static float DistMainScene = 0;
	  float Dist  = 0;

	public static LevelTransition Instance
	{
		get
		{
			if(instance == null)
			{
				instance = GameObject.FindObjectOfType(typeof(LevelTransition)) as LevelTransition;
			}
			return instance;
		}
	}


	void Start () 
	{
		 StartCoroutine(WaitStart());
	}

	IEnumerator WaitStart()
	{
		SetScale();
		yield return new WaitForEndOfFrame();
		SetScale();
		yield return new WaitForEndOfFrame();
		SetScale();
		yield return new WaitForEndOfFrame();
	}
 
	void SetScale()
	{
		
		float xPlanet = 1;
		float xPlanet2 = 0;
		
		foreach( Transform tr in  transform.GetChild(0) )
		{
			if(tr.name =="Planet") 
			{
				transform.localScale =  Vector3.one;
				xPlanet =  Mathf.Abs( Screen.width/2f      -             Camera.main.WorldToScreenPoint(	  tr.position ).x );
 
				if(Application.loadedLevelName == "MainScene")
				{
					DistMainScene = Mathf.Abs	(  xPlanet   );
					transform.localScale =  Vector3.one;
				}
				else
				{
					if(xPlanet != 0) transform.localScale = DistMainScene/ xPlanet  *    	Vector3.one;
				}
			}
			
		}
		
		
	}


 

 
	void Update  () {
	 
	}

	void Awake()
	{
		if(bFirstStart)
		{
			bFirstStart = false;
			transform.GetComponent<Animator>().SetTrigger("tStart");
		}
		else
		{
			LevelTransition.Instance.ShowScene();
			SetScale();
		}

	 
	
	}



	public void HideScene(string levelName)
	{

		StartCoroutine(SetBlockAll(0.05f,false));
		StartCoroutine("LoadScene" , levelName);
		transform.GetComponent<Animator>().SetTrigger("tClose");
	}

	public void ShowScene()
	{
		StartCoroutine(SetBlockAll(0.1f,true));
		transform.GetComponent<Animator>().SetTrigger("tOpen");
		if(Application.loadedLevelName == "MainScene") { StartCoroutine(SetBlockAll(6f,false));}
		else StartCoroutine(SetBlockAll(1.5f,false)); 
	}

	CanvasGroup BlockAll;

	IEnumerator SetBlockAll(float time, bool blockRays)
	{
		
		if(BlockAll == null) BlockAll = GameObject.Find("ForegroundBlockAll").GetComponent<CanvasGroup>();
		yield return new WaitForSeconds(time);
		BlockAll.blocksRaycasts = blockRays;
		
	}

	IEnumerator LoadScene (string levelName)
	{
		yield return new WaitForSeconds(1.5f);
		Application.LoadLevel (levelName);

	}
}
