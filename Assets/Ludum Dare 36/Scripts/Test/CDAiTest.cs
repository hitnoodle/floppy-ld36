using UnityEngine;
using System.Collections;

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

    // Use this for initialization
    void Start ()
    {
        _CDStorage = GetComponent<FileStorage>();
        _CanvasGroup = GetComponent<CanvasGroup>();
    }

    public void StartAI()
    {
        _AIRoutine = AIRoutine();
        StartCoroutine(_AIRoutine);
    }

    public void StopAI()
    {
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
                for (int i = 0; i < tookNames.Length; i++)
                    tookNames[i] = fileNames[i];

                if (tookNames.Length > 0)
                {
                    if (Type == TransferType.Burn)
                        yield return StartCoroutine(FileManager.TransferFileBurnRoutine(tookNames, HDDStorage, _CDStorage));
                    else if (Type == TransferType.Normal)
                    {
                        yield return StartCoroutine(FileManager.TransferFileRoutine(tookNames[0], HDDStorage, _CDStorage));

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
}
