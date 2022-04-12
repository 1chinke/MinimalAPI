using MinimalAPI.Utils;

namespace MinimalAPI.Models;

public class Person
{
    public string Id { get; set; } = Generate.Id();

    public string FirstName { get; set; }

    public string LastName { get; set; }
}
