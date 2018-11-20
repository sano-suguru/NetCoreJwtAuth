using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetCoreJwtAuth.Data;
using NetCoreJwtAuth.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace NetCoreJwtAuth.Services {
  public interface IAuthService {
    AppUser Authenticate(string userName, string password);
  }

  public class JwtAuthService : IAuthService {
    readonly MyDbContext myDbContext;
    readonly AppSettings appSettings;

    public JwtAuthService(MyDbContext myDbContext, IOptions<AppSettings> appSettings, IHostingEnvironment env) {
      this.myDbContext = myDbContext;
      this.appSettings = appSettings.Value;
      if (env.IsDevelopment() && !this.myDbContext.Users.Any()) {
        this.myDbContext.Database.EnsureCreated();
      }
    }

    AppUser IAuthService.Authenticate(string userName, string password) {
      var foundUser = myDbContext.Users.SingleOrDefault(
        x => x.UserName == userName && x.Password == password);

      if (foundUser is null) {
        return null;
      }
      foundUser.Token = GetSerializedToken(foundUser.ID, expiredDay: DateTime.UtcNow.AddDays(7));
      foundUser.Password = null;
      return foundUser;
    }

    string GetSerializedToken(int id, DateTime expiredDay) {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(appSettings.Secret);
      var tokenDescriptor = new SecurityTokenDescriptor {
        Subject = new ClaimsIdentity(new Claim[] {
          new Claim(ClaimTypes.Name, id.ToString())
        }),
        Expires = expiredDay,
        SigningCredentials = new SigningCredentials(
          key: new SymmetricSecurityKey(key),
          algorithm: SecurityAlgorithms.HmacSha256Signature
        )
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      var serializedToken = tokenHandler.WriteToken(token);
      return serializedToken;
    }
  }
}
