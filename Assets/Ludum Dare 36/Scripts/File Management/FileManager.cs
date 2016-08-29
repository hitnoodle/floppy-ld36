using UnityEngine;
using System.Collections;
using System.Linq;
using UniRx;

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
        if (oldFile != null && oldFile.IsTransferring) return; // DO error alert

        TransferFile(fileName, from, to);
    }

    public void TransferFile(string fileName, FileStorage from, FileStorage to)
    {
        StartCoroutine(TransferFileRoutine(fileName, from, to));
    }

    public IEnumerator TransferFileRoutine(string fileName, FileStorage from, FileStorage to)
    {

		from.DisableEject ();
		to.DisableEject ();

		File oldFile 	= null;
		File newFile	= null;

        // Ha ha harcode..
        if (to.CurrentSize > 5)
        {
            oldFile = from.GetFile(fileName);
            oldFile.IsTransferring = true;

            newFile = to.GetFile(fileName);
            if (newFile == null) newFile = to.CloneFile(oldFile);

			// nandain siapa lagi transfer apaan
			from.TransferringFile.Add (oldFile);
			to.TransferringFile.Add (newFile);

            float transferRate = to.TransferSpeed;
            float sizeToTransfer = oldFile.Progress.Value / 100f * oldFile.Size;
            float fileTransferSize = 0;

            float sizeToTransferTotal = sizeToTransfer;
            FloatReactiveProperty transferProgress = new FloatReactiveProperty();
            BoolReactiveProperty isTransferFinished = new BoolReactiveProperty();
            to.ShowCopy(transferProgress, isTransferFinished);

            while (to.CurrentSize > 0)
            {
                yield return null;
                fileTransferSize = transferRate * Time.deltaTime;

                if (to.CurrentSize - fileTransferSize >= 0)
                {
                    to.CurrentSize -= fileTransferSize;

                    sizeToTransfer -= fileTransferSize;
                    if (sizeToTransfer < 0) sizeToTransfer = 0;

                    transferProgress.Value = (1 - (sizeToTransfer / sizeToTransferTotal)) * 100f;

                    oldFile.Progress.Value = Mathf.Clamp(sizeToTransfer / oldFile.Size * 100f, 0, 100);
                    newFile.Progress.Value = Mathf.Clamp(100 - oldFile.Progress.Value, 0, 100);

                    // Finish transferring
                    if (sizeToTransfer == 0)
                    {
                        from.DeleteFile(fileName);

                        // Done
                        isTransferFinished.Value = true;
                        break;
                    }
                }
                else
                {
                    // Not enough space
                    to.ShowAlertFull();
                    oldFile.IsTransferring = false;

                    // Done
                    isTransferFinished.Value = true;
                    break;
                }
            }

			from.TransferringFile.Remove (oldFile);
			to.TransferringFile.Remove (newFile);

            transferProgress.Dispose();
            transferProgress = null;

            isTransferFinished.Dispose();
            isTransferFinished = null;
        }

		if (from.TransferringFile.Count <= 0)
			from.EnableEject ();
		if (to.TransferringFile.Count <= 0)
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
