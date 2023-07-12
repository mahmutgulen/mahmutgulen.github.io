using Autofac.Core.Activators.Delegate;
using FranchiseMenu.BLL.Abstract;
using FranchiseMenu.BLL.Contants;
using FranchiseMenu.CORE.Entities;
using FranchiseMenu.CORE.Utilities.Result;
using FranchiseMenu.DAL.Abstract;
using FranchiseMenu.DAL.Concrete;
using FranchiseMenu.ENTITY.Concrete;
using FranchiseMenu.ENTITY.Dtos.CategoryDtos;
using FranchiseMenu.ENTITY.Dtos.FoodDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FranchiseMenu.BLL.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private ICategoryDal _categoryDal;
        private ISessionService _sessionService;

        public CategoryManager(ICategoryDal categoryDal, ISessionService sessionService)
        {
            _categoryDal = categoryDal;
            _sessionService = sessionService;
        }

        public IDataResult<bool> CategoryAdd(CategoryAddDto dto)
        {
            try
            {
                var tokenCheck = _sessionService.TokenCheck(dto.Token);
                if (!tokenCheck.Success)
                {
                    return new ErrorDataResult<bool>(false, tokenCheck.Message, tokenCheck.MessageCode);
                }

                var category = _categoryDal.Get(x => x.CategoryName == dto.CategoryName);
                if (category != null)
                {
                    if (category.CategoryStatus == false)
                    {
                        return new ErrorDataResult<bool>(false, "category available but not active", Messages.category_available_but_not_active);
                    }
                    return new ErrorDataResult<bool>(false, "category already exists", Messages.category_already_exists);
                }

                var categoryAdd = new Category
                {
                    CategoryDescription = dto.CategoryDescription,
                    CategoryName = dto.CategoryName,
                    CategoryStatus = dto.CategoryStatus,
                };
                _categoryDal.Add(categoryAdd);
                return new SuccessDataResult<bool>(true, "category added", Messages.success);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<bool>(false, e.Message, Messages.unknownError);
            }
        }

        public IDataResult<bool> CategoryChangeStatus(int categoryId, string token)
        {
            try
            {
                var tokenCheck = _sessionService.TokenCheck(token);
                if (!tokenCheck.Success)
                {
                    return new ErrorDataResult<bool>(false, tokenCheck.Message, tokenCheck.MessageCode);
                }

                var category = _categoryDal.Get(x => x.Id == categoryId);
                if (category == null)
                {
                    return new ErrorDataResult<bool>(false, "category not found", Messages.category_not_found);
                }
                if (category.CategoryStatus == true)
                {
                    category.CategoryStatus = false;
                    _categoryDal.Update(category);
                    return new SuccessDataResult<bool>(true, "category status changed", Messages.category_status_changed);
                }
                category.CategoryStatus = true;
                _categoryDal.Update(category);
                return new SuccessDataResult<bool>(true, "category status changed", Messages.category_status_changed);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<bool>(false, e.Message, Messages.unknownError);
            }
        }

        public IDataResult<List<CategoryGetAllDto>> CategoryGetAll(string token)
        {
            try
            {
                var tokenCheck = _sessionService.TokenCheck(token);
                if (!tokenCheck.Success)
                {
                    return new ErrorDataResult<List<CategoryGetAllDto>>(new List<CategoryGetAllDto>(), tokenCheck.Message, tokenCheck.MessageCode);
                }

                var categories = _categoryDal.GetAll(x => x.CategoryStatus == true);
                if (categories == null)
                {
                    return new ErrorDataResult<List<CategoryGetAllDto>>(new List<CategoryGetAllDto>(), "category not found", Messages.category_not_found);
                }

                var list = new List<CategoryGetAllDto>();
                foreach (var item in categories)
                {
                    list.Add(new CategoryGetAllDto
                    {
                        CategoryDescription = item.CategoryDescription,
                        CategoryName = item.CategoryName,
                        CategoryStatus = item.CategoryStatus,
                        Id = item.Id
                    });
                }
                return new SuccessDataResult<List<CategoryGetAllDto>>(list, "ok", Messages.success);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<List<CategoryGetAllDto>>(new List<CategoryGetAllDto>(), e.Message, Messages.unknownError);
            }
        }

        public IDataResult<CategoryGetByIdDto> CategoryGetById(int categoryId, string token)
        {
            try
            {
                var tokenCheck = _sessionService.TokenCheck(token);
                if (!tokenCheck.Success)
                {
                    return new ErrorDataResult<CategoryGetByIdDto>(new CategoryGetByIdDto(), tokenCheck.Message, tokenCheck.MessageCode);
                }

                var category = _categoryDal.Get(x => x.Id == categoryId);
                if (category == null)
                {
                    return new ErrorDataResult<CategoryGetByIdDto>(new CategoryGetByIdDto(), "category not found", Messages.category_not_found);
                }

                var dto = new CategoryGetByIdDto
                {
                    CategoryDescription = category.CategoryDescription,
                    CategoryName = category.CategoryName,
                    CategoryStatus = category.CategoryStatus,
                    Id = category.Id
                };
                return new SuccessDataResult<CategoryGetByIdDto>(dto, "ok", Messages.success);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<CategoryGetByIdDto>(new CategoryGetByIdDto(), e.Message, Messages.unknownError);
            }
        }

        public IDataResult<bool> CategoryUpdate(CategoryUpdateDto dto)
        {
            try
            {
                var tokenCheck = _sessionService.TokenCheck(dto.Token);
                if (!tokenCheck.Success)
                {
                    return new ErrorDataResult<bool>(false, tokenCheck.Message, tokenCheck.MessageCode);
                }

                var category = _categoryDal.Get(x => x.Id == dto.CategoryId);

                if (category == null)
                {
                    return new ErrorDataResult<bool>(false, "category not found", Messages.category_not_found);
                }

                var nameCheck = _categoryDal.Get(x => x.Id != category.Id && x.CategoryName == dto.CategoryName);

                if (nameCheck != null)
                {
                    return new ErrorDataResult<bool>(false, "category of the same name exists", Messages.category_of_the_same_name_exists);
                }

                category.CategoryStatus = dto.CategoryStatus;
                category.CategoryName = dto.CategoryName;
                category.CategoryDescription = dto.CategoryDescription;
                _categoryDal.Update(category);
                return new SuccessDataResult<bool>(true, "category updated", Messages.success);

            }
            catch (Exception e)
            {
                return new ErrorDataResult<bool>(false, e.Message, Messages.unknownError);
            }
        }

        public IDataResult<List<DeactiveCategoryGetAllDto>> DeactiveCategoryGetAll(string token)
        {
            try
            {
                var tokenCheck = _sessionService.TokenCheck(token);
                if (!tokenCheck.Success)
                {
                    return new ErrorDataResult<List<DeactiveCategoryGetAllDto>>(new List<DeactiveCategoryGetAllDto>(), tokenCheck.Message, tokenCheck.MessageCode);
                }

                var deactiveCategories = _categoryDal.GetAll(x => x.CategoryStatus == false).ToList();

                if (deactiveCategories.Count == 0)
                {
                    return new ErrorDataResult<List<DeactiveCategoryGetAllDto>>(new List<DeactiveCategoryGetAllDto>(), "category not found", Messages.category_not_found);
                }

                var list = new List<DeactiveCategoryGetAllDto>();

                foreach (var item in deactiveCategories)
                {
                    list.Add(new DeactiveCategoryGetAllDto
                    {
                        CategoryStatus = item.CategoryStatus,
                        CategoryDescription = item.CategoryDescription,
                        CategoryName = item.CategoryName,
                        Id = item.Id
                    });
                }

                return new SuccessDataResult<List<DeactiveCategoryGetAllDto>>(list, "success", Messages.success);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<List<DeactiveCategoryGetAllDto>>(new List<DeactiveCategoryGetAllDto>(), e.Message, Messages.unknownError);
            }
        }
    }
}
