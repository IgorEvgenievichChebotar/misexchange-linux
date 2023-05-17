#define LIS

using System;
using System.IO;
using ru.novolabs.MisExchangeService.Classes;
using ru.novolabs.SuperCore;
using ru.novolabs.MisExchangeService.ExchangeHelpers.Infonom.DTOs;
using System.Text;
using ru.novolabs.MisExchange.MainDependenceInterfaces;
using Ninject;
using ru.novolabs.MisExchange.MainDependenceImplementation;
using ru.novolabs.MisExchange.Interfaces;
using ru.novolabs.MisExchange.Classes;
using ru.novolabs.MisExchange;
using ru.novolabs.MisExchange.HelperDependencies;
using ru.novolabs.MisExchange.HelperDependencies.SimpleCreateRequestDependencies;
using ru.novolabs.MisExchange.MainDependenceInterfaces.CommunicatorInterfaces;
using ru.novolabs.MisExchange.MainDependenceImplementation.Communicators;
using ru.novolabs.MisExchange.MainDependenceInterfaces.SettingInterfaces;
using MisExchange.User.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
//using ru.novolabs.MisExchange.Classes.Storage;

namespace ru.novolabs.MisExchangeService
{
    class Program
    {
        static void Main(String[] args)
        {
            //string st = File.ReadAllText(@"C:\dd2.xml", Encoding.Unicode);
            //ExchangeDTOs.Request req = st.Deserialize<ExchangeDTOs.Request>(Encoding.Unicode);
            //ReceiptOperation op = File.ReadAllText(@"C:\ReceiptOperation.xml", Encoding.UTF8).Deserialize<ReceiptOperation>(Encoding.UTF8);

            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddHostedService<MisExchangeWorker>();
                })
                .UseWindowsService()
                .UseSystemd()
                .Build();

            host.Run();

