using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Commons.Ordering;
using TrackX.Application.Dtos.Whs.Request;
using TrackX.Application.Dtos.Whs.Response;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.FileExcel;
using TrackX.Infrastructure.Persistences.Interfaces;
using TrackX.Utilities.Static;
using WatchDog;

namespace TrackX.Application.Services;

public class WhsApplication : IWhsApplication
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IOrderingQuery _orderingQuery;
    private readonly IFileStorageLocalApplication _fileStorage;
    private readonly IClienteApplication _clienteApplication;
    private readonly IImportExcel _importExcel;

    public WhsApplication(IUnitOfWork unitOfWork, IMapper mapper, IOrderingQuery orderingQuery, IFileStorageLocalApplication fileStorage, IClienteApplication clienteApplication, IImportExcel importExcel)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _orderingQuery = orderingQuery;
        _fileStorage = fileStorage;
        _clienteApplication = clienteApplication;
        _importExcel = importExcel;
    }

    public async Task<BaseResponse<IEnumerable<WhsResponseDto>>> ListWhs(BaseFiltersRequest filters, string whs)
    {
        var response = new BaseResponse<IEnumerable<WhsResponseDto>>();
        try
        {
            var WHS = _unitOfWork.Whs
                .GetAllQueryable()
                .AsQueryable();

            WHS = WHS.Where(x => x.POL!.Contains(whs));

            if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
            {
                switch (filters.NumFilter)
                {
                    case 1:
                        WHS = WHS.Where(x => x.StatusWHS!.Contains(filters.TextFilter));
                        break;
                    case 2:
                        WHS = WHS.Where(x => x.POL!.Contains(filters.TextFilter));
                        break;
                    case 3:
                        WHS = WHS.Where(x => x.POD!.Contains(filters.TextFilter));
                        break;
                    case 4:
                        WHS = WHS.Where(x => x.Idtra!.Contains(filters.TextFilter));
                        break;
                    case 5:
                        WHS = WHS.Where(x => x.PO!.Contains(filters.TextFilter));
                        break;
                    case 6:
                        WHS = WHS.Where(x => x.TipoRegistro!.Contains(filters.TextFilter));
                        break;
                    case 7:
                        WHS = WHS.Where(x => x.NombreCliente!.Contains(filters.TextFilter));
                        break;
                }
            }

            if (filters.StateFilter is not null)
            {
                WHS = WHS.Where(x => x.Estado.Equals(filters.StateFilter));
            }

            if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
            {
                WHS = WHS.Where(x => x.FechaCreacionAuditoria >= Convert.ToDateTime(filters.StartDate)
                    && x.FechaCreacionAuditoria <= Convert.ToDateTime(filters.EndDate)
                    .AddDays(1));
            }

            filters.Sort ??= "Id";

            var items = await _orderingQuery
                .Ordering(filters, WHS, !(bool)filters.Download!).ToListAsync();

            response.IsSuccess = true;
            response.TotalRecords = await WHS.CountAsync();
            response.Data = _mapper.Map<IEnumerable<WhsResponseDto>>(items);
            response.Message = ReplyMessage.MESSAGE_QUERY;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ReplyMessage.MESSAGE_EXCEPTION;
            WatchLogger.Log(ex.Message);
        }

        return response;
    }

    public async Task<BaseResponse<IEnumerable<WhsResponseDto>>> ListWhsCliente(BaseFiltersRequest filters, string cliente, string whs)
    {
        var response = new BaseResponse<IEnumerable<WhsResponseDto>>();
        try
        {
            var WHS = _unitOfWork.Whs
                .GetAllQueryable()
                .AsQueryable();


            WHS = WHS.Where(x => x.POL!.Contains(whs) && x.Cliente!.Equals(cliente));

            if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
            {
                switch (filters.NumFilter)
                {
                    case 1:
                        WHS = WHS.Where(x => x.StatusWHS!.Contains(filters.TextFilter));
                        break;
                }
            }

            if (filters.StateFilter is not null)
            {
                WHS = WHS.Where(x => x.Estado.Equals(filters.StateFilter));
            }

            if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
            {
                WHS = WHS.Where(x => x.FechaCreacionAuditoria >= Convert.ToDateTime(filters.StartDate)
                    && x.FechaCreacionAuditoria <= Convert.ToDateTime(filters.EndDate)
                    .AddDays(1));
            }

            filters.Sort ??= "Id";

            var items = await _orderingQuery
                .Ordering(filters, WHS, !(bool)filters.Download!).ToListAsync();

            response.IsSuccess = true;
            response.TotalRecords = await WHS.CountAsync();
            response.Data = _mapper.Map<IEnumerable<WhsResponseDto>>(items);
            response.Message = ReplyMessage.MESSAGE_QUERY;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ReplyMessage.MESSAGE_EXCEPTION;
            WatchLogger.Log(ex.Message);
        }

        return response;
    }

    public async Task<BaseResponse<WhsResponseByIdDto>> WhsById(int id)
    {
        var response = new BaseResponse<WhsResponseByIdDto>();
        try
        {
            var Whs = await _unitOfWork.Whs.GetByIdAsync(id);

            if (Whs is not null)
            {
                response.IsSuccess = true;
                response.Data = _mapper.Map<WhsResponseByIdDto>(Whs);
                response.Message = ReplyMessage.MESSAGE_QUERY;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ReplyMessage.MESSAGE_EXCEPTION;
            WatchLogger.Log(ex.Message);
        }

        return response;
    }

    public async Task<BaseResponse<bool>> RegisterWhs(WhsRequestDto requestDto)
    {
        var response = new BaseResponse<bool>();
        try
        {
            var Whs = _mapper.Map<TbWhs>(requestDto);

            if (requestDto.WHSReceipt is not null)
                Whs.WHSReceipt = await _fileStorage.SaveFile(AzureContainers.WHS, requestDto.WHSReceipt);

            if (requestDto.Documentoregistro is not null)
                Whs.Documentoregistro = await _fileStorage.SaveFile(AzureContainers.WHS, requestDto.Documentoregistro);

            if (requestDto.Imagen1 is not null)
                Whs.Imagen1 = await _fileStorage.SaveFile(AzureContainers.WHS, requestDto.Imagen1);

            if (requestDto.Imagen2 is not null)
                Whs.Imagen2 = await _fileStorage.SaveFile(AzureContainers.WHS, requestDto.Imagen2);

            if (requestDto.Imagen3 is not null)
                Whs.Imagen3 = await _fileStorage.SaveFile(AzureContainers.WHS, requestDto.Imagen3);

            if (requestDto.Imagen4 is not null)
                Whs.Imagen4 = await _fileStorage.SaveFile(AzureContainers.WHS, requestDto.Imagen4);

            if (requestDto.Imagen5 is not null)
                Whs.Imagen5 = await _fileStorage.SaveFile(AzureContainers.WHS, requestDto.Imagen5);

            var shipperValuesList = new List<string> { requestDto.Cliente! };
            var nuevoValorCliente = await _clienteApplication.NombreCliente(shipperValuesList);

            foreach (var datos in nuevoValorCliente.Data!.value!)
            {
                Whs.NombreCliente = datos.name;
            }

            response.Data = await _unitOfWork.Whs.RegisterAsync(Whs);
            if (response.Data)
            {
                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_SAVE;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_FAILED;
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ReplyMessage.MESSAGE_EXCEPTION;
            WatchLogger.Log(ex.Message);
        }

        return response;
    }

    public async Task<BaseResponse<bool>> EditWhs(int id, WhsRequestDto requestDto)
    {
        var response = new BaseResponse<bool>();
        try
        {
            var WhsEdit = await WhsById(id);

            if (WhsEdit.Data is null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            var Whs = _mapper.Map<TbWhs>(requestDto);
            Whs.Id = id;

            if (requestDto.WHSReceipt is not null)
                Whs.WHSReceipt = await _fileStorage
                    .EditFile(AzureContainers.WHS, requestDto.WHSReceipt, WhsEdit.Data!.WHSReceipt!);

            if (requestDto.WHSReceipt is null)
                Whs.WHSReceipt = WhsEdit.Data!.WHSReceipt!;

            if (requestDto.Documentoregistro is not null)
                Whs.Documentoregistro = await _fileStorage
                    .EditFile(AzureContainers.WHS, requestDto.Documentoregistro, WhsEdit.Data!.Documentoregistro!);

            if (requestDto.Documentoregistro is null)
                Whs.Documentoregistro = WhsEdit.Data!.Documentoregistro!;

            if (requestDto.Imagen1 is not null)
                Whs.Imagen1 = await _fileStorage
                    .EditFile(AzureContainers.WHS, requestDto.Imagen1, WhsEdit.Data!.Imagen1!);

            if (requestDto.Imagen1 is null)
                Whs.Imagen1 = WhsEdit.Data!.Imagen1!;

            if (requestDto.Imagen2 is not null)
                Whs.Imagen2 = await _fileStorage
                    .EditFile(AzureContainers.WHS, requestDto.Imagen2, WhsEdit.Data!.Imagen2!);

            if (requestDto.Imagen2 is null)
                Whs.Imagen2 = WhsEdit.Data!.Imagen2!;

            if (requestDto.Imagen3 is not null)
                Whs.Imagen3 = await _fileStorage
                    .EditFile(AzureContainers.WHS, requestDto.Imagen3, WhsEdit.Data!.Imagen3!);

            if (requestDto.Imagen3 is null)
                Whs.Imagen3 = WhsEdit.Data!.Imagen3!;

            if (requestDto.Imagen4 is not null)
                Whs.Imagen4 = await _fileStorage
                    .EditFile(AzureContainers.WHS, requestDto.Imagen4, WhsEdit.Data!.Imagen4!);

            if (requestDto.Imagen4 is null)
                Whs.Imagen4 = WhsEdit.Data!.Imagen4!;

            if (requestDto.Imagen5 is not null)
                Whs.Imagen5 = await _fileStorage
                    .EditFile(AzureContainers.WHS, requestDto.Imagen5, WhsEdit.Data!.Imagen5!);

            if (requestDto.Imagen5 is null)
                Whs.Imagen5 = WhsEdit.Data!.Imagen5!;

            var shipperValuesList = new List<string> { requestDto.Cliente! };
            var nuevoValorCliente = await _clienteApplication.NombreCliente(shipperValuesList);

            foreach (var datos in nuevoValorCliente.Data!.value!)
            {
                Whs.NombreCliente = datos.name;
            }

            response.Data = await _unitOfWork.Whs.EditAsync(Whs);

            if (response.Data)
            {
                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_UPDATE;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_FAILED;
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ReplyMessage.MESSAGE_EXCEPTION;
            WatchLogger.Log(ex.Message);
        }

        return response;
    }

    public async Task<BaseResponse<bool>> RemoveWhs(int id)
    {
        var response = new BaseResponse<bool>();
        try
        {
            var Whs = await WhsById(id);

            if (Whs.Data is null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            response.Data = await _unitOfWork.Whs.RemoveAsync(id);

            await _fileStorage.RemoveFile(Whs.Data!.WHSReceipt!, AzureContainers.WHS);
            await _fileStorage.RemoveFile(Whs.Data!.Documentoregistro!, AzureContainers.WHS);
            await _fileStorage.RemoveFile(Whs.Data!.Imagen1!, AzureContainers.WHS);
            await _fileStorage.RemoveFile(Whs.Data!.Imagen2!, AzureContainers.WHS);
            await _fileStorage.RemoveFile(Whs.Data!.Imagen3!, AzureContainers.WHS);
            await _fileStorage.RemoveFile(Whs.Data!.Imagen4!, AzureContainers.WHS);
            await _fileStorage.RemoveFile(Whs.Data!.Imagen5!, AzureContainers.WHS);

            if (response.Data)
            {
                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_DELETE;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_FAILED;
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ReplyMessage.MESSAGE_EXCEPTION;
            WatchLogger.Log(ex.Message);
        }

        return response;
    }

    public async Task<BaseResponse<bool>> ImportExcelWhs(ImportRequest request, string whs)
    {
        var response = new BaseResponse<bool>();
        try
        {
            using var stream = new MemoryStream();
            await request.excel!.CopyToAsync(stream);
            stream.Position = 0;

            var data = _importExcel.ImportFromExcel<TbWhs>(stream);

            foreach (var items in data)
            {
                items.POL = whs;
            }

            response.Data = await _unitOfWork.Whs.RegisterRangeAsync(data);
            if (response.Data)
            {
                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_SAVE;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_FAILED;
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ReplyMessage.MESSAGE_EXCEPTION;
            WatchLogger.Log(ex.Message);
        }

        return response;
    }
}