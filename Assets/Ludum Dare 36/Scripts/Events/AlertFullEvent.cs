using UnityEngine;
using System.Collections;

public class AlertFullEvent : GameEvent
{
    public string StorageID;

    public AlertFullEvent(string storageId)
    {
        StorageID = storageId;
    }
}
