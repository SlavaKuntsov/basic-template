namespace TranslationService.Persistence.Entities;

public class Language
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public string Code { get; set; }
	public bool IsSelected { get; set; }

	public virtual ICollection<Translation> Translations { get; set; } = [];

	public Language()
	{
	}

	public Language(
		string name,
		string code,
		bool isSelected = false)
	{
		Id = Guid.NewGuid();
		Name = name;
		Code = code;
		IsSelected = isSelected;
	}
}