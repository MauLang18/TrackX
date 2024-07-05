using TrackX.Domain.Entities;

namespace TrackX.Application.Interfaces;

public interface IGenerateExcelApplication
{
    byte[] GenerateToExcel(List<Value2> data, List<(string ColumnName, string PropertyName)> columns);
    byte[] GenerateToExcelGeneric<T>(IEnumerable<T> data, List<(string ColumnName, string PropertyName)> columns);
}