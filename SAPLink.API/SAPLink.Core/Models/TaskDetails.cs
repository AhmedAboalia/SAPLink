namespace SAPLink.Core.Models;
public class TaskDetails
{
    private string Status;
    private string TaskName;
    private int ResponseStatusCode;
    private string CreatedTime;
    private string ExecutionTime;
    public TaskDetails CreateTaskDetails(string status, string taskName, string createdTime, string executionTime)
    {
        Status = status;
        TaskName = taskName;
        CreatedTime = createdTime;
        ExecutionTime = executionTime;
        return this;
    }

    public TaskDetails(string status, string taskName, string createdTime, string executionTime)
    {
        CreateTaskDetails(status, taskName, createdTime, executionTime);
    }
}
