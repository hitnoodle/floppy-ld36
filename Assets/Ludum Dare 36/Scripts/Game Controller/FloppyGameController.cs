using UnityEngine;
using System.Collections;

public class FloppyGameController : MonoBehaviour {

	public FileStorage HDDStorage;

	private LevelData _LevelData;

	public int CurrentLevelIndex = 0;

	public bool DoLoadLevel = false;

	// Use this for initialization
	void Start () {
		_LevelData = GetComponent<LevelData> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (DoLoadLevel) {
			DoLoadLevel = false;

			LoadLevel (CurrentLevelIndex);
		}
	
	}

	public void LoadLevel(int levelIndex) {

		DestroyPreviousLevel ();

		LevelModel levelModel = _LevelData.LevelArr [levelIndex];
		foreach (File file in levelModel.FileList) {
			HDDStorage.GenerateFile (file);
		}
	}

	private void DestroyPreviousLevel() {
		HDDStorage.DeleteFiles ();
	}

}
