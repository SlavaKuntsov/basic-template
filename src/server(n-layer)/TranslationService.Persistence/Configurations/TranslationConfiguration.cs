using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TranslationService.Persistence.Entities;

namespace TranslationService.Persistence.Configurations;

public class TranslationConfiguration : IEntityTypeConfiguration<Translation>
{
	public void Configure(EntityTypeBuilder<Translation> builder)
	{
		builder.Property(t => t.Id)
			.IsRequired();

		builder.Property(t => t.Text)
			.IsRequired(false);

		// builder.HasOne(t => t.LocalizationKey)
		// 	.WithMany(lk => lk.Translations)
		// 	.HasForeignKey(t => t.LocalizationKeyId)
		// 	.OnDelete(DeleteBehavior.Cascade);
		//
		// builder.HasOne(t => t.Language)
		// 	.WithMany(l => l.Translations)                            
		// 	.HasForeignKey(t => t.LanguageId)
		// 	.OnDelete(DeleteBehavior.Cascade);

		builder.HasIndex(t => new { t.LocalizationKeyId, t.LanguageId })
			.IsUnique();
	}
}