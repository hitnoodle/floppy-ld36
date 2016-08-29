using UnityEngine;
using System.Collections;

public class PauseTransferEvent : GameEvent
{
    public string StorageID;
    public bool ResetAfterPause;

    public PauseTransferEvent(string storageID)
    {
        StorageID = storageID;
        ResetAfterPause = true;
    }

    public PauseTransferEvent(string storageID, bool resetAfterPause)
    {
        StorageID = storageID;
        ResetAfterPause = resetAfterPause;
    }
}
