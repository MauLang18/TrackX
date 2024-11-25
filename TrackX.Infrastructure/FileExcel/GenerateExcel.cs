using ClosedXML.Excel;
using Newtonsoft.Json;
using TrackX.Utilities.Static;

namespace TrackX.Infrastructure.FileExcel;

public class GenerateExcel : IGenerateExcel
{
    private readonly string jsonFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Static", "JSON");

    public MemoryStream GenerateToExcel<T>(List<T> data, List<TableColumns> columns)
    {
        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Listado");

        for (int i = 0; i < columns.Count; i++)
        {
            worksheet.Cell(1, i + 1).Value = columns[i].Label;
        }

        var rowIndex = 2;

        foreach (var item in data)
        {
            for (int i = 0; i < columns.Count; i++)
            {
                var propertyValue = GetPropertyValue(item, columns[i]);
                worksheet.Cell(rowIndex, i + 1).Value = propertyValue;
            }

            rowIndex++;
        }

        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;

        return stream;
    }

    public MemoryStream GenerateToExcelGeneric<T>(IEnumerable<T> data, List<TableColumns> columns)
    {
        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Listado");

        for (int i = 0; i < columns.Count; i++)
        {
            worksheet.Cell(1, i + 1).Value = columns[i].Label;
        }

        var rowIndex = 2;

        foreach (var item in data)
        {
            for (int i = 0; i < columns.Count; i++)
            {
                var propertyValue = typeof(T).GetProperty(columns[i].PropertyName!)?.GetValue(item)?.ToString();
                worksheet.Cell(rowIndex, i + 1).Value = propertyValue;
            }

            rowIndex++;
        }

        var stream = new MemoryStream();
        workbook.SaveAs(stream);

        stream.Position = 0;

        return stream;
    }

    private string GetPropertyValue(object item, TableColumns column)
    {
        string propertyName = column.PropertyName!;
        string jsonFileName = GetJsonFileName(propertyName);

        if (!string.IsNullOrEmpty(jsonFileName))
        {
            string jsonFilePath = Path.Combine(jsonFolderPath, jsonFileName);
            if (File.Exists(jsonFilePath))
            {
                // Obtener el valor de la propiedad del item
                var valor = item.GetType().GetProperty(propertyName)?.GetValue(item)?.ToString();

                // Si 'valor' es nulo, devolver una cadena vacía
                if (string.IsNullOrEmpty(valor))
                {
                    return "";
                }

                string jsonContent = File.ReadAllText(jsonFilePath);
                Dictionary<string, string> jsonValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonContent)!;

                // Verificar si el valor existe en el diccionario
                if (jsonValues.ContainsKey(valor))
                {
                    return jsonValues[valor];
                }
                else
                {
                    return ""; // O algún valor predeterminado si es necesario
                }
            }
            else
            {
                return ""; // Manejar el caso en que el archivo JSON no exista
            }
        }

        // Si no hay archivo JSON asociado, devolver el valor de la propiedad directamente
        var propertyValue = item.GetType().GetProperty(propertyName)?.GetValue(item)?.ToString();
        return propertyValue ?? "";
    }

    private string GetJsonFileName(string propertyName)
    {
        return propertyName switch
        {
            "new_origen" => "origen.json",
            "new_destino" => "destino.json",
            "new_cantequipo" => "cantidadEquipo.json",
            "new_tamaoequipo" => "tamanoEquipo.json",
            "new_incoterm" => "incoterm.json",
            "new_poe" => "poe.json",
            "new_pol" => "pol.json",
            "new_preestado2" => "status.json",
            "new_transporte" => "transporte.json",
            "new_ejecutivocomercial" => "ejecutivo.json",
            "new_tipoaforo" => "aforo.json",
            _ => ""
        };
    }
}
