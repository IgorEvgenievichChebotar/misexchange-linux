using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using ru.novolabs.SuperCore.LimsDictionary;
using ru.novolabs.SuperCore.CommonBusinesObjects;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{

    public class ControlsForm : TableForm<ControlsFormCell, ControlRow>
    {

    }

    public class ControlsFormCell : TableFormCell<ControlRow>
    {
        public ControlsFormCell()
        {
            Content = new ControlRow();
        }
    }
}