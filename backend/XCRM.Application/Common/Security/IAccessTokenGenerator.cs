using System;
using System.Collections.Generic;
using System.Text;

namespace XCRM.Application.Common.Security
{
    public interface IAccessTokenGenerator
    {
        AccessTokenResult Generate(long userId, string username);
    }
}
