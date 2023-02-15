using System.Text;
using System.Security.Cryptography;
using HexaPokerNet.Domain;

namespace HexaPokerNet.Adapter;

public class EntityIdGenerator : IEntityIdGenerator
{
    private const int IdLength = 10;
    
    public string NewId()
    {
        return GetSHA1Hash(Guid.NewGuid().ToString());
    }
    
    private static string GetSHA1Hash(string input)
    {
        var bytes = Encoding.Default.GetBytes(input);
        bytes = SHA1.HashData(bytes);
        var sb = new StringBuilder(IdLength);
        for (int i = 0, l = bytes.Length; i < l; i++)
            sb.Append(bytes[i].ToString("x2"));
        return sb.ToString();
    }
}