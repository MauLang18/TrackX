﻿using Google.Apis.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Dtos.Usuario.Request;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Persistences.Interfaces;
using TrackX.Infrastructure.Secret;
using TrackX.Utilities.AppSettings;
using TrackX.Utilities.Static;
using WatchDog;
using BC = BCrypt.Net.BCrypt;

namespace TrackX.Application.Services;

public class AuthApplication : IAuthApplication
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISecretService _secretService;
    private readonly IClienteApplication _clienteApplication;
    private readonly AppSettings _appSettings;

    public AuthApplication(IUnitOfWork unitOfWork, ISecretService secretService, IClienteApplication clienteApplication, IOptions<AppSettings> appSettings)
    {
        _unitOfWork = unitOfWork;
        _secretService = secretService;
        _clienteApplication = clienteApplication;
        _appSettings = appSettings.Value;
    }

    public async Task<BaseResponse<string>> Login(TokenRequestDto requestDto, string authType)
    {
        var response = new BaseResponse<string>();

        try
        {
            var Config = await GetConfigAsync();
            if (Config == null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_TOKEN_ERROR;
                return response;
            }

            var user = await _unitOfWork.Usuario.UserByEmail(requestDto.Correo!);

            if (user is null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_TOKEN_ERROR;
                return response;
            }

            if (user.Tipo != authType)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_AUTH_TYPE_GOOGLE;
                return response;
            }

            if (BC.Verify(requestDto.Pass, user.Pass))
            {
                response.IsSuccess = true;
                string shipperValue = user.Cliente!;

                if (shipperValue is not null)
                {
                    var shipperValuesList = new List<string> { shipperValue };
                    var nuevoValorCliente = await _clienteApplication.NombreCliente(shipperValuesList);

                    foreach (var items in nuevoValorCliente.Data!.value!)
                    {
                        user.NombreCliente = items.name!;
                    }
                }
                else
                {
                    user.NombreCliente = "";
                }

                response.Data = await GenerateToken(user);
                response.Message = ReplyMessage.MESSAGE_TOKEN;
                return response;
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

    public async Task<BaseResponse<string>> LoginWithGoogle(string credentials, string authType)
    {
        var response = new BaseResponse<string>();

        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>
                {
                    _appSettings.ClientId!
                },
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(credentials, settings);
            var user = await _unitOfWork.Usuario.UserByEmail(payload.Email);

            if (user is null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_GOOGLE_ERROR;
                return response;
            }

            if (user.Tipo != authType)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_AUTH_TYPE;
                return response;
            }

            response.IsSuccess = true;
            response.Data = await GenerateToken(user);
            response.Message = ReplyMessage.MESSAGE_TOKEN;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ReplyMessage.MESSAGE_EXCEPTION;
            WatchLogger.Log(ex.Message);
        }

        return response;
    }

    private async Task<JwtConfig?> GetConfigAsync()
    {
        var secretJson = await _secretService.GetSecret("TrackX/data/Jwt");
        var SecretResponse = JsonConvert.DeserializeObject<SecretResponse<JwtConfig>>(secretJson);
        return SecretResponse?.Data?.Data;
    }

    private async Task<string> GenerateToken(TbUsuario usuario)
    {
        var Config = await GetConfigAsync();

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config!.Secret!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.NameId, usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.FamilyName, $"{usuario.Nombre} {usuario.Apellido}"),
            new Claim(JwtRegisteredClaimNames.GivenName, usuario.IdRol.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Correo ?? ""),
            new Claim(JwtRegisteredClaimNames.Name, usuario.Cliente ?? ""),
            new Claim(JwtRegisteredClaimNames.Gender, usuario.Imagen ?? ""),
            new Claim(JwtRegisteredClaimNames.Acr, usuario.NombreCliente ?? ""),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Typ, usuario.Paginas ?? ""),
            new Claim(JwtRegisteredClaimNames.Nonce, usuario.Telefono ?? ""),
            new Claim(JwtRegisteredClaimNames.Azp, usuario.Direccion ?? ""),
            new Claim(JwtRegisteredClaimNames.Prn, usuario.Pais ?? ""),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: Config.Issuer,
            audience: Config.Issuer,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(int.Parse(Config.Expires!)),
            notBefore: DateTime.UtcNow,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}