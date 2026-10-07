namespace TaskManager;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public bool Done { get; set; }
}

public static class TaskService
{
    public static void Add(int userId, string title, string description)
    {
        using var conn = Database.GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO tasks (user_id, title, description) VALUES ($u, $t, $d)";
        cmd.Parameters.AddWithValue("$u", userId);
        cmd.Parameters.AddWithValue("$t", title);
        cmd.Parameters.AddWithValue("$d", description);
        cmd.ExecuteNonQuery();
    }

    public static List<TaskItem> List(int userId)
    {
        var result = new List<TaskItem>();
        using var conn = Database.GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT id, title, description, done FROM tasks WHERE user_id = $u";
        cmd.Parameters.AddWithValue("$u", userId);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            result.Add(new TaskItem
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
                Done = reader.GetInt32(3) == 1
            });
        }
        return result;
    }

    public static void MarkDone(int taskId)
    {
        using var conn = Database.GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE tasks SET done = 1 WHERE id = $id";
        cmd.Parameters.AddWithValue("$id", taskId);
        cmd.ExecuteNonQuery();
    }

    public static void Delete(int taskId)
    {
        using var conn = Database.GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM tasks WHERE id = $id";
        cmd.Parameters.AddWithValue("$id", taskId);
        cmd.ExecuteNonQuery();
    }

    public static void Print(List<TaskItem> tasks)
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("Список задач пуст.");
            return;
        }
        foreach (var t in tasks)
        {
            var status = t.Done ? "[✔]" : "[ ]";
            Console.WriteLine($"{status} #{t.Id} — {t.Title} ({t.Description})");
        }
    }
}