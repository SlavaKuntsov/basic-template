namespace TranslationService.Persistence.Entities;

public class LocalizationKey
{
	public Guid Id { get; set; }
	public string Key { get; set; } = string.Empty;

	public virtual ICollection<Translation> Translations { get; set; } = [];

	public LocalizationKey()
	{
	}

	public LocalizationKey(string key)
	{
		Id = Guid.NewGuid();
		Key = key;
	}
}