using ru.novolabs.SuperCore.DictionaryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    [OldSaveMethod]
    public class CommentSourceDictionaryItem : DictionaryItem {
        public CommentSourceDictionaryItem()
        {

        }

        private Int32 kind = LimsCommentSourceType.INFO;

        [CSN("Kind")]
        public Int32 Kind
        {
            get { return kind; }
            set { kind = value; }
        }
    }

    public class CommentSourceDictionary : DictionaryClass<CommentSourceDictionaryItem>
    {
        public CommentSourceDictionary(String DictionaryName) : base(DictionaryName) { }
        [CSN("CommentSource")]
        public List<CommentSourceDictionaryItem> CommentSource
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}
