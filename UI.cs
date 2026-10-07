namespace TaskManager;

public static class UI
{
    public static User? AuthMenu()
    {
        Console.WriteLine("\n=== Вход ===");
        Console.WriteLine("1. Регистрация");
        Console.WriteLine("2. Вход");
        Console.Write("Выбор: ");
        var choice = Console.ReadLine();

        Console.Write("Логин: ");
        var username = Console.ReadLine() ?? "";
        Console.Write("Пароль: ");
        var password = Console.ReadLine() ?? "";

        if (choice == "1")
        {
            Console.WriteLine(Auth.Register(username, password)
                ? "Пользователь создан."
                : "Ошибка: логин занят.");
            return null;
        }
        if (choice == "2")
        {
            var user = Auth.Login(username, password);
            if (user != null)
            {
                Console.WriteLine($"Добро пожаловать, {user.Username}!");
                return user;
            }
            Console.WriteLine("Неверный логин или пароль.");
        }
        return null;
    }

    public static void TasksMenu(User user)
    {
        while (true)
        {
            Console.WriteLine("\n=== Задачи ===");
            Console.WriteLine("1. Добавить задачу");
            Console.WriteLine("2. Показать задачи");
            Console.WriteLine("3. Отметить выполненной");
            Console.WriteLine("4. Удалить задачу");
            Console.WriteLine("0. Выход");
            Console.Write("Выбор: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Название: ");
                    var title = Console.ReadLine() ?? "";
                    Console.Write("Описание: ");
                    var desc = Console.ReadLine() ?? "";
                    TaskService.Add(user.Id, title, desc);
                    Console.WriteLine("Задача добавлена.");
                    break;
                case "2":
                    TaskService.Print(TaskService.List(user.Id));
                    break;
                case "3":
                    Console.Write("ID задачи: ");
                    if (int.TryParse(Console.ReadLine(), out var idDone))
                        TaskService.MarkDone(idDone);
                    break;
                case "4":
                    Console.Write("ID задачи: ");
                    if (int.TryParse(Console.ReadLine(), out var idDel))
                        TaskService.Delete(idDel);
                    break;
                case "0":
                    return;
            }
        }
    }
}