using FranchiseMenu.CORE.Utilities.Result;
using FranchiseMenu.ENTITY.Dtos.CategoryDtos;

namespace FranchiseMenu.BLL.Abstract
{
    public interface ICategoryService
    {
        IDataResult<bool> CategoryAdd(CategoryAddDto dto);
        IDataResult<bool> CategoryUpdate(CategoryUpdateDto dto);
        IDataResult<bool> CategoryChangeStatus(int categoryId, string token);
        IDataResult<List<CategoryGetAllDto>> CategoryGetAll(string token);
        IDataResult<List<DeactiveCategoryGetAllDto>> DeactiveCategoryGetAll(string token);
        IDataResult<CategoryGetByIdDto> CategoryGetById(int categoryId, string token);
        //Paging ekle !!!
    }
}
