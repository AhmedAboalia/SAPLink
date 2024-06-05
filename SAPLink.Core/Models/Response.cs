namespace SAPLink.Core.Models;

public class Response
{
    public string Name { get; set; }
    public string Metatype { get; set; }
    public string Comment { get; set; }
    public string TranslationId { get; set; }
    public List<Error> Errors { get; set; }
    public object[] Data { get; set; }
}

public class Response<T>
{
    public string Name { get; set; }
    public string Metatype { get; set; }
    public string Comment { get; set; }
    public string TranslationId { get; set; }
    public List<Error> Errors { get; set; }
    public List<T> Data { get; set; }
}
