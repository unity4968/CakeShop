using UnityEngine;
using UnityEngine.EventSystems;

public class DragTreshold : MonoBehaviour {

	private const float inchToCm = 2.54f;
	private EventSystem eventSystem = null;
	private float dragThresholdCM = 0.5f; //vrednost u cm

	// Use this for initialization
	void Start () {
		if (eventSystem == null)
		{
			eventSystem = GetComponent<EventSystem>();
		}
		SetDragThreshold();
	}
	
	private void SetDragThreshold()
	{
		if (eventSystem != null)
		{
			eventSystem.pixelDragThreshold = (int)( dragThresholdCM * Screen.dpi / inchToCm);
		}
	}
}
