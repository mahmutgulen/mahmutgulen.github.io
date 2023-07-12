using FranchiseMenu.BLL.Abstract;
using FranchiseMenu.BLL.Contants;
using FranchiseMenu.CORE.Entities;
using FranchiseMenu.CORE.Utilities.Result;
using FranchiseMenu.DAL.Abstract;
using FranchiseMenu.ENTITY.Concrete;
using FranchiseMenu.ENTITY.Dtos.FoodDtos;
using FranchiseMenu.ENTITY.Enums;
using System;
using System.Collections.Generic;

namespace FranchiseMenu.BLL.Concrete
{
    public class FoodManager : IFoodService
    {
        private IFoodDal _foodDal;
        private ISessionService _sessionService;

        public FoodManager(IFoodDal foodDal, ISessionService sessionService)
        {
            _foodDal = foodDal;
            _sessionService = sessionService;
        }

        public IDataResult<List<DeactiveFoodGetAllDto>> DeactiveFoodGetAll(string token)
        {
            try
            {
                var tokenCheck = _sessionService.TokenCheck(token);
                if (!tokenCheck.Success)
                {
                    return new ErrorDataResult<List<DeactiveFoodGetAllDto>>(new List<DeactiveFoodGetAllDto>(), tokenCheck.Message, tokenCheck.MessageCode);
                }

                var deactiveFoods = _foodDal.GetAll(x => x.FoodStatus == false).ToList();

                if (deactiveFoods.Count == 0)
                {
                    return new ErrorDataResult<List<DeactiveFoodGetAllDto>>(new List<DeactiveFoodGetAllDto>(), "food not found", Messages.food_not_found);
                }

                var list = new List<DeactiveFoodGetAllDto>();

                foreach (var item in deactiveFoods)
                {
                    list.Add(new DeactiveFoodGetAllDto
                    {
                        Id = item.Id,
                        CategoryId = item.CategoryId,
                        FoodDescription = item.FoodDescription,
                        FoodName = item.FoodName,
                        FoodImage = item.FoodImage,
                        FoodPrice = item.FoodPrice,
                        FoodStatus = item.FoodStatus
                    });
                }

                return new SuccessDataResult<List<DeactiveFoodGetAllDto>>(list, "success", Messages.success);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<List<DeactiveFoodGetAllDto>>(new List<DeactiveFoodGetAllDto>(), e.Message, Messages.unknownError);
            }
        }


        public IDataResult<bool> FoodAdd(FoodAddDto dto)
        {
            try
            {
                var tokenCheck = _sessionService.TokenCheck(dto.Token);
                if (!tokenCheck.Success)
                {
                    return new ErrorDataResult<bool>(false, tokenCheck.Message, tokenCheck.MessageCode);
                }

                var food = _foodDal.Get(x => x.FoodName == dto.FoodName);
                if (food != null)
                {
                    if (food.FoodStatus == false)
                    {
                        return new ErrorDataResult<bool>(false, "food available but not active", Messages.food_available_but_not_active);
                    }
                    return new ErrorDataResult<bool>(false, "food already exists", Messages.food_already_exists);
                }

                var foodAdd = new Food
                {
                    FoodName = dto.FoodName,
                    FoodDescription = dto.FoodDescription,
                    FoodImage = dto.FoodImage,
                    FoodPrice = dto.FoodPrice,
                    FoodStatus = dto.FoodStatus,
                    CategoryId = dto.CategoryId,
                };
                _foodDal.Add(foodAdd);
                return new SuccessDataResult<bool>(true, "food added", Messages.success);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<bool>(false, e.Message, Messages.unknownError);
            }
        }

        public IDataResult<bool> FoodChangeStatus(int foodId, string token)
        {
            try
            {
                var tokenCheck = _sessionService.TokenCheck(token);
                if (!tokenCheck.Success)
                {
                    return new ErrorDataResult<bool>(false, tokenCheck.Message, tokenCheck.MessageCode);
                }

                var food = _foodDal.Get(x => x.Id == foodId);
                if (food == null)
                {
                    return new ErrorDataResult<bool>(false, "food not found", Messages.food_not_found);
                }

                if (food.FoodStatus == true)
                {
                    food.FoodStatus = false;
                    _foodDal.Update(food);
                    return new SuccessDataResult<bool>(true, "food status changed", Messages.food_status_changed);
                }
                food.FoodStatus = true;
                _foodDal.Update(food);
                return new SuccessDataResult<bool>(true, "food status changed", Messages.food_status_changed);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<bool>(false, e.Message, Messages.unknownError);
            }
        }

