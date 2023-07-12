using FranchiseMenu.BLL.Abstract;
using FranchiseMenu.ENTITY.Dtos.CategoryDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FranchiseMenu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("addcategory")]
        public IActionResult CategoryAdd(CategoryAddDto dto)
        {
            var result = _categoryService.CategoryAdd(dto);
            return Ok(result);
        }

        [HttpPost("changestatuscategory")]
        public IActionResult CategoryChangeStatus(int categoryId, string token)
        {
            var result = _categoryService.CategoryChangeStatus(categoryId, token);
            return Ok(result);
        }

        [HttpGet("getallcategory")]
        public IActionResult CategoryGetALl(string token)
        {
            var result = _categoryService.CategoryGetAll(token);
            return Ok(result);
        }

        [HttpGet("getalldeactivecategory")]
        public IActionResult DeactiveCategoryGetALl(string token)
        {
            var result = _categoryService.DeactiveCategoryGetAll(token);
            return Ok(result);
        }

        [HttpGet("getbyidcategory")]
        public IActionResult CategoryGetById(int categoryId, string token)
        {
            var result = _categoryService.CategoryGetById(categoryId, token);
            return Ok(result);
        }

        [HttpPost("updatecategory")]
        public IActionResult CategoryUpdate(CategoryUpdateDto dto)
        {
            var result = _categoryService.CategoryUpdate(dto);
            return Ok(result);
        }
    }
}
