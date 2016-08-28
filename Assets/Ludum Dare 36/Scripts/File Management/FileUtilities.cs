using UnityEngine;
using System.Collections;
using System.Text;

public static class FileUtilities
{
    public static string GetSizeText(float sizeInKB)
    {
        StringBuilder sb = new StringBuilder();

        float sizeInMB = sizeInKB / 1024;
        if (Mathf.FloorToInt(sizeInMB) > 0)
        {
            float sizeInGB = sizeInMB / 1024;
            if (Mathf.FloorToInt(sizeInGB) > 0)
            {
                sb.Append(sizeInGB.ToString("0.00"));
                sb.Append(" GB");
            }
            else
            {
                sb.Append(sizeInMB.ToString("0.00"));
                sb.Append(" MB");
            }
        }
        else
        {
            sb.Append(sizeInKB);
            sb.Append(" KB");
        }

        return sb.ToString();
    }
}
