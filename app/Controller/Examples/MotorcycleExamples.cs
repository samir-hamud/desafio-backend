using Swashbuckle.Examples;

namespace App.Controller.Examples;

public class MotorcycleExamples : IExamplesProvider
{
    public object GetExamples()
    {
        return new Response("Hello World");
    }
}