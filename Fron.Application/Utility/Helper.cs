using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Data;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Fron.Application.Utility;

public static class Helper
{
    private static readonly TripleDESCryptoServiceProvider _des = new();

    private static readonly MD5CryptoServiceProvider _md5 = new();

    public static string Encrypt(string plainText, string key)
    {
        _des.Key = MD5Hash(key);
        _des.Mode = CipherMode.ECB;
        byte[] Buffer = Encoding.ASCII.GetBytes(plainText);
        return Convert.ToBase64String(_des.CreateEncryptor().TransformFinalBlock(Buffer, 0, Buffer.Length));
    }

    public static string Decrypt(string ciphertext, string key)
    {
        _des.Key = MD5Hash(key);
        _des.Mode = CipherMode.ECB;
        byte[] Buffer = Convert.FromBase64String(ciphertext);
        return Encoding.ASCII.GetString(_des.CreateDecryptor().TransformFinalBlock(Buffer, 0, Buffer.Length));
    }

    public static string Base64Encode(string plainText)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(bytes);
    }
    public static string Base64Encode(byte[] bytes)
    {
        return Convert.ToBase64String(bytes);
    }
    public static string Base64Encode(byte[] bytes, int offset, int length)
    {
        return Convert.ToBase64String(bytes, offset, length);
    }
    public static string Base64Encode(string plainText, int offset, int length)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(bytes, offset, length);
    }
    public static string Base64Encode(byte[] bytes, int offset, int length, Base64FormattingOptions options)
    {
        return Convert.ToBase64String(bytes, offset, length, options);
    }
    public static string Base64Encode(string plainText, int offset, int length, Base64FormattingOptions options)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(bytes, offset, length, options);
    }
    public static string Base64Decode(byte[] bytes)
    {
        return Encoding.UTF8.GetString(bytes);
    }
    public static string Base64Decode(byte[] bytes, int offset, int length)
    {
        return Encoding.UTF8.GetString(bytes, offset, length);
    }
    public static string Base64Decode(string base64EncodedData, int offset, int length)
    {
        byte[] bytes = Convert.FromBase64String(base64EncodedData);
        return Encoding.UTF8.GetString(bytes, offset, length);
    }
    public static string Base64Decode(string base64EncodedData, int offset, int length, Base64FormattingOptions options, Encoding encoding)
    {
        byte[] bytes = Convert.FromBase64String(base64EncodedData);
        return encoding.GetString(bytes, offset, length);
    }
    public static string Base64Decode(string base64EncodedData, Encoding encoding)
    {
        byte[] bytes = Convert.FromBase64String(base64EncodedData);
        return encoding.GetString(bytes);
    }
    public static string Base64Decode(string base64EncodedData, int offset, int length, Encoding encoding)
    {
        byte[] bytes = Convert.FromBase64String(base64EncodedData);
        return encoding.GetString(bytes, offset, length);
    }

    public static string Base64Decode(string base64EncodedData)
    {
        byte[] bytes = Convert.FromBase64String(base64EncodedData);
        return Encoding.UTF8.GetString(bytes);
    }

    public static byte[] MD5Hash(string value)
    {
        return _md5.ComputeHash(Encoding.ASCII.GetBytes(value));
    }

    public static SqlParameter CreateInputParameter(string name, SqlDbType dbType, object? value)
           => CreateParameter(name, dbType, ParameterDirection.Input, 0, 0, 0, value);

    public static SqlParameter CreateInputParameter(string name, SqlDbType dbType, int size, object? value)
        => CreateParameter(name, dbType, ParameterDirection.Input, size, 0, 0, value);

    public static SqlParameter CreateOutputParameter(string name, SqlDbType dbType, object? value)
        => CreateParameter(name, dbType, ParameterDirection.Output, 0, 0, 0, value);
    public static SqlParameter CreateOutputParameter(string name, SqlDbType dbType)
    => CreateParameter(name, dbType, ParameterDirection.Output, 0, 0, 0, DBNull.Value);

    private static SqlParameter CreateParameter(string name, SqlDbType dbType, ParameterDirection direction, int size, byte scale, byte precision, object? value)
        => new SqlParameter
        {
            ParameterName = name,
            SqlDbType = dbType,
            Direction = direction,
            Size = size,
            Scale = scale,
            Precision = precision,
            Value = ChObjectToDb(value)
        };

    private static object ChObjectToDb(object? value)
    {
        if (value == null)
        {
            value = DBNull.Value;
        }
        else if (value.GetType() == typeof(string))
        {
            value = ((string)value).Trim();
        }

        return value;
    }

    public static dynamic? GetObjectFromString(string? jsonString)
    {
        try
        {
            if (!string.IsNullOrEmpty(jsonString))
                return JsonConvert.DeserializeObject<dynamic>(jsonString);
            else
                return null;
        }
        catch (Exception ex)
        {
            throw new Exception($"Please check the json string is in correct format: {ex.Message}");
        }
    }
}
