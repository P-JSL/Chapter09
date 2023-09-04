using static System.Console;
using System.Xml.Serialization;
using Packt.Shared;
using static System.Environment;
using static System.IO.Path;
using NewJson = System.Text.Json.JsonSerializer;
using Microsoft.VisualBasic;

namespace WorkingWithSerialization;

public  class Program
{
    public static void Main(string[] args)
    {
        List<Person> people = new()
        {
            new(30000M)
            {
                FirstName = "Alice",
                LastName = "Smith",
                DateOfBitrh = new(1974,3,14)
            },
            new(40000M)
            {
                FirstName = "Bob",
                LastName = "Jones",
                DateOfBitrh = new(1969,11,23)
            },
            new(20000M)
            {
                FirstName = "Charlie",
                LastName = "Cox",
                DateOfBitrh = new(1984,5,4),
                Children = new()
                {
                    new(0M)
                    {
                        FirstName = "Sally",
                        LastName = "Cox",
                        DateOfBitrh = new(2000,7,12)
                    }
                }
            }
        };

        XmlSerializer xs = new(people.GetType());
        //쓰기파일
        string path = Combine(CurrentDirectory, "People.xml");
        using(FileStream stream = File.Create(path))
        {
            xs.Serialize(stream, people);
        }

        WriteLine("Written {0:N0} bytes of XML to {1}",
            new FileInfo(path).Length,path);
        WriteLine();
        WriteLine(File.ReadAllText(path));

        Deserialize(path,xs);
        jsonWriter(people);
    }

    static void Deserialize(string path, XmlSerializer xs)
    {
        using (FileStream xmlLoad = File.Open(path,FileMode.Open) )
        {
            if (xmlLoad != null)
            {
                List<Person>? loadedPeople = xs.Deserialize(xmlLoad) as List<Person>;
                if(loadedPeople is not null)
                {
                    foreach (Person p in loadedPeople)
                    {
                        WriteLine("{0} has {1} children.", p.LastName, p.Children?.Count ?? 0);
                    }
                }
            }
        }
    }

    public static async void jsonWriter(List<Person>? people)
    {
        string jsonPath = Combine(CurrentDirectory, "people.json");
        /*using (StreamWriter jsonStream = File.CreateText(jsonPath))
        {
            Newtonsoft.Json.JsonSerializer jss = new();
            jss.Serialize(jsonStream,people);
        }*/
        using(FileStream jsonLoad = File.Open(jsonPath, FileMode.Open))
        {
            List<Person>? loadedPeople = await NewJson.DeserializeAsync(utf8Json:jsonLoad,returnType:typeof(List<Person>)) as List<Person>;
            if(loadedPeople is not null)
            {
                foreach(Person p in loadedPeople)
                {
                    WriteLine("{0} has {1} children.", p.LastName, p.Children?.Count ?? 0);
                }
            }
        }
        WriteLine();
        WriteLine("Written {0:N0} bytes of JSON to : {1}", new FileInfo(jsonPath).Length,jsonPath);
        WriteLine(File.ReadAllText(jsonPath));
    }
}
