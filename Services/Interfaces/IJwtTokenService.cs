using Services.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IJwtTokenService
    {
        public string GenerateUserToken(RequestTokenModel request);
    }
}
