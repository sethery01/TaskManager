using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TaskManager
{
    class TaskList
    {
        // Class fields
        private List<Task> tasks;

        // Class constructor
        public TaskList()
        {
            tasks = new List<Task>();
        }

        // Adds a task to the list
        public bool AddTask(Task task)
        {
            foreach (var t in tasks)
            {
                if (t.Title == task.Title)
                {
                    return false;
                }
            }

            tasks.Add(task);
            return true;
        }

        // Removes a task from the List
        public void completeTask(string title)
        {
            foreach (var task in tasks)
            {
                if (task.Title == title)
                {
                    tasks.Remove(task);
                }
            }
        }

        // Sort by priority
        public List<Task> prioritySort()
        {
            List<Task> sortedTasks = new List<Task>();

            // Copy current task list
            foreach (var task in tasks)
            {
                sortedTasks.Add(task);
            }

            // found at https://stackoverflow.com/questions/3309188/how-to-sort-a-listt-by-a-property-in-the-object 
            return sortedTasks.OrderBy(t => t.Priority).ThenBy(t => t.Date).ToList();
        }
    }
}
