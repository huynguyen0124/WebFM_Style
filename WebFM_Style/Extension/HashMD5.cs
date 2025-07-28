using System.Text;
using System.Security.Cryptography;
namespace WebFM_Style.Extension
{
    public static class HashMD5
    {
        public static string ToMD5(this string str)
        {
#pragma warning disable SYSLIB0021 // Type or member is obsolete
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
#pragma warning restore SYSLIB0021 // Type or member is obsolete
            byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder sbHash = new StringBuilder();
            foreach (byte b in bHash)
            {
                sbHash.Append(String.Format("{0:x2}", b));
            }
            return sbHash.ToString();

        }

    }
}
