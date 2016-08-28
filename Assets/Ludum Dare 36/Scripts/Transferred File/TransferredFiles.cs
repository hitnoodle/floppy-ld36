using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TransferredFiles : MonoBehaviour {

	public List<TransferredFileModel> FileList;

	FloppyGameController _GameController;

	public delegate void _OnTransferDone();
	public _OnTransferDone OnTransferDone;

	// Use this for initialization
	void Start () {
		_GameController = GetComponent<FloppyGameController> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TransferFile(FileStorage storageSource, List<File> sourceFiles) {
		foreach (File file in sourceFiles) {

			// search file in file list
			bool foundSameFile = false;
			int sameFileIndex = 0;
			while (!foundSameFile && sameFileIndex < FileList.Count) {
				if (FileList [sameFileIndex].FileModel.Name == file.Name) {
					foundSameFile = true;
				} else {
					sameFileIndex++;
				}
			}

			if (foundSameFile) {
				FileList [sameFileIndex].FileModel.SetFileSize(file.Size);
				FileList [sameFileIndex].FileModel.SetFileProgress (file.Progress.Value);
			} else {
				File fileModel = new File (file.Name, file.Size, file.ImageURL);
				fileModel.SetFileProgress (file.Progress.Value);
				
				TransferredFileModel transferredFileModel = new TransferredFileModel (storageSource.StorageType, fileModel);
				FileList.Add (transferredFileModel);
			}
		}

		// check transfer done
		if (CheckTransferDone ()) {
			if (OnTransferDone != null) {
				OnTransferDone ();
			}

			//reset file list
			FileList.Clear();

		}

	}

	bool CheckTransferDone() {

		LevelModel currentLevelModel = _GameController.GetCurrentLevelModel ();

		bool isAllComplete = true;

		// cek jumlahnya sama dengan si level model
		if (currentLevelModel.FileList.Length == FileList.Count) {
			int i = 0;
			while (i < FileList.Count && isAllComplete) {
				TransferredFileModel file = FileList [i];
				
				if (file.FileModel.Progress.Value < 100) {
					// filenya belum selese dikopi tjuy
					isAllComplete = false;
				} else {
					// udah selese
					i++;
				}
			}
		} else {
			// jumlah filenya nggak sama dengan si level
			isAllComplete = false;
		}

		return isAllComplete;


	}
}
