using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Console;
using static System.Environment;
using static System.IO.Path;

Book csharp10 = new(title: "C# 10 and .NET 6 - Modren Cross-platform Development")
{
    Author = "Mark J Price",
    publishDate = new(2121, 11, 9),
    Pages = 823,
    Created = DateTimeOffset.UtcNow
};
JsonSerializerOptions option = new()
{
    IncludeFields = true,
    PropertyNameCaseInsensitive = true,
    WriteIndented = true,
    PropertyNamingPolicy  = JsonNamingPolicy.CamelCase
};

string filePath = Combine(CurrentDirectory, "book.json");
using(Stream fileStream = File.Create(filePath))
{
    JsonSerializer.Serialize<Book>(fileStream, csharp10 ,option);
}
WriteLine("Writtem {0:N0} bytes of JSON to {1}",new FileInfo(filePath).Length,filePath);
WriteLine();
WriteLine(File.ReadAllText(filePath));

public class Book
{
    public Book(string title)
    {
        Title = title;
    }
    public string Title { get; set; }

    public string? Author { get; set; }

    [JsonInclude]
    public DateOnly publishDate;

    [JsonInclude]
    public DateTimeOffset Created;
    public ushort Pages;
}
