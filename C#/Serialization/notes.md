# C# Serialization with JSON and XML

## 1. What is Serialization ?
Serialization converts an object in memory into a format that can be:
- saved to a file
- sent through API
- stored in a database or cache
- shared between different application

Deserialization converts that stored or transmitted data back into a C# object.

C# object -> Serialization -> JSON/XML text
JSON/XML text -> Deserialization -> C# object

Example C# object:
```csharp
Product product = new Product(1,"Keyboard",49.99m);
```
Serialized JSON:
```json
{
    "id": 1,
    "name": "Keyboard",
    "price": 49.99
}
```
Serialized XML:
```xml
<Product>
    <Id>1</Id>
    <Name>Keyboard</Name>
    <Price>49.99</Name>
</Product>
```

## 2. JSON vs XML

`JSON` means Javascript Object Notation.
It is commonly used for:
- Rest APIs
- Config files
- Web applications
- Modern .NET applications

`Advantages`
- Compact
- Easy to read
- Fast
- Works naturally with objects and collections
- Standard choice for APIs

`XML` means Extensible Markup Language. 
It is commonly used for:
- Older enterprise system
- Config formats
- Systems requiring attributed and document structure

`Advantages`
- Support attributes
- Strong document structure
- Good compatibility with older systems
- Can represent complex documents

## 3. The Model Class
A model class represents the data being serialized.
```csharp
public class Product {

    public int Id {get; set;}
    public string Name {get; set;}
    public decimal Price {get; set;}
    public List<string> Tags {get; set;} 
}
```
This is also called a `DTO`, meaning Data Transfer Object. A DTO is a simple class used to move data between parts of an application.

## 4. JSON Serialization
Modern C# uses System.Text.Json.
```csharp
Product product = new() {
    Id = 1,
    Name = "Keyboard",
    Price = 49.99m,
    Tags = new List<string> {"USB", "Mechanical"}
};

string json = JsonSerializer.Serialize(product);
Console.WriteLine(json);
```
output
```json
{"Id":1,"Name":"Keyboard","Price":49.99,"Tags":["USB","Mechanical"]}
```

## 5. JSON Deserialization
```csharp
string json = """
"Id": 1,
"Name": "Keyboard",
"Price": 49.99,
"Tags": ["USB","Mechanical"]
""";

Product product = JsonSerializer.Deserialize<Product>(json);

if(product is not null) {
    Console.WriteLine(product.Name);
    Console.WriteLine(product.Price);
}
```
output
```bash
Keyboard
49.99
```

## 6. JSON Naming Options
By default, property names are usually written exactly as they appear in C#. 
You can use camelCase, which is common in APIs.
```csharp
using System.Text.Json;;

JsonSerializerOptions options = new() {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true
};

string json = JsonSerializer.Serialize(product,options);
Console.WriteLine(json);
```

output
```json
{
  "id": 1,
  "name": "Keyboard",
  "price": 49.99,
  "tags": [
    "USB",
    "Mechanical"
  ]
}
```

## 7. JSON Attributes
Attributes customize serialization behavior.
```csharp
using System.Text.Json.Serialization;

public class Product {

    public int Id {get; set;}

    [JsonPropertyName("product_name")]
    // wins over PropertyNamingPolicy
    public string Name {get; set;}

    public decimal Price {get; set;}

    [JsonIgnore]
    public string InternalCode {get; set;}
}
```
This object:
```csharp
Product product = new()
{
    Id = 1,
    Name = "Keyboard",
    Price = 49.99m,
    InternalCode = "SECRET-123"
};
```
Produces:
```json
{
  "Id": 1,
  "product_name": "Keyboard",
  "Price": 49.99
}
```

## 8. XML Serialization
XML Serialization means converting a C# object into XML text.

Example C# class:
```csharp
public class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }
}
```
Create an object using:
```csharp
Product product = new Product();

product.Id = 1;
product.Name = "Keyboard";
product.Price = 49.99m;
```
`step 1`: Create an XML Serializer
```csharp
XmlSerializer serializer = new XmlSerializer(typeof(Product));
```
`XmlSerializer` is a .NET class that converts objects to XML and XML back to objects.
`step 2`: Create a writer
```csharp
StringWriter writer = new StringWriter();
```
StringWriter is an object that allows us to write text into a string.
Normally, a writer writes to a file. StringWriter writes to memory.
`step 3`: Serialize the object
```csharp
serializer.Serialize(writer,product);
```
This means: convert `product` into XML and write the result into `writer`.
`step 4`: Get the XML string
```csharp
string xml = writer.ToString();
```
writer.ToString() returns the XML text stored inside the writer.
`Complete Example`
```csharp
using System;
using System.IO;
using System.Xml.Serialization;

[XmlRoot("Product")]
public class Product
{
     [XmlAttribute("id")]
    public int Id { get; set; }

    [XmlElement("ProductName")]
    public string Name { get; set; } = string.Empty;
}

public class Program
{
    public static void Main()
    {
        Product product = new Product();

        product.Id = 1;
        product.Name = "Keyboard";

        XmlSerializer serializer =
            new XmlSerializer(typeof(Product));

        StringWriter writer =
            new StringWriter();

        serializer.Serialize(writer, product);

        string xml =
            writer.ToString();

        writer.Close();

        Console.WriteLine(xml);
    }
}
```
Possible Output
```xml
<Product id="1">
  <ProductName>Keyboard</ProductName>
  <Price>49.99</Price>
</Product>
```

## 9. XML Deserialization
```csharp
using System;
using System.IO;
using System.Xml.Serialization;

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }
}

public class Program
{
    public static void Main()
    {
        string xml = """
        <Product>
          <Id>1</Id>
          <Name>Keyboard</Name>
          <Price>49.99</Price>
        </Product>
        """;

        XmlSerializer serializer =
            new XmlSerializer(typeof(Product));

        StringReader reader =
            new StringReader(xml);

        Product product =
            (Product)serializer.Deserialize(reader);

        Console.WriteLine(product.Id);
        Console.WriteLine(product.Name);
        Console.WriteLine(product.Price);

        reader.Close();
    }
}
```
Output:
```bash
1
Keyboard
49.99
```
