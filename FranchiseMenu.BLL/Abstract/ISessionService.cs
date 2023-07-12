using FranchiseMenu.CORE.Security.Dtos;
using FranchiseMenu.CORE.Utilities.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FranchiseMenu.BLL.Abstract
{
    public interface ISessionService
    {
        IDataResult<SessionCheckResponseDto> TokenCheck(string token);
    }
}
