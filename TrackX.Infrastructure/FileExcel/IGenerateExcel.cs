using TrackX.Utilities.Static;

namespace TrackX.Infrastructure.FileExcel;

public interface IGenerateExcel
{
    MemoryStream GenerateToExcel<T>(List<T> data, List<TableColumns> columns);
    MemoryStream GenerateToExcelGeneric<T>(IEnumerable<T> data, List<TableColumns> columns);
}