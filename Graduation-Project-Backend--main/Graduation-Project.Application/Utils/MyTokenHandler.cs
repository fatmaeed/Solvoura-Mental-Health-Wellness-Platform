using Graduation_Project.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Graduation_Project.Application.Utils {

    public class MyTokenHandler {
        private static readonly string _secretKey = "C3k0EINrjzlfR6BD0a/5O/kkG5+02rPRz0cOn2EM7IiwJ2iKchP+zKHuNKn3cbhgmhR5S9AdHGwnGfNnFh6aHw==";

        private static string PrivateGenerateToken(UserEntity user, int expireDays = 1, List<Claim>? additionalClaims = null) {
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                claims: new List<Claim> {
                    new Claim(MyCalims.UserId, user.Id.ToString()),
                    new Claim(MyCalims.UserName, user.UserName!),
                    new Claim(MyCalims.Email, user.Email!),
                }.Concat(additionalClaims ?? Enumerable.Empty<Claim>()),
                signingCredentials: new SigningCredentials(
                    key: GetSecurityKey(),
                    algorithm: SecurityAlgorithms.HmacSha256
                    ),
                expires: DateTime.Now.AddDays(expireDays)
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        public static string GenerateToken(UserEntity user) => PrivateGenerateToken(user, 1);

        public static string GenerateToken(UserEntity user, int expireDays) => PrivateGenerateToken(user, expireDays);

        public static string GenerateToken(UserEntity user, List<Claim> additionalClaims) => PrivateGenerateToken(user, 1, additionalClaims);

        public static string GenerateToken(UserEntity user, Claim claim) => PrivateGenerateToken(user, 1, new List<Claim> { claim });

        public static string GenerateToken(UserEntity user, int expireDays, List<Claim> additionalClaims) => PrivateGenerateToken(user, expireDays, additionalClaims);



        public static SecurityKey GetSecurityKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
    }
}