
namespace ru.novolabs.SuperCore
{
    public class DictionaryItemIdCaptionPair
    {
        public string Id { get; set; }
        public string Caption { get; set; }

        public override string ToString()
        {
            return Caption;
        }
    }
}