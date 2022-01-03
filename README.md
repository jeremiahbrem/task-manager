## Preliminary Setup

After cloaning the project, create an appsettings.Development.json in the TaskManager folder with the following schema:
```c#
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "ConnectionStrings": {
    "TaskManagerContext": "Host=localhost;Database=task_manager;Username=YOUR_USERNAME;Password=YOUR_PASSWORD",
    "DbUserName": "",
    "DbPassword": ""
  }
}
```

Make sure `postgresql` is installed on your local machine.
## Run Project

In the root folder:
```
cd TaskManager
dotnet run
```
The project is not running at `localhost:5000`.
## Routes
**POST** Create User: `/api/users/create`    
Example request:
```
{
  "firstName": "Jeremiah",
  "lastName": "Brem",
  "email": "jeremiah.brem@gmail.com"
}
```

**GET** User: `/api/users/{email}`    

**GET** Users: `/api/users`   

**POST** Create category: `/api/category/create`   
Example request:
```
{
  "name": "Category one"
}
```

**GET** Categories: `/api/categories`   

**POST** Create task: `/api/tasks/create`   
Example request:
```
{
  "name: "Task one",
  "category: "Category one"
}
```

**GET** Task: `/api/tasks/{name}`    

**GET** Tasks: `/api/tasks`   

\*\***All `scheduled-task` routes require an `email` header value in the request.**\*\*

**POST** Create scheduled task: `/api/scheduled-tasks/create`   
Example request:
```
{
  "task": "Task one"
}
```
Example success response:
```
Scheduled task 7746615b-6359-45fa-900c-3053f494b8db created.
```
Example request with designating a preceding scheduled task:
```
{
  "task": "Task two",
  "precedingId: "7746615b-6359-45fa-900c-3053f494b8db"
}
```

**GET** Scheduled task: `/api/scheduled-tasks{id}`   
Example request:
```
http://localhost:5000/api/scheduled-tasks/7746615b-6359-45fa-900c-3053f494b8db`
```

**GET** Scheduled tasks: `/api/scheduled-tasks`    

**POST** Complete scheduled task: `/api/scheduled-tasks/complete`    
Example request:
```
{
  "id": "7746615b-6359-45fa-900c-3053f494b8db"
}
```
