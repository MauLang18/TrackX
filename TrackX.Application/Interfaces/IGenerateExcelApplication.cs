namespace TrackX.Application.Interfaces;

public interface IGenerateExcelApplication
{
    byte[] GenerateToExcel<T>(List<T> data, List<(string ColumnName, string PropertyName)> columns);
    byte[] GenerateToExcelGeneric<T>(IEnumerable<T> data, List<(string ColumnName, string PropertyName)> columns);
}