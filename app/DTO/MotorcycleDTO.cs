namespace App.DTO;

public class MotorcycleDTO : BaseDTO
{
    public string Identification { get; set; }
    public ushort Year { get; set; }
    public string Model { get; set; }
    public string LicensePlate { get; set; }
}
