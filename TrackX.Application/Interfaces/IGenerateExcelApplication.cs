using TrackX.Infrastructure.Commons.Bases.Response;

namespace TrackX.Application.Interfaces
{
    public interface IGenerateExcelApplication
    {
        byte[] GenerateToExcel<T>(BaseEntityResponse<T> data, List<(string ColumnName, string PropertyName)> columns);
    }
}