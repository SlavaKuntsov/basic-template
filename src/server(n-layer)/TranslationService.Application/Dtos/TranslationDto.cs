namespace TranslationService.Application.Dtos;

public sealed record TranslationDto(
	Guid KeyId,
	string Key,
	IEnumerable<LanguageWithTranslationDto> Translations);