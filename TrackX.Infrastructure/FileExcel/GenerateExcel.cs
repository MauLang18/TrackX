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
                var valor = item.GetType().GetProperty(propertyName)?.GetValue(item)?.ToString();
                string jsonContent = File.ReadAllText(jsonFilePath);
                Dictionary<string, string> jsonValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonContent)!;
                if (jsonValues.ContainsKey(valor!))
                {
                    return jsonValues[valor!];
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

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
            _ => ""
        };
    }
}
