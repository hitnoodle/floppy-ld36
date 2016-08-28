using UnityEngine;
using System.Collections;

public class DogeStartSpeakEvent : GameEvent {

	public string ID;
	public float WaitTime;

	public DogeStartSpeakEvent(string id, float waitTime) {
		this.ID = id;
		this.WaitTime = waitTime;
	}

}
