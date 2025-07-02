using Asp.Versioning;
using Common.Results;
using Microsoft.AspNetCore.Mvc;
using TranslationService.Application.Abstractions.Services;
using TranslationService.Application.Dtos;
using TranslationService.Persistence.Entities;

namespace TranslationService.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/translations")]
[ApiVersion("1.0")]
public class TranslationController(ITranslationService service) : ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> GetAll(
		[FromQuery] int pageNumber = 1,
		[FromQuery] int pageSize = 10,
		[FromQuery] bool sortByAsc = true,
		CancellationToken ct = default)
	{
		var result = await service.GetAsync(pageNumber, pageSize, sortByAsc, ct);

		return result.ToActionResult();
	}

	[HttpPost]
	public async Task<IActionResult> Add([FromBody] TranslationCreateDto dto, CancellationToken ct = default)
	{
		var result = await service.AddAsync(dto, ct);

		return result.ToActionResult();
	}

	[HttpPut]
	public async Task<IActionResult> Update([FromBody] Translation dto, CancellationToken ct = default)
	{
		var result = await service.UpdateAsync(dto, ct);

		return result.ToActionResult();
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
	{
		var result = await service.DeleteAsync(id, ct);

		return result.ToActionResult();
	}
}