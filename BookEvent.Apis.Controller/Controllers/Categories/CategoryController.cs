using BookEvent.Apis.Controller.Controllers.Base;
using BookEvent.Core.Application.Abstraction;
using BookEvent.Shared.Models.Categories;
using BookEvent.Shared.Models.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookEvent.Apis.Controller.Controllers.Categories
{
    [Authorize]
    public class CategoryController(IServiceManager serviceManager) : BaseApiController
    {

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("CreateCategory")]
        public async Task<ActionResult> CreateCategory([FromBody] CategoryDto categorydto, CancellationToken cancellationToken)
        {
            var result = await serviceManager.CategoriesService.CreateCategory(categorydto, cancellationToken);
            return NewResult(result);

        }
    }
}
