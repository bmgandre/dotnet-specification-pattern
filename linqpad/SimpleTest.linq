<Query Kind="Program">
  <Connection>
    <ID>a8ca8077-cd9f-42ad-a0ac-c38f093b3c12</ID>
    <Persist>true</Persist>
    <Driver Assembly="EF7Driver" PublicKeyToken="469b5aa5a4331a8c">EF7Driver.StaticDriver</Driver>
    <CustomAssemblyPath>C:\Users\andre\Projects\dotnet-specification-pattern\src\SpecificationDemoConsole\bin\Debug\netcoreapp2.0\SpecificationDemo.dll</CustomAssemblyPath>
    <CustomTypeName>SpecificationDemo.Data.BloggingContext</CustomTypeName>
    <CustomCxString>Server=(localdb)\mssqllocaldb;Database=Blog;Trusted_Connection=True;ConnectRetryCount=0</CustomCxString>
  </Connection>
  <Namespace>SpecificationDemo.Data</Namespace>
  <Namespace>SpecificationDemo.Entities</Namespace>
  <Namespace>SpecificationDemo.Specifications</Namespace>
  <Namespace>System</Namespace>
  <Namespace>System.Data</Namespace>
  <Namespace>System.Linq</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

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