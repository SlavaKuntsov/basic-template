using Common.Results;
using FluentResults;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TranslationService.Application.Abstractions.Services;
using TranslationService.Application.Dtos;
using TranslationService.Persistence;
using TranslationService.Persistence.Entities;

namespace TranslationService.Application.Services;

public class TranslationService(
	ApplicationDbContext dbContext,
	IValidator<TranslationCreateDto> createValidator)
	: ITranslationService
{
	public async Task<Result<ApiResponsePaginated<IEnumerable<TranslationDto>>>> GetAsync(
		int pageNumber,
		int pageSize,
		bool sortByAsc,
		CancellationToken ct = default)
	{
		IQueryable<LocalizationKey> query = dbContext.LocalizationKeys
			.AsNoTracking()
			.Include(lk => lk.Translations)
			.ThenInclude(t => t.Language);

		query = sortByAsc
			? query.OrderBy(lk => lk.Key)
			: query.OrderByDescending(lk => lk.Key);

		var totalRecords = await query.CountAsync(ct);

		var page = await query
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToListAsync(ct);

		var dtoList = page.Select(
				lk => new TranslationDto(
					lk.Id,
					lk.Key,
					lk.Translations
						.Select(
							t => new LanguageWithTranslationDto(
								t.Language.Code,
								t.Text ?? string.Empty))
				))
			.ToList();

		var pagedResponse = new ApiResponsePaginated<IEnumerable<TranslationDto>>(
			dtoList,
			pageNumber,
			pageSize,
			totalRecords);

		return Result.Ok(pagedResponse);
	}

	public async Task<Result<Guid>> AddAsync(
		TranslationCreateDto dto,
		CancellationToken ct = default)
	{
		var validation = await createValidator.ValidateAsync(dto, ct);

		if (!validation.IsValid)
			return Result.Fail<Guid>(
				validation.Errors.Select(e => e.ErrorMessage).ToList());

		var keyEntity = await dbContext.LocalizationKeys
			.FirstOrDefaultAsync(k => k.Id == dto.KeyId, ct);

		if (keyEntity is null)
			return Result.Fail<Guid>(
				new Error($"Localization key with id '{dto.KeyId}' not found")
					.WithMetadata("Type", "NotFound"));

		var lang = await dbContext.Languages
			.FirstOrDefaultAsync(l => l.Code == dto.LanguageCode, ct);

		if (lang is null)
			return Result.Fail<Guid>(
				new Error($"Language '{dto.LanguageCode}' not found")
					.WithMetadata("Type", "NotFound"));

		var exists = await dbContext.Translations
			.AnyAsync(
				t =>
					t.LocalizationKeyId == keyEntity.Id &&
					t.LanguageId == lang.Id,
				ct);

		if (exists)
			return Result.Fail<Guid>(
				new Error("Translation already exists")
					.WithMetadata("Type", "AlreadyExists"));

		var translation = new Translation(
			keyEntity.Id,
			lang.Id,
			dto.Text);

		await dbContext.Translations.AddAsync(translation, ct);
		await dbContext.SaveChangesAsync(ct);

		return Result.Ok(translation.Id);
	}

	public async Task<Result<Translation>> UpdateAsync(
		Translation dto,
		CancellationToken ct = default)
	{
		var existing = await dbContext.Translations
			.FindAsync([dto.Id], ct);

		if (existing is null)
			return Result.Fail<Translation>(
				new Error("Translation not found")
					.WithMetadata("Type", "NotFound"));

		existing.Text = dto.Text;
		await dbContext.SaveChangesAsync(ct);

		return Result.Ok(existing);
	}

	public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
	{
		var existing = await dbContext.Translations
			.FindAsync([id], ct);

		if (existing is null)
			return Result.Fail(
				new Error("Translation not found")
					.WithMetadata("Type", "NotFound"));

		dbContext.Translations.Remove(existing);
		await dbContext.SaveChangesAsync(ct);

		return Result.Ok();
	}

	public async Task<Result<Guid>> AddAsync(
		Guid keyId,
		IEnumerable<LanguageWithTranslationDto> translations,
		CancellationToken ct = default)
	{
		var keyEntity = await dbContext.LocalizationKeys
			.FirstOrDefaultAsync(k => k.Id == keyId, ct);

		if (keyEntity is null)
			return Result.Fail(
				new Error($"Key with id '{keyId}' not found")
					.WithMetadata("Type", "NotFoundException"));

		foreach (var lt in translations)
		{
			var lang = await dbContext.Languages
				.FirstOrDefaultAsync(l => l.Code == lt.LanguageCode, ct);

			if (lang is null)
				return Result.Fail<Guid>(
					new Error($"Language '{lt.LanguageCode}' not found")
						.WithMetadata("Type", "NotFound"));

			var exists = await dbContext.Translations
				.AnyAsync(
					t =>
						t.LocalizationKeyId == keyEntity.Id &&
						t.LanguageId == lang.Id,
					ct);

			if (exists)
				return Result.Fail(
					new Error($"Translation for '{lt.LanguageCode}' already exists")
						.WithMetadata("Type", "AlreadyExists"));

			var translation = new Translation(
				keyEntity.Id,
				lang.Id,
				lt.Text);
			await dbContext.Translations.AddAsync(translation, ct);
		}

		await dbContext.SaveChangesAsync(ct);

		return Result.Ok(keyEntity.Id);
	}
}