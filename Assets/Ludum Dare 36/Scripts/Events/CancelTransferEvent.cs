using UnityEngine;
using System.Collections;

public class CancelTransferEvent : GameEvent
{
    public string StorageID;

    public CancelTransferEvent(string storageID)
    {
        StorageID = storageID;
    }
}
