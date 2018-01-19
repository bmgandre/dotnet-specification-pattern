# dotnet-specification-pattern
> Specification pattern implementation in C#

[![Build status](https://ci.appveyor.com/api/projects/status/6srn4gooob303bvg?svg=true)](https://ci.appveyor.com/project/bmgandre/dotnet-specification-pattern)
[![Build Status](https://travis-ci.org/bmgandre/dotnet-specification-pattern.svg?branch=master)](https://travis-ci.org/bmgandre/dotnet-specification-pattern)

This repository provides a C# Composite based implementation of 
the Specification pattern.

The Composite implementation adds flexibility by allowing the 
business rules to be combined. In additional to the logical 
operations, this implementation uses a Builder pattern with 
Fluent Interfaces. As a result, we can use extensions methods,
instead of classes, to create new rules.

## Comparison

The examples below compares the standard composite implementation 
to an implementation using the builder and fluent interfaces.

###  Standard implementation

In a standard implementation, the rule definition is located inside 
the class:

```csharp
public class BlogCreatedAfterSpecification : BaseSpecification<Blog>
{
   public BlogCreatedAfterSpecification(DateTime dateTime)
       : base(x => x.Created >= dateTime)
   {
   }
}
```

This is how to instantiate and build the specifications:
``` csharp
var createdAfter = new BlogCreatedAfterSpecification(dateTime);
var notBanned = new BlogNotBannedSpecification();
var notExpired = new BlogNotExpiredSpecification();
var specification = createdAfter.And(notBanned).And(notExpired);
```

### Builder with Fluent Interfaces

Instead of a class per rule, a extension method is used to add rules.
In the sample code below, Add is a method with an Expression parameter, 
it create an instance of the Specification and add to the composite 
structure.

```csharp
public static class BlogSpecificationExtensions
{
    public static ISpecification<Blog> CreatedAfter(this ISpecification<Blog> specification,
           DateTime dateTime)
    {
        return specification.And(x => x.Created >= dateTime);
    }
}
```

The composed rule can be instantiated like below. The result is that
the business rules can be read more fluently.
```csharp
var specification = SpecificationBuilder<Blog>.Create()
                        .CreatedAfter(dateTime)
                        .NotBanned()
                        .NotExpired();
```

## Building and running

### Requirments

```
dotnet core >= 2.0
```

This repository provides a console application in additional to an xUnit
and a Specflow test project. *Please note that Specflow project only works 
in Windows environment.* 

### Running

Running the console application:

```shell
cd src/SpecificationDemo
dotnet ef migrations add InitialCreate --startup-project ../SpecificationDemoConsole
dotnet ef database update --startup-project ../SpecificationDemoConsole


dotnet run
```

Running tests:

```shell
dotnet test SpecificationDemoXunitTest
dotnet test SpecificationDemoBddTest
```

### Testing specifications on LinqPad

Requirements:

```
LinqPad >= 5.26
Entity Framework 7 (EF Core) Driver >= 2.0.1
``` 

Before configuring the connection, you need to publish one project:

```shell
cd src/SpecificationDemoConsole
dotnet publish
```

Then configure the connection following the steps below:
- Add a new connection
  - Use a typed data context using Entity Framework 2.0.1
  - In the **Path to custom assembly**, select the publish directory. Example:
`D:\dotnet-specification-pattern\src\SpecificationDemoConsole\bin\Debug\netcoreapp2.0\SpecificationDemo.dll`
  - Select the **Full type name of the typed DbContext**: SpecificationDemo.Data.BloggingContext
  - Select **Via a constructor that accepts a string**: 
 `Server=(localdb)\mssqllocaldb;Database=Blog;Trusted_Connection=True;ConnectRetryCount=0`
  - Select **Remember this connection**

Create a new query and change to **C# Program**. Then, select the 
**BloggingContext in SpecificationDemo.dll** connection.

In **query properties** (F4), add the following namespaces:

```
SpecificationDemo.Data
SpecificationDemo.Entities
SpecificationDemo.Specifications
System
System.Data
System.Linq
System.Threading.Tasks
```

LinqPad **C# Program** sample code :

```csharp
void Main()
{
    var blogRepository = new EfReadRepository<Blog>(this);
    
    var specification = SpecificationBuilder<Blog>.Create()
        .NotExpired()
        .CreatedAfter(new DateTime(2017, 1, 1));
        
    var result = blogRepository
        .Where(specification, b => b.Posts)
        .ToList();
        
    Console.WriteLine(result);
}
```

Using **C# Statement(s)** this can be simplified to:

```csharp
var specification = SpecificationBuilder<Blog>.Create()
    .NotExpired()
    .CreatedAfter(new DateTime(2017, 1, 1));
    
Console.Write(Blogs.Where(specification));
```

## License
Copyright (c) 2018 André Gomes
Licensed under the [MIT](LICENSE) License.
