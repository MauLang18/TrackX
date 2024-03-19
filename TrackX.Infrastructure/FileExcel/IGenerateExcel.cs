using TrackX.Domain.Entities;
using TrackX.Utilities.Static;

namespace TrackX.Infrastructure.FileExcel
{
    public interface IGenerateExcel
    {
        MemoryStream GenerateToExcel(List<Value2> data, List<TableColumns> columns);
    }
}