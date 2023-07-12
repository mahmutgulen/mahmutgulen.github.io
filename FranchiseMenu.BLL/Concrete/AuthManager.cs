using FranchiseMenu.BLL.Abstract;
using FranchiseMenu.BLL.Contants;
using FranchiseMenu.CORE.Entities.Concrete;
using FranchiseMenu.CORE.Security;
using FranchiseMenu.CORE.Utilities.Result;
using FranchiseMenu.DAL.Abstract;
using FranchiseMenu.ENTITY.Concrete;
using FranchiseMenu.ENTITY.Dtos.AuthDtos;

namespace FranchiseMenu.BLL.Concrete
{
    public class AuthManager : IAuthService
    {
        private IAdminDal _adminDal;
        private ITokenHelper _tokenHelper;
        private ISecurityHistoryDal _securityHistoryDal;
        private ISessionService _sessionService;

        public AuthManager(IAdminDal adminDal, ITokenHelper tokenHelper, ISecurityHistoryDal securityHistoryDal, ISessionService sessionService)
        {
            _adminDal = adminDal;
            _tokenHelper = tokenHelper;
            _securityHistoryDal = securityHistoryDal;
            _sessionService = sessionService;
        }

        public IDataResult<bool> ExistsCheck(string adminEmail, string phoneNumber)
        {
            var admin = _adminDal.Get(x => x.AdminEmail == adminEmail || x.AdminPhoneNumber == phoneNumber);
            if (admin == null)
            {
                return new SuccessDataResult<bool>(true);
            }
            if (admin.AdminEmail == adminEmail)
            {
                return new ErrorDataResult<bool>(false, "email already exists", Messages.email_already_exists);
            }
            if (admin.AdminPhoneNumber == phoneNumber)
            {
                return new ErrorDataResult<bool>(false, "phone number already exists", Messages.phone_number_already_exists);
            }
            return new SuccessDataResult<bool>(true);
        }

        public IDataResult<LoginSecurityDto> AdminLogin(AdminLoginDto dto)
        {
            try
            {
                var admin = _adminDal.Get(x => x.AdminEmail == dto.AdminEmail);
                if (admin == null)
                {
                    return new ErrorDataResult<LoginSecurityDto>(new LoginSecurityDto(), "admin not found", Messages.admin_not_found);
                }

                if (!HashingHelper.VerifyPasswordHash(dto.AdminPassword, admin.AdminPasswordSalt, admin.AdminPasswordHash))
                {
                    return new ErrorDataResult<LoginSecurityDto>(new LoginSecurityDto(), "admin password is wrong", Messages.admin_wrong_password);
                }

                //eski tokenleri deactive ediyorum
                var oldTokens = _securityHistoryDal.GetAll(x => x.AdminId == admin.Id).ToList();
                foreach (var item in oldTokens)
                {
                    item.Status = false;
                    _securityHistoryDal.Update(item);
                }

                var token = _tokenHelper.CreateNewToken(admin);

                var loginDto = new LoginSecurityDto
                {
                    Token = token.Data.TokenString,
                    ExpireDate = token.Data.ExpireDate
                };

                var securityHistory = new SecurityHistory
                {
                    AdminId = admin.Id,
                    ExpireDate = token.Data.ExpireDate,
                    Status = true,
                    TokenString = token.Data.TokenString,
                };
                _securityHistoryDal.Add(securityHistory);

                return new SuccessDataResult<LoginSecurityDto>(loginDto, "ok", Messages.success);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<LoginSecurityDto>(new LoginSecurityDto(), e.Message, Messages.unknownError);
            }
        }

        public IDataResult<bool> RegisterForManagerAdmin(RegisterForManagerDto dto)
        {
            try
            {
                var checkMail = ExistsCheck(dto.AdminEmail, dto.AdminPhoneNumber);
                if (!checkMail.Success)
                {
                    return new ErrorDataResult<bool>(false, checkMail.Message, checkMail.MessageCode);
                }

                byte[] passwordSalt, passwordHash;
                HashingHelper.CreatePasswordHash(dto.AdminPassword, out passwordSalt, out passwordHash);

                var adminAdd = new Admin
                {
                    AdminEmail = dto.AdminEmail,
                    AdminName = dto.AdminName,
                    AdminPhoneNumber = dto.AdminPhoneNumber,
                    AdminStatus = true,
                    AdminSurname = dto.AdminSurname,
                    AdminPasswordHash = passwordHash,
                    AdminPasswordSalt = passwordSalt,
                };
                _adminDal.Add(adminAdd);
                return new SuccessDataResult<bool>(true, "ok", Messages.success);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<bool>(false, e.Message, Messages.unknownError);
            }
        }

        public IDataResult<bool> AdminPasswordChange(AdminPasswordChangeDto dto)
        {
            try
            {
                var tokenCheck = _sessionService.TokenCheck(dto.Token);
                if (!tokenCheck.Success)
                {
                    return new ErrorDataResult<bool>(false, tokenCheck.Message, tokenCheck.MessageCode);
                }

                var admin = _adminDal.Get(x => x.Id == tokenCheck.Data.AdminId);

                if (admin == null)
                {
                    return new ErrorDataResult<bool>(false, "admin not found", Messages.admin_not_found);
                }

                if (!HashingHelper.VerifyPasswordHash(dto.OldPassword, admin.AdminPasswordSalt, admin.AdminPasswordHash))
                {
                    return new ErrorDataResult<bool>(false, "old password password is wrong", Messages.admin_wrong_password);
                }

                if (dto.OldPassword == dto.NewPassword)
                {
                    return new ErrorDataResult<bool>(false, "new password cannot be same old password", Messages.new_password_cannot_the_same_old_password);
                }

                if (dto.NewPassword != dto.ConfirmPassword)
                {
                    return new ErrorDataResult<bool>(false, "passwords not match", Messages.passwords_not_match);
                }

                byte[] passwordSalt, passwordHash;
                HashingHelper.CreatePasswordHash(dto.NewPassword, out passwordSalt, out passwordHash);

                admin.AdminPasswordHash = passwordHash;
                admin.AdminPasswordSalt = passwordSalt;
                _adminDal.Update(admin);

                return new SuccessDataResult<bool>(true, "admin password changed", Messages.admin_password_changed);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<bool>(false, e.Message, Messages.unknownError);
            }
        }
    }
}
