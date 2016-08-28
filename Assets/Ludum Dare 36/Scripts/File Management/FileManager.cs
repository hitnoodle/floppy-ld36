using UnityEngine;
using System.Collections;
using System.Linq;

public class FileManager : MonoBehaviour
{
    public FileStorage[] Storages;

	// Use this for initialization
	void Start ()
    {

	}

    public void TransferFile(string fileName, string storageFrom, string toStorage)
    {
        // Assume the name is right
        FileStorage from = Storages.Where(x => x.Name.Equals(storageFrom)).First();
        FileStorage to = Storages.Where(x => x.Name.Equals(toStorage)).First();

        TransferFile(fileName, from, to);
    }

    public void TransferFile(string fileName, FileStorage from, FileStorage to)
    {
        StartCoroutine(TransferFileRoutine(fileName, from, to));
    }

    protected IEnumerator TransferFileRoutine(string fileName, FileStorage from, FileStorage to)
    {
        File oldFile = from.GetFile(fileName);
        File newFile = to.CloneFile(oldFile);

        float transferRate = to.TransferSpeed;
        float sizeToTransfer = oldFile.Size;
        float fileTransferSize = 0;

        while (to.CurrentSize > 0)
        {
            yield return null;
            fileTransferSize = transferRate * Time.deltaTime;

            if (to.CurrentSize - fileTransferSize >= 0)
            {
                to.CurrentSize -= fileTransferSize;

                sizeToTransfer -= fileTransferSize;
                if (sizeToTransfer < 0) sizeToTransfer = 0;

                oldFile.Progress.Value = Mathf.Clamp(sizeToTransfer / oldFile.Size * 100f, 0, 100);
                newFile.Progress.Value = Mathf.Clamp(100 - oldFile.Progress.Value, 0, 100);

                // Finish transferring
                if (sizeToTransfer == 0)
                {
                    from.DeleteFile(fileName);
                }
            }
            else
            {
                // Not enough space
            }
        }
    }
}
