namespace ToDoApp.Application.Errors;

public class NotFoundException : Exception
{

    public NotFoundException(string message) : base(message)
    {

    }
}