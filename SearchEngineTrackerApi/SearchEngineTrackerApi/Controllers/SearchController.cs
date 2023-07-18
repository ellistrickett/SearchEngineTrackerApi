using Microsoft.AspNetCore.Mvc;
using SearchEngineTrackerApi.Interfaces;
using SearchEngineTrackerApi.Models;

namespace SearchEngineTrackerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;
        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        // Post: api/[controller]
        [HttpPost]
        public async Task<ActionResult> Search([FromBody] SearchModel searchModel)
        {
            try
            {
                var result = await _searchService.Search(searchModel);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
