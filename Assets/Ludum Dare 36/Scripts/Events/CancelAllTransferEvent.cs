using UnityEngine;
using System.Collections;

public class CancelAllTransferEvent : GameEvent
{
    public string StorageID;

    public CancelAllTransferEvent(string storageID)
    {
        StorageID = storageID;
    }
}
