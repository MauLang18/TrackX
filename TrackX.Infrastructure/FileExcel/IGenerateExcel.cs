using TrackX.Infrastructure.Commons.Bases.Response;
using TrackX.Utilities.Static;

namespace TrackX.Infrastructure.FileExcel
{
    public interface IGenerateExcel
    {
        MemoryStream GenerateToExcel<T>(BaseEntityResponse<T> data, List<TableColumns> columns);
    }
}