using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Commons.Ordering;
using TrackX.Application.Dtos.Finance.Request;
using TrackX.Application.Dtos.Finance.Response;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Persistences.Interfaces;
using TrackX.Utilities.Static;
using WatchDog;

namespace TrackX.Application.Services
{
    public class FinanceApplication : IFinanceApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOrderingQuery _orderingQuery;
        private readonly IFileStorageLocalApplication _fileStorage;
        private readonly IClienteApplication _clienteApplication;

        public FinanceApplication(IUnitOfWork unitOfWork, IMapper mapper, IOrderingQuery orderingQuery, IFileStorageLocalApplication fileStorage, IClienteApplication clienteApplication)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderingQuery = orderingQuery;
            _fileStorage = fileStorage;
            _clienteApplication = clienteApplication;
        }

        public async Task<BaseResponse<IEnumerable<FinanceResponseDto>>> ListFinance(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<IEnumerable<FinanceResponseDto>>();
            try
            {
                var Finance = _unitOfWork.Finance
                    .GetAllQueryable()
                    .AsQueryable();

                if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
                {
                    switch (filters.NumFilter)
                    {
                        case 1:
                            var resp = _clienteApplication.CodeCliente(filters.TextFilter!);

                            foreach (var datos in resp.Result.Data!.value!)
                            {
                                Finance = Finance.Where(x => x.Cliente!.Contains(datos.accountid!));
                            }
                            break;
                    }
                }

                if (filters.StateFilter is not null)
                {
                    Finance = Finance.Where(x => x.Estado.Equals(filters.StateFilter));
                }

                if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
                {
                    Finance = Finance.Where(x => x.FechaCreacionAuditoria >= Convert.ToDateTime(filters.StartDate)
                        && x.FechaCreacionAuditoria <= Convert.ToDateTime(filters.EndDate)
                        .AddDays(1));
                }

                filters.Sort ??= "Id";

                var items = await _orderingQuery
                    .Ordering(filters, Finance, !(bool)filters.Download!).ToListAsync();

                foreach (var item in items!)
                {
                    string shipperValue = item.Cliente!;

                    if (shipperValue is not null)
                    {
                        var nuevoValorCliente = await _clienteApplication.NombreCliente(shipperValue);

                        foreach (var datos in nuevoValorCliente.Data!.value!)
                        {
                            item.NombreCliente = datos.name;
                        }
                    }
                    else
                    {
                        item.NombreCliente = "";
                    }
                }

                response.IsSuccess = true;
                response.TotalRecords = await Finance.CountAsync();
                response.Data = _mapper.Map<IEnumerable<FinanceResponseDto>>(items);
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

        public async Task<BaseResponse<IEnumerable<FinanceResponseDto>>> ListFinanceCliente(BaseFiltersRequest filters, string cliente)
        {
            var response = new BaseResponse<IEnumerable<FinanceResponseDto>>();
            try
            {
                var Finance = _unitOfWork.Finance
                    .GetAllQueryable()
                    .AsQueryable();


                Finance = Finance.Where(x => x.Cliente!.Equals(cliente));

                if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
                {
                }

                if (filters.StateFilter is not null)
                {
                    Finance = Finance.Where(x => x.Estado.Equals(filters.StateFilter));
                }

                if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
                {
                    Finance = Finance.Where(x => x.FechaCreacionAuditoria >= Convert.ToDateTime(filters.StartDate)
                        && x.FechaCreacionAuditoria <= Convert.ToDateTime(filters.EndDate)
                        .AddDays(1));
                }

                filters.Sort ??= "Id";

                var items = await _orderingQuery
                    .Ordering(filters, Finance, !(bool)filters.Download!).ToListAsync();

                foreach (var item in items!)
                {
                    string shipperValue = item.Cliente!;

                    if (shipperValue is not null)
                    {
                        var nuevoValorCliente = await _clienteApplication.NombreCliente(shipperValue);

                        foreach (var datos in nuevoValorCliente.Data!.value!)
                        {
                            item.NombreCliente = datos.name;
                        }
                    }
                    else
                    {
                        item.NombreCliente = "";
                    }
                }

                response.IsSuccess = true;
                response.TotalRecords = await Finance.CountAsync();
                response.Data = _mapper.Map<IEnumerable<FinanceResponseDto>>(items);
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

        public async Task<BaseResponse<FinanceByIdResponeDto>> FinanceById(int id)
        {
            var response = new BaseResponse<FinanceByIdResponeDto>();
            try
            {
                var Finance = await _unitOfWork.Finance.GetByIdAsync(id);

                if (Finance is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<FinanceByIdResponeDto>(Finance);
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

        public async Task<BaseResponse<bool>> RegisterFinance(FinanceRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var Finance = _mapper.Map<TbFinance>(requestDto);

                if (requestDto.EstadoCuenta is not null)
                    Finance.EstadoCuenta = await _fileStorage.SaveFile(AzureContainers.FINANCES, requestDto.EstadoCuenta);

                response.Data = await _unitOfWork.Finance.RegisterAsync(Finance);
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

        public async Task<BaseResponse<bool>> EditFinance(int id, FinanceRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var FinanceEdit = await FinanceById(id);

                if (FinanceEdit.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                var Finance = _mapper.Map<TbFinance>(requestDto);
                Finance.Id = id;

                if (requestDto.EstadoCuenta is not null)
                    Finance.EstadoCuenta = await _fileStorage
                        .EditFile(AzureContainers.FINANCES, requestDto.EstadoCuenta, FinanceEdit.Data!.EstadoCuenta!);

                if (requestDto.EstadoCuenta is null)
                    Finance.EstadoCuenta = FinanceEdit.Data!.EstadoCuenta!;

                response.Data = await _unitOfWork.Finance.EditAsync(Finance);

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

        public async Task<BaseResponse<bool>> RemoveFinance(int id)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var Finance = await FinanceById(id);

                if (Finance.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                response.Data = await _unitOfWork.Finance.RemoveAsync(id);

                await _fileStorage.RemoveFile(Finance.Data!.EstadoCuenta!, AzureContainers.FINANCES);

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
    }
}