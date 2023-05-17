using ru.novolabs.ExchangeDTOs;
using ru.novolabs.MisExchange.MainDependenceInterfaces;
using ru.novolabs.MisExchange.MainDependenceInterfaces.CommunicatorInterfaces;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.DictionaryCommon;
using ru.novolabs.SuperCore.LimsBusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.HelperDependencies.SimpleCreateRequestDependencies
{
    class PatientSaver
    {
        public PatientSaver(SimpleCreateRequest3UserValuesFactory userValuesFactory, IProcessRequestCommunicator processRequestCommunicator, IDictionaryCache dictionaryCache)
        {
            UserValuesFactory = userValuesFactory;
            ProcessRequestCommunicator = processRequestCommunicator;
            DictionaryCache = dictionaryCache;
        }
        SimpleCreateRequest3UserValuesFactory UserValuesFactory { get; set; }
        IProcessRequestCommunicator ProcessRequestCommunicator { get; set; }
        IDictionaryCache DictionaryCache { get; set; }

        public virtual void PatientSave(CreateRequest3Request request, ExchangeDTOs.Patient patientDTO)
        {
            SuperCore.LimsBusinessObjects.Patient patient = ToLimsPatient(patientDTO);
            try
            {
                List<SuperCore.LimsBusinessObjects.Patient> foundPatients = ProcessRequestCommunicator.PatientsByCode(patient.Code);
                if (foundPatients.Count == 1)
                {
                    // В этом случае произойдёт редактирование существующего пациента
                    patient.Id = foundPatients.First().Id;
                    if (patient.PatientCards == null || patient.PatientCards.Count <= 0)
                    {
                        return;
                    }
                    ru.novolabs.SuperCore.LimsBusinessObjects.PatientCard card = foundPatients.First().PatientCards.Find(x => x.CardNr == patient.PatientCards[0].CardNr);
                    if (card != null)
                    {
                        patient.PatientCards[0].Id = card.Id;
                        request.PatientCard.Id = card.Id;
                    }
                    List<PatientCardId> patientCardIds;
                    ProcessRequestCommunicator.PatientSave(patient, out patientCardIds);
                    if (card == null && patientCardIds.Count > 0)
                    {
                        request.PatientCard.Id = patientCardIds[0].Id;
                        patient.PatientCards[0].Id = patientCardIds[0].Id;
                    }
                }
                else
                {
                    List<PatientCardId> patientCardIds;
                    patient.Id = ProcessRequestCommunicator.PatientSave(patient, out patientCardIds);
                    if (patient.PatientCards == null || patient.PatientCards.Count <= 0)
                    {
                        return;
                    }
                    if (patientCardIds != null && patientCardIds.Count > 0)
                        request.PatientCard.Id = patientCardIds[0].Id;
                }
            }
            finally
            {
                request.Patient = new ObjectRef(patient.Id);
            }
        }
        protected virtual SuperCore.LimsBusinessObjects.Patient ToLimsPatient(ExchangeDTOs.Patient patientDTO)
        {
            SuperCore.LimsBusinessObjects.Patient limsPatient = new SuperCore.LimsBusinessObjects.Patient();

            limsPatient.Code = patientDTO.Code;
            limsPatient.FirstName = patientDTO.FirstName;
            limsPatient.LastName = patientDTO.LastName;
            limsPatient.MiddleName = patientDTO.MiddleName;
            limsPatient.BirthDay = (patientDTO.BirthDay > 0) ? (int?)patientDTO.BirthDay : null;
            limsPatient.BirthMonth = (patientDTO.BirthMonth > 0) ? (int?)patientDTO.BirthMonth : null;
            limsPatient.BirthYear = (patientDTO.BirthYear > 0) ? (int?)patientDTO.BirthYear : null;
            limsPatient.Sex = DictionaryCache.GetDictionaryItem<SexDictionaryItem>(patientDTO.Sex);
            limsPatient.Country = patientDTO.Country;
            limsPatient.Region = patientDTO.Region;
            limsPatient.Area = patientDTO.Area;
            limsPatient.City = patientDTO.City;
            limsPatient.Location = patientDTO.Location;
            limsPatient.Street = patientDTO.Street;
            limsPatient.Building = patientDTO.Building;
            limsPatient.Flat = patientDTO.Flat;
            limsPatient.LivingAddressCountry = patientDTO.LivingAddressCountry;
            limsPatient.LivingAddressRegion = patientDTO.LivingAddressRegion;
            limsPatient.LivingAddressArea = patientDTO.LivingAddressArea;
            limsPatient.LivingAddressCity = patientDTO.LivingAddressCity;
            limsPatient.LivingAddressLocation = patientDTO.LivingAddressLocation;
            limsPatient.LivingAddressStreet = patientDTO.LivingAddressStreet;
            limsPatient.LivingAddressBuilding = patientDTO.LivingAddressBuilding;
            limsPatient.LivingAddressFlat = patientDTO.LivingAddressFlat;
            limsPatient.InsuranceCompany = patientDTO.InsuranceCompany;
            limsPatient.PolicySeries = patientDTO.PolicySeries;
            limsPatient.PolicyNumber = patientDTO.PolicyNumber;
            limsPatient.Email = patientDTO.Email;
            limsPatient.Telephone = patientDTO.Telephone;

            foreach (UserField userField in patientDTO.UserFields)
            {
                UserValue userValue = UserValuesFactory.BuildUserValue(userField);
                userValue.Name = userField.Name;
                userValue.Code = userField.Code;
                limsPatient.UserValues.Add(userValue);
            }
            if (patientDTO.PatientCard != null && !String.IsNullOrEmpty(patientDTO.PatientCard.CardNr))
                limsPatient.PatientCards.Add(new SuperCore.LimsBusinessObjects.PatientCard() { CardNr = patientDTO.PatientCard.CardNr });
            return limsPatient;
        }
    }
}
