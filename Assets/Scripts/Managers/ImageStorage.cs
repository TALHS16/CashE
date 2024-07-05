using System.IO;
using UnityEngine;

public class ImageStorage
{
    private string folder_name;
    public ImageStorage(string name)
    {
        folder_name = name;
    }

    private string GetImagePath(string transactionIndex)
    {
        return Path.Combine(Application.persistentDataPath, transactionIndex + ".jpg");
    }

    public void SaveImage(string transactionIndex, byte[] imageBytes)
    {
        File.WriteAllBytes(GetImagePath(transactionIndex), imageBytes);
        Debug.Log(GetImagePath(transactionIndex));
    }

    public Texture2D LoadImage(string transactionIndex)
    {
        string path = GetImagePath(transactionIndex);
        if (File.Exists(path))
        {
            byte[] imageBytes = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes);
            return texture;
        }
        return null;
    }

    public void DeleteImage(string transactionIndex)
    {
        string path = GetImagePath(transactionIndex);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
