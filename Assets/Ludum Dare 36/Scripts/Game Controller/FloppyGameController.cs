using UnityEngine;
using System.Collections;

public class FloppyGameController : MonoBehaviour {


	private LevelData _LevelData;
	private TransferredFiles _TransferredFile;

	public DaySplash DaySplashScreen;

	[Header("File Source")]
	public FileStorage HDDStorage;

	[Header("Level")]
	public int CurrentLevelIndex = 0;
	public bool DoLoadLevel = false;

	// Use this for initialization
	void Start () {
		_LevelData = GetComponent<LevelData> ();
		_TransferredFile = GetComponent<TransferredFiles> ();
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

		DaySplashScreen.ShowSplash (levelModel.Day);

		foreach (File file in levelModel.FileList) {
			HDDStorage.GenerateFile (file);
		}
	}

	private void DestroyPreviousLevel() {
		HDDStorage.DeleteFiles ();
	}

	public void TransferFile(LevelModel.StorageType storageType, File file) {

		TransferredFileModel transferredFile = new TransferredFileModel () {
			StorageType = storageType,
			FileModel = file
		};

		_TransferredFile.FileList.Add (transferredFile);
	}

}
