namespace App.MessageBroker;

public class RabbitMqSettings
{
    public RabbitMqSettings(string hostName, string userName, string password)
    {
        HostName = hostName;
        UserName = userName;
        Password = password;
    }

    public string HostName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}

public static class RabbitMqQueues
{
    public const string OrderValidationQueue = "orderValidationQueue";
    public const string MotoQueue = "motoQueue";
}