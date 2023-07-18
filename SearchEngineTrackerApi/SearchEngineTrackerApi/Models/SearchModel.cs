namespace SearchEngineTrackerApi.Models
{
    public class SearchModel
    {
        public required string SearchPhrase { get; set; }
        public required string TargetUrl { get; set; }
    }
}
