using ClosedXML.Excel;
using System.Reflection;

namespace TrackX.Infrastructure.FileExcel
{
    public class ImportExcel : IImportExcel
    {
        public IEnumerable<T> ImportFromExcel<T>(Stream excelStream) where T : class, new()
        {
            using var workbook = new XLWorkbook(excelStream);
            var worksheet = workbook.Worksheet(1);

            var entities = new List<T>();
            var properties = typeof(T).GetProperties();

            // Get the header row
            var headerRow = worksheet.Row(1);

            // Create a dictionary to map column names to property names
            var columnMappings = new Dictionary<int, PropertyInfo>();
            foreach (var property in properties)
            {
                var column = headerRow.Cells().FirstOrDefault(c => c.Value.ToString().Equals(property.Name, StringComparison.OrdinalIgnoreCase));
                if (column != null)
                {
                    columnMappings[column.Address.ColumnNumber] = property;
                }
            }

            // Iterate over the data rows
            var rows = worksheet.RowsUsed().Skip(1);
            foreach (var row in rows)
            {
                var entity = new T();
                foreach (var mapping in columnMappings)
                {
                    var cell = row.Cell(mapping.Key);
                    if (cell != null && !string.IsNullOrEmpty(cell.GetString()))
                    {
                        var property = mapping.Value;
                        var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                        try
                        {
                            var cellValue = Convert.ChangeType(cell.GetString(), propertyType);
                            property.SetValue(entity, cellValue);
                        }
                        catch (Exception ex)
                        {
                            // Handle the exception or log it
                            Console.WriteLine($"Error converting value for property '{property.Name}': {ex.Message}");
                        }
                    }
                }
                entities.Add(entity);
            }

            return entities;
        }
    }
}
