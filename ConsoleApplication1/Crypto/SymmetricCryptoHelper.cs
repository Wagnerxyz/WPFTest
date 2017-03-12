using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


class SymmetricCryptoHelper
{
    private const int buffersize = 5;
    /// <summary>
    /// Aes solution the keys must be 32
    /// </summary>
    public static string AesKey = "asekey32w";
    /// <summary>
    /// Gets a Aes32 bit key
    /// </summary>
    /// <param name="key">Aes key string</param>
    /// <returns>Aes32 bit key</returns>
    static byte[] GetAesKey(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException("key", "The Aes key can not be empty");
        }
        if (key.Length < 32)
        {
            // Less than 32 complete
            key = key.PadRight(32, '0');
        }
        if (key.Length > 32)
        {
            key = key.Substring(0, 32);
        }
        return Encoding.UTF8.GetBytes(key);
    }
    /// <summary>
    /// 获取密钥
    /// </summary>
    private static string Key
    {
        get { return @"wocaonimalegebi1"; }
    }

    /// <summary>
    /// 获取向量
    /// </summary>
    private static string IV
    {
        get { return @"L%n67}G\Mk@k%:~Y"; }
    }

    /// <summary>
    /// AES加密
    /// </summary>
    /// <param name="plainStr">明文字符串</param>
    /// <returns>密文</returns>
    public static string SymmetricEncrypt(string plainStr)
    {
        byte[] bKey = Encoding.UTF8.GetBytes(Key);
        byte[] bIV = Encoding.UTF8.GetBytes(IV);
        byte[] byteArray = Encoding.UTF8.GetBytes(plainStr);

        string encrypt = null;
        AesCryptoServiceProvider des = new AesCryptoServiceProvider();
        // try
        // {
        using (MemoryStream mStream = new MemoryStream())
        {
            using (CryptoStream cStream = new CryptoStream(mStream, des.CreateEncryptor(bKey, bIV), CryptoStreamMode.Write))
            {
                int read;
                do
                {//分段读取意义不大，因为缓存区大了容易出异常，超过数组界限
                    read = mStream.Read(byteArray, 0, buffersize);
                    cStream.Write(byteArray, 0, buffersize);
                } while (read > 0);


                //   cStream.Write(byteArray, 0, byteArray.Length);
                cStream.FlushFinalBlock();
                encrypt = Convert.ToBase64String(mStream.ToArray());
            }
        }
        // } 
        //  catch { }
        des.Clear();

        return encrypt;
    }

    /// <summary>
    /// AES解密
    /// </summary>
    /// <param name="encryptStr">密文字符串</param>
    /// <returns>明文</returns>
    public static string SymmetricDecrypt(string encryptStr)
    {
        byte[] bKey = Encoding.UTF8.GetBytes(Key);
        byte[] bIV = Encoding.UTF8.GetBytes(IV);
        byte[] byteArray = Convert.FromBase64String(encryptStr);

        string decrypt = null;
        AesCryptoServiceProvider des = new AesCryptoServiceProvider();
        try
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                using (CryptoStream cStream = new CryptoStream(mStream, des.CreateDecryptor(bKey, bIV), CryptoStreamMode.Write))
                {
                    cStream.Write(byteArray, 0, byteArray.Length);
                    cStream.FlushFinalBlock();
                    decrypt = Encoding.UTF8.GetString(mStream.ToArray());
                }
            }
        }
        catch { }
        des.Clear();

        return decrypt;
    }
}


