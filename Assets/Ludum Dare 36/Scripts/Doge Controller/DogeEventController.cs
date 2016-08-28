using UnityEngine;
using System.Collections;

public class DogeEventController : MonoBehaviour {

	FloppyGameController _FloppyGameController;

	public float WaitTime = 2f;

	// Use this for initialization
	void Start () {
		_FloppyGameController = GetComponent<FloppyGameController> ();
		_FloppyGameController.OnLoadLevel += Handle_OnLoadLevel;
	}

	void Handle_OnLoadLevel (int levelIndex)
	{
		// hardcoded events

		if (levelIndex == 0) {
			EventManager.Instance.TriggerEvent (new DogeStartSpeakEvent ("TRANSPARENT", 0));
			EventManager.Instance.TriggerEvent (new DogeStartSpeakEvent ("TALK_FLOPPY", 3));
			EventManager.Instance.TriggerEvent (new DogeStartSpeakEvent ("STOP_TALK_FLOPPY", 6));

			// CD
		} else if (levelIndex == 1) {
			EventManager.Instance.TriggerEvent (new DogeStartSpeakEvent ("TALK_CD", 0));
			EventManager.Instance.TriggerEvent (new DogeStartSpeakEvent ("TALK_CD2", 1));
			EventManager.Instance.TriggerEvent (new DogeStartSpeakEvent ("TALK_CD", 2));
			EventManager.Instance.TriggerEvent (new DogeStartSpeakEvent ("TALK_CD2", 3));
			EventManager.Instance.TriggerEvent (new DogeStartSpeakEvent ("STOP_TALK_CD", 4));
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
