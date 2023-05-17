using ru.novolabs.SuperCore.CommonBusinesObjects;
using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;

namespace ru.novolabs.SuperCore
{
    public class WebJournalColumn
    {
        [CSN("id")]
        public String id { get; set; }
        [CSN("name")]
        public String name { get; set; }
        [CSN("format")]
        public String format { get; set; }
        [CSN("field")]
        public String field { get; set; }
        [CSN("width")]
        public int width { get; set; }
        [CSN("sortable")]
        public Boolean sortable { get; set; }
        [CSN("autoSort")]
        public Boolean autoSort { get; set; }
        [CSN("autoSize")]
        public Boolean autoSize { get; set; }

        private static List<WebJournalColumn> GetColumnsFromLayout(List<TreeViewColumnLayout> layoutColumns)
        {
            List<WebJournalColumn> columns = new List<WebJournalColumn>();
            if (layoutColumns.Count == 0)
            {
                return columns;
            }

            layoutColumns.Sort((a, b) => a.Rank.CompareTo(b.Rank));
            for (int i = 0; i < layoutColumns.Count; i++)
            {
                TreeViewColumnLayout column = layoutColumns[i];
                if (column.UserField != null)
                {
                    UserFieldDictionaryItem userField = (UserFieldDictionaryItem)ProgramContext.Dictionaries[LimsDictionaryNames.UserField, column.UserField.Id];
                    columns.Add(new WebJournalColumn() { id = userField.Code, name = column.Caption, field = userField.Code, width = column.Width, sortable = true, format = column.Format, autoSort = column.AutoSort, autoSize = column.AutoSize });
                }
                else
                {
                    if (!String.IsNullOrEmpty(column.DictionaryPropName))
                        columns.Add(new WebJournalColumn() { id = column.PropName + "_" + column.DictionaryPropName, name = column.Caption, field = column.PropName + "_" + column.DictionaryPropName, width = column.Width, sortable = true, format = column.Format, autoSort = column.AutoSort, autoSize = column.AutoSize });
                    else
                        columns.Add(new WebJournalColumn() { id = column.PropName, name = column.Caption, field = column.PropName, width = column.Width, sortable = true, format = column.Format, autoSort = column.AutoSort, autoSize = column.AutoSize });
                }
            }

            return columns;
        }

        public static List<WebJournalColumn> GetJournalColumnsForEmployee(ru.novolabs.SuperCore.LimsDictionary.EmployeeDictionaryItem user, TreeViewLayoutList.TreeTypes journalType, int departmentId)
        {
            
            List<TreeViewColumnLayout> layoutColumns = new List<TreeViewColumnLayout>();
            if (user.UserProfile.Journals == null || user.UserProfile.Journals.Count == 0)
            {
                TreeViewLayout layout = user.UserProfile.TreeViewLayouts[(int)journalType, departmentId];
                if (layout != null)
                    layoutColumns = layout.Columns;
            }
            else
            {
                JournalLayout layout = user.UserProfile.Journals[(int)journalType, departmentId];
                if (layout != null)
                    layoutColumns = layout.Columns;
            }
            
            return GetColumnsFromLayout(layoutColumns);
        }

        public static List<WebJournalColumn> GetJournalColumnsForEmployee(ru.novolabs.SuperCore.LimsDictionary.EmployeeDictionaryItem user, string journalType, string departmentId)
        {

            List<TreeViewColumnLayout> layoutColumns = new List<TreeViewColumnLayout>();
            if (user.UserProfile.Journals == null || user.UserProfile.Journals.Count == 0)
            {
                TreeViewLayout layout = user.UserProfile.TreeViewLayouts[journalType, departmentId];
                if (layout != null)
                    layoutColumns = layout.Columns;
            }
            else
            {
                JournalLayout layout = user.UserProfile.Journals[journalType, departmentId];
                if (layout != null)
                    layoutColumns = layout.Columns;
            }

            return GetColumnsFromLayout(layoutColumns);
        }
    }

    public class FilterInfoList : List<FilterInfo>
    {
        public FilterInfo this[String journalObject, String departmemnt]
        {
            get
            {
                Int32 departmentId = 0;
                Int32.TryParse(departmemnt, out departmentId);
                return this[journalObject, departmentId];
            }
            set { }
        }
        public FilterInfo this[String journalObject, Int32 departmemntId]
        {
            get
            {
                foreach (FilterInfo filterInfo in this)
                {
                    String fltrName = filterInfo.ObjectName;
                    if (fltrName.StartsWith("Tlis"))
                    {
                        fltrName = fltrName.Substring(4);
                    }

                    if (fltrName.ToLower().Equals(journalObject.ToLower()))
                    {
                        if (departmemntId < 1)
                        {
                            return filterInfo;
                        }
                        else if ((filterInfo.Department != 0) && departmemntId.Equals(filterInfo.Department))
                        {
                            return filterInfo;
                        }
                    }
                }
                return null;
            }
            set { }
        }

    }

