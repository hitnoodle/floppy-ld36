using UnityEngine;
using System.Collections;

public class PauseTransferEvent : GameEvent
{
    public string StorageID;

    public PauseTransferEvent(string storageID)
    {
        StorageID = storageID;
    }
}
