using System;

namespace Uber.Module.Movie.Search.Abstraction.Models
{
    public enum SearchItemType
    {
        FreeText = 1,
        Movie,
        Person,
        Organization
    }

    public class SearchItem
    {
        public Guid Key { get; set; }
        public string Text { get; set; }
        public SearchItemType Type { get; set; }
    }
}
