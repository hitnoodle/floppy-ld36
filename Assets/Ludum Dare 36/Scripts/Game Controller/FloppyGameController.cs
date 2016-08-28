using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloppyGameController : MonoBehaviour {


	private LevelData _LevelData;
	private TransferredFiles _TransferredFile;

	public DaySplash DaySplashScreen;

	[Header("File Source")]
	public FileStorage HDDStorage;
	public FileStorage FloppyStorage;

    [Header("Opponent Source")]
    public IconUI CDIcon;
    public FileStorage CDStorage;

	[Header("Level")]
	public int CurrentLevelIndex = 0;
	public bool DoLoadLevel = false;
	public float NextLevelWaitTime = 1f;

	// Use this for initialization
	void Start () {
		_LevelData = GetComponent<LevelData> ();
		_TransferredFile = GetComponent<TransferredFiles> ();

		FloppyStorage.OnTransferFile += Floppy_OnTransferFile;
        CDStorage.OnTransferFile += CD_OnTransferFile;
		_TransferredFile.OnTransferDone += Handle_OnTransferDone;
	}

	void Handle_OnTransferDone ()
	{
		Debug.Log ("Alhamdulillah bu, sudah ditransfer. Udah tjuth");
		StartCoroutine (NextLevelRoutine ());

		FloppyStorage.StopEjectRoutine ();
		FloppyStorage.ShowPanel ();
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
			File newFile = new File (file.Name, file.Size, file.ImageURL);

			// pasti 100 di harddisk mah kalau hari pertama
			newFile.SetFileProgress (100);

			HDDStorage.GenerateFile (newFile);
		}

        CDIcon.gameObject.SetActive(levelModel.OpponentStorage == LevelModel.StorageType.CD);
        if (levelModel.OpponentStorage == LevelModel.StorageType.CD)
            CDStorage.ShowPanel();
    }

	private void DestroyPreviousLevel() {
		HDDStorage.DeleteFiles ();
	}
		

	void Floppy_OnTransferFile(FileStorage fileStorage, List<File> files) {
		_TransferredFile.TransferFile (fileStorage, files); 
	}

    void CD_OnTransferFile(FileStorage fileStorage, List<File> files) {
        _TransferredFile.TransferFile(fileStorage, files);
    }

    public LevelModel GetCurrentLevelModel() {
		return _LevelData.LevelArr [CurrentLevelIndex];
	}

	IEnumerator NextLevelRoutine() {
		yield return new WaitForSeconds (NextLevelWaitTime);
		CurrentLevelIndex++;
		LoadLevel (CurrentLevelIndex);
	}

}
