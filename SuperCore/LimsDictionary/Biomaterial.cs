using System;
using System.Collections.Generic;
using System.Linq;
using ru.novolabs.SuperCore.DictionaryCore;
using System.Text;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    [OldSaveMethod]
    public class BiomaterialDictionaryItem : DictionaryItem
    {
        public BiomaterialDictionaryItem()
        {
            CommentSources = new List<CommentSourceDictionaryItem>();
        }


        [CSN("CommentSources")]
        public List<CommentSourceDictionaryItem> CommentSources { get; set; }

        [CSN("EngName")]
        public string EngName { get; set; }

        [CSN("Color")]
        public Int32 Color { get; set; }

        [CSN("NrSuffix")]
        public String NrSuffix { get; set; }

        [CSN("Comment")]
        [SendToServer(false)]
        public string Comment
        {
            get
            {
                StringBuilder result = new StringBuilder();
                List<CommentSourceDictionaryItem> comments = new List<CommentSourceDictionaryItem>();
                foreach (CommentSourceDictionaryItem comment in this.CommentSources.Where(x => x.Removed == false && x.Kind == LimsCommentSourceType.MANUAL))
                {
                    if (!comments.Contains(comment))
                    {
                        comments.Add(comment);
                        result.Append(comment.Name).Append(". ");
                    }
                }
                return result.ToString();
            }
        }
    }

    public class BiomaterialDictionary : DictionaryClass<BiomaterialDictionaryItem>
    {
        public BiomaterialDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("BioMaterial")]
        public List<BiomaterialDictionaryItem> BioMaterial
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}