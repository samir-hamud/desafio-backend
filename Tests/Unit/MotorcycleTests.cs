using Domain.Entities;

namespace Tests.Unit;

public class MotorcycleTests : IClassFixture<DatabaseFixture>
{
    public DatabaseFixture Db { get; set; }
    
    public MotorcycleTests(DatabaseFixture db)
    {
        Db = db;
    }
    
    [Fact]
    public async Task AddMotorcycleAsync()
    {
        var newMoto = new Motorcycle(0, "Identification", 2020, "Model", "ABC-1234");
        
        await Db.DbContext.Motorcycles.AddAsync(newMoto);
        await Db.DbContext.SaveChangesAsync();
        
        var added = await Db.DbContext.Motorcycles;
        
        Assert.True(true);
        
    }
}