using TaskManager;

Database.Init();

while (true)
{
    var user = UI.AuthMenu();
    if (user != null)
    {
        UI.TasksMenu(user);
        break;
    }
}