using Common.Results;
using FluentResults;
using TranslationService.Application.Dtos;
using TranslationService.Persistence.Entities;

namespace TranslationService.Application.Abstractions.Services;

public interface ITranslationService
{
	Task<Result<ApiResponsePaginated<IEnumerable<TranslationDto>>>> GetAsync(
		int pageNumber,
		int pageSize,
		bool sortByAsc,
		CancellationToken ct = default);
	Task<Result<Guid>> AddAsync(TranslationCreateDto dto, CancellationToken ct = default);
	Task<Result<Translation>> UpdateAsync(Translation dto, CancellationToken ct = default);
	Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);
}