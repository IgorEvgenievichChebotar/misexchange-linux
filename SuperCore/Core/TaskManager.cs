using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace ru.novolabs.SuperCore
{
    /// <summary>
    /// Базовый класс задачи из пространства имён ru.novolabs.SuperCore
    /// </summary>
    public abstract class Task
    {
        // Требуется реализация этого метода в наследнике
        public abstract void Execute();
    }

    /// <summary>
    /// Класс предназначен для выполнения каких либо задач по расписанию
    /// </summary>
    public class TaskManager
    {
        private class TaskStopState
        {
            private Boolean needStop = false;
            public Boolean NeedStop
            {
                get { return needStop; }
                set { needStop = value; }
            }
        }

        static Thread loop;

        static List<Task> tasks = new List<Task>();
        static TaskStopState stopState = new TaskStopState();

        public static void RegisterTasks()
        {
            // Ищем все базовые классы задач в текущей сборке
            var tasksTypes =
                from type in Assembly.GetExecutingAssembly().GetTypes()
                where type.IsSubclassOf(typeof(Task))
                select type;

            // Добавляются только уникальные процессоры.
            foreach (Type type in tasksTypes)
                if (!tasks.Exists(t => t.GetType() == type))
                {
                    tasks.Add((Task)Activator.CreateInstance(type));
                    Log.WriteText(String.Format("Зарегистрирована задача {0}", type.FullName));
                }

            //------------------------------------------------------------------------------------------------------

            // Ищем все классы задач в исполняемой сборке(c расширением *.exe, которая подключила текущую сборку)
            tasksTypes =
                from type in Assembly.GetEntryAssembly().GetTypes()
                where type.IsSubclassOf(typeof(Task))
                select type;

            // Добавляются только уникальные процессоры.
            foreach (Type type in tasksTypes)
                if (!tasks.Exists(t => t.GetType() == type))
                {
                    tasks.Add((Task)Activator.CreateInstance(type));
                    Log.WriteText(String.Format("Зарегистрирована задача {0}", type.FullName));
                }
        }

        static TimeSpan waitInterval = TimeSpan.FromMinutes(1);
        /// <summary>
        /// Интервал ожидания между запусками списка задач
        /// </summary>
        public static TimeSpan WaitInterval
        {
            get { return waitInterval; }
            set { waitInterval = value; }
        }

        /// <summary>
        /// Стартует бесконечный цикл запусков задач в неблокирующем(фоновом) или блокирующем вызвавший поток режиме
        /// </summary>
        /// <param name="taskManagerWaitInterval"></param>
        /// <param name="backgroundMode"></param>
        public static void Run(Int32 taskManagerWaitInterval, bool backgroundMode = false)
        {
            Log.WriteText("----- task manager thread started -----");
            WaitInterval = TimeSpan.FromSeconds(taskManagerWaitInterval);

            loop = new Thread(() =>
                {
                    while (!stopState.NeedStop)
                    {
                        TimeSpan delayTime = WaitInterval;
                        try
                        {
                            lock (stopState)
                            {
                                Stopwatch tasksCompletionTime = new Stopwatch();
                                tasksCompletionTime.Start();

                                var workTask = new System.Threading.Tasks.Task(RunTasksProc);
                                workTask.Start();
                                workTask.Wait();

                                tasksCompletionTime.Stop();
                                if (stopState.NeedStop)
                                    return;

                                if (tasksCompletionTime.Elapsed < WaitInterval)
                                    delayTime = WaitInterval - tasksCompletionTime.Elapsed;
                                else
                                    delayTime = new TimeSpan();
                                //Log.WriteText("Next tasks execution delay: " + delayTime.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.WriteError(ex.ToString());
                        }

                        GC.Collect();
                        Thread.Sleep(delayTime);
                    }
                });
            // Если backgroundMode = true, то поток "loop" - фоновый и приложение может завершиться, не дожидаясь завершения потока "loop".
            // Иначе приложение будет выполняться до тех пор, пока извне не будет вызван метод TaskManager.Stop()
            loop.IsBackground = backgroundMode;
            loop.Start();
        }

        private static void RunTasksProc()
        {
            try
            {
                foreach (Task task in tasks)
                {
                    try
                    {
                        task.Execute();
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError(String.Format("Необработанная ошибка в задаче [{0}]: {1}", task.GetType().Name, ex.Message));
                        Log.WriteError(ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex.ToString());
            }
        }

        /// <summary>
        /// Останавливает бесконечный цикл запусков задач
        /// </summary>
        public static void Stop()
        {
            lock (stopState)
                stopState.NeedStop = true;

            if (loop != null)
            {
                Log.WriteText("----- task manager thread stoped -----");
            }
        }
    }
}