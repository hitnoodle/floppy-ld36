using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TransferredFiles : MonoBehaviour {

	public List<TransferredFileModel> FileList;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TransferFile(FileStorage storageSource, List<File> sourceFiles) {
		foreach (File file in sourceFiles) {

			File fileModel = new File (file.Name, file.Size, file.ImageURL);
			fileModel.SetFileProgress (file.Progress.Value);

			TransferredFileModel transferredFileModel = new TransferredFileModel (storageSource.StorageType, fileModel);
			FileList.Add (transferredFileModel);

		}
	}
}
