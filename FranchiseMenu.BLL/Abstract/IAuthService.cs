using FranchiseMenu.CORE.Utilities.Result;
using FranchiseMenu.ENTITY.Dtos.AuthDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FranchiseMenu.BLL.Abstract
{
    public interface IAuthService
    {
        IDataResult<bool> RegisterForManagerAdmin(RegisterForManagerDto dto);
        IDataResult<LoginSecurityDto> AdminLogin(AdminLoginDto dto);
        IDataResult<bool> AdminPasswordChange(AdminPasswordChangeDto dto);
    }
}
