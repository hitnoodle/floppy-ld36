using UnityEngine;
using System.Collections;

[System.Serializable]
public class LevelModel {

	public enum StorageType
	{
		NONE 		= 0,
		FLOPPY		= 1,
		CD			= 2,
		DVD			= 3,
		THUMBDRIVE	= 4,
		CLOUD		= 5
	}

	public int Day;
	public File[] FileList;
	public StorageType OpponentStorage;

}
