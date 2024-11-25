using QuestPDF.Fluent;
using QuestPDF.Helpers;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.FilePdf;
using System.IO;
using System.Linq;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;

public class GeneratePdfService : IGeneratePdfService
{
    // Ruta del folder donde se encuentran los archivos JSON
    private readonly string jsonFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Static", "JSON");

    public MemoryStream GeneratePdf(Dynamics<DynamicsTransInternacional> jsonResponse)
    {
        if (jsonResponse == null || jsonResponse.value == null || !jsonResponse.value.Any())
            throw new ArgumentNullException("No hay datos disponibles para generar el PDF.");

        var record = jsonResponse.value.FirstOrDefault();

        if (record == null)
            throw new ArgumentNullException("No se encontró un registro válido.");

        using var stream = new MemoryStream();
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A4);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10));

                // Encabezado
                page.Header().AlignCenter().Text("SISTEMA SINCRONIZADO DE ADUANAS")
                    .SemiBold()
                    .FontSize(18)
                    .FontColor(Colors.Blue.Darken2);

                // Contenido principal
                page.Content().PaddingVertical(20).Column(column =>
                {
                    // Cliente, Status y Ejecutivo
                    column.Item().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("Cliente:").Bold();
                            col.Item().Text(record._customerid_value ?? "N/A");
                        });
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("Status:").Bold();
                            col.Item().Text(MapJsonField(record.new_preestado2?.ToString() ?? "N/A", "status.json"));
                        });
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("Ejecutivo Asignado:").Bold();
                            col.Item().Text(MapJsonField(record.new_ejecutivocomercial?.ToString() ?? "N/A", "ejecutivo.json"));
                        });
                    });

                    // IDTRA y detalles generales
                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Black).Padding(5).AlignCenter().Text("IDTRA").Bold().FontColor(Colors.White);
                            header.Cell().Background(Colors.Black).Padding(5).AlignCenter().Text("# RECIBO").Bold().FontColor(Colors.White);
                            header.Cell().Background(Colors.Black).Padding(5).AlignCenter().Text("NOMBRE PEDIMENTADOR").Bold().FontColor(Colors.White);
                        });

                        table.Cell().Border(1).Padding(5).Text(record.title ?? "N/A");
                        table.Cell().Border(1).Padding(5).Text(record.new_numerorecibo ?? "N/A");
                        table.Cell().Border(1).Padding(5).Text(record.new_nombrepedimentador ?? "N/A");
                    });

                    // Detalles de la carga (2 tablas juntas)
                    column.Item().Text("Detalles de la Carga").Bold();
                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Black).Padding(5).AlignCenter().Text("Factura Comercial").Bold().FontColor(Colors.White);
                            header.Cell().Background(Colors.Black).Padding(5).AlignCenter().Text("Commodity").Bold().FontColor(Colors.White);
                            header.Cell().Background(Colors.Black).Padding(5).AlignCenter().Text("Cantidad de Bulto").Bold().FontColor(Colors.White);
                        });

                        table.Cell().Border(1).Padding(5).Text(record.new_factura ?? "N/A");
                        table.Cell().Border(1).Padding(5).Text(record.new_commodity ?? "N/A");
                        table.Cell().Border(1).Padding(5).Text(record.new_contidadbultos ?? "N/A");
                    });

                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Black).Padding(5).AlignCenter().Text("Peso").Bold().FontColor(Colors.White);
                            header.Cell().Background(Colors.Black).Padding(5).AlignCenter().Text("Cantidad y Tamaño").Bold().FontColor(Colors.White);
                            header.Cell().Background(Colors.Black).Padding(5).AlignCenter().Text("PO").Bold().FontColor(Colors.White);
                        });

                        table.Cell().Border(1).Padding(5).Text(record.new_peso ?? "N/A");
                        table.Cell().Border(1).Padding(5).Text($"{MapJsonField(record.new_cantequipo.ToString() ?? "N/A", "cantidadEquipo.json")} x {MapJsonField(record.new_tamaoequipo.ToString() ?? "N/A", "tamanoEquipo.json")}" ?? "N/A");
                        table.Cell().Border(1).Padding(5).Text(record.new_po ?? "N/A");
                    });

                    // Detalles del transporte (2 tablas juntas)
                    column.Item().Text("Detalles del Transporte").Bold();
                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Black).Padding(5).AlignCenter().Text("BL").Bold().FontColor(Colors.White);
                            header.Cell().Background(Colors.Black).Padding(5).AlignCenter().Text("# Contenedor").Bold().FontColor(Colors.White);
                            header.Cell().Background(Colors.Black).Padding(5).AlignCenter().Text("Confirmación Zarpe").Bold().FontColor(Colors.White);
                        });

                        table.Cell().Border(1).Padding(5).Text(record.new_bcf ?? "N/A");
                        table.Cell().Border(1).Padding(5).Text(record.new_contenedor ?? "N/A");
                        table.Cell().Border(1).Padding(5).Text(record.new_confirmacionzarpe?.ToString() ?? "N/A");
                    });

                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Black).Padding(5).AlignCenter().Text("POL").Bold().FontColor(Colors.White);
                            header.Cell().Background(Colors.Black).Padding(5).AlignCenter().Text("POE").Bold().FontColor(Colors.White);
                            header.Cell().Background(Colors.Black).Padding(5).AlignCenter().Text("ETA").Bold().FontColor(Colors.White);
                        });

                        table.Cell().Border(1).Padding(5).Text(MapJsonField(record.new_pol?.ToString() ?? "N/A", "pol.json") ?? "N/A");
                        table.Cell().Border(1).Padding(5).Text(MapJsonField(record.new_poe?.ToString() ?? "N/A", "poe.json") ?? "N/A");
                        table.Cell().Border(1).Padding(5).Text(record.new_eta?.ToString() ?? "N/A");
                    });

                    // Confirmación y Liberación
                    column.Item().Text("Confirmación y Liberación").Bold();
                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Black).Padding(5).AlignCenter().Text("Liberación Documental").Bold().FontColor(Colors.White);
                            header.Cell().Background(Colors.Black).Padding(5).AlignCenter().Text("Liberación Financiera").Bold().FontColor(Colors.White);
                        });

                        table.Cell().Border(1).Padding(5).Text(record.new_liberacionmovimientoinventario?.ToString() ?? "N/A");
                        table.Cell().Border(1).Padding(5).Text(record.new_fechaliberacionfinanciera?.ToString() ?? "N/A");
                    });

                    // Status Aduanas
                    column.Item().Text("Status Aduanas").Bold();
                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Black).Padding(5).AlignCenter().Text("Tipo Aforo").Bold().FontColor(Colors.White);
                            header.Cell().Background(Colors.Black).Padding(5).AlignCenter().Text("# DUA Anticipado").Bold().FontColor(Colors.White);
                            header.Cell().Background(Colors.Black).Padding(5).AlignCenter().Text("# DUA Nacional").Bold().FontColor(Colors.White);
                        });

                        table.Cell().Border(1).Padding(5).Text(MapJsonField(record.new_tipoaforo?.ToString() ?? "N/A", "aforo.json") ?? "N/A");
                        table.Cell().Border(1).Padding(5).Text(record.new_duaanticipados ?? "N/A");
                        table.Cell().Border(1).Padding(5).Text(record.new_duanacional ?? "N/A");
                    });

                    // 22 campos de texto adicionales con formato adecuado
                    column.Item().Text($"Borrador de Impuestos: {record.new_borradordeimpuestos ?? "N/A"}");
                    column.Item().Text($"Documentación Nacional: {record.new_documentodenacionalizacion ?? "N/A"}");

                    column.Item().Text("Documentación de la carga").Bold();
                    column.Item().Text($"Factura Comercial: {record.new_facturacomercial ?? "N/A"}");
                    column.Item().Text($"Lista de Empaque: {record.new_listadeempaque ?? "N/A"}");
                    column.Item().Text($"Fecha Entrega Traducción: {record.new_entregatraduccion?.ToString() ?? "N/A"}");
                    column.Item().Text($"Traducción de Factura: {record.new_traducciondefacturas ?? "N/A"}");
                    column.Item().Text($"Permisos: {record.new_permisos ?? "N/A"}");
                    column.Item().Text($"Exoneración: {CreateYesNo(record.new_llevaexoneracion)}");
                    column.Item().Text($"Exoneración: {record.new_exoneracion ?? "N/A"}");

                    column.Item().Text("Documentación Conocimiento Embarque").Bold();
                    column.Item().Text($"Draft BL: {record.new_draftbl ?? "N/A"}");
                    column.Item().Text($"Fecha BL Digitado: {record.new_fechabldigittica?.ToString() ?? "N/A"}");
                    column.Item().Text($"Entrega BL Original: {CreateYesNo(record.new_entregabloriginal) ?? "N/A"}");
                    column.Item().Text($"Fecha BL Impreso: {record.new_fechablimpreso?.ToString() ?? "N/A"}");
                    column.Item().Text($"BL Original: {record.new_bloriginal ?? "N/A"}");
                    column.Item().Text($"Entrega Carta Trazabilidad: {CreateYesNo(record.new_entregacartatrazabilidad) ?? "N/A"}");
                    column.Item().Text($"Carta Trazabilidad: {record.new_cartatrazabilidad ?? "N/A"}");
                    column.Item().Text($"Desglose de Cargos: {record.new_cartadesglosecargos ?? "N/A"}");

                    // Preferencia Arancelaria
                    column.Item().Text("Preferencia Arancelaria").Bold();
                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Black).Padding(5).AlignCenter().Text("Certificado Origen (Si/No)").Bold().FontColor(Colors.White);
                            header.Cell().Background(Colors.Black).Padding(5).AlignCenter().Text("Cert. Re-exportación (Si/No)").Bold().FontColor(Colors.White);
                        });

                        table.Cell().Border(1).Padding(5).Text(CreateYesNo(record.new_aplicacertificadodeorigen));
                        table.Cell().Border(1).Padding(5).Text(CreateYesNo(record.new_aplicacertificadoreexportacion));
                    });

                    column.Item().Text($"Borrador Certificado Origen: {record.new_borradordecertificadodeorigen ?? "N/A"}");
                    column.Item().Text($"Certificado Origen: {record.new_certificadoorigen ?? "N/A"}");
                    column.Item().Text($"Certificado Re-Exportación: {record.new_certificadoreexportacion ?? "N/A"}");
                });

                // Footer
                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Generado el: ");
                    x.Span(DateTime.Now.ToString("dd/MM/yyyy")).SemiBold();
                });
            });
        });

        document.GeneratePdf(stream);
        return stream;
    }

    // Métodos adicionales para manejar los tipos de datos específicos
    private string CreateYesNo(object? value)
    {
        if (value is bool booleanValue)
        {
            // If it's a boolean, return "Sí" for true and "No" for false
            return booleanValue ? "Sí" : "No";
        }

        // If it's not a boolean, check if it has a value (non-null and non-empty string)
        return value != null && !string.IsNullOrEmpty(value.ToString()) ? "Sí" : "No";
    }

    private string MapJsonField(string value, string jsonFileName)
    {
        if (string.IsNullOrEmpty(value))
            return "N/A";

        string filePath = Path.Combine(jsonFolderPath, jsonFileName);
        if (!File.Exists(filePath))
            return value;

        var json = File.ReadAllText(filePath);
        var jsonData = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

        if (jsonData != null && jsonData.ContainsKey(value))
        {
            return jsonData[value] ?? "N/A";
        }

        return value;
    }
}
