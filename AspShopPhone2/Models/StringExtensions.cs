using System;
using System.Text;
using System.Text.RegularExpressions;

namespace AspShopPhone2.Models
{
    public static class StringExtensions
    {
        public static string ToFriendlyUrl(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "";
            }

            // Loại bỏ dấu tiếng Việt
            input = RemoveVietnameseChars(input);

            // Loại bỏ ký tự không hợp lệ
            input = Regex.Replace(input, @"[^a-zA-Z0-9\s-]", "");
            input = input.Trim();
            input = Regex.Replace(input, @"\s+", "-");

            return input;
        }

        private static string RemoveVietnameseChars(string input)
        {
            string[] vietnameseChars = new string[]
            {
            "áàảãạăắằẳẵặâấầẩẫậ",
            "éèẻẽẹêếềểễệ",
            "íìỉĩị",
            "óòỏõọôốồổỗộơớờởỡợ",
            "úùủũụưứừửữự",
            "ýỳỷỹỵ",
            "đ",
            "ÁÀẢÃẠĂẮẰẲẴẶÂẤẦẨẪẬ",
            "ÉÈẺẼẸÊẾỀỂỄỆ",
            "ÍÌỈĨỊ",
            "ÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢ",
            "ÚÙỦŨỤƯỨỪỬỮỰ",
            "ÝỲỶỸỴ",
            "Đ"
            };

            string[] replacementChars = new string[]
            {
            "a",
            "e",
            "i",
            "o",
            "u",
            "y",
            "d",
            "A",
            "E",
            "I",
            "O",
            "U",
            "Y",
            "D"
            };

            for (int i = 0; i < vietnameseChars.Length; i++)
            {
                input = Regex.Replace(input, "[" + vietnameseChars[i] + "]", replacementChars[i]);
            }

            return input;
        }
    }
}