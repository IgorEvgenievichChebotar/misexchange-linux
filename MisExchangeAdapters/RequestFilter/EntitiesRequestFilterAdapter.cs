using ru.novolabs.SuperCore.LimsBusinessObjects.Exchange;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ru.novolabs.MisExchangeService.Adapters
{
    public class EntitiesRequestFilterAdapter : RequestFilterAdapter
    {
        public override ExternalRequestFilter ReadDTO(object obj)
        {
            var dto = (MisExchangeEntities.RequestFilter)obj;
            var requestFilter = new ExternalRequestFilter();

            requestFilter.FirstName = dto.FirstName;
            requestFilter.LastName = dto.LastName;
            requestFilter.MiddleName = dto.MiddleName;
            requestFilter.Priority = dto.Priority;
            requestFilter.DefectState = dto.DefectState;
            requestFilter.SampleDeliveryDateFrom = dto.DateFrom;
            requestFilter.SampleDeliveryDateTill = dto.DateTill;
            requestFilter.Sex = dto.Sex;
            requestFilter.EndDateFrom = dto.EndDateFrom;
            requestFilter.EndDateTill = dto.EndDateTill;

            if (dto.BirthDate != null)
                requestFilter.BirthDate = dto.BirthDate.Value;

            if (!String.IsNullOrEmpty(dto.PatientCodes))
                requestFilter.PatientCodes = new List<string>(dto.PatientCodes.Split(new char[] { ',' }));

            if (!String.IsNullOrEmpty(dto.States))
            {
                try
                {
                    var query =
                        from s in dto.States.Split(new char[] { ',' })
                        select Int32.Parse(s);

                    requestFilter.States = query.ToList();
                }
                catch { }
            }

            if (!String.IsNullOrEmpty(dto.RequestCodes))
                requestFilter.RequestCodes = new List<string>(dto.RequestCodes.Split(new char[] { ',' }));

            if (!String.IsNullOrEmpty(dto.CustomStates))
                requestFilter.CustomStates = new List<string>(dto.CustomStates.Split(new char[] { ',' }));

            if (!String.IsNullOrEmpty(dto.Targets))
                requestFilter.Targets = new List<string>(dto.Targets.Split(new char[] { ',' }));

            if (!String.IsNullOrEmpty(dto.Departments))
                requestFilter.Departments = new List<string>(dto.Departments.Split(new char[] { ',' }));

            if (!String.IsNullOrEmpty(dto.Hospitals))
                requestFilter.Hospitals = new List<string>(dto.Hospitals.Split(new char[] { ',' }));

            if (!String.IsNullOrEmpty(dto.CustDepartments))
                requestFilter.CustDepartments = new List<string>(dto.CustDepartments.Split(new char[] { ',' }));

            if (!String.IsNullOrEmpty(dto.Doctors))
                requestFilter.Doctors = new List<string>(dto.Doctors.Split(new char[] { ',' }));

            return requestFilter;
        }
    }
}