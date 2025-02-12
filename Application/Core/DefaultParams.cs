namespace Application.Core;

public class DefaultParams : PagingParams
{
  public string SortBy { get; set; } = "Id";
  public string OrderBy { get; set; } = "ASC";
  public string Search { get; set; } = "";
}