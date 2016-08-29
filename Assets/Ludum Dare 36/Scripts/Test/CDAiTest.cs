using UnityEngine;
using System.Collections;
using System;

public class CDAiTest : MonoBehaviour
{
    [System.Serializable]
    public enum TransferType
    {
        Normal,
        Burn
    }

    public FileManager FileManager;
    public FileStorage HDDStorage;

    public float DelayStart = 5f;
    public float DelayUpdate = 5f;
    public int TakeFile = 2;
    public TransferType Type;

    protected FileStorage _CDStorage;
    protected CanvasGroup _CanvasGroup;

    protected IEnumerator _AIRoutine;
    protected IEnumerator _TransferRoutine;
    protected string[] _CurrentTransferredFiles;

    // Use this for initialization
    void Start ()
    {
        _CDStorage = GetComponent<FileStorage>();
        _CanvasGroup = GetComponent<CanvasGroup>();

        EventManager.Instance.AddListener<CancelTransferEvent>(OnCancelTransferEvent);
    }

    public void StartAI()
    {
        _AIRoutine = AIRoutine();
        StartCoroutine(_AIRoutine);
    }

    public void StopAI()
    {
        if (_TransferRoutine != null)
        {
            StopCoroutine(_TransferRoutine);
            _TransferRoutine = null;
        }

        if (_AIRoutine != null)
        {
            StopCoroutine(_AIRoutine);
            _AIRoutine = null;
        }
    }

    IEnumerator AIRoutine()
    {
        yield return new WaitForSeconds(DelayStart);

        while (true)
        {
            if (_CanvasGroup.alpha != 0)
            {
                // Pick files
                string[] fileNames = HDDStorage.GetIdleFiles();
                string[] tookNames = new string[(fileNames.Length > TakeFile ? TakeFile : fileNames.Length)];

                _CurrentTransferredFiles = tookNames;

                for (int i = 0; i < tookNames.Length; i++)
                    tookNames[i] = fileNames[i];

                if (tookNames.Length > 0)
                {
                    if (Type == TransferType.Burn)
                    {
                        _TransferRoutine = FileManager.TransferFileBurnRoutine(tookNames, HDDStorage, _CDStorage);
                        yield return StartCoroutine(_TransferRoutine);
                    }
                    else if (Type == TransferType.Normal)
                    {
                        _TransferRoutine = FileManager.TransferFileRoutine(tookNames[0], HDDStorage, _CDStorage);
                        yield return StartCoroutine(_TransferRoutine);

                        yield return new WaitForSeconds(2);

                        // HUE HUE HUE
                        if (_CDStorage.CurrentSize < 5 || HDDStorage.Files.Count == 0)
                            _CDStorage.Eject();

                        yield return null;
                    }
                }
            }

            yield return new WaitForSeconds(DelayUpdate);
        }
    }

    private void OnCancelTransferEvent(CancelTransferEvent e)
    {
        if (e.StorageID.Equals(_CDStorage.Name))
        {
            StopAI();

            _CDStorage.DeleteFiles();
            foreach (string file in _CurrentTransferredFiles)
            {
                File oldFile = HDDStorage.GetFile(file);
                oldFile.IsTransferring = false;
            }

            StartAI();
        }
    }
}
