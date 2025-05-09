using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace TaskManager
{
    [Serializable]
    class TaskList
    {
        // Class fields
        [JsonInclude]
        private List<Task> tasks;
        [JsonInclude]
        private string path;
        [JsonInclude]
        private bool saved = false;

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
            for (int i = 0; i < tasks.Count; i++)
            {
                if (tasks[i].Title == title)
                {
                    tasks.RemoveAt(i);
                    break;
                }
            }
        }

        // Returns a task based on title query
        public Task searchTasks(string title)
        {
            foreach (var task in tasks)
            {
                if (task.Title == title)
                {
                    return task;
                }
            }
            return null;
        }

        // Returns a list of strings for each task title
        public List<string> GetTaskTitles()
        {
            List<string> titles = new List<string>();
            
            foreach (var task in tasks)
            {
                titles.Add(task.Title);
            }

            return titles;
        }

        // Sort by priority with the best sorting algorithm ever
        public void PrioritySort()
        {
            Task temp;

            for (int i = 0; i < tasks.Count-1; i++)
            {
                for (int j = 0; j < tasks.Count-1-i; j++)
                {
                    bool swap = false;

                    if (tasks[j].Priority < tasks[j + 1].Priority) // Priority check
                    {
                        swap = true;
                    }
                    else if (tasks[j].Priority == tasks[j + 1].Priority && // Priority and date check
                             tasks[j].Date > tasks[j + 1].Date) 
                    {
                        swap = true;
                    }

                    if (swap)
                    {
                        temp = tasks[j];
                        tasks[j] = tasks[j+1];
                        tasks[j+1] = temp;
                    }
                }
            }
        }

        // Sort by closest due date
        public void ClosestDateSort()
        {
            Task temp;

            for (int i = 0; i < tasks.Count - 1; i++)
            {
                for (int j = 0; j < tasks.Count - 1 - i; j++)
                {
                    bool swap = false;

                    if (tasks[j].Date > tasks[j + 1].Date) // Date check
                    {
                        swap = true;
                    }
                    else if (tasks[j].Date == tasks[j + 1].Date && // Priority and date check
                             tasks[j].Priority < tasks[j + 1].Priority)
                    {
                        swap = true;
                    }

                    if (swap)
                    {
                        temp = tasks[j];
                        tasks[j] = tasks[j + 1];
                        tasks[j + 1] = temp;
                    }
                }
            }
        }

        // ToString method
        public override string ToString()
        {
            string str = "You have " + tasks.Count + " tasks.\n\n";

            foreach (Task task in tasks)
            {
                str += task.ToString();
                str += "\n\n";
            }

            return str;
        }

        // Save changes to current file
        public string SaveAs()
        {
            path = "";

            // Open Save File Dialog
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";
            saveFileDialog.Title = "Save Task List";
            saveFileDialog.DefaultExt = "json";

            if (saveFileDialog.ShowDialog() == true)
            {
                path = saveFileDialog.FileName;

                try
                {
                    // Serialize object
                    saved = true;
                    string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(path, json);

                    // Save a .txt file as well
                    string textPath = Path.ChangeExtension(path, ".txt");
                    File.WriteAllText(textPath, this.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to save file: " + ex.Message);
                }
            }

            return path;
        }

        // Save as a new file
        public void Save()
        {
            if (saved)
            {
                try
                {
                    // Serialize object
                    string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(path, json);

                    // Save a .txt file as well
                    string textPath = Path.ChangeExtension(path, ".txt");
                    File.WriteAllText(textPath, this.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to save file: " + ex.Message);
                }
            }
            else
            {
                SaveAs();
            }
        }

        // Open an existing file
        public static TaskList Open()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";
            openFileDialog.Title = "Open Task List";

            if (openFileDialog.ShowDialog() == true)
            {
                string path = openFileDialog.FileName;

                try
                {
                    string json = File.ReadAllText(path);
                    TaskList loadedTaskList = JsonSerializer.Deserialize<TaskList>(json);

                    if (loadedTaskList != null)
                    {
                        loadedTaskList.path = path;
                        loadedTaskList.saved = true;
                        return loadedTaskList;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load file: " + ex.Message);
                }
            }

            return null; // error
        }
    }
}
