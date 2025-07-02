namespace TranslationService.Application.Dtos;

public record TranslationCreateDto(
	Guid KeyId,
	string LanguageCode,
	string Text);