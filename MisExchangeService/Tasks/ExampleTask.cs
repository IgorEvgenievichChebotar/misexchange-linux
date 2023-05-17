
/*
namespace ru.novolabs.NEWIntegrationServiсe.Tasks
{
    public class ExampleTask: Task
    {
        public override void Execute()
        {
            //======================================  Пример обращения к справочникам  ========================================================================================

            Debug.WriteLine("");

            TargetDictionary dict = (TargetDictionary)ProgramContext.Dictionaries[LisDictionaryNames.Target];
            dict.Target.RemoveAll(t => t.Removed);
            int j = (dict.Target.Count > 10) ? 10 : dict.Target.Count;
            for (int i = 0; i < j; i++)
            { 
                TargetDictionaryItem trg = (TargetDictionaryItem)dict.Target[i];
                Debug.WriteLine("Target: Name = [{0}], Code = [{1}], Mnemonics = {2}", new object[] { trg.Name, trg.Code, trg.Mnemonics });                          
            }
            // Быстрый способ получить интересующий элемент справочника по id/коду
            TargetDictionaryItem trg1 = (TargetDictionaryItem)ProgramContext.Dictionaries[LimsDictionaryNames.Target, elementCode: "27-120"];

            //======================================  Пример обращения к настройкам  ===========================================================================================

            Debug.WriteLine("");
            
            // Если настройка обязательно должна присутствовать в файле настроек
            String serverAddress = (String)ProgramContext.Settings["serverAddress"];

            // Если настройка опциональна
            Object appServerListenerURI = ProgramContext.Settings["appServerListenerURI", false];
            if (appServerListenerURI != null)
                Debug.WriteLine(String.Format("appServerListenerURI = [{0}]", appServerListenerURI));
            else
                Debug.WriteLine(String.Format("appServerListenerURI = [{0}]", "null"));

            //======================================  Пример вызова методов, обращающихся к серверному приложению  =============================================================

            Debug.WriteLine("");
            
            List<BaseSample> samples = ProgramContext.LisCommunicator.RequestSamples(11739955);

            foreach (var sample in samples)
            {
                Debug.WriteLine(String.Format("Sample id = [{0}], internal_nr = [{1}]", sample.Id, sample.InternalNr));            
            }
            //===================================================================================================================================================================
        }
    }
}
*/