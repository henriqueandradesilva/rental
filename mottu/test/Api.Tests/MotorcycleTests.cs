using Api.Tests.Common.Enums;
using Api.Tests.Common.Factories;
using CrossCutting.Common.Dtos.Request;
using CrossCutting.Const;
using CrossCutting.Dtos.Motorcycle.Request;
using Domain.Common.Enums;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xunit;

namespace Api.Tests;

public class MotorcycleTests(
    WebApplicationFactory<Program> factory) : CustomWebApplicationFactory(factory)
{
    #region Delete

    [Fact]
    public async Task Delete_ShouldReturnOk_WhenIdIsValid()
    {
        var createMotorcycle = await CreateMotorcycle();

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/motos/{createMotorcycle.Id}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        response = await _client.GetAsync($"/motos/{createMotorcycle.Id}");

        content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldReturnBadRequest_WhenIdIsInvalid()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/motos/invalid-id");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenIdDoesNotExist()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/motos/0");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldReturnBadRequest_WhenRentalExist()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createMotorcycle = await CreateMotorcycle();

        await CreateRental(createDriver.Id, createMotorcycle.Id, SystemConst.PlanSevenDays);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/entregadores/{createDriver.Id}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region Get

    [Fact]
    public async Task Get_ShouldReturnOk_WhenIdIsValid()
    {
        var createMotorcycle = await CreateMotorcycle();

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/motos/{createMotorcycle.Id}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_ShouldReturnBadRequest_WhenIdIsInvalid()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/motos/invalid-id");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Get_ShouldReturnNotFound_WhenIdDoesNotExist()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/motos/0");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region Get List

    [Fact]
    public async Task GetList_ShouldReturnOk_WhenRequestIsValid()
    {
        var createMotorcycle = await CreateMotorcycle();

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/motos/listar");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetList_ShouldReturnNotFound_WhenNoResultsFound()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/motos/listar");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetListSearch_ShouldReturnOk_WhenSearchIsValid()
    {
        var createMotorcycle = await CreateMotorcycle();

        var motorcycle = await GetMotorcycle(createMotorcycle.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var genericSearchPaginationRequest = new GenericSearchPaginationRequest();
        genericSearchPaginationRequest.Texto = motorcycle.Placa;
        genericSearchPaginationRequest.PaginaAtual = 1;
        genericSearchPaginationRequest.TamanhoPagina = 100;
        genericSearchPaginationRequest.CampoOrdenacao = "plate";
        genericSearchPaginationRequest.DirecaoOrdenacao = "asc";

        var response = await _client.PostAsJsonAsync($"api/v1/motos/consultar", genericSearchPaginationRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetList_ShouldReturnOk_WhenPlateIsValid()
    {
        var createMotorcycle = await CreateMotorcycle();

        var motorcycle = await GetMotorcycle(createMotorcycle.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/motos?placa={motorcycle.Placa}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetList_ShouldReturnNotFound_WhenPlateDoesNotExist()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/motos?placa=Teste");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetListSearch_ShouldReturnInternalServerError_WhenExceptionOccurs()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var genericSearchPaginationRequest = new GenericSearchPaginationRequest();
        genericSearchPaginationRequest.Texto = "Teste";
        genericSearchPaginationRequest.PaginaAtual = 1;
        genericSearchPaginationRequest.TamanhoPagina = 100;
        genericSearchPaginationRequest.CampoOrdenacao = "description";
        genericSearchPaginationRequest.DirecaoOrdenacao = "asc";

        var response = await _client.PostAsJsonAsync($"api/v1/motos/consultar", genericSearchPaginationRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task GetListSearch_ShouldReturnNotFound_WhenNoResultsFound()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var genericSearchPaginationRequest = new GenericSearchPaginationRequest();
        genericSearchPaginationRequest.Texto = "Teste";
        genericSearchPaginationRequest.PaginaAtual = 1;
        genericSearchPaginationRequest.TamanhoPagina = 100;
        genericSearchPaginationRequest.CampoOrdenacao = "plate";
        genericSearchPaginationRequest.DirecaoOrdenacao = "asc";

        var response = await _client.PostAsJsonAsync($"api/v1/motos/consultar", genericSearchPaginationRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region Post

    [Fact]
    public async Task Post_ShouldReturnCreated_WhenIsValid()
    {
        var postMotorcycleRequest = GenerateRandomPostMotorcycleRequest(null, false, false);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/motos", postMotorcycleRequest);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenPlateIsDuplicate()
    {
        var postMotorcycleRequest = GenerateRandomPostMotorcycleRequest(null, false, false);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        await _client.PostAsJsonAsync("api/v1/motos", postMotorcycleRequest);

        var postMotorcycleDuplicatePlateRequest = GenerateRandomPostMotorcycleRequest(null, false, false);
        postMotorcycleDuplicatePlateRequest.Placa = postMotorcycleRequest.Placa;

        var response = await _client.PostAsJsonAsync("api/v1/motos", postMotorcycleDuplicatePlateRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenIdentifierIsDuplicate()
    {
        var postMotorcycleRequest = GenerateRandomPostMotorcycleRequest(null, false, false);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        await _client.PostAsJsonAsync("api/v1/motos", postMotorcycleRequest);

        var postMotorcycleDuplicateIdentifierRequest = GenerateRandomPostMotorcycleRequest(null, false, false);
        postMotorcycleDuplicateIdentifierRequest.Identificador = postMotorcycleRequest.Identificador;

        var response = await _client.PostAsJsonAsync("api/v1/motos", postMotorcycleDuplicateIdentifierRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenIdentifierIsMissing()
    {
        var postMotorcycleRequest = GenerateRandomPostMotorcycleRequest(FieldEnum.Identifier, true, false);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/motos", postMotorcycleRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenIdentifierSizeIsInvalid()
    {
        var postMotorcycleRequest = GenerateRandomPostMotorcycleRequest(FieldEnum.Identifier, false, true);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/motos", postMotorcycleRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenPlateIsMissing()
    {
        var postMotorcycleRequest = GenerateRandomPostMotorcycleRequest(FieldEnum.Plate, true, false);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/motos", postMotorcycleRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenPlateSizeIsInvalid()
    {
        var postMotorcycleRequest = GenerateRandomPostMotorcycleRequest(FieldEnum.Plate, false, true);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/motos", postMotorcycleRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenYearIsMissing()
    {
        var postMotorcycleRequest = GenerateRandomPostMotorcycleRequest(FieldEnum.Year, true, false);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/motos", postMotorcycleRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region Put

    [Fact]
    public async Task Put_ShouldReturnOk_WhenIsValid()
    {
        var createMotorcycle = await CreateMotorcycle();

        var motorcycle = await GetMotorcycle(createMotorcycle.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putMotorcycleRequest = GenerateRandomPutMotorcycleRequest(null, false, false);

        var response = await _client.PutAsJsonAsync($"api/v1/motos/{motorcycle.Id}", putMotorcycleRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnOk_WhenPlateIsValid()
    {
        var createMotorcycle = await CreateMotorcycle();

        var motorcycle = await GetMotorcycle(createMotorcycle.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putMotorcycleSetPlateRequest = new PutMotorcycleSetPlateRequest();
        putMotorcycleSetPlateRequest.Placa = "ABC1234";

        var response = await _client.PutAsJsonAsync($"api/v1/motos/{motorcycle.Id}/placa", putMotorcycleSetPlateRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnOk_WhenRentedIsValid()
    {
        var createMotorcycle = await CreateMotorcycle();

        var motorcycle = await GetMotorcycle(createMotorcycle.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putMotorcycleSetRentedRequest = new PutMotorcycleSetRentedRequest();
        putMotorcycleSetRentedRequest.Alugado = true;

        var response = await _client.PutAsJsonAsync($"api/v1/motos/{motorcycle.Id}/alugado", putMotorcycleSetRentedRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenPlateIsDuplicate()
    {
        var createMotorcycle = await CreateMotorcycle();

        var motorcycle = await GetMotorcycle(createMotorcycle.Id);

        var createMotorcycleDuplicatePlate = await CreateMotorcycle();

        var motorcycleDuplicatePlate = await GetMotorcycle(createMotorcycleDuplicatePlate.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putMotorcycleDuplicatePlateRequest = GenerateRandomPutMotorcycleRequest(null, false, false);
        putMotorcycleDuplicatePlateRequest.Placa = motorcycle.Placa;

        var response = await _client.PutAsJsonAsync($"api/v1/motos/{motorcycleDuplicatePlate.Id}", putMotorcycleDuplicatePlateRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenIdentifierIsDuplicate()
    {
        var createMotorcycle = await CreateMotorcycle();

        var motorcycle = await GetMotorcycle(createMotorcycle.Id);

        var createMotorcycleDuplicateIdentifier = await CreateMotorcycle();

        var motorcycleDuplicateIdentifier = await GetMotorcycle(createMotorcycleDuplicateIdentifier.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putMotorcycleDuplicateIdentifierRequest = GenerateRandomPutMotorcycleRequest(null, false, false);
        putMotorcycleDuplicateIdentifierRequest.Identificador = motorcycle.Identificador;

        var response = await _client.PutAsJsonAsync($"api/v1/motos/{motorcycleDuplicateIdentifier.Id}", putMotorcycleDuplicateIdentifierRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenIdentifierIsMissing()
    {
        var createMotorcycle = await CreateMotorcycle();

        var motorcycle = await GetMotorcycle(createMotorcycle.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putMotorcycleRequest = GenerateRandomPutMotorcycleRequest(FieldEnum.Identifier, true, false);

        var response = await _client.PutAsJsonAsync($"api/v1/motos/{motorcycle.Id}", putMotorcycleRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenIdentifierSizeIsInvalid()
    {
        var createMotorcycle = await CreateMotorcycle();

        var motorcycle = await GetMotorcycle(createMotorcycle.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putMotorcycleRequest = GenerateRandomPutMotorcycleRequest(FieldEnum.Identifier, false, true);

        var response = await _client.PutAsJsonAsync($"api/v1/motos/{motorcycle.Id}", putMotorcycleRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenPlateIsMissing()
    {
        var createMotorcycle = await CreateMotorcycle();

        var motorcycle = await GetMotorcycle(createMotorcycle.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putMotorcycleRequest = GenerateRandomPutMotorcycleRequest(FieldEnum.Plate, true, false);

        var response = await _client.PutAsJsonAsync($"api/v1/motos/{motorcycle.Id}", putMotorcycleRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenPlateSizeIsInvalid()
    {
        var createMotorcycle = await CreateMotorcycle();

        var motorcycle = await GetMotorcycle(createMotorcycle.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putMotorcycleRequest = GenerateRandomPutMotorcycleRequest(FieldEnum.Plate, false, true);

        var response = await _client.PutAsJsonAsync($"api/v1/motos/{motorcycle.Id}", putMotorcycleRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenYearIsMissing()
    {
        var createMotorcycle = await CreateMotorcycle();

        var motorcycle = await GetMotorcycle(createMotorcycle.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putMotorcycleRequest = GenerateRandomPutMotorcycleRequest(FieldEnum.Year, true, false);

        var response = await _client.PutAsJsonAsync($"api/v1/motos/{motorcycle.Id}", putMotorcycleRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region Authorize

    [Fact]
    public async Task Authorize_ShouldReturnUnauthorized_WhenTokenIsMissing()
    {
        var createMotorcycle = await CreateMotorcycle();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", null);

        var response = await _client.GetAsync($"api/v1/motos/{createMotorcycle.Id}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Authorize_ShouldReturnForbidden_WhenRoleIsInvalid()
    {
        var createMotorcycle = await CreateMotorcycle();

        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/motos/{createMotorcycle.Id}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    #endregion
}