    public class JournalLayout : DictionaryItem
    {
        public JournalLayout()
        {
            Columns = new List<TreeViewColumnLayout>();
        }
        [CSN("Rank")]
        public Int32 Rank { get; set; }
        //Тип журнала
        [CSN("TreeType")]
        public Int32 TreeType { get; set; }
        [CSN("DetailTreeCode")]
        public String DetailTreeCode { get; set; }
        [CSN("Journal")]
        public ObjectRef Journal { get; set; }
        //Множество колонок журнала
        [CSN("Columns")]
        public List<TreeViewColumnLayout> Columns { get; set; }
        //ID файла с настройками фильтра на файл-сервере
        [CSN("Filter")]
        public Int32 Filter { get; set; }
        //Имя класса файла
        [CSN("FilterClassName")]
        public String FilterClassName { get; set; }
        //Имя класса, который отображается в журнале
        [CSN("JournalClassName")]
        public String JournalClassName { get; set; }
        [CSN("ExtendedFrame")]
        public ObjectRef ExtendedFrame { get; set; }
        [CSN("Hidden")]
        public Boolean Hidden { get; set; }
    }

    public class JournalLayoutList : List<JournalLayout>
    {

        public new JournalLayout this[Int32 Id]
        {
            get {
                foreach (JournalLayout journal in this)
                {
                    if (journal.Id == Id)
                        return journal;
                }
                return null;
            }
            set { }
        }

        public JournalLayout this[String treeType, String journal]
        {
            get
            {
                Int32 departmentId = 0;
                Int32.TryParse(journal, out departmentId);
                Int32 treeTypeId = 0;
                Int32.TryParse(treeType, out treeTypeId);
                return this[treeTypeId, departmentId];
            }
            set { }
        }

        public JournalLayout this[String className, Int32 journal]
        {
            get
            {
                foreach (JournalLayout layout in this)
                {
                    String gridName = layout.JournalClassName;
                    if (gridName == null) continue;
                    if (gridName.StartsWith("Tlis"))
                    {
                        gridName = gridName.Substring(4);
                    }

                    if (gridName.ToLower().Equals(className.ToLower()))
                    {
                        if (journal < 1)
                        {
                            return layout;
                        }
                        else if ((layout.Journal != null) && journal.Equals(layout.Journal.Id))
                        {
                            return layout;
                        }
                    }
                }
                return null;
            }
            set { }
        }
        public JournalLayout this[Int32 treeType, Int32 journal]
        {
            get
            {
                foreach (JournalLayout layout in this)
                {
                    Int32 CurrentTreeType = layout.TreeType;
                    if (CurrentTreeType.Equals(treeType))
                    {
                        if (journal < 1)
                            return layout;
                        else if ((layout.Journal != null) && journal.Equals(layout.Journal.Id))
                            return layout;
                    }
                }
                return null;
            }
            set { }
        }

        public JournalLayout this[String code]
        {
            get
            {
                foreach (JournalLayout layout in this)
                {
                    if (layout.Code.Equals(code))
                        return layout;
                }
                return null;
            }
        }
    }

    public class TreeViewLayoutList : List<TreeViewLayout>
    {
        public enum TreeTypes : int
        {
            RequestInRegistrationJournal = 1,
            SampleInWorkJournal = 4,
            WorkInWorkJournal = 5,
            WorkInSampleForm = 6,
            PatientInPatientJournal = 12,
            RequestInPatientJornal = 52
        }

        public TreeViewLayout this[String treeType, String department]
        {
            get
            {
                Int32 departmentId = 0;
                Int32.TryParse(department, out departmentId);
                Int32 treeTypeId = 0;
                Int32.TryParse(treeType, out treeTypeId);
                return this[treeTypeId, departmentId];
            }
            set { }
        }

        public TreeViewLayout this[String className, Int32 department]
        {
            get
            {
                foreach (TreeViewLayout layout in this)
                {
                    String gridName = layout.ClassName;
                    if (gridName == null) continue;
                    if (gridName.StartsWith("Tlis"))
                    {
                        gridName = gridName.Substring(4);
                    }

                    if (gridName.ToLower().Equals(className.ToLower()))
                    {
                        if (department < 1)
                        {
                            return layout;
                        }
                        else if ((layout.Department != null) && department.Equals(layout.Department.Id))
                        {
                            return layout;
                        }
                    }
                }
                return null;
            }
            set { }
        }

