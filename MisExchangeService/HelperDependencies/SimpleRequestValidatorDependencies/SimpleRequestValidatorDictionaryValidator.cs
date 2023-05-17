using ru.novolabs.ExchangeDTOs;
using ru.novolabs.MisExchange.HelperDependencies.SimpleRequestValidatorDependencies;
using ru.novolabs.MisExchange.MainDependenceInterfaces;
using ru.novolabs.MisExchange.MainDependenceInterfaces.CommunicatorInterfaces;
using ru.novolabs.MisExchange.MainDependenceInterfaces.SettingInterfaces;
using ru.novolabs.MisExchangeService;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.HelperDependencies
{
    class SimpleRequestValidatorDictionaryValidator
    {
        public SimpleRequestValidatorDictionaryValidator(SimpleUserValueSearcher userValueSearcher, IDictionaryCache dictionaryCache, IProcessRequestCommunicator communicator, IProcessRequestSettings settings)
        {
            this.UserValueSearcher = userValueSearcher;
            DictionaryCache = dictionaryCache;
            LimsCommunicator = communicator;
            Settings = settings;
        }

        IDictionaryCache DictionaryCache { get; set; }
        IProcessRequestCommunicator LimsCommunicator { get; set; }
        IProcessRequestSettings Settings { get; set; }
        SimpleUserValueSearcher UserValueSearcher { get; set; }

        public void ValidateWithDictionaries(Request requestDTO, ref string errorStr)
        {
            List<string> errors = new List<string>();

            ValidateRequestForm(requestDTO, errors);

            ValidatePatientCode(requestDTO, errors);

            ValidateUserFieldsDict(requestDTO, errors);

            ValidateSampleDict(requestDTO, errors);
            if (errors.Count > 0)
            {
                errorStr += "\r\n" + String.Join("\r\n", errors);
            }
        }

        protected void ValidateSampleDict(Request requestDTO, List<string> errors)
        {
            foreach (Sample sample in requestDTO.Samples)
            {
                ValidateBiomerialCode(requestDTO, errors, sample);
                foreach (Target target in sample.Targets)
                {
                    ValidateTargetDict(requestDTO, errors, sample, target);
                }

            }
        }

        protected void ValidateTargetDict(Request requestDTO, List<string> errors, Sample sample, Target target)
        {
            var targetDictionaryItem = DictionaryCache.GetDictionaryItem<TargetDictionaryItem>(target.Code);
            if (null == targetDictionaryItem)
            {
                errors.Add(String.Format("Исследование с кодом [{0}] пробы [{1}] заявки [{2}] отсутствует в справочнике", target.Code,
                    sample.Barcode, requestDTO.RequestCode));
            }
            if ((targetDictionaryItem != null) && !targetDictionaryItem.IsProfile() && (targetDictionaryItem.Biomaterials.FirstOrDefault(b => b.Code == sample.BiomaterialCode) == null))
            {
                IEnumerable<string> allowedBiomaterials = targetDictionaryItem.Biomaterials.Select(b => String.Format("Code:[{0}]; Id:[{1}]", b.Code, b.Id.ToString()));
                errors.Add(String.Format("Исследование с кодом [{0}] пробы [{1}] заявки [{2}] передается с недопустимым биоматериалом [{3}]. Список допустимых биоматериалов:\r\n\t{4}",
                    target.Code, sample.Barcode, requestDTO.RequestCode, sample.BiomaterialCode, String.Join("\r\n\t", allowedBiomaterials)));
            }
            else if ((targetDictionaryItem != null) && targetDictionaryItem.IsProfile() && !IsValidProfileTarget(sample, targetDictionaryItem))
            {
                errors.Add(String.Format("Профиль исследований с кодом [{0}] пробы [{1}] заявки [{2}] передается с недопустимым биоматериалом [{3}]", target.Code,
                    sample.Barcode, requestDTO.RequestCode, sample.BiomaterialCode));
            }
            foreach (Test test in target.Tests)
            {
                ValidateTestDict(requestDTO, errors, sample, target, test);
            }
        }
        protected bool IsValidProfileTarget(Sample sample, TargetDictionaryItem targetDictionaryItem)
        {
            var biomaterialDictItem = DictionaryCache.GetDictionaryItem<BiomaterialDictionaryItem>(sample.BiomaterialCode);
            if (biomaterialDictItem == null)
                return true;
            if (targetDictionaryItem.Samples.FirstOrDefault(s => s.Biomaterial.Id == biomaterialDictItem.Id) != null)
                return true;
            return false;
        }

        protected void ValidateTestDict(Request requestDTO, List<string> errors, Sample sample, Target target, Test test)
        {
            if (null == DictionaryCache.GetDictionaryItem<TestDictionaryItem>(test.Code))
            {
                errors.Add(String.Format("Тест с кодом [{0}] исследования[{1}] пробы [{2}] заявки [{3}] отсутствует в справочнике", test.Code,
                    target.Code, sample.Barcode, requestDTO.RequestCode));
            }
        }

        protected void ValidateBiomerialCode(Request requestDTO, List<string> errors, Sample sample)
        {
            if (null == DictionaryCache.GetDictionaryItem<BiomaterialDictionaryItem>(sample.BiomaterialCode))
            {
                errors.Add(String.Format("Биоматериал с кодом [{0}] пробы [{1}] заявки [{2}] отсутствует в справочнике", sample.BiomaterialCode,
                    sample.Barcode, requestDTO.RequestCode));
            }
        }

        protected void ValidateUserFieldsDict(Request requestDTO, List<string> errors)
        {
            foreach (UserField userField in requestDTO.UserFields)
            {
                ValidateUserField(userField,
                    () => errors.Add(String.Format("Пользовательское поле с кодом [{0}] заявки [{1}] не найдено в справочнике", userField.Code, requestDTO.RequestCode)),
                    errors);
            }

            foreach (UserField userField in requestDTO.Patient.UserFields)
            {
                ValidateUserField(userField,
                    () => errors.Add(String.Format("Пользовательское поле с кодом [{0}] заявки [{1}] для пациента [{2}] не найдено в справочнике",
                        userField.Code, requestDTO.RequestCode, requestDTO.Patient.Code)),
                    errors);
            }
        }
        private void ValidateUserField(UserField userField, Action errorHandle, List<string> errors)
        {
            UserFieldDictionaryItem userFieldDict = DictionaryCache.GetDictionaryItem<UserFieldDictionaryItem>(userField.Code);
            if (null == userFieldDict)
            {
                errorHandle();
            }
            if (null != userFieldDict)
            {
                ValidateUserValues(userField, userFieldDict, errors);
            }
        }

        protected void ValidatePatientCode(Request requestDTO, List<string> errors)
        {
            if (!Settings.ResetPatientCode)
            {
                List<SuperCore.LimsBusinessObjects.Patient> foundPatients = LimsCommunicator.PatientsByCode(requestDTO.Patient.Code);
                if (foundPatients.Count > 1)
                    // Это сообщение об ошибке увидят партнёры из МИС. Причина - несколько пациентов с указанным кодом в нашей БД. Ошибка критическая и лечить её нужно SQL-скриптами
                    throw new Exception("Некорректный код пациента. Обратитесь к разработчикам");
            }

            if (null == DictionaryCache.GetDictionaryItem<HospitalDictionaryItem>(requestDTO.HospitalCode))
            {
                errors.Add(String.Format("Заказчик с кодом [{0}] заявки [{1}] отсутствует в справочнике ", requestDTO.HospitalCode, requestDTO.RequestCode));
            }
        }
        protected void ValidateRequestForm(Request requestDTO, List<string> errors)
        {
            if (null == DictionaryCache.GetDictionaryItem<RequestFormDictionaryItem>(Settings.RegistrationFormCode))
            {
                errors.Add("Неверно указанна регистрационная форма");
            }
        }
        protected void ValidateUserValues(UserField userField, UserFieldDictionaryItem userFieldDict, List<string> errors)
        {
            if (userFieldDict.FieldType != UserFieldTypes.ENUMERATION.GetHashCode() && userFieldDict.FieldType != UserFieldTypes.SET.GetHashCode())
                return;

            if (userFieldDict.UserDirectory == null)
            {
                throw new Exception(String.Format(String.Format("UserDirectory for userFieldDictionaryItem is null for UserField [Code:{0}]", userField.Code)));
            }
            UserDirectoryDictionaryItem userDictionary = DictionaryCache.GetDictionaryItem<UserDirectoryDictionaryItem>(userFieldDict.UserDirectory.Id);
            if (userDictionary == null)
            {
                throw new Exception(String.Format("Cannot find item for dictionary [UserDirectory] by id [{0}] for UserField [Code:{1}]", userFieldDict.UserDirectory.Id,
                    userField.Code));
            }
            switch (userFieldDict.FieldType)
            {
                case (int)UserFieldTypes.ENUMERATION:
                    ValidateUserValue(userField, userField.Value, (uf, str) => UserValueSearcher.FindEnumerationUserValue(uf, userDictionary), errors);
                    break;
                case (int)UserFieldTypes.SET:
                    String[] valueSet = userField.Value.Trim().Split(new Char[] { ',' });
                    foreach (String value in valueSet)
                    {
                        ValidateUserValue(userField, value, (uf, str) => UserValueSearcher.FindSetUserValue(uf, str, userDictionary), errors);
                    }
                    break;
            }
        }
        protected void ValidateUserValue(UserField userField, string value, Func<UserField, string, UserDictionaryValue> findFunc, List<string> errors)
        {
            UserDictionaryValue dictionaryItem = findFunc(userField, value);
            bool isAllowedToAddUserValues = Settings.IsAllowedToAddUserValues;
            if (dictionaryItem == null && !isAllowedToAddUserValues)
            {
                errors.Add(String.Format("Value {0} is not allowed for Enumeration|Set Type userField [UserFieldCode:{1}]", userField.Value, userField.Code));
            }
        }
    }
}
