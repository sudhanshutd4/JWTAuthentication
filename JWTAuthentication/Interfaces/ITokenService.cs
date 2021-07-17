namespace JWTAuthentication.Interfaces
{
    using JWTAuthentication.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public interface ITokenService
    {
        string CreateToken(AppUser appUser);
    }
}
