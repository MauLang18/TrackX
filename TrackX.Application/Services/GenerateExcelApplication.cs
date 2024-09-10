using TrackX.Application.Interfaces;
using TrackX.Infrastructure.FileExcel;
using TrackX.Utilities.Static;

namespace TrackX.Application.Services;

public class GenerateExcelApplication : IGenerateExcelApplication
{
    private readonly IGenerateExcel _generateExcel;

    public GenerateExcelApplication(IGenerateExcel generateExcel)
    {
        _generateExcel = generateExcel;
    }

    public byte[] GenerateToExcel<T>(List<T> data, List<(string ColumnName, string PropertyName)> columns)
    {
        var excelColumns = ExcelColumnNames.GetColumns(columns);
        var memoryStreamExcel = _generateExcel.GenerateToExcel(data, excelColumns);
        var fileBytes = memoryStreamExcel.ToArray();

        return fileBytes;
    }

    public byte[] GenerateToExcelGeneric<T>(IEnumerable<T> data, List<(string ColumnName, string PropertyName)> columns)
    {
        var excelColumns = ExcelColumnNames.GetColumns(columns);
        var memoryStreamExcel = _generateExcel.GenerateToExcelGeneric(data, excelColumns);
        var fileBytes = memoryStreamExcel.ToArray();

        return fileBytes;
    }
}