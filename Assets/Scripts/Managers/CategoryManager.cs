using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategoryManager : MonoBehaviour
{
    private static CategoryManager instance;
    //public List<CategoryModel> cat_list = new List<CategoryModel>();
    private Dictionary<string, CategoryModel> category = new Dictionary<string, CategoryModel>();

    public ImageManager imageManager;
    public ImageStorage imageStorage;

    public Dictionary<string, CategoryModel> CategoryDic
    {
        get
        {
            return category;
        }
        set
        {
            category = value;
        }
    }

    public static CategoryManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CategoryManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("Singleton");
                    instance = obj.AddComponent<CategoryManager>();
                }
                instance.imageManager = new ImageManager("usageQueueCategories.txt", "cat_icons");
                instance.imageStorage = new ImageStorage("cat_icons");
            }
            return instance;
        }
    }
    private void Awake()
    {
        FirebaseManager.Instance.GetAllCategories(this, null, null, null);

    }

    private void Start()
    {
    }

    public void AddCategory(CategoryModel categoryModel, Sprite sprite, GameObject popUp)
    {
        FirebaseManager.Instance.GetAllCategories(this, categoryModel, sprite, popUp);
    }

    public void SetList(CatList list, CategoryModel categoryModel, Sprite sprite, GameObject popUp)
    {
        for (int i = 0; i < list.categories.Count; i++)
        {
            category[(list.categories)[i].name] = (list.categories)[i];
        }
        if (categoryModel != null)
        {
            category[categoryModel.name] = categoryModel;
            byte[] byte_png = Resize(sprite.texture, 64, 64);
            FirebaseManager.Instance.UploadSprite(byte_png, categoryModel.name, "categories/", ".png", popUp, imageManager, imageStorage);
            FirebaseManager.Instance.SendNewCategoryToDatabase(categoryModel);

        }

    }

    Texture2D duplicateTexture(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }

    private byte[] Resize(Texture2D texture, int newWidth, int newHeight)
    {
        Texture2D readableTexture = duplicateTexture(texture);

        for (int w = 0; w < readableTexture.width; w++)
        {
            for (int h = 0; h < readableTexture.height; h++)
            {
                Color pixel = readableTexture.GetPixel(w, h);
                if (pixel.a < 0.5)
                {
                    readableTexture.SetPixel(w, h, new Color(pixel.r, pixel.g, pixel.b, 0));
                }
                else
                {
                    readableTexture.SetPixel(w, h, Color.white);
                }
            }
        }
        readableTexture.Apply();

        RenderTexture rt = new RenderTexture(newWidth, newHeight, 24);
        RenderTexture.active = rt;
        Graphics.Blit(readableTexture, rt);
        Texture2D result = new Texture2D(newWidth, newHeight);
        result.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
        result.Apply();

        byte[] pngBytes = result.EncodeToPNG();

        return pngBytes;
    }


}
