using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum TaskType
{
    Assignment,
    Exam,
    Quiz,
    Project,
    Custom
}

public enum Priority
{
    Moderate,
    High,
    Low
}

namespace TaskManager
{
    internal class Task // Model
    {
        // Class fields 
        private string title;
        private TaskType type;
        private string course;
        private Priority priority;
        private string description;
        private DateOnly date;

        // Class properties 
        public string Title { get { return title; } set { title = value; } }
        public string Course { get { return course; } set { course = value; } }
        public Priority Priority { get { return priority; } set { priority = value; } } 
        public string Description { get { return description; } set { description = value; } }
        public DateOnly Date { get { return date; } set { date = value; } }

        // Class constructor
        public Task(string title, TaskType type, string course, Priority priority, string description, DateOnly date)
        {
            this.title = title;
            this.type = type;
            this.course = course;
            this.priority = priority;
            this.description = description;
            this.date = date;
        }
    }
}
