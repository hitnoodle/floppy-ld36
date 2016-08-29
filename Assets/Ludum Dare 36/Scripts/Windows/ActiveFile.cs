using UnityEngine;
using System.Collections;
using System;

public class ActiveFile : MonoBehaviour
{
    [Header("Setup")]
    public GameObject TransparentFile;

    [Header("Runtime")]
    public string FileName;
    public string StorageName;

    [Header("Testing")]
    public RectTransform FloppyTransform;
    public FileManager FileManager;

	// Use this for initialization
	void Start ()
    {
        EventManager.Instance.AddListener<FileDraggingStartEvent>(OnFileDraggingStartEvent);
	}

    private void OnFileDraggingStartEvent(FileDraggingStartEvent ev)
    {
        FileName = ev.FileName;
        StorageName = ev.StorageName;

        StartDragging();
    }

    public void StartDragging()
    {
        TransparentFile.SetActive(true);
    }

    public void EndDragging()
    {
        TransparentFile.SetActive(false);

        // Hard test against floppy
        if (RectTransformUtility.RectangleContainsScreenPoint(FloppyTransform, TransparentFile.transform.position))
        {
            FileManager.TransferFile(FileName, StorageName, "Floppy Disk");
            SoundManager.PlaySoundEffect("pick");
        }
    }

    void Update()
    {
        if (TransparentFile.activeInHierarchy)
        {
            TransparentFile.transform.position = Input.mousePosition;
            if (Input.GetMouseButtonUp(0))
                EndDragging();
        }
    }
}
