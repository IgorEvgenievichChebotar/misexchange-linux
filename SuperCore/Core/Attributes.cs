using System;

namespace ru.novolabs.SuperCore
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ProcessorName : Attribute
    {
        public ProcessorName(String name)
        {
            Name = name;
        }

        public String Name { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ContentType : Attribute
    {
        public ContentType(Type contentType)
        {
            Type = contentType;
        }

        public Type Type { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class LinkToJava : Attribute
    {
        public LinkToJava(Int32 link)
        {
            Link = link;
        }

        public Int32 Link { get; private set; }
    }


    [AttributeUsage(AttributeTargets.All)]
    public class WriteNull : Attribute
    {
        private bool isWriteNull = false;
        private Object defaultValue = null;


        public WriteNull(bool isWriteNull, Object defaultValue)
        {
            this.isWriteNull = isWriteNull;
            this.defaultValue = defaultValue;
        }

        public virtual bool IsWriteNull
        {
            get { return isWriteNull; }
        }

        public virtual Object DefaultValue
        {
            get { return defaultValue; }
        }
    }


    [AttributeUsage(AttributeTargets.All)]
    public class Mandatory : Attribute
    {
        private bool isMandatory = false;

        public Mandatory(bool isMandatory)
        {
            this.isMandatory = isMandatory;
        }

        public virtual bool IsMandatory
        {
            get { return isMandatory; }
        }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class VisibleName : Attribute
    {
        private string name = string.Empty;

        public VisibleName(string name)
        {
            this.name = name;
        }

        public virtual string displayName
        {
            get { return displayName; }
        }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class SendToServer : Attribute
    {
        private bool sendable = true;

        public SendToServer(bool send)
        {
            this.sendable = send;
        }

        public virtual bool Sendable
        {
            get { return sendable; }
        }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class SendAsRef : Attribute
    {
        private bool asRef = true;

        public SendAsRef(bool send)
        {
            this.asRef = send;
        }

        public virtual bool AsRef
        {
            get { return asRef; }
        }
    }


    [AttributeUsage(AttributeTargets.All)]
    public class SendAsInt : Attribute
    {
        private bool asInt = true;

        public SendAsInt(bool send)
        {
            this.asInt = send;
        }

        public virtual bool AsRef
        {
            get { return asInt; }
        }
    }


    [AttributeUsage(AttributeTargets.All)]
    public class Unnamed : Attribute
    { }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ClonePropName : Attribute
    {
        public ClonePropName(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }


    [AttributeUsage(AttributeTargets.All)]
    public class DateTimeFormat : Attribute
    {
        private string format = @"dd/MM/yyyy HH:mm";      

        public DateTimeFormat(string format)
        {
            this.format = format;
        }

        public virtual string Format
        {
            get { return format; }
        }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class SendEmpty : System.Attribute
    { }

    [AttributeUsage(AttributeTargets.Property)]
    public class GuiIgnore : Attribute
    {
    }

    /// <summary>
    /// Предназначено для SerializeHelper2
    /// Атрибутом помечаются списки, не имеющие основного отркувающего и закрывающего тэга
    /// Элементы такого списка размещаются непосредственно в родительском объекте
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class NoMainNode : System.Attribute
    { }



    /// <summary>
    /// Предназначено для SerializeHelper2
    /// Отображаемое в XML имя элемента внутри списка
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class ElementName : Attribute
    {
        private string name = "";

        public ElementName(string name)
        {
            this.name = name;
        }

        public virtual string Name
        {
            get { return name; }
            set { this.name = value; }
        }
    }

}
