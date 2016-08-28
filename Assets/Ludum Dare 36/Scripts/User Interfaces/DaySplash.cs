using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DaySplash : MonoBehaviour {

	[SerializeField]
	protected RectTransform Container;

	[SerializeField]
	protected Text DayText;

	public string Prefix = "Day ";
	public string Suffix = ".";
	public float WaitTime = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowSplash(int day) {		
		StartCoroutine (SplashRoutine (day));
	}

	public void HideSplash() {
		Container.gameObject.SetActive (false);
	}

	IEnumerator SplashRoutine(int day) {
		DayText.text = Prefix + day.ToString () + Suffix;
		Container.gameObject.SetActive (true);

		yield return new WaitForSeconds (WaitTime);

		Container.gameObject.SetActive (false);
	}
}
