namespace SAPLink.Domain.Models.System;

public class Clients
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool Active { get; set; }

    public ICollection<Credentials?> Credentials { get; set; }
}