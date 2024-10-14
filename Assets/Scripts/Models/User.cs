using System;
using System.Security.Cryptography;
using UnityEngine;
[Serializable]
public class User
{
    public string name;
    public string user_name;
    public string password_hash;
    public string salt;

    public User (string username, string password,string name_)
    {
        byte[] salt_ = GenerateSalt();
        password_hash = HashPassword(password, salt_);
        salt = Convert.ToBase64String(salt_);
        user_name = username;
        name = name_;
        // Debug.Log(username + ": " + salt);
        // Debug.Log(username + ": " + password_hash);
    }

    public bool VerifyPassword(string password)
    {
        string hashedPassword = HashPassword(password, Convert.FromBase64String(salt));
        return password_hash == hashedPassword;
    }

    private static byte[] GenerateSalt()
    {
        byte[] salt_ = new byte[16];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(salt_);
        }
        return salt_;
    }

    private static string HashPassword(string password, byte[] salt_)
    {
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt_, 10000))
        {
            byte[] hash = pbkdf2.GetBytes(20); // 20 bytes for SHA-1
            return Convert.ToBase64String(hash);
        }
    }
}

