using UnityEngine;
using System.Collections;

[System.Serializable]
public class LevelModel {

	public enum OpponentStorageType
	{
		NONE 		= 0,
		CD			= 1,
		DVD			= 2,
		THUMBDRIVE	= 3,
		CLOUD		= 4
	}

	public int Day;
	public File[] FileList;
	public OpponentStorageType OpponentStorage;

}
