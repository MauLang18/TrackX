using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using TrackX.Application.Commons.Bases;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Utilities.Static;
using WatchDog;

namespace TrackX.Application.Services
{
    public class TrackingNoLoginApplication : ITrackingNoLoginApplication
    {
        public async Task<BaseResponse<Dynamics>> TrackingByIDTRA(string idtra)
        {
            var response = new BaseResponse<Dynamics>();

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
                Console.WriteLine(accessToken);

                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(crmUrl);
                    httpClient.Timeout = new TimeSpan(0, 2, 0);
                    httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                    httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    string entityName = "incidents";

                    HttpResponseMessage httpResponseMessaje = await httpClient.GetAsync($"api/data/v9.2/{entityName}?$select=new_contenedor,new_bcf,new_confirmacinzarpe,new_destino,new_eta,new_etd1,new_instcliente,new_ingreso,new_ingresoabodegas,new_nombrebuque,new_origen,new_poe,new_pol,new_transporte,title,new_preestado2,modifiedon&$filter=contains(title,'{idtra}')&$orderby=title asc");
                    httpResponseMessaje.EnsureSuccessStatusCode();
                    
                    if(httpResponseMessaje.IsSuccessStatusCode)
                    {
                        string jsonResponse = await httpResponseMessaje.Content.ReadAsStringAsync();

                        // Deserializar la cadena JSON a un objeto Dynamics
                        Dynamics dynamicsObject = JsonConvert.DeserializeObject<Dynamics>(jsonResponse)!;

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
            }catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                WatchLogger.Log(ex.Message);
            }

            return response;
        }

        public Task<BaseResponse<string>> TrackingByContenedor(string contenedor)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<string>> TrackingByBCF(string bcf)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<string>> TrackingByPO(string po)
        {
            throw new NotImplementedException();
        }
    }
}