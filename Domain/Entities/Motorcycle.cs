namespace Domain.Entities;

public class Motorcycle : Entity
{
    public Motorcycle(long id, string identification, ushort year, string model,
        string licensePlate)
    {
        Id = id;
        Identification = identification;
        Year = year;
        Model = model;
        LicensePlate = licensePlate;
    }

    public string Identification { get; set; }
    public ushort Year { get; set; }
    public string Model { get; set; }
    public string LicensePlate { get; set; }

}