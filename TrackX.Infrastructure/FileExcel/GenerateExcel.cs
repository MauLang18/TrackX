using ClosedXML.Excel;
using Newtonsoft.Json;
using TrackX.Domain.Entities;
using TrackX.Utilities.Static;

namespace TrackX.Infrastructure.FileExcel
{
    public class GenerateExcel : IGenerateExcel
    {
        // Path relativo a la carpeta de trabajo donde se encuentran los archivos JSON
        private readonly string jsonFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Static", "JSON");

        public MemoryStream GenerateToExcel(List<Value2> data, List<TableColumns> columns)
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

        private string GetPropertyValue(Value2 item, TableColumns column)
        {
            string propertyName = column.PropertyName!;

            if (propertyName == "new_origen")
            {
                string jsonFilePath = Path.Combine(jsonFolderPath, "origen.json");
                if (File.Exists(jsonFilePath))
                {
                    var valor = typeof(Value2).GetProperty(propertyName!)?.GetValue(item)?.ToString();
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
            else if (propertyName == "new_destino")
            {
                string jsonFilePath = Path.Combine(jsonFolderPath, "destino.json");
                if (File.Exists(jsonFilePath))
                {
                    var valor = typeof(Value2).GetProperty(propertyName!)?.GetValue(item)?.ToString();
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
            else if (propertyName == "new_cantequipo")
            {
                string jsonFilePath = Path.Combine(jsonFolderPath, "cantidadEquipo.json");
                if (File.Exists(jsonFilePath))
                {
                    var valor = typeof(Value2).GetProperty(propertyName!)?.GetValue(item)?.ToString();
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
            else if (propertyName == "new_tamaoequipo")
            {
                string jsonFilePath = Path.Combine(jsonFolderPath, "tamanoEquipo.json");
                if (File.Exists(jsonFilePath))
                {
                    var valor = typeof(Value2).GetProperty(propertyName!)?.GetValue(item)?.ToString();
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
            else if (propertyName == "new_incoterm")
            {
                string jsonFilePath = Path.Combine(jsonFolderPath, "incoterm.json");
                if (File.Exists(jsonFilePath))
                {
                    var valor = typeof(Value2).GetProperty(propertyName!)?.GetValue(item)?.ToString();
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
            else if (propertyName == "new_poe")
            {
                string jsonFilePath = Path.Combine(jsonFolderPath, "poe.json");
                if (File.Exists(jsonFilePath))
                {
                    var valor = typeof(Value2).GetProperty(propertyName!)?.GetValue(item)?.ToString();
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
            else if (propertyName == "new_pol")
            {
                string jsonFilePath = Path.Combine(jsonFolderPath, "pol.json");
                if (File.Exists(jsonFilePath))
                {
                    var valor = typeof(Value2).GetProperty(propertyName!)?.GetValue(item)?.ToString();
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
            else if (propertyName == "new_preestado2")
            {
                string jsonFilePath = Path.Combine(jsonFolderPath, "status.json");
                if (File.Exists(jsonFilePath))
                {
                    var valor = typeof(Value2).GetProperty(propertyName!)?.GetValue(item)?.ToString();
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
            else if (propertyName == "new_transporte")
            {
                string jsonFilePath = Path.Combine(jsonFolderPath, "transporte.json");
                if (File.Exists(jsonFilePath))
                {
                    var valor = typeof(Value2).GetProperty(propertyName!)?.GetValue(item)?.ToString();
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

            var propertyValue = typeof(Value2).GetProperty(propertyName!)?.GetValue(item)?.ToString();
            return propertyValue ?? "";
        }
    }
}
