using System.Security.Cryptography;
using System.Text;

namespace HostelProperty.Shared;

public static class Hash
{
    public static string CreateHash(string s)
    {
        var method = SHA256.Create();

        return BitConverter.ToString(method.ComputeHash(Encoding.UTF8.GetBytes(s)));
    }
}
