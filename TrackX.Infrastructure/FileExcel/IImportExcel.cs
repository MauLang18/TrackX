namespace TrackX.Infrastructure.FileExcel
{
    public interface IImportExcel
    {
        IEnumerable<T> ImportFromExcel<T>(Stream excelStream) where T : class, new();
    }
}