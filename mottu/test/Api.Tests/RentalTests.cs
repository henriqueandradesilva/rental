using Api.Tests.Common.Enums;
using Api.Tests.Common.Factories;
using CrossCutting.Common.Dtos.Request;
using CrossCutting.Const;
using CrossCutting.Dtos.Rental.Request;
using Domain.Common.Enums;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xunit;

namespace Api.Tests;

public class RentalTests(
    WebApplicationFactory<Program> factory) : CustomWebApplicationFactory(factory)
{
    #region Delete

    [Fact]
    public async Task Delete_ShouldReturnOk_WhenIdIsValid()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createMotorcycle = await CreateMotorcycle();

        var createRental = await CreateRental(createDriver.Id, createMotorcycle.Id, SystemConst.PlanSevenDays);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/locacao/{createRental.Id}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        response = await _client.GetAsync($"/locacao/{createRental.Id}");

        content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldReturnBadRequest_WhenIdIsInvalid()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/locacao/invalid-id");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenIdDoesNotExist()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/locacao/0");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region Get

    [Fact]
    public async Task Get_ShouldReturnOk_WhenIdIsValid()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createMotorcycle = await CreateMotorcycle();

        var createRental = await CreateRental(createDriver.Id, createMotorcycle.Id, SystemConst.PlanSevenDays);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/locacao/{createRental.Id}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_ShouldReturnBadRequest_WhenIdIsInvalid()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/locacao/invalid-id");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Get_ShouldReturnNotFound_WhenIdDoesNotExist()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/locacao/0");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region Get List

    [Fact]
    public async Task GetList_ShouldReturnOk_WhenRequestIsValid()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createMotorcycle = await CreateMotorcycle();

        var createRental = await CreateRental(createDriver.Id, createMotorcycle.Id, SystemConst.PlanSevenDays);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/locacao/listar");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetList_ShouldReturnNotFound_WhenNoResultsFound()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/locacao/listar");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetListSearch_ShouldReturnOk_WhenSearchIsValid()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createMotorcycle = await CreateMotorcycle();

        var createRental = await CreateRental(createDriver.Id, createMotorcycle.Id, SystemConst.PlanSevenDays);

        var rental = await GetRental(createRental.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var listRelational = new List<Tuple<string, long>>();
        listRelational.Add(new Tuple<string, long>("driverId", rental.Entregador_Id));

        var genericSearchPaginationRequest = new GenericSearchPaginationRequest();
        genericSearchPaginationRequest.ListaRelacionamento = listRelational;
        genericSearchPaginationRequest.PaginaAtual = 1;
        genericSearchPaginationRequest.TamanhoPagina = 100;
        genericSearchPaginationRequest.CampoOrdenacao = "driverId";
        genericSearchPaginationRequest.DirecaoOrdenacao = "asc";

        var response = await _client.PostAsJsonAsync($"api/v1/locacao/consultar", genericSearchPaginationRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetListSearch_ShouldReturnInternalServerError_WhenExceptionOccurs()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var listRelational = new List<Tuple<string, long>>();
        listRelational.Add(new Tuple<string, long>("driverId", 0));

        var genericSearchPaginationRequest = new GenericSearchPaginationRequest();
        genericSearchPaginationRequest.ListaRelacionamento = listRelational;
        genericSearchPaginationRequest.PaginaAtual = 1;
        genericSearchPaginationRequest.TamanhoPagina = 100;
        genericSearchPaginationRequest.CampoOrdenacao = "name";
        genericSearchPaginationRequest.DirecaoOrdenacao = "asc";

        var response = await _client.PostAsJsonAsync($"api/v1/locacao/consultar", genericSearchPaginationRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task GetListSearch_ShouldReturnNotFound_WhenNoResultsFound()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var listRelational = new List<Tuple<string, long>>();
        listRelational.Add(new Tuple<string, long>("driverId", 0));

        var genericSearchPaginationRequest = new GenericSearchPaginationRequest();
        genericSearchPaginationRequest.ListaRelacionamento = listRelational;
        genericSearchPaginationRequest.PaginaAtual = 1;
        genericSearchPaginationRequest.TamanhoPagina = 100;
        genericSearchPaginationRequest.CampoOrdenacao = "driverId";
        genericSearchPaginationRequest.DirecaoOrdenacao = "asc";

        var response = await _client.PostAsJsonAsync($"api/v1/locacao/consultar", genericSearchPaginationRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region Post

    [Fact]
    public async Task Post_ShouldReturnCreated_WhenIsValid()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        var createMotorcycle = await CreateMotorcycle();

        var postRentalRequest =
            GenerateRandomPostRentalRequest(createDriver.Id,
                                            createMotorcycle.Id,
                                            SystemConst.PlanSevenDays,
                                            null,
                                            false);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/locacao", postRentalRequest);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenMotorcycleIdIsMissing()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        var createMotorcycle = await CreateMotorcycle();

        var postRentalRequest =
            GenerateRandomPostRentalRequest(createDriver.Id,
                                            createMotorcycle.Id,
                                            SystemConst.PlanSevenDays,
                                            FieldEnum.MotorcycleId,
                                            true);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/locacao", postRentalRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenDriverIdIsMissing()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        var createMotorcycle = await CreateMotorcycle();

        var postRentalRequest =
            GenerateRandomPostRentalRequest(createDriver.Id,
                                            createMotorcycle.Id,
                                            SystemConst.PlanSevenDays,
                                            FieldEnum.DriverId,
                                            true);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/locacao", postRentalRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenPlanIdIsMissing()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        var createMotorcycle = await CreateMotorcycle();

        var postRentalRequest =
            GenerateRandomPostRentalRequest(createDriver.Id,
                                            createMotorcycle.Id,
                                            SystemConst.PlanSevenDays,
                                            FieldEnum.PlanId,
                                            true);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/locacao", postRentalRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenStartDateIsMissing()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        var createMotorcycle = await CreateMotorcycle();

        var postRentalRequest =
            GenerateRandomPostRentalRequest(createDriver.Id,
                                            createMotorcycle.Id,
                                            SystemConst.PlanSevenDays,
                                            FieldEnum.StartDate,
                                            true);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/locacao", postRentalRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenStartDateIsInvalid()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        var createMotorcycle = await CreateMotorcycle();

        var postRentalRequest =
            GenerateRandomPostRentalRequest(createDriver.Id,
                                            createMotorcycle.Id,
                                            SystemConst.PlanSevenDays,
                                            null,
                                            false);

        postRentalRequest.Data_Inicio = DateTime.UtcNow;

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/locacao", postRentalRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenMotorcyleIsRented()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        var createMotorcycle = await CreateMotorcycle();

        await MotorcycleSetRented(createMotorcycle.Id);

        var postRentalRequest =
            GenerateRandomPostRentalRequest(createDriver.Id,
                                            createMotorcycle.Id,
                                            SystemConst.PlanSevenDays,
                                            null,
                                            false);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/locacao", postRentalRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenDriverNotCnhTypeA()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.B);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        var createMotorcycle = await CreateMotorcycle();

        var postRentalRequest =
            GenerateRandomPostRentalRequest(createDriver.Id,
                                            createMotorcycle.Id,
                                            SystemConst.PlanSevenDays,
                                            null,
                                            false);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/locacao", postRentalRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenDriverNotActive()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A, false);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        var createMotorcycle = await CreateMotorcycle();

        var postRentalRequest =
            GenerateRandomPostRentalRequest(createDriver.Id,
                                            createMotorcycle.Id,
                                            SystemConst.PlanSevenDays,
                                            null,
                                            false);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/locacao", postRentalRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    #endregion

    #region Put

    [Fact]
    public async Task Put_ShouldReturnOk_WhenIsValid()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createMotorcycle = await CreateMotorcycle();

        var createRental = await CreateRental(createDriver.Id, createMotorcycle.Id, SystemConst.PlanSevenDays);

        var rental = await GetRental(createRental.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putRentalRequest =
            GenerateRandomPutRentalRequest(createDriver.Id,
                                           createMotorcycle.Id,
                                           SystemConst.PlanSevenDays,
                                           null,
                                           false);

        var response = await _client.PutAsJsonAsync($"api/v1/locacao/{rental.Id}", putRentalRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnOk_WhenSetReturn()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var createMotorcycle = await CreateMotorcycle();

        var createRental = await CreateRental(createDriver.Id, createMotorcycle.Id, SystemConst.PlanSevenDays);

        var rental = await GetRental(createRental.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putRentalRequest = new PutRentalSetReturnRequest();
        putRentalRequest.Data_Devolucao = rental.DataCriacao.AddDays(3);

        var response = await _client.PutAsJsonAsync($"api/v1/locacao/{rental.Id}/devolucao", putRentalRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenMotorcycleIdIsMissing()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createMotorcycle = await CreateMotorcycle();

        var createRental = await CreateRental(createDriver.Id, createMotorcycle.Id, SystemConst.PlanSevenDays);

        var rental = await GetRental(createRental.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putRentalRequest =
            GenerateRandomPutRentalRequest(createDriver.Id,
                                           createMotorcycle.Id,
                                           SystemConst.PlanSevenDays,
                                           FieldEnum.MotorcycleId,
                                           true);

        var response = await _client.PutAsJsonAsync($"api/v1/locacao/{rental.Id}", putRentalRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenDriverIdIsMissing()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createMotorcycle = await CreateMotorcycle();

        var createRental = await CreateRental(createDriver.Id, createMotorcycle.Id, SystemConst.PlanSevenDays);

        var rental = await GetRental(createRental.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putRentalRequest =
            GenerateRandomPutRentalRequest(createDriver.Id,
                                           createMotorcycle.Id,
                                           SystemConst.PlanSevenDays,
                                           FieldEnum.DriverId,
                                           true);

        var response = await _client.PutAsJsonAsync($"api/v1/locacao/{rental.Id}", putRentalRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenPlanIdIsMissing()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createMotorcycle = await CreateMotorcycle();

        var createRental = await CreateRental(createDriver.Id, createMotorcycle.Id, SystemConst.PlanSevenDays);

        var rental = await GetRental(createRental.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putRentalRequest =
            GenerateRandomPutRentalRequest(createDriver.Id,
                                           createMotorcycle.Id,
                                           SystemConst.PlanSevenDays,
                                           FieldEnum.PlanId,
                                           true);

        var response = await _client.PutAsJsonAsync($"api/v1/locacao/{rental.Id}", putRentalRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenStartDateIsMissing()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createMotorcycle = await CreateMotorcycle();

        var createRental = await CreateRental(createDriver.Id, createMotorcycle.Id, SystemConst.PlanSevenDays);

        var rental = await GetRental(createRental.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putRentalRequest =
            GenerateRandomPutRentalRequest(createDriver.Id,
                                           createMotorcycle.Id,
                                           SystemConst.PlanSevenDays,
                                           FieldEnum.StartDate,
                                           true);

        var response = await _client.PutAsJsonAsync($"api/v1/locacao/{rental.Id}", putRentalRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenStartDateIsInvalid()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createMotorcycle = await CreateMotorcycle();

        var createRental = await CreateRental(createDriver.Id, createMotorcycle.Id, SystemConst.PlanSevenDays);

        var rental = await GetRental(createRental.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putRentalRequest =
            GenerateRandomPutRentalRequest(createDriver.Id,
                                           createMotorcycle.Id,
                                           SystemConst.PlanSevenDays,
                                           null,
                                           false);

        putRentalRequest.Data_Inicio = DateTime.UtcNow;

        var response = await _client.PutAsJsonAsync($"api/v1/locacao/{rental.Id}", putRentalRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenDriverNotCnhTypeA()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createMotorcycle = await CreateMotorcycle();

        var createRental = await CreateRental(createDriver.Id, createMotorcycle.Id, SystemConst.PlanSevenDays);

        var rental = await GetRental(createRental.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        await DriverSetCnhType(createDriver.Id, CnhTypeEnum.B);

        var putRentalRequest =
            GenerateRandomPutRentalRequest(createDriver.Id,
                                           createMotorcycle.Id,
                                           SystemConst.PlanSevenDays,
                                           null,
                                           false);

        var response = await _client.PutAsJsonAsync($"api/v1/locacao/{rental.Id}", putRentalRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenDriverNotActive()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createMotorcycle = await CreateMotorcycle();

        var createRental = await CreateRental(createDriver.Id, createMotorcycle.Id, SystemConst.PlanSevenDays);

        var rental = await GetRental(createRental.Id);

        var token = await LoginAdmin();

        var driver = await GetDriver(createDriver.Id);

        await UserSetActive(driver.UsuarioId.Value, false);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putRentalRequest =
            GenerateRandomPutRentalRequest(createDriver.Id,
                                           createMotorcycle.Id,
                                           SystemConst.PlanSevenDays,
                                           null,
                                           false);

        var response = await _client.PutAsJsonAsync($"api/v1/locacao/{rental.Id}", putRentalRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region Authorize

    [Fact]
    public async Task Authorize_ShouldReturnUnauthorized_WhenTokenIsMissing()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createMotorcycle = await CreateMotorcycle();

        var createRental = await CreateRental(createDriver.Id, createMotorcycle.Id, SystemConst.PlanSevenDays);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", null);

        var response = await _client.GetAsync($"api/v1/locacao/{createRental.Id}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Authorize_ShouldReturnForbidden_WhenRoleIsInvalid()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createMotorcycle = await CreateMotorcycle();

        var createRental = await CreateRental(createDriver.Id, createMotorcycle.Id, SystemConst.PlanSevenDays);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/locacao/{createRental.Id}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    #endregion
}