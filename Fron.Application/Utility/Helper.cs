using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text;

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
}
