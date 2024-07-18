using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Commons.Ordering;
using TrackX.Application.Dtos.Noticia.Request;
using TrackX.Application.Dtos.Noticia.Response;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.FileExcel;
using TrackX.Infrastructure.Persistences.Interfaces;
using TrackX.Utilities.Static;
using WatchDog;

namespace TrackX.Application.Services;

public class NoticiaApplication : INoticiaApplication
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFileStorageLocalApplication _fileStorageLocalApplication;
    private readonly IOrderingQuery _orderingQuery;
    private readonly IImportExcel _importExcel;

    public NoticiaApplication(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageLocalApplication fileStorageLocalApplication, IOrderingQuery orderingQuery, IImportExcel importExcel)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileStorageLocalApplication = fileStorageLocalApplication;
        _orderingQuery = orderingQuery;
        _importExcel = importExcel;
    }

    public async Task<BaseResponse<IEnumerable<NoticiaResponseDto>>> ListNoticias(BaseFiltersRequest filters)
    {
        var response = new BaseResponse<IEnumerable<NoticiaResponseDto>>();
        try
        {
            var noticias = _unitOfWork.Noticia
                .GetAllQueryable()
                .AsQueryable();

            if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
            {
                switch (filters.NumFilter)
                {
                    case 1:
                        noticias = noticias.Where(x => x.Titulo!.Contains(filters.TextFilter));
                        break;
                }
            }

            if (filters.StateFilter is not null)
            {
                noticias = noticias.Where(x => x.Estado.Equals(filters.StateFilter));
            }

            if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
            {
                noticias = noticias.Where(x => x.FechaCreacionAuditoria >= Convert.ToDateTime(filters.StartDate)
                    && x.FechaCreacionAuditoria <= Convert.ToDateTime(filters.EndDate)
                    .AddDays(1));
            }

            filters.Sort ??= "Id";

            var items = await _orderingQuery
                .Ordering(filters, noticias, !(bool)filters.Download!).ToListAsync();

            response.IsSuccess = true;
            response.TotalRecords = await noticias.CountAsync();
            response.Data = _mapper.Map<IEnumerable<NoticiaResponseDto>>(items);
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

    public async Task<BaseResponse<NoticiaByIdResponseDto>> NoticiaById(int id)
    {
        var response = new BaseResponse<NoticiaByIdResponseDto>();

        try
        {
            var noticia = await _unitOfWork.Noticia.GetByIdAsync(id);

            if (noticia is not null)
            {
                response.IsSuccess = true;
                response.Data = _mapper.Map<NoticiaByIdResponseDto>(noticia);
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

    public async Task<BaseResponse<bool>> RegisterNoticia(NoticiaRequestDto requestDto)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var noticia = _mapper.Map<TbNoticia>(requestDto);

            if (requestDto.Imagen is not null)
                noticia.Imagen = await _fileStorageLocalApplication.SaveFile(AzureContainers.NOTICIAS, requestDto.Imagen);

            response.Data = await _unitOfWork.Noticia.RegisterAsync(noticia);

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

    public async Task<BaseResponse<bool>> EditNoticia(int id, NoticiaRequestDto requestDto)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var noticiaEdit = await NoticiaById(id);

            if (noticiaEdit.Data is null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            var noticia = _mapper.Map<TbNoticia>(requestDto);
            noticia.Id = id;

            if (requestDto.Imagen is not null)
                noticia.Imagen = await _fileStorageLocalApplication
                    .EditFile(AzureContainers.NOTICIAS, requestDto.Imagen, noticiaEdit.Data!.Imagen!);

            if (requestDto.Imagen is null)
                noticia.Imagen = noticiaEdit.Data!.Imagen!;

            response.Data = await _unitOfWork.Noticia.EditAsync(noticia);

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

    public async Task<BaseResponse<bool>> RemoveNoticia(int id)
    {
        var response = new BaseResponse<bool>();
        try
        {
            var noticia = await NoticiaById(id);

            if (noticia.Data is null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            response.Data = await _unitOfWork.Noticia.RemoveAsync(id);

            await _fileStorageLocalApplication.RemoveFile(noticia.Data!.Imagen!, AzureContainers.NOTICIAS);

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

    public async Task<BaseResponse<bool>> ImportExcelNoticia(ImportRequest request)
    {
        var response = new BaseResponse<bool>();
        try
        {
            using var stream = new MemoryStream();
            await request.excel!.CopyToAsync(stream);
            stream.Position = 0;

            var data = _importExcel.ImportFromExcel<TbNoticia>(stream);

            response.Data = await _unitOfWork.Noticia.RegisterRangeAsync(data);
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