        public TreeViewLayout this[Int32 treeType, Int32 department]
        {
            get
            {
                foreach (TreeViewLayout layout in this)
                {
                    Int32 CurrentTreeType = layout.TreeType;
                    if (CurrentTreeType.Equals(treeType))
                    {
                        if (department < 1)
                            return layout;
                        else if ((layout.Department != null) && department.Equals(layout.Department.Id))
                            return layout;
                    }
                }
                return null;
            }
            set { }
        }


    }

    public class TreeViewColumnLayout
    {
        [CSN("Id")]
        public Int32 Id { get; set; }
        [CSN("FieldId")]
        public Int32 FieldId { get; set; }
        [CSN("Caption")]
        public String Caption { get; set; }
        [CSN("Width")]
        public Int32 Width { get; set; }
        [CSN("Format")]
        public String Format { get; set; }
        [CSN("AutoSize")]
        public Boolean AutoSize { get; set; }
        [CSN("ToggleSort")]
        public Boolean ToggleSort { get; set; }
        [CSN("AutoSort")]
        public Boolean AutoSort { get; set; }
        [CSN("FixedWidth")]
        public Boolean FixedWidth { get; set; }
        [CSN("Fixed")]
        public Boolean Fixed { get; set; }
        [CSN("UserField")]
        public ObjectRef UserField { get; set; }
        [CSN("FieldType")]
        public Int32 FieldType { get; set; }
        [CSN("OptionsName")]
        public String OptionsName { get; set; }
        [CSN("PropName")]
        public String PropName { get; set; }
        [CSN("DictionaryName")]
        public String DictionaryName { get; set; }
        [CSN("DictionaryPropName")]
        public String DictionaryPropName { get; set; }
        [CSN("Rank")]
        public Int32 Rank { get; set; }
        [CSN("BloodParameter")]
        public ObjectRef BloodParameter { get; set; }
        [CSN("ProductAttribute")]
        public ObjectRef ProductAttribute { get; set; }
    }

    public class TreeViewLayout : DictionaryItem//BaseObject
    {
        public TreeViewLayout()
        {
            Columns = new List<TreeViewColumnLayout>();
        }

        [CSN("TreeType")]
        public Int32 TreeType { get; set; }
        [CSN("Department")]
        public ObjectRef Department { get; set; }
        [CSN("Columns")]
        public List<TreeViewColumnLayout> Columns { get; set; }

        [CSN("TreeTypeName")]
        public String TreeTypeName { get; set; }
        [CSN("DepartmentName")]
        public String DepartmentName { get; set; }
        [CSN("ClassName")]
        public String ClassName { get; set; }

        public List<String> GetColumnNames()
        {
            List<String> names = new List<string>();
            foreach (TreeViewColumnLayout column in Columns)
            {
                names.Add(column.PropName);
            }
            return names;
        }

        public Boolean isValidColumn(String name)
        {
            return Columns.Find(a => a.PropName.Equals(name)) != null;
        }

        public TreeViewColumnLayout FindColumn(String PropName)
        {
            if (String.IsNullOrWhiteSpace(PropName)) return null;
            foreach (TreeViewColumnLayout column in Columns)
            {
                if (PropName.Equals(column.PropName)) return column;
            }
            return null;
        }
    }

    public class UserProfileDictionaryItem : DictionaryItem
    {

        public UserProfileDictionaryItem()
        {
            TreeViewLayouts = new TreeViewLayoutList();
            FilterInfos = new FilterInfoList();
            Journals = new JournalLayoutList();
        }

        [CSN("TreeViewLayouts")]
        public TreeViewLayoutList TreeViewLayouts { get; set; }
        [CSN("FilterInfos")]
        public FilterInfoList FilterInfos { get; set; }

        [CSN("Journals")]
        public JournalLayoutList Journals { get; set; }
        
        [CSN("SettingsFile")]
        public int SettingsFile { get; set; }

    }

    public class FilterInfo
    {
        [CSN("Filter")]
        public Int32 Filter { get; set; }
        [CSN("JournalFilter")]
        public JournalFilterSettings JournalFilter { get; set; }
        [CSN("Name")]
        public String Name { get; set; }
        [CSN("ObjectName")]
        public String ObjectName { get; set; }
        [CSN("Department")]
        public Int32 Department { get; set; }
        [CSN("DepartmentName")]
        public String DepartmentName { get; set; }
        [CSN("DefaultValue")]
        public String DefaultValue { get; set; }
    }

    public class UserProfileDictionary : DictionaryClass<UserProfileDictionaryItem>
    {
        public UserProfileDictionary(String dictionaryName) : base(dictionaryName) { }

        [CSN("UserProfile")]
        public List<UserProfileDictionaryItem> UserProfile
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}