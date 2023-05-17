using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.Reporting
{
    public static class ReportingHelper
    {
        public static List<TaskDescriptionInfo> GetTaskDescriptionInfo(this TaskDescriptionList taskList)
        {
            var result = new List<TaskDescriptionInfo>();

            foreach (TaskDescription item in taskList.Tasks)
                result.Add(new TaskDescriptionInfo() { TaskDescription = item });

            return result;
        }

        public static string GenerateTaskDescriptionName(TaskDescriptionList taskList)
        {
            for (int i = 1; i < 1000; i++)
                if (taskList.Tasks.Find(t => t.Name.Equals("Новая задача " + i.ToString())) == null)
                    return "Новая задача " + i.ToString();
            return String.Empty;
        }
    }
}
