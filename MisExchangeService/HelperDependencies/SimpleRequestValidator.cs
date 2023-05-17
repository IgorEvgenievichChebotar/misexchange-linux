using Ninject;
using ru.novolabs.ExchangeDTOs;
using ru.novolabs.MisExchange.Classes;
using ru.novolabs.MisExchange.HelperDependencies.SimpleRequestValidatorDependencies;
using ru.novolabs.MisExchange.Interfaces;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.LimsBusinessObjects;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.HelperDependencies
{
    class SimpleRequestValidator : IRequestValidator
    {
        [Inject]
        public SimpleRequestValidatorDictionaryValidator DictionaryValidator { get; set; }
        public SimpleRequestValidator(IRequestValidatorFactory internalValidatorFactory)
        {
            InternalValidatorFactory = internalValidatorFactory;
        }
        IRequestValidatorFactory InternalValidatorFactory { get; set; }
        public virtual void CheckData(Request requestDTO)
        {
            string errorStr = String.Empty;
            try
            {
                IRequestValidatorInner validator = InternalValidatorFactory.CreateRequestValidator();
                validator.CheckData(requestDTO);
            }
            catch (CustomDataCheckException ex)
            {
                errorStr = ex.Message;
            }
            DictionaryValidator.ValidateWithDictionaries(requestDTO, ref errorStr);
            if (!String.IsNullOrEmpty(errorStr))
            {
                throw new CustomDataCheckException(new List<string> { errorStr });
            }
        }
    }
}
