using Ninject;
using Ninject.Extensions.Factory;
using ru.novolabs.MisExchange.Classes;
using ru.novolabs.MisExchange.HelperDependencies;
using ru.novolabs.MisExchange.HelperDependencies.SimpleCreateRequestDependencies;
using ru.novolabs.MisExchange.HelperDependencies.SimpleRequestValidatorDependencies;
using ru.novolabs.MisExchange.Interfaces;
using ru.novolabs.MisExchange.MainDependenceInterfaces;
using ru.novolabs.MisExchange.Utils;
using ru.novolabs.MisExchangeService.Classes;
using ru.novolabs.SuperCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Extensions.ContextPreservation;
using ru.novolabs.MisExchange.Classes.Communication;

namespace ru.novolabs.MisExchange
{
    public class ExchangeHelperInitializeResolver
    {
        protected StandardKernel Kernel { get; set; }
        public ExchangeHelperInitializeResolver(StandardKernel kernel)
        {
            Kernel = kernel;
        }
        public virtual void Init()
        {
            CommonInit();
        }

        #region Concrete Helper Init
        private void CommonInit()
        {
            Kernel.Bind<IRequestValidatorInner>().To<ConfigGeneralRequestValidator>();
            Kernel.Bind<IHelperSettingsProvider>().To<HelperSettingsFileProvider>().InSingletonScope();
            Kernel.Bind<IRequestValidatorFactory>().ToFactory();
        }
        
        #endregion
    }
}
