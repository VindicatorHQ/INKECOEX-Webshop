using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace WebshopService.Utils;

public static class SlugifyExtensions
{
    public static string ToSlug(this string phrase)
    {
        string str = phrase.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (var c in from c in str let uc = CharUnicodeInfo.GetUnicodeCategory(c) where uc != UnicodeCategory.NonSpacingMark select c)
        {
            sb.Append(c);
        }

        str = sb.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant();

        str = Regex.Replace(str, @"[^a-z0-9\s-]", "");

        str = Regex.Replace(str, @"[\s_-]+", "-").Trim();
        
        str = str.Trim('-');

        return str;
    }
}