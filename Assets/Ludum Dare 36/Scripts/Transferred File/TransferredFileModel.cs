using UnityEngine;
using System.Collections;

[System.Serializable]
public class TransferredFileModel{
	public LevelModel.StorageType StorageType;
	public File FileModel;

	public TransferredFileModel(LevelModel.StorageType storageType, File fileModel) {
		this.StorageType 	= storageType;
		this.FileModel 		= fileModel;
	}
}
