using UnityEngine;
using System.Collections;

public class BacgroundBorder : MonoBehaviour {
	public Transform ResetkaL;
	public Transform ResetkaR;
	// Use this for initialization
	void Start () {
		ResetkaL.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0,0.5f,10));
		ResetkaR.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(1,0.5f,10));
	}
	
	// Update is called once per frame
	void Update () {
		ResetkaL.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0,0.5f,10));
		ResetkaR.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(1,0.5f,10));
	}
}
