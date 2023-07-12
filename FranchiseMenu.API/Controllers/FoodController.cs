using FranchiseMenu.BLL.Abstract;
using FranchiseMenu.ENTITY.Dtos.FoodDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FranchiseMenu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private IFoodService _foodService;

        public FoodController(IFoodService foodService)
        {
            _foodService = foodService;
        }

        [HttpPost("addfood")]
        public IActionResult FoodAdd(FoodAddDto dto)
        {
            var result = _foodService.FoodAdd(dto);
            return Ok(result);
        }

        [HttpPost("changefoodstatus")]
        public IActionResult FoodStatusChange(int foodId,string token)
        {
            var result = _foodService.FoodChangeStatus(foodId,token);
            return Ok(result);
        }

        [HttpGet("getallfood")]
        public IActionResult FoodGetAll(string token)
        {
            var result = _foodService.FoodGetAll(token);
            return Ok(result);
        }

        [HttpGet("getalldeactivefood")]
        public IActionResult DeactiveFoodGetAll(string token)
        {
            var result = _foodService.DeactiveFoodGetAll(token);
            return Ok(result);
        }

        [HttpGet("getbyidfood")]
        public IActionResult FoodGetById(int foodId, string token)
        {
            var result = _foodService.FoodGetById(foodId,token);
            return Ok(result);
        }

        [HttpPost("updatefood")]
        public IActionResult FoodUpdate(FoodUpdateDto dto)
        {
            var result = _foodService.FoodUpdate(dto);
            return Ok(result);
        }

        [HttpGet("foodgetbycategoryid")]
        public IActionResult FoodGetByCategoryId(int categoryId, string token)
        {
            var result = _foodService.FoodGetByCategoryId(categoryId, token);
            return Ok(result);
        }
    }
}
