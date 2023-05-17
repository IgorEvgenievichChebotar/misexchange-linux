using System.Collections.Generic;

namespace ru.novolabs.SuperCore.Reporting
{
    public class GetDictionaryResponse
    {
        public GetDictionaryResponse()
        {
            Items = new List<DictionaryItemIdCaptionPair>();
        }

        public ErrorMessage Message { get; set; }
        public string DictionaryName { get; set; }
        public List<DictionaryItemIdCaptionPair> Items { get; set; }
    }
}