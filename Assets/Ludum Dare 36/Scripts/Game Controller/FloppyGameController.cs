using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class FloppyGameController : MonoBehaviour {

    [System.Serializable]
    public class OpponentData
    {
        public LevelModel.StorageType StorageType;
        public IconUI Icon;
        public FileStorage Storage;
    }

	private LevelData _LevelData;
	private TransferredFiles _TransferredFile;

	public DaySplash DaySplashScreen;

	[Header("File Source")]
	public FileStorage HDDStorage;
	public FileStorage FloppyStorage;

    [Header("Opponent Source")]

    public OpponentData[] Opponents;
    public OpponentData OpponentCurrent;

	[Header("Level")]
	public int CurrentLevelIndex = 0;
	public bool DoLoadLevel = false;
	public float NextLevelWaitTime = 1f;

	public delegate void _OnLoadLevel(int levelIndex);
	public _OnLoadLevel OnLoadLevel;

	// Use this for initialization
	void Start () {
		_LevelData = GetComponent<LevelData> ();
		_TransferredFile = GetComponent<TransferredFiles> ();

		FloppyStorage.OnTransferFile += Floppy_OnTransferFile;
		_TransferredFile.OnTransferDone += Handle_OnTransferDone;
    }

	void Handle_OnTransferDone ()
	{
		Debug.Log ("Alhamdulillah bu, sudah ditransfer. Udah tjuth");

        FloppyStorage.StopEjectRoutine();
        //FloppyStorage.ShowPanel();
        HDDStorage.HidePanel();

        StartCoroutine (NextLevelRoutine ());
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

		if (levelIndex < _LevelData.LevelArr.Length) {
			LevelModel levelModel = _LevelData.LevelArr [levelIndex];
			
			DaySplashScreen.ShowSplash (levelModel.Day);
			
			foreach (File file in levelModel.FileList) {
				File newFile = new File (file.Name, file.Size, file.ImageURL);
				
				// pasti 100 di harddisk mah kalau hari pertama
				newFile.SetFileProgress (100);
				
				HDDStorage.GenerateFile (newFile);
			}

            if (levelModel.OpponentStorage != LevelModel.StorageType.NONE)
            {
                OpponentData data = Opponents.Where(x => x.StorageType == levelModel.OpponentStorage).First();
                OpponentCurrent = data;

                OpponentCurrent.Icon.gameObject.SetActive(true);
                OpponentCurrent.Storage.ShowPanel();
                OpponentCurrent.Storage.OnTransferFile += Storage_OnTransferFile;

                CDAiTest aiTest = OpponentCurrent.Storage.gameObject.GetComponent<CDAiTest>();
                if (aiTest != null)
                    aiTest.StartAI();
            }

            foreach (GameObject go in levelModel.ObjectsToEnable)
                go.SetActive(true);

			if (OnLoadLevel != null) {
				OnLoadLevel (levelIndex);
			}
		} 
		else 
		{
			SceneManager.LoadScene ("SplashScene");
		}
    }

	private void DestroyPreviousLevel() {
		HDDStorage.DeleteFiles ();

        if (OpponentCurrent != null)
        {
            if (OpponentCurrent.Icon != null) OpponentCurrent.Icon.gameObject.SetActive(false);
            if (OpponentCurrent.Storage != null)
            {
                OpponentCurrent.Storage.OnTransferFile -= Storage_OnTransferFile;
                OpponentCurrent.Storage.StopEjectRoutine();
                OpponentCurrent.Storage.HidePanel();

                CDAiTest aiTest = OpponentCurrent.Storage.gameObject.GetComponent<CDAiTest>();
                if (aiTest != null)
                    aiTest.StopAI();
            } 

            OpponentCurrent = null;
        }

        FloppyStorage.HidePanel();
	}

	void Floppy_OnTransferFile(FileStorage fileStorage, List<File> files) {
		_TransferredFile.TransferFile (fileStorage, files); 
	}

    void Storage_OnTransferFile(FileStorage fileStorage, List<File> files) {
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
