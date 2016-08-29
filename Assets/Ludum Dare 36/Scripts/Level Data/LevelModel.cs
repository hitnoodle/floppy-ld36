using UnityEngine;
using System.Collections;

[System.Serializable]
public class LevelModel {

	public enum StorageType
	{
		NONE 		= 0,
		HDD			= 1,
		FLOPPY		= 2,
		CD			= 4,
		DVD			= 5,
		THUMBDRIVE	= 6,
		CLOUD		= 7
	}

	public int Day;
	public File[] FileList;
	public StorageType OpponentStorage;
    public GameObject[] ObjectsToEnable;
}
