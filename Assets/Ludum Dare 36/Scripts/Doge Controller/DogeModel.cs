using UnityEngine;
using System.Collections;

[System.Serializable]
public class DogeModel{

	public enum DOGE_ANIMATIONS{
		TRANSPARENT = 0,
		IDLE		= 1,
		BARK		= 2,
		STOP		= 3
	}


	public string ID;
	public DOGE_ANIMATIONS Animation;
	public bool IsShowObject;
	public GameObject SpeakObject;
	public Vector2 Position;

}
