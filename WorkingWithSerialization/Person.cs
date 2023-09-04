using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Packt.Shared;

public class Person
{
    public Person() { }
    public Person(decimal initialSalay)
    {
        Salary = initialSalay;
    }
    protected decimal Salary { get; set; }
    [XmlAttribute("fname")]
    public string? FirstName { get; set; }
    [XmlAttribute("lname")]
    public string? LastName { get; set;}
    [XmlAttribute("dob")]
    public DateTime DateOfBitrh { get; set; }
    public HashSet<Person>? Children { get; set;}

}
