using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Graduation_Project.Application.Utils {
    public class EncodeDecodeHandler {
        public static string Encode(string input) => WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(input));
        public static string Decode(string input) => Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(input));
    }
}
