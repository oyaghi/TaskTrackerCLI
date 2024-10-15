using Newtonsoft.Json;


var obj1 = new TaskTracker();
public class Task
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "TODO";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
public class TaskTracker
{
    private const string FilePath = "tasks.json";
    private List<Task> tasks;

    public TaskTracker()
    {
        tasks = new List<Task>();
        LoadTasks();
    }

    private void LoadTasks()
    {
        try
        {
            if (File.Exists(FilePath))
            {
                // Read the JSON data from the file
                string jsonData = File.ReadAllText(FilePath);
                tasks = JsonConvert.DeserializeObject<List<Task>>(jsonData) ?? new List<Task>();
                Console.WriteLine("Tasks loaded from file.");
            }
            else
            {
                File.WriteAllText(FilePath, "[]");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public void AddTask(string description)
    {
        var newTask = new Task
        {
            Id = tasks.Count > 0 ? tasks[^1].Id + 1 : 1,
            Description = description,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        tasks.Add(newTask);
        SaveTasks();
    }

    public void UpdateTask(int id, string description)
    {
        tasks[id - 1].Description = description;
        tasks[id - 1].UpdatedAt = DateTime.Now;
        SaveTasks();
    }

    public void DeleteTask(int id)
    {
        tasks.RemoveAt(id-1);
        SaveTasks();
    }

    public void MarkInProgress(int id)
    {
        tasks[id - 1].Status = "In-Progress";
        SaveTasks();
    }

    public void MarkDone(int id)
    {
        tasks[id - 1].Status = "Done";
        SaveTasks();
    }

    public void List(string filter = "all")
    {
        var filteredTasks = filter.ToLower() switch
        {
            "done" => tasks.Where(task => task.Status == "Done"),
            "in-progress" => tasks.Where(task => task.Status == "In-Progress"),
            _ => tasks 
        };

        // Display task description and status in parallel format
        foreach (var task in filteredTasks)
        {
            Console.WriteLine($"{task.Description.PadRight(20)} {task.Status}");
        }

        if (!filteredTasks.Any())
        {
            Console.WriteLine("No tasks found with the specified filter.");
        }
    }


    private void SaveTasks()
    {
        string jsonData = JsonConvert.SerializeObject(tasks, Formatting.Indented);
        File.WriteAllText(FilePath, jsonData);
    }
}