            //CompositionRoot compositionRoot = new CompositionRoot();
            //compositionRoot.Main();
        }
    }
    public class CompositionRoot
    {
        ILoadSettings Settings { get; set; }
        IPreprocessorManager PreprocessorManager { get; set; }
        ITaskManager TaskManager { get; set; }
        ILoginCommunicator Communicator { get; set; }
        ITimeFileSubjectPerformer TimeFileSubjectPreformer { get; set; }
        SingletonApplicationChecker _singletonApplicationChecker = new SingletonApplicationChecker();
        DictionarySynchronizer dictionarySynchronizer = new DictionarySynchronizer();

        protected StandardKernel Kernel { get; set; }
        protected ExchangeHelperInitializeResolver ExchangeHelperInitializer { get; set; }

        void ApplicationExitRegistrator()
        {
            // Подписываемся на закрытие процесса.
            DateTime startTime = DateTime.Now;
            AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
            {
                if (Communicator != null)
                    Communicator.Dispose();

                try
                {
                    Settings.SaveSettings(Path.Combine(FileHelper.AssemblyDirectory, SettingsConst.Settings_File_Name));
                    PreprocessorManager.Stop();
                    TaskManager.Stop();
                    TimeFileSubjectPreformer.Stop();
                    _singletonApplicationChecker.Close();
                }
                finally { }

                GAP.Logger.WriteText("[END] Сервис завершен логикой приложения в {0}, отработав {1} минут.",
                    DateTime.Now, Math.Round((DateTime.Now - startTime).TotalMinutes, 2));
            };
        }
        public void Main()
        {
            if (_singletonApplicationChecker.IsApplicationExisted())
                return;
            bool isLoggerInited = false;
            DependencyInjectionInitializer();
            try
            {
                ApplicationExitRegistrator();
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                Settings.LoadSettings(Path.Combine(FileHelper.AssemblyDirectory, SettingsConst.Settings_File_Name));

                GAP.Logger.Init();
                isLoggerInited = true;
                ProgramContext.SolutionType = ProgramContext.SolutionTypes.LIMS;
                GAP.Logger.WriteText("MisExchange version {0} starting ...", VersionHelper.GetVersionString());
                GAP.Logger.WriteText("exchangeMode is set to " + GAP.ResultSettings.ExchangeMode);

                try
                {
                    Communicator.Login();
                }
                catch (Exception ex)
                {
                    GAP.Logger.WriteError("Login fail. Reason: {0}", ex.Message);
                    GAP.Logger.WriteError("Stack trace: {0}", ex.StackTrace);
                    return;
                }

                TimeFileSubjectPreformer.Start();
                PreprocessorManager.Start();
                dictionarySynchronizer.Run();
                TaskManager.Start();
            }
            catch (SettingNotFoundException ex)
            {
                //MessageBox.Show(ex.Message, "Ошибка в настройках приложения", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                if (!isLoggerInited)
                    throw;
                GAP.Logger.WriteError(ex.ToString());
            }
        }

        protected virtual void DependencyInjectionInitializer()
        {
            InitKernel();
            InitDefaultBaseExchangeHelper();
            InitCompositionRoot();
            InitGAP();
            ExchangeHelperInitializer.Init();
        }
        /// <summary>
        /// Main method in sense of overridding
        /// </summary>
        protected virtual void InitKernel()
        {
            Kernel = new StandardKernel();
            Kernel.Bind(typeof(ILoadSettings), typeof(IProcessRequestSettings),
                typeof(IProcessResultSettings), typeof(IFileLoggerSettings), typeof(ITimeFileSubjectSettings), typeof(IPDFSignerSettings)).To<Settings>().InSingletonScope();
            Kernel.Bind<ILoginCommunicator, IProcessRequestCommunicator, IProcessResultCommunicator>().To<LisCommunicatorProd>().InSingletonScope();
            Kernel.Bind<ITaskManager>().To<TaskManagerProd>().InSingletonScope();
            Kernel.Bind<IPreprocessorManager>().To<PreprocessorManagerProd>().InSingletonScope();
            Kernel.Bind<ILogger>().To<LoggerProd>().InSingletonScope();
            Kernel.Bind<INotifierLis>().To<NotifierLis>().InSingletonScope();
            Kernel.Bind<IDictionaryCache>().To<DictionaryCacheProd>().InSingletonScope();
            Kernel.Bind<ExchangeHelperInitializeResolver>().ToSelf().WithConstructorArgument<StandardKernel>(Kernel);
            Kernel.Bind<ITimeFileSubjectPerformer>().To<TimeFileSubjectPerformer>().InSingletonScope();

            GAP.DictionarySynchronizerFactory = () => ProgramContext.Dictionaries;
        }
        private void InitCompositionRoot()
        {
            Settings = Kernel.Get<ILoadSettings>();
            TaskManager = Kernel.Get<ITaskManager>();
            PreprocessorManager = Kernel.Get<IPreprocessorManager>();
            Communicator = Kernel.Get<ILoginCommunicator>();
            ExchangeHelperInitializer = Kernel.Get<ExchangeHelperInitializeResolver>();
            TimeFileSubjectPreformer = Kernel.Get<ITimeFileSubjectPerformer>();
        }
        protected virtual void InitDefaultBaseExchangeHelper()
        {
            Kernel.Bind<IDbExchangeProvider>().To<DbExchangeProvider>().InSingletonScope();
            Kernel.Bind<IRequestValidator>().To<SimpleRequestValidator>().InSingletonScope();
            Kernel.Bind<ICreateRequestAdapter>().To<SimpleCreateRequestAdapter>().InSingletonScope();
            Kernel.Bind<IExchangeCacheHelper>().To<ExchangeCacheHelper>().InSingletonScope();
        }
        private void InitGAP()
        {
            GAP.ResultCommunicator = Kernel.Get<IProcessResultCommunicator>();
            GAP.DictionaryCache = Kernel.Get<IDictionaryCache>();
            GAP.Logger = Kernel.Get<ILogger>();
            GAP.NotifierLis = Kernel.Get<INotifierLis>();
            GAP.ResultSettings = Kernel.Get<IProcessResultSettings>();
            GAP.PDFSignerSettings = Kernel.Get<IPDFSignerSettings>();

            GAP.ExchangeHelper3Factory = (type) => (ExchangeHelper3)Kernel.Get(type);
        }
    }
}