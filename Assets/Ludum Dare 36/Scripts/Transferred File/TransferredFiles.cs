using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TransferredFiles : MonoBehaviour {

	public List<TransferredFileModel> TransferredList;

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
			while (!foundSameFile && sameFileIndex < TransferredList.Count) {
				if (TransferredList [sameFileIndex].FileModel.Name == file.Name) {
					foundSameFile = true;
				} else {
					sameFileIndex++;
				}
			}

			if (foundSameFile) {
				TransferredList [sameFileIndex].FileModel.SetFileSize(file.Size);
				TransferredList [sameFileIndex].FileModel.SetFileProgress (file.Progress.Value);
			} else {
				File fileModel = new File (file.Name, file.Size, file.ImageURL);
				fileModel.SetFileProgress (file.Progress.Value);
				
				TransferredFileModel transferredFileModel = new TransferredFileModel (storageSource.StorageType, fileModel);
				TransferredList.Add (transferredFileModel);
			}
		}

		// check transfer done
		if (CheckTransferDone ()) {
			if (OnTransferDone != null) {
				OnTransferDone ();
			}

			//reset file list
			TransferredList.Clear();

		}

	}

	bool CheckTransferDone() {

		LevelModel currentLevelModel = _GameController.GetCurrentLevelModel ();

		bool isAllComplete = true;

		// aggregate dulu semua transferred list
		List<TransferredFileModel> transferredFiles = new List<TransferredFileModel>();

		for (int i = 0; i < currentLevelModel.FileList.Length; i++) {
			// itung file yang ini udah 100% belom
			File currentFile = currentLevelModel.FileList[i];

			// cari di transferred list
			float fileProgress = 0;
			for (int j = 0; j < TransferredList.Count; j++) {
				if (TransferredList[j].FileModel.Name == currentFile.Name) {
					fileProgress += TransferredList [j].FileModel.Progress.Value;
				}
			}

			// masih ada yang belum 100 tuh progressnya
			if (fileProgress < 100) {
				isAllComplete = false;
			}

		}

//		// cek jumlahnya sama dengan si level model
//		if (currentLevelModel.FileList.Length == TransferredList.Count) {
			
//			int i = 0;
//			while (i < TransferredList.Count && isAllComplete) {
//				TransferredFileModel file = TransferredList [i];
//				
//				if (file.FileModel.Progress.Value < 100) {
//					// filenya belum selese dikopi tjuy
//					isAllComplete = false;
//				} else {
//					// udah selese
//					i++;
//				}
//			}

//
//		} else {
//			// jumlah filenya nggak sama dengan si level
//			isAllComplete = false;
//		}

		return isAllComplete;


	}
}
