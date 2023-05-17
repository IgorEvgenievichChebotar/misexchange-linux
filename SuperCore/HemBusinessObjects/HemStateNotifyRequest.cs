using ru.novolabs.SuperCore.HemDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class HemStateNotifyRequest
    {
        public HemStateNotifyRequest()
        {
            States = new List<StateGroupItem>();
            Donors = new List<Donor>();
            Donations = new List<Donation>();
            Visits = new List<Visit>();
            Products = new List<Product>();
            Processes = new List<Process>();
            TransfusionRequests = new List<TransfusionRequest>();
            Transfusions = new List<Transfusion>();
        }

        /// <summary>
        /// Набор справочных элементов, отражает новое состояние каждого из объектов
        /// </summary>
        [CSN("States")]
        public List<StateGroupItem> States { get; set; }

        [CSN("Donors")]
        public List<Donor> Donors { get; set; }

        [CSN("Donations")]
        public List<Donation> Donations { get; set; }

        [CSN("Visits")]
        public List<Visit> Visits { get; set; }

        [CSN("Products")]
        public List<Product> Products { get; set; }

        [CSN("Processes")]
        public List<Process> Processes { get; set; }

        [CSN("TransfusionRequests")]
        public List<TransfusionRequest> TransfusionRequests { get; set; }

        [CSN("Transfusions")]
        public List<Transfusion> Transfusions { get; set; }

        ////[CSN("Production")]
        ////public List<Production

    }
}
