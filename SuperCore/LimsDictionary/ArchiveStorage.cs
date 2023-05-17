using ru.novolabs.SuperCore.DictionaryCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    public class ArchiveStorageDictionaryItem : DictionaryItem
    {
        /// <summary>
        /// Ранг
        /// </summary>
        [CSN("Rank")]
        public int Rank { get; set; }
        /// <summary>
        /// Присвоенный номер штрихкода хранилища
        /// </summary>
        [CSN("Barcode")]
        public string Barcode { get; set; }
        /// <summary>
        /// Цвет, ассоциированный с хранилищем
        /// </summary>
        [CSN("Color")]
        public Int32 Color { get; set; }
        /// <summary>
        /// Признак того, что объект является конечным местом хранения и не может являться родителем для другого элемента 
        /// </summary>
        [CSN("FinalStorage")]
        public Boolean FinalStorage { get; set; }
        /// <summary>
        /// Cсылка на офис, в котором находится хранилище
        /// </summary>        
        [CSN("Office")]
        public OfficeDictionaryItem Office { get; set; }
        /// <summary>
        /// Родительское хранилище(то, в котором находится рассматриваемое хранилище)
        /// </summary>
        [CSN("Parent")]
        public ArchiveStorageDictionaryItem Parent { get; set; }
        /// <summary>
        /// Правило хранения
        /// </summary>
        [CSN("StorageRule")] 
        public StorageRuleDictionaryItem StorageRule { get; set; }

        public List<ArchiveStorageDictionaryItem> GetChildStorages(List<ArchiveStorageDictionaryItem> storages)
        {
            return storages.FindAll(s => !s.Removed && s.Parent != null && s.Parent.Equals(this));
        }

        public void GetDescendantStorages(List<ArchiveStorageDictionaryItem> storages, List<ArchiveStorageDictionaryItem> descendants)
        {
            foreach (var child in GetChildStorages(storages))
            {
                descendants.Add(child);
                descendants.AddRange(child.GetChildStorages(storages));
            }
        }

        public string GetFullName(string separator = " -> ")
        {
            var sb = new StringBuilder(this.Name);
            var parent = this.Parent;
            while (parent != null)
            {
                sb.Insert(0, parent.Name + separator);
                parent = parent.Parent;
            }
            return sb.ToString();
        }

        public string GetShortName(string separator = "-")
        {
            string lastNamePart = this.Name.Split(' ').Last();
            var sb = new StringBuilder(lastNamePart);
            var parent = this.Parent;
            while (parent != null)
            {
                lastNamePart = parent.Name.Split(' ').Last();
                sb.Insert(0, lastNamePart + separator);
                parent = parent.Parent;
            }
            return sb.ToString();
        }
    }

    public class ArchiveStorageDictionary : DictionaryClass<ArchiveStorageDictionaryItem>
    {
        public ArchiveStorageDictionary(String dictionaryName) : base(dictionaryName) { }

        [CSN("ArchiveStorage")]
        public List<ArchiveStorageDictionaryItem> ArchiveStorage
        {
            get { return Elements; }
            set { Elements = value; }
        }

        public List<ArchiveStorageDictionaryItem> GetRootStorages()
        {
            return ArchiveStorage.FindAll(s => (!s.Removed) && (s.Parent == null || s.Parent.Id == 0));        
        }
    }
}