namespace Common.Results;

public record ApiResponse<T>(T Data);

public record ApiResponsePaginated<T>(
	T Data,
	int CurrentPage,
	int PageSize,
	int TotalRecords) : ApiResponse<T>(Data)
{
	public int TotalPages { get; } = (int)Math.Ceiling(TotalRecords / (double)PageSize);
}