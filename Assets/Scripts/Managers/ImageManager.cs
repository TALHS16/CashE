using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ImageManager
{
    private Queue<string> usageQueue = new Queue<string>();
    private int cacheSize = 100;
    private string usageQueuePath;

    private string folder_name;

    public ImageManager(string name, string folder)
    {
        folder_name = folder;
        usageQueuePath = Path.Combine(Application.persistentDataPath, name);
        LoadUsageQueue();
    }

    private void LoadUsageQueue()
    {
        if (File.Exists(usageQueuePath))
        {
            string[] lines = File.ReadAllLines(usageQueuePath);
            foreach (var line in lines)
            {
                usageQueue.Enqueue(line);
            }
        }
    }

    private void SaveUsageQueue()
    {
        File.WriteAllLines(usageQueuePath, usageQueue.Select(i => i.ToString()).ToArray());
    }

    public void AddToQueue(string transactionIndex)
    {
        if (usageQueue.Contains(transactionIndex))
        {
            // Move this transactionIndex to the end of the queue to mark it as recently used
            usageQueue = new Queue<string>(usageQueue.Where(i => i != transactionIndex));
        }
        else if (usageQueue.Count >= cacheSize)
        {
            // Remove the least recently used image
            string oldestTransactionIndex = usageQueue.Dequeue();
            // Also delete the image file from local storage
            ImageStorage storage = new ImageStorage(folder_name);
            storage.DeleteImage(oldestTransactionIndex.ToString());
        }
        // Add the new image and mark it as recently used
        usageQueue.Enqueue(transactionIndex);
        SaveUsageQueue();
    }

    public bool IsInCache(string transactionIndex)
    {
        return usageQueue.Contains(transactionIndex);
    }

    public void RemoveFromCache(string transactionIndex)
    {
        if (usageQueue.Contains(transactionIndex))
        {
            usageQueue = new Queue<string>(usageQueue.Where(i => i != transactionIndex));
            SaveUsageQueue();
        }
    }
}
