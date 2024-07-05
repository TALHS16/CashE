using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SpriteHelper
{
    public static byte[] SpriteToByteArray(Sprite sprite)
    {
        // Get the texture from the sprite
        Texture2D texture = sprite.texture;

        // Encode the texture to a PNG byte array
        byte[] bytes = texture.GetRawTextureData();

        return bytes;
    }

    public static Sprite BytesToSprite(byte[] bytes)
    {
        try
        {
            // Create a new Texture2D
            Texture2D texture = new Texture2D(1, 1);
            
            // Load the image bytes into the Texture2D
            if (texture.LoadImage(bytes))
            {
                // Create a new Sprite using the Texture2D
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                return sprite;
            }
            else
            {
                Debug.LogError("Failed to load image bytes into Texture2D.");
                return null;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error converting byte array to sprite: {e.Message}");
            return null;
        }
    }
}
