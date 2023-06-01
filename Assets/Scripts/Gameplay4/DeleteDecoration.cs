using UnityEngine;
using System.Collections;

public class DeleteDecoration : MonoBehaviour {


	public static bool bDelte = false;
	public static string sDelteName = "";
	public GameObject buttonDeletePosition;

	void Start () {
		bDelte = false;

		//Vector3 pos = 
		//transform.position = buttonDeletePosition.transform.position;
	}



	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
	 
		if(other.tag == "Decoration"  )  
		{
			bDelte = true;
			sDelteName = other.name;
		}

	}

	void OnTriggerExit2D(Collider2D other) 
	{
		if(other.name == sDelteName )  
		{
			bDelte = false;
			sDelteName = other.name;



		}
		
	}

}
