using FranchiseMenu.BLL.Abstract;
using FranchiseMenu.BLL.Contants;
using FranchiseMenu.CORE.Security.Dtos;
using FranchiseMenu.CORE.Utilities.Result;
using FranchiseMenu.DAL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FranchiseMenu.BLL.Concrete
{
    public class SessionManager : ISessionService
    {
        private ISecurityHistoryDal _securityHistoryDal;

        public SessionManager(ISecurityHistoryDal securityHistoryDal)
        {
            _securityHistoryDal = securityHistoryDal;
        }

        public IDataResult<SessionCheckResponseDto> TokenCheck(string token)
        {
            var result = new SessionCheckResponseDto();

            var checkList = _securityHistoryDal.Get(x => x.TokenString == token && x.Status == true);

            if (checkList == null)
            {
                return new ErrorDataResult<SessionCheckResponseDto>(new SessionCheckResponseDto(), "Token not found", Messages.token_not_found);
            }

            result.Success = checkList.ExpireDate > DateTime.UtcNow;
            result.AdminId = checkList.AdminId;
            result.ExpireDate = checkList.ExpireDate;

            if (!result.Success)
            {
                //token expired olduysa token status değiştir
                checkList.Status = false;
                _securityHistoryDal.Update(checkList);
                return new ErrorDataResult<SessionCheckResponseDto>(result, "Token expired", Messages.token_expired);
            }
            return new SuccessDataResult<SessionCheckResponseDto>(result);
        }
    }
}
