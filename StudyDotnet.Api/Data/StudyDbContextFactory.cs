using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace StudyDotnet.Api.Data;

public sealed class StudyDbContextFactory : IDesignTimeDbContextFactory<StudyDbContext>
{
    public StudyDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<StudyDbContext>();

        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=study_dotnet;Username=postgres;Password=postgres");

        return new StudyDbContext(optionsBuilder.Options);
    }
}
