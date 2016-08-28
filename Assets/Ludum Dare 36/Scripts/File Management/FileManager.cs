using UnityEngine;
using System.Collections;
using System.Linq;

public class FileManager : MonoBehaviour
{
    public FileStorage[] Storages;

    // Use this for initialization
    void Start()
    {

    }

    public void TransferFile(string fileName, string storageFrom, string toStorage)
    {
        // Assume the name is right
        FileStorage from = Storages.Where(x => x.Name.Equals(storageFrom)).First();
        FileStorage to = Storages.Where(x => x.Name.Equals(toStorage)).First();

        File oldFile = from.GetFile(fileName);
        if (oldFile.IsTransferring) return; // DO error alert

        TransferFile(fileName, from, to);
    }

    public void TransferFile(string fileName, FileStorage from, FileStorage to)
    {
        StartCoroutine(TransferFileRoutine(fileName, from, to));
    }

    protected IEnumerator TransferFileRoutine(string fileName, FileStorage from, FileStorage to)
    {
		from.DisableEject ();
		to.DisableEject ();

        // Ha ha harcode..
        if (to.CurrentSize > 5)
        {
            File oldFile = from.GetFile(fileName);
            oldFile.IsTransferring = true;

            File newFile = to.GetFile(fileName);
            if (newFile == null) newFile = to.CloneFile(oldFile);

            float transferRate = to.TransferSpeed;
            float sizeToTransfer = oldFile.Progress.Value / 100f * oldFile.Size;
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

                        // Done
                        break;
                    }
                }
                else
                {
                    // Not enough space
                    oldFile.IsTransferring = false;

                    // Done
                    break;
                }
            }
        }

		from.EnableEject ();
		to.EnableEject ();
    }

    // Asumption: all file exists on "from"
    // total file size are equal or less than "to" size
    public IEnumerator TransferFileBurnRoutine(string[] fileNames, FileStorage from, FileStorage to)
    {
        File[] files = new File[fileNames.Length];
        File[] newFiles = new File[fileNames.Length];
        float totalSize = 0;

        for (int i = 0; i < files.Length; i++)
        {
            files[i] = from.GetFile(fileNames[i]);
            files[i].IsTransferring = true;

            newFiles[i] = to.CloneFile(files[i]);
            totalSize += files[i].Size * files[i].Progress.Value / 100f;
        }

        while (totalSize > 0)
        {
            totalSize -= to.TransferSpeed * Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < files.Length; i++)
        {
            files[i].Progress.Value = 0f;
            newFiles[i].Progress.Value = 100f;
            from.DeleteFile(files[i].Name);
        }

        yield return new WaitForSeconds(2);

        to.Eject();

        yield return null;
    }
}
