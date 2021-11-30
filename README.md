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