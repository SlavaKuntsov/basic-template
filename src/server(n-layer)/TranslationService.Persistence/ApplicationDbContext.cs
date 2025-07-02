using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TranslationService.Persistence.Entities;

namespace TranslationService.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
	: DbContext(options)
{
	public DbSet<Language> Languages { get; set; }
	public DbSet<LocalizationKey> LocalizationKeys { get; set; }
	public DbSet<Translation> Translations { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		base.OnModelCreating(modelBuilder);
	}
}