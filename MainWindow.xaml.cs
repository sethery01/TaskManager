using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window // Controller
    {
        TaskList taskList;

        public MainWindow()
        {
            taskList = new TaskList();
            InitializeComponent();
        }

        // Clears the form
        private void ClearForm()
        {
            TaskTitleTextBox.Clear();
            TypeComboBox.SelectedIndex = -1;
            ClassTextBox.Clear();
            PriorityComboBox.SelectedIndex = -1;
            TaskDescriptionTextBox.Clear();
            TaskCalendar.SelectedDate = DateTime.Now;
        }

        // Fills the Form
        private void FillForm(Task task)
        {
            ClearForm();
            TaskTitleTextBox.Text = task.Title;
            TypeComboBox.SelectedIndex = (int)task.Type;
            ClassTextBox.Text = task.Course;
            PriorityComboBox.SelectedIndex = (int)task.Priority;
            TaskDescriptionTextBox.Text = task.Description;
            TaskCalendar.SelectedDate = task.Date.ToDateTime(TimeOnly.MinValue);
        }

        // Validates form input
        private bool ValidateForm()
        {
            if (TaskTitleTextBox.Text == "" || 
                TypeComboBox.SelectedIndex == -1 ||
                ClassTextBox.Text == "" ||
                PriorityComboBox.SelectedIndex == -1 ||
                TaskDescriptionTextBox.Text == "")
            {
                return false;
            }
            return true;
        }

        // Populates the Task list box
        private void PopulateTasks(List<string> tasks)
        {
            // Clear tasks first
            TasksListBox.Items.Clear();

            // Repopulate with sorted tasks
            foreach (var task in tasks)
            {
                TasksListBox.Items.Add(task);
            }
        }

        private void OnAddTaskClick(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Add Task Clicked!");

            // Check for valid input
            if (!ValidateForm())
            {
                MessageBox.Show("Please make sure all required fields are filled!");
                return;
            }

            // Get text boxes and comboboxes
            string title = TaskTitleTextBox.Text;
            TaskType type = (TaskType)TypeComboBox.SelectedIndex;
            string course = ClassTextBox.Text;
            Priority priority = (Priority)PriorityComboBox.SelectedIndex;
            string description = TaskDescriptionTextBox.Text;


            // Get Date
            DateOnly date;
            if (TaskCalendar.SelectedDate.HasValue)
            {
                date = DateOnly.FromDateTime(TaskCalendar.SelectedDate.Value);
            }
            else
            {
                MessageBox.Show("Please Select a Date before adding a task.");
                return;
            }
            
            // Attempt to add the task
            Task task = new Task(title, type, course, priority, description, date);
            bool success = taskList.AddTask(task);
            if (success)
            {
                MessageBox.Show(task.Title + " was addedd successfully!");
                TaskTitleTextBox.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Task with name \"" + task.Title + "\" already exists. Please choose a different name");
            }

            TasksListBox.Items.Add(task.Title);
            ClearForm();
        }

        private void OnEditTaskClick(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Edit Task Clicked!");

            if (TasksListBox.SelectedItems.Count == 0)
            {
                MessageBox.Show("No task selected to edit!");
                return;
            }

            string title = (string)TasksListBox.SelectedItem;
            Task task = taskList.searchTasks(title);

            if (task != null)
            {
                task.Type = (TaskType)TypeComboBox.SelectedIndex;
                task.Course = ClassTextBox.Text;
                task.Priority = (Priority)PriorityComboBox.SelectedIndex;
                task.Description = TaskDescriptionTextBox.Text;


                // Get Date
                if (TaskCalendar.SelectedDate.HasValue)
                {
                    task.Date = DateOnly.FromDateTime(TaskCalendar.SelectedDate.Value);
                }
                else
                {
                    MessageBox.Show("Please Select a Date before editing a task.");
                    return;
                }

                MessageBox.Show(task.Title + " edited successfully!");
                TaskTitleTextBox.IsEnabled = true;
                TasksListBox.SelectedIndex = -1;
                ClearForm();
            }
            else
            {
                MessageBox.Show(title + " Not found in storage!");
            }
        }

        private void OnCompleteTaskClick(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Complete Task Clicked!");

            if (TasksListBox.SelectedItems.Count == 0)
            {
                MessageBox.Show("No task selected!");
                return;
            }

            string title = (string)TasksListBox.SelectedItem;
            Task task = taskList.searchTasks(title);

            if (task != null)
            {
                taskList.completeTask(title);
                ClearForm();
                TasksListBox.Items.Remove(TasksListBox.SelectedItem);
                if (TasksListBox.Items.Count == 0)
                {
                    MessageBox.Show("All tasks complete! Good Job!");
                }
                else
                {
                    MessageBox.Show("Task \"" + title + "\" completed! Good Job!");
                }
                TaskTitleTextBox.IsEnabled = true;
            }
            else
            {
                MessageBox.Show(title + " Not found in storage!");
            }


        }

        private void TaskListBoxSelectionChanged(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Task selected!");

            string title = (string)TasksListBox.SelectedItem;
            Task task = taskList.searchTasks(title);

            if (task != null)
            {
                TaskTitleTextBox.IsEnabled = false;
                FillForm(task);
            }
        }

        private void OnClearFormClicked(object sender, RoutedEventArgs e)
        {
            TaskTitleTextBox.IsEnabled = true;
            TasksListBox.SelectedIndex = -1;
            ClearForm();
        }

        private void OnPrioritySortClicked(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Priority sort selected!");

            // Check ListBox has items in it first
            if (TasksListBox.Items.Count > 0)
            {
                ClearForm();
                taskList.PrioritySort();
                PopulateTasks(taskList.GetTaskTitles());
            }
            else
            {
                MessageBox.Show("There are no tasks to sort!");
            }
        }

        private void OnClosestDateSortClicked(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Closest date sort selected!");

            ClearForm();
            taskList.ClosestDateSort();
            PopulateTasks(taskList.GetTaskTitles());
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(taskList.ToString());

            taskList.Save();
        }

        private void OnSaveAsClick(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(taskList.ToString());

            taskList.SaveAs();
        }

        private void OnOpenClick(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(taskList.ToString());

            ClearForm();

            taskList = TaskList.Open();

            PopulateTasks(taskList.GetTaskTitles());
        }
    }
}