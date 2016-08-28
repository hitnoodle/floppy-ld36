using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SplashSceneController : MonoBehaviour {

	public string NextScene;
	public float WaitTime;

	// Use this for initialization
	void Start () {
		StartCoroutine (IELoadScene (WaitTime, NextScene));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator IELoadScene(float waitLoad, string nextScene) {
		yield return new WaitForSeconds (waitLoad);
		LoadScene (nextScene);
	}

	void LoadScene(string nextScene) {
		SceneManager.LoadScene (nextScene);
	}
}
