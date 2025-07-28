using System.Text.RegularExpressions;
using System.Text;

namespace WebFM_Style.Helper
{
    public static class Utilities
    {
        public static string GetRandomKey(int length = 5)
        {
            string pattern = @"0123456789zxcvbnmasdfghjklqwertyuiop[]{}:~!@#$%^&*()+";
            Random rd = new Random();
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                sb.Append(pattern[rd.Next(0, pattern.Length)]);
            }

            return sb.ToString();
        }
        public static string ToUrlFriendly(this string url)
        {
            var result = url.ToLower().Trim();

            result = Regex.Replace(result, "[áàạảãâấầậẩẫ]", "a");
            result = Regex.Replace(result, "[éèẹẻẽêếềệểễ]", "e");
            result = Regex.Replace(result, "[óòọỏõôốồộổỗơớờợởỡ]", "o");
            result = Regex.Replace(result, "[úùụủũưứừựửữ]", "u");
            result = Regex.Replace(result, "[íìịỉĩ]", "i");
            result = Regex.Replace(result, "[ýỳỵỷỹ]", "y");
            result = Regex.Replace(result, "đ", "d");

            result = Regex.Replace(result, "[^a-z0-9-]", "-");

            result = Regex.Replace(result, "(-)+", "-");

            return result;
        }
        public static async Task<string> UploadFile(IFormFile file, string sDirectory, string newname = null)
        {
            try
            {
                if (newname == null) newname = file.FileName;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Images", sDirectory);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var supportedTypes = new[] { "jpg", "jpeg", "png", "gif" };
                var fileExt = Path.GetExtension(file.FileName).Substring(1);
                if (!supportedTypes.Contains(fileExt.ToLower()))
                {
                    return null;
                }
                else
                {
                    string fullPath = path + "\\" + newname;
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    return newname;
                }
            }
            catch
            {
                return null;
            }
        }

    }
}
