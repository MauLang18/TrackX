﻿using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Utilities.Static;
using WatchDog;

namespace TrackX.Application.Services
{
    public class TrackingLoginApplication : ITrackingLoginApplication
    {
        private readonly IClienteApplication _clienteApplication;

        public TrackingLoginApplication(IClienteApplication clienteApplication)
        {
            _clienteApplication = clienteApplication;
        }

        [Obsolete]
        public async Task<BaseResponse<DynamicsTrackingLogin>> TrackingFinalizadoByCliente(string cliente)
        {
            var response = new BaseResponse<DynamicsTrackingLogin>();

            try
            {
                string clientId = "04f616d1-fb10-4c4f-ba02-45d2562fa9a8";
                string clientSecrets = "1cn8Q~reOm4kQQ5fuaMUbR_X.cmtbQwyxv22IaVH";
                string authority = "https://login.microsoftonline.com/48f7ad87-a406-4c72-98f5-d1c996e7e6f2";
                string crmUrl = "https://sibaja07.crm.dynamics.com/";

                string accessToken = string.Empty;

                ClientCredential credentials = new ClientCredential(clientId, clientSecrets);
                var authContext = new AuthenticationContext(authority);
                var result = await authContext.AcquireTokenAsync(crmUrl, credentials);
                accessToken = result.AccessToken;

                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(crmUrl);
                    httpClient.Timeout = new TimeSpan(0, 2, 0);
                    httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                    httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    string entityName = "incidents";

                    HttpResponseMessage httpResponseMessaje = await httpClient.GetAsync($"api/data/v9.2/{entityName}?$select=new_contenedor,new_factura,new_bcf,new_cantequipo,new_commodity,new_confirmacinzarpe,new_contidadbultos,new_destino,new_eta,new_etd1,modifiedon,new_incoterm,new_origen,new_po,new_poe,new_pol,new_preestado2,new_seal,_new_shipper_value,new_statuscliente,new_tamaoequipo,new_new_facturacompaia,new_transporte,title,new_lugarcolocacion,new_redestino,new_diasdetransito,new_barcodesalida,new_viajedesalida,_customerid_value,new_ofertatarifaid,new_montocostoestimado&$filter=((_customerid_value eq {cliente}) and (new_preestado2 eq 100000023 or new_preestado2 eq 100000022 or new_preestado2 eq 100000021 or new_preestado2 eq 100000019 or new_preestado2 eq 100000012 or new_preestado2 eq 100000008 or new_preestado2 eq 100000009 or new_preestado2 eq 100000010 or new_preestado2 eq 100000011))&$orderby=title desc");
                    httpResponseMessaje.EnsureSuccessStatusCode();

                    if (httpResponseMessaje.IsSuccessStatusCode)
                    {
                        string jsonResponse = await httpResponseMessaje.Content.ReadAsStringAsync();

                        DynamicsTrackingLogin dynamicsObject = JsonConvert.DeserializeObject<DynamicsTrackingLogin>(jsonResponse)!;

                        foreach (var item in dynamicsObject.value!)
                        {
                            string shipperValue = item._new_shipper_value!;

                            var nuevoValorCliente = _clienteApplication.NombreCliente(shipperValue);

                            foreach(var items in nuevoValorCliente.Result.Data!.value!)
                            {
                                item._new_shipper_value = items.name;
                            }
                        }

                        response.IsSuccess = true;
                        response.Data = dynamicsObject;
                        response.Message = ReplyMessage.MESSAGE_QUERY;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    }

                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                WatchLogger.Log(ex.Message);
            }

            return response;
        }

        [Obsolete]
        public async Task<BaseResponse<DynamicsTrackingLogin>> TrackingActivoByCliente(string cliente)
        {
            var response = new BaseResponse<DynamicsTrackingLogin>();

            try
            {
                string clientId = "04f616d1-fb10-4c4f-ba02-45d2562fa9a8";
                string clientSecrets = "1cn8Q~reOm4kQQ5fuaMUbR_X.cmtbQwyxv22IaVH";
                string authority = "https://login.microsoftonline.com/48f7ad87-a406-4c72-98f5-d1c996e7e6f2";
                string crmUrl = "https://sibaja07.crm.dynamics.com/";

                string accessToken = string.Empty;

                ClientCredential credentials = new ClientCredential(clientId, clientSecrets);
                var authContext = new AuthenticationContext(authority);
                var result = await authContext.AcquireTokenAsync(crmUrl, credentials);
                accessToken = result.AccessToken;

                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(crmUrl);
                    httpClient.Timeout = new TimeSpan(0, 2, 0);
                    httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                    httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    string entityName = "incidents";

                    HttpResponseMessage httpResponseMessaje = await httpClient.GetAsync($"api/data/v9.2/{entityName}?$select=new_contenedor,new_factura,new_bcf,new_cantequipo,new_commodity,new_confirmacinzarpe,new_contidadbultos,new_destino,new_eta,new_etd1,modifiedon,new_incoterm,new_origen,new_po,new_poe,new_pol,new_preestado2,new_seal,_new_shipper_value,new_statuscliente,new_tamaoequipo,new_transporte,new_new_facturacompaia,title,new_lugarcolocacion,new_redestino,new_diasdetransito,new_barcodesalida,new_viajedesalida,_customerid_value,new_ofertatarifaid,new_montocostoestimado&$filter=((_customerid_value eq {cliente}) and (new_preestado2 eq 100000000 or new_preestado2 eq 100000016 or new_preestado2 eq 100000001 or new_preestado2 eq 100000017 or new_preestado2 eq 100000002 or new_preestado2 eq 100000003 or new_preestado2 eq 100000004 or new_preestado2 eq 100000005 or new_preestado2 eq 100000006 or new_preestado2 eq 100000018 or new_preestado2 eq 100000007 or new_preestado2 eq 100000024 or new_preestado2 eq 100000025 or new_preestado2 eq 100000026))&$orderby=title desc");
                    httpResponseMessaje.EnsureSuccessStatusCode();

                    if (httpResponseMessaje.IsSuccessStatusCode)
                    {
                        string jsonResponse = await httpResponseMessaje.Content.ReadAsStringAsync();

                        DynamicsTrackingLogin dynamicsObject = JsonConvert.DeserializeObject<DynamicsTrackingLogin>(jsonResponse)!;

                        foreach (var item in dynamicsObject.value!)
                        {
                            string shipperValue = item._new_shipper_value!;

                            var nuevoValorCliente = _clienteApplication.NombreCliente(shipperValue);

                            if(nuevoValorCliente.Result.Data is not null)
                            {
                                foreach (var items in nuevoValorCliente.Result.Data!.value!)
                                {
                                    item._new_shipper_value = items.name;
                                }
                            }
                        }

                        response.IsSuccess = true;
                        response.Data = dynamicsObject;
                        response.Message = ReplyMessage.MESSAGE_QUERY;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    }

                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                WatchLogger.Log(ex.Message);
            }

            return response;
        }
    }
}