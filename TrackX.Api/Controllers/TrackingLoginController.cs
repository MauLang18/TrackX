﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using System.Threading.Tasks;
using TrackX.Application.Interfaces;
using TrackX.Utilities.Static;

namespace TrackX.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TrackingLoginController : ControllerBase
{
    private readonly ITrackingLoginApplication _trackingLoginApplication;
    private readonly IGenerateExcelApplication _generateExcelApplication;
    private readonly IOutputCacheStore _outputCacheStore;

    public TrackingLoginController(ITrackingLoginApplication trackingLoginApplication, IGenerateExcelApplication generateExcelApplication, IOutputCacheStore outputCacheStore)
    {
        _trackingLoginApplication = trackingLoginApplication;
        _generateExcelApplication = generateExcelApplication;
        _outputCacheStore = outputCacheStore;
    }

    [HttpGet("Activo")]
    public async Task<IActionResult> TrackingActivoByCliente(string cliente)
    {
        var response = await _trackingLoginApplication.TrackingActivoByCliente(cliente);

        return Ok(response);
    }

    [HttpGet("Finalizado")]
    public async Task<IActionResult> TrackingFinalizadoByCliente(string cliente)
    {
        var response = await _trackingLoginApplication.TrackingFinalizadoByCliente(cliente);

        return Ok(response);
    }

    [HttpGet("Historial")]
    public async Task<IActionResult> TrackingHistorialByCliente(string cliente)
    {
        var response = await _trackingLoginApplication.TrackingHistorialByCliente(cliente);

        return Ok(response);
    }

    [HttpGet("Activo/Download")]
    public async Task<IActionResult> DownloadActivo(string cliente)
    {
        var response = await _trackingLoginApplication.TrackingActivoByCliente(cliente);

        var columnNames = ExcelColumnNames.GetColumnsTramitesActivos();
        var fileBytes = _generateExcelApplication.GenerateToExcel(response.Data!.value!, columnNames);

        return File(fileBytes, ContentType.ContentTypeExcel);
    }
}