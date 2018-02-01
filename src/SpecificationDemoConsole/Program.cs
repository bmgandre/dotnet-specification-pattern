using ConsoleTableExt;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Samples.EFLogging;
using SpecificationDemo.Data;
using SpecificationDemo.Entities;
using SpecificationDemo.Specifications;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SpecificationDemoConsole
{
    internal class Program
    {
        private static Func<string, LogLevel, bool> SqlLoggerFilter = (c, l) =>
        {
            return l == LogLevel.Error || c == DbLoggerCategory.Database.Command.Name;
        };

        private static async Task Main()
        {
            using (var context = new DesignTimeDbContextFactory().CreateDbContext(null))
            {
                context.ConfigureLogging(s => Console.WriteLine(s), SqlLoggerFilter);
                await BloggingContextSeed.SeedAsync(context);

                var specificationFactory = new SpecificationFactory();
                var specification = specificationFactory.Create<Blog>()
                    .CreatedAfter(new DateTime(2017, 1, 1));

                var blogRepository = new EfReadRepository<Blog>(context);
                var result = blogRepository
                    .Where(specification, b => b.Posts)
                    .ToList();

                ConsoleTableBuilder
                    .From(result.ToDataTable())
                    .WithFormat(ConsoleTableBuilderFormat.Minimal)
                    .ExportAndWriteLine();
            }
        }
    }
}