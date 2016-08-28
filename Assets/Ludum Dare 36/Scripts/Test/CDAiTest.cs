using UnityEngine;
using System.Collections;

public class CDAiTest : MonoBehaviour
{
    public FileManager FileManager;
    public FileStorage HDDStorage;

    public float Delay = 5f;
    public int TakeFile = 2;

    protected FileStorage _CDStorage;
    protected CanvasGroup _CanvasGroup;
    protected IEnumerator _AIRoutine;

    // Use this for initialization
    void Start ()
    {
        _CDStorage = GetComponent<FileStorage>();
        _CanvasGroup = GetComponent<CanvasGroup>();

        _AIRoutine = AIRoutine();
        StartCoroutine(_AIRoutine);
    }

    IEnumerator AIRoutine()
    {
        yield return new WaitForSeconds(Delay);

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
                    yield return StartCoroutine(FileManager.TransferFileBurnRoutine(tookNames, HDDStorage, _CDStorage));
            }

            yield return new WaitForSeconds(Delay);
        }
    }
}