        public IDataResult<List<FoodGetAllDto>> FoodGetAll(string token)
        {
            try
            {
                var tokenCheck = _sessionService.TokenCheck(token);
                if (!tokenCheck.Success)
                {
                    return new ErrorDataResult<List<FoodGetAllDto>>(new List<FoodGetAllDto>(), tokenCheck.Message, tokenCheck.MessageCode);
                }

                var foods = _foodDal.GetAll(x => x.FoodStatus == true);
                if (foods == null)
                {
                    return new ErrorDataResult<List<FoodGetAllDto>>(new List<FoodGetAllDto>(), "food not found", Messages.food_not_found);
                }
                var list = new List<FoodGetAllDto>();
                foreach (var item in foods)
                {
                    list.Add(new FoodGetAllDto
                    {
                        FoodDescription = item.FoodDescription,
                        FoodImage = item.FoodImage,
                        FoodName = item.FoodName,
                        FoodPrice = item.FoodPrice,
                        FoodStatus = item.FoodStatus,
                        Id = item.Id,
                        CategoryId = item.CategoryId
                    });
                }
                return new SuccessDataResult<List<FoodGetAllDto>>(list, "success", Messages.success);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<List<FoodGetAllDto>>(new List<FoodGetAllDto>(), e.Message, Messages.unknownError);
            }
        }

        public IDataResult<List<FoodGetByCategoryIdDto>> FoodGetByCategoryId(int categoryId, string token)
        {
            try
            {
                var tokenCheck = _sessionService.TokenCheck(token);
                if (!tokenCheck.Success)
                {
                    return new ErrorDataResult<List<FoodGetByCategoryIdDto>>(new List<FoodGetByCategoryIdDto>(), tokenCheck.Message, tokenCheck.MessageCode);
                }

                var foods = _foodDal.GetAll(x => x.CategoryId == categoryId).ToList();
                if (foods.Count == 0)
                {
                    return new ErrorDataResult<List<FoodGetByCategoryIdDto>>(new List<FoodGetByCategoryIdDto>(), "food not found", Messages.food_not_found);
                }

                var list = new List<FoodGetByCategoryIdDto>();

                foreach (var item in foods)
                {
                    list.Add(new FoodGetByCategoryIdDto
                    {
                        CategoryId = item.CategoryId,
                        FoodDescription = item.FoodDescription,
                        FoodImage = item.FoodImage,
                        FoodName = item.FoodName,
                        FoodPrice = item.FoodPrice,
                        FoodStatus = item.FoodStatus,
                        Id = item.Id
                    });
                }
                return new SuccessDataResult<List<FoodGetByCategoryIdDto>>(list, "ok", Messages.success);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<List<FoodGetByCategoryIdDto>>(new List<FoodGetByCategoryIdDto>(), e.Message, Messages.unknownError);
            }
        }

        public IDataResult<FoodGetByIdDto> FoodGetById(int foodId, string token)
        {
            try
            {
                var tokenCheck = _sessionService.TokenCheck(token);
                if (!tokenCheck.Success)
                {
                    return new ErrorDataResult<FoodGetByIdDto>(new FoodGetByIdDto(), tokenCheck.Message, tokenCheck.MessageCode);
                }

                var food = _foodDal.Get(x => x.Id == foodId);
                if (food == null)
                {
                    return new ErrorDataResult<FoodGetByIdDto>(new FoodGetByIdDto(), "food not found", Messages.food_not_found);
                }

                var dto = new FoodGetByIdDto
                {
                    CategoryId = food.CategoryId,
                    FoodDescription = food.FoodDescription,
                    FoodImage = food.FoodImage,
                    FoodName = food.FoodName,
                    FoodPrice = food.FoodPrice,
                    FoodStatus = food.FoodStatus,
                    Id = food.Id,
                };
                return new SuccessDataResult<FoodGetByIdDto>(dto, "ok", Messages.success);

            }
            catch (Exception e)
            {
                return new ErrorDataResult<FoodGetByIdDto>(new FoodGetByIdDto(), e.Message, Messages.unknownError);
            }
        }

        public IDataResult<bool> FoodUpdate(FoodUpdateDto dto)
        {

            var tokenCheck = _sessionService.TokenCheck(dto.Token);
            if (!tokenCheck.Success)
            {
                return new ErrorDataResult<bool>(false, tokenCheck.Message, tokenCheck.MessageCode);
            }

            try
            {
                var food = _foodDal.Get(x => x.Id == dto.FoodId);

                if (food == null)
                {
                    return new ErrorDataResult<bool>(false, "food not found", Messages.food_not_found);
                }

                var nameCheck = _foodDal.Get(x => x.Id != food.Id && x.FoodName == dto.FoodName);

                if (nameCheck != null)
                {
                    return new ErrorDataResult<bool>(false, "food of the same name exists", Messages.food_of_the_same_name_exists);
                }

                food.FoodStatus = dto.FoodStatus;
                food.FoodName = dto.FoodName;
                food.FoodPrice = dto.FoodPrice;
                food.FoodDescription = dto.FoodDescription;
                food.CategoryId = dto.CategoryId;
                food.FoodImage = dto.FoodImage;

                _foodDal.Update(food);
                return new SuccessDataResult<bool>(true, "food updated", Messages.success);

            }
            catch (Exception e)
            {
                return new ErrorDataResult<bool>(false, e.Message, Messages.unknownError);
            }
        }
    }
}
