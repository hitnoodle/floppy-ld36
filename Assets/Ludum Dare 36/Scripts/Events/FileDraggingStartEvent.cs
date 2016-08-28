using UnityEngine;
using System.Collections;

public class FileDraggingStartEvent : GameEvent
{
    public string FileName;
    public string StorageName;

    public FileDraggingStartEvent(string fileName, string storageName)
    {
        FileName = fileName;
        StorageName = storageName;
    }
}
