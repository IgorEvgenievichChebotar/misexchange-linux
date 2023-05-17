namespace ru.novolabs.SuperCore.Service.Tasks
{
    public class RequestErrorCompensationTask: Task
    {
        public override void Execute()
        {
            // Запускаем компенсацию ошибок процессоров.
            //RequestErrorCompensation.Process();
        }
    }
}