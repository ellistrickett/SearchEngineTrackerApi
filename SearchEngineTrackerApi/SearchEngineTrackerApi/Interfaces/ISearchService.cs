using SearchEngineTrackerApi.Models;

namespace SearchEngineTrackerApi.Interfaces
{
    public interface ISearchService
    {
        Task<string> Search(SearchModel searchModel);
    }
}
