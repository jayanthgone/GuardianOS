using System.Security.Cryptography;
using System.Text;

namespace Guardian.Common.Security;

/// <summary>
/// Interface for encryption/decryption operations.
/// </summary>
public interface IEncryptionService
{
    /// <summary>
    /// Encrypt a plain-text string.
    /// </summary>
    /// <param name="plainText">Text to encrypt.</param>
    /// <returns>Encrypted string (Base64 encoded).</returns>
    string Encrypt(string plainText);

    /// <summary>
    /// Decrypt an encrypted string.
    /// </summary>
    /// <param name="encryptedText">Encrypted text (Base64 encoded).</param>
    /// <returns>Decrypted plain-text string.</returns>
    string Decrypt(string encryptedText);
}

/// <summary>
/// AES encryption service for sensitive data.
/// </summary>
public class AesEncryptionService : IEncryptionService
{
    private readonly byte[] _key;
    private readonly byte[] _iv;

    /// <summary>
    /// Initialize encryption service with key and IV.
    /// </summary>
    public AesEncryptionService(string keyPhrase)
    {
        if (string.IsNullOrWhiteSpace(keyPhrase))
            throw new ArgumentException("Key phrase cannot be empty", nameof(keyPhrase));

        // Derive key and IV from key phrase
        using (var sha256 = SHA256.Create())
        {
            _key = sha256.ComputeHash(Encoding.UTF8.GetBytes(keyPhrase));
        }

        _iv = new byte[16];
        Buffer.BlockCopy(_key, 0, _iv, 0, 16);
    }

    /// <summary>
    /// Encrypt plain-text string.
    /// </summary>
    public string Encrypt(string plainText)
    {
        if (string.IsNullOrWhiteSpace(plainText))
            throw new ArgumentException("Plain text cannot be empty", nameof(plainText));

        using (var aes = Aes.Create())
        {
            aes.Key = _key;
            aes.IV = _iv;

            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }

                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    /// <summary>
    /// Decrypt encrypted string.
    /// </summary>
    public string Decrypt(string encryptedText)
    {
        if (string.IsNullOrWhiteSpace(encryptedText))
            throw new ArgumentException("Encrypted text cannot be empty", nameof(encryptedText));

        using (var aes = Aes.Create())
        {
            aes.Key = _key;
            aes.IV = _iv;

            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            using (var ms = new MemoryStream(Convert.FromBase64String(encryptedText)))
            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (var sr = new StreamReader(cs))
            {
                return sr.ReadToEnd();
            }
        }
    }
}
