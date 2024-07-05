using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImagePicker
{
    public Sprite sprite = null;
    public bool CheckForReadPermisson()
    {
        NativeGallery.PermissionType permissionType = NativeGallery.PermissionType.Read;
        NativeGallery.MediaType mediaType = NativeGallery.MediaType.Image;
        NativeGallery.Permission permission = NativeGallery.CheckPermission(permissionType, mediaType);
        if (permission == NativeGallery.Permission.Granted)
            return true;
        //permission = await NativeGallery.RequestPermissionAsync( permissionType, mediaType );
        return permission == NativeGallery.Permission.Granted;
    }

    public void PickImage(int maxSize)
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                Debug.Log("Load image from " + path);
                sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
        });
    }

    public void TakePicture(int maxSize)
    {
        NativeCamera.Permission permission = NativeCamera.TakePicture((path) =>
        {
            if (path != null)
            {
                // Create a Texture2D from the captured image
                Texture2D texture = NativeCamera.LoadImageAtPath(path, maxSize);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                Debug.Log("Load image from " + path);
                sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
        }, maxSize);
    }
}
