using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using TrackX.Application.Interfaces;

namespace TrackX.Tests.Cliente;

[TestClass]
public class ClienteApplicationTest
{
    private static WebApplicationFactory<Program>? _factory = null;
    private static IServiceScopeFactory? _scopeFactory = null;

    [ClassInitialize]
    public static void Initialize(TestContext _testContext)
    {
        _factory = new CustomWebApplicationFactory();
        _scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
    }

    [TestMethod]
    public async Task ListClientesName_QuerySuccessfully()
    {
        using var scope = _scopeFactory?.CreateScope();
        var context = scope?.ServiceProvider.GetService<IClienteApplication>();

        //Arrange
        var name = "3-101-632776 SOCIEDAD ANONIMA";
        var expected = true;

        //Act
        var result = await context!.CodeCliente(name);
        var current = result.IsSuccess;

        //Assert
        Assert.AreEqual(expected, current);
    }

    [TestMethod]
    public async Task ListClientesCode_QuerySuccessfully()
    {
        using var scope = _scopeFactory?.CreateScope();
        var context = scope?.ServiceProvider.GetService<IClienteApplication>();

        //Arrange
        var code = "ef898622-b6d1-ea11-a812-000d3a334ee9";
        var expected = true;

        //Act
        var result = await context!.NombreCliente(code);
        var current = result.IsSuccess;

        //Assert
        Assert.AreEqual(expected, current);
    }
}