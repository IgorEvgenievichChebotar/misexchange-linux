using ru.novolabs.SuperCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class ControlsFormLayout
    {
        public ControlsFormLayout()
        {
            Pages = new ControlsFormLayoutPageList();        
        }
        [CSN("Pages")]
        public ControlsFormLayoutPageList Pages { get; set; }
    }

    public class ControlsFormLayoutPageList : List<ControlsFormLayoutPage>
    {
        public ControlsFormLayoutPage AddPage()
        {
            var newPage = new ControlsFormLayoutPage();
            base.Add(newPage);
            newPage.PageIndex = this.IndexOf(newPage);
            newPage.Caption = "Страница " + newPage.PageIndex.ToString();
            return newPage;
        }    
    }

    public class ControlsFormLayoutPage
    {
        public ControlsFormLayoutPage()
        {
            ControlsForm = new ControlsForm();        
        }

        [DisplayName("Порядковый номер")]
        [CSN("PageIndex")]
        public int PageIndex { get; set; }
        [DisplayName("Заголовок")]
        [CSN("Caption")]
        public String Caption { get; set; }
        [Browsable(false)]
        [CSN("ControlsForm")]
        public ControlsForm ControlsForm { get; set; }
    }
}
