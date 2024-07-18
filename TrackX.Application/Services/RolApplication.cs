using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Commons.Ordering;
using TrackX.Application.Dtos.Rol.Request;
using TrackX.Application.Dtos.Rol.Response;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.FileExcel;
using TrackX.Infrastructure.Persistences.Interfaces;
using TrackX.Utilities.Static;
using WatchDog;

namespace TrackX.Application.Services;

public class RolApplication : IRolApplication
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IOrderingQuery _orderingQuery;
    private readonly IImportExcel _importExcel;

    public RolApplication(IUnitOfWork unitOfWork, IMapper mapper, IOrderingQuery orderingQuery, IImportExcel importExcel)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _orderingQuery = orderingQuery;
        _importExcel = importExcel;
    }

    public async Task<BaseResponse<IEnumerable<RolResponseDto>>> ListRoles(BaseFiltersRequest filters)
    {
        var response = new BaseResponse<IEnumerable<RolResponseDto>>();
        try
        {
            var roles = _unitOfWork.Rol
                .GetAllQueryable()
                .AsQueryable();

            if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
            {
                switch (filters.NumFilter)
                {
                    case 1:
                        roles = roles.Where(x => x.Nombre!.Contains(filters.TextFilter));
                        break;
                }
            }

            if (filters.StateFilter is not null)
            {
                roles = roles.Where(x => x.Estado.Equals(filters.StateFilter));
            }

            if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
            {
                roles = roles.Where(x => x.FechaCreacionAuditoria >= Convert.ToDateTime(filters.StartDate)
                    && x.FechaCreacionAuditoria <= Convert.ToDateTime(filters.EndDate)
                    .AddDays(1));
            }

            filters.Sort ??= "Id";

            var items = await _orderingQuery
                .Ordering(filters, roles, !(bool)filters.Download!).ToListAsync();

            response.IsSuccess = true;
            response.TotalRecords = await roles.CountAsync();
            response.Data = _mapper.Map<IEnumerable<RolResponseDto>>(items);
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

    public async Task<BaseResponse<IEnumerable<RolSelectResponseDto>>> ListSelectRol()
    {
        var response = new BaseResponse<IEnumerable<RolSelectResponseDto>>();
        try
        {
            var roles = await _unitOfWork.Rol.GetAllAsync();

            if (roles is not null)
            {
                response.IsSuccess = true;
                response.Data = _mapper.Map<IEnumerable<RolSelectResponseDto>>(roles);
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

    public async Task<BaseResponse<RolResponseDto>> RolById(int id)
    {
        var response = new BaseResponse<RolResponseDto>();
        try
        {
            var rol = await _unitOfWork.Rol.GetByIdAsync(id);

            if (rol is not null)
            {
                response.IsSuccess = true;
                response.Data = _mapper.Map<RolResponseDto>(rol);
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

    public async Task<BaseResponse<bool>> RegisterRol(RolRequestDto requestDto)
    {
        var response = new BaseResponse<bool>();
        try
        {
            var rol = _mapper.Map<TbRol>(requestDto);
            response.Data = await _unitOfWork.Rol.RegisterAsync(rol);
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

    public async Task<BaseResponse<bool>> EditRol(int id, RolRequestDto requestDto)
    {
        var response = new BaseResponse<bool>();
        try
        {
            var rolEdit = await RolById(id);

            if (rolEdit.Data is null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            var rol = _mapper.Map<TbRol>(requestDto);
            rol.Id = id;
            response.Data = await _unitOfWork.Rol.EditAsync(rol);

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

    public async Task<BaseResponse<bool>> RemoveRol(int id)
    {
        var response = new BaseResponse<bool>();
        try
        {
            var rol = await RolById(id);

            if (rol.Data is null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            response.Data = await _unitOfWork.Rol.RemoveAsync(id);

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

    public async Task<BaseResponse<bool>> ImportExcelRol(ImportRequest request)
    {
        var response = new BaseResponse<bool>();
        try
        {
            using var stream = new MemoryStream();
            await request.excel!.CopyToAsync(stream);
            stream.Position = 0;

            var data = _importExcel.ImportFromExcel<TbRol>(stream);

            response.Data = await _unitOfWork.Rol.RegisterRangeAsync(data);
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