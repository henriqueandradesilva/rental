using Api.Tests.Common.Enums;
using Api.Tests.Common.Factories;
using CrossCutting.Common.Dtos.Request;
using CrossCutting.Dtos.DriverNotificated.Request;
using Domain.Common.Enums;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xunit;

namespace Api.Tests;

public class DriverNotificatedTests(
    WebApplicationFactory<Program> factory) : CustomWebApplicationFactory(factory)
{
    #region Delete

    [Fact]
    public async Task Delete_ShouldReturnOk_WhenIdIsValid()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createOrder = await CreateOrder();

        var createNotification = await CreateNotification(createOrder.Id);

        var createDriverNotificated = await CreateDriverNotificated(createDriver.Id, createNotification.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/entregadoresNotificados/{createDriverNotificated.Id}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        response = await _client.GetAsync($"/entregadoresNotificados/{createDriverNotificated.Id}");

        content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldReturnBadRequest_WhenIdIsInvalid()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/entregadoresNotificados/invalid-id");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenIdDoesNotExist()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/entregadoresNotificados/0");

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

        var createOrder = await CreateOrder();

        var createNotification = await CreateNotification(createOrder.Id);

        var createDriverNotificated = await CreateDriverNotificated(createDriver.Id, createNotification.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/entregadoresNotificados/{createDriverNotificated.Id}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_ShouldReturnBadRequest_WhenIdIsInvalid()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/entregadoresNotificados/invalid-id");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Get_ShouldReturnNotFound_WhenIdDoesNotExist()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/entregadoresNotificados/0");

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

        var createOrder = await CreateOrder();

        var createNotification = await CreateNotification(createOrder.Id);

        var createDriverNotificated = await CreateDriverNotificated(createDriver.Id, createNotification.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/entregadoresNotificados/listar");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetList_ShouldReturnNotFound_WhenNoResultsFound()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/entregadoresNotificados/listar");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetListSearch_ShouldReturnOk_WhenSearchIsValid()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createOrder = await CreateOrder();

        var createNotification = await CreateNotification(createOrder.Id);

        var createDriverNotificated = await CreateDriverNotificated(createDriver.Id, createNotification.Id);

        var driverNotificated = await GetDriverNotificated(createDriverNotificated.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var listRelational = new List<Tuple<string, long>>();
        listRelational.Add(new Tuple<string, long>("notificationId", createNotification.Id));

        var genericSearchPaginationRequest = new GenericSearchPaginationRequest();
        genericSearchPaginationRequest.ListaRelacionamento = listRelational;
        genericSearchPaginationRequest.PaginaAtual = 1;
        genericSearchPaginationRequest.TamanhoPagina = 100;
        genericSearchPaginationRequest.CampoOrdenacao = "notificationId";
        genericSearchPaginationRequest.DirecaoOrdenacao = "asc";

        var response = await _client.PostAsJsonAsync($"api/v1/entregadoresNotificados/consultar", genericSearchPaginationRequest);

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
        listRelational.Add(new Tuple<string, long>("notificationId", 0));

        var genericSearchPaginationRequest = new GenericSearchPaginationRequest();
        genericSearchPaginationRequest.ListaRelacionamento = listRelational;
        genericSearchPaginationRequest.PaginaAtual = 1;
        genericSearchPaginationRequest.TamanhoPagina = 100;
        genericSearchPaginationRequest.CampoOrdenacao = "description";
        genericSearchPaginationRequest.DirecaoOrdenacao = "asc";

        var response = await _client.PostAsJsonAsync($"api/v1/entregadoresNotificados/consultar", genericSearchPaginationRequest);

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
        listRelational.Add(new Tuple<string, long>("notificationId", 0));

        var genericSearchPaginationRequest = new GenericSearchPaginationRequest();
        genericSearchPaginationRequest.ListaRelacionamento = listRelational;
        genericSearchPaginationRequest.PaginaAtual = 1;
        genericSearchPaginationRequest.TamanhoPagina = 100;
        genericSearchPaginationRequest.CampoOrdenacao = "NotificationId";
        genericSearchPaginationRequest.DirecaoOrdenacao = "asc";

        var response = await _client.PostAsJsonAsync($"api/v1/entregadoresNotificados/consultar", genericSearchPaginationRequest);

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

        var createOrder = await CreateOrder();

        var createNotification = await CreateNotification(createOrder.Id);

        var postDriverNotificatedRequest = GenerateRandomPostDriverNotificatedRequest(createDriver.Id, createNotification.Id, null, false);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/entregadoresNotificados", postDriverNotificatedRequest);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenDriverIdIsMissing()
    {
        var createOrder = await CreateOrder();

        var createNotification = await CreateNotification(createOrder.Id);

        var postDriverNotificatedRequest = GenerateRandomPostDriverNotificatedRequest(0, createNotification.Id, FieldEnum.DriverId, true);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/entregadoresNotificados", postDriverNotificatedRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenNotificationIdIsMissing()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var postDriverNotificatedRequest = GenerateRandomPostDriverNotificatedRequest(createDriver.Id, 0, FieldEnum.NotificationId, true);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/entregadoresNotificados", postDriverNotificatedRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenIsDuplicate()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createOrder = await CreateOrder();

        var createNotification = await CreateNotification(createOrder.Id);

        var postDriverNotificatedRequest = GenerateRandomPostDriverNotificatedRequest(createDriver.Id, createNotification.Id, null, false);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        await _client.PostAsJsonAsync("api/v1/entregadoresNotificados", postDriverNotificatedRequest);

        var postDriverNotificatedDuplicateRequest = new PostDriverNotificatedRequest();
        postDriverNotificatedDuplicateRequest.EntregadorId = postDriverNotificatedRequest.EntregadorId;
        postDriverNotificatedDuplicateRequest.NotificacaoId = postDriverNotificatedRequest.NotificacaoId;

        var response = await _client.PostAsJsonAsync("api/v1/entregadoresNotificados", postDriverNotificatedDuplicateRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenDateIsMissing()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createOrder = await CreateOrder();

        var createNotification = await CreateNotification(createOrder.Id);

        var postDriverNotificatedRequest = GenerateRandomPostDriverNotificatedRequest(createDriver.Id, createNotification.Id, FieldEnum.Date, true);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/entregadoresNotificados", postDriverNotificatedRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenDriverIsDelivering()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        await DriverSetDelivering(createDriver.Id, true);

        var createOrder = await CreateOrder();

        var createNotification = await CreateNotification(createOrder.Id);

        var postDriverNotificatedRequest = GenerateRandomPostDriverNotificatedRequest(createDriver.Id, createNotification.Id, null, false);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/entregadoresNotificados", postDriverNotificatedRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenDriverNotCnhTypeA()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.B);

        var createOrder = await CreateOrder();

        var createNotification = await CreateNotification(createOrder.Id);

        var postDriverNotificatedRequest = GenerateRandomPostDriverNotificatedRequest(createDriver.Id, createNotification.Id, null, false);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/entregadoresNotificados", postDriverNotificatedRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenDriverNotActive()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A, false);

        var createOrder = await CreateOrder();

        var createNotification = await CreateNotification(createOrder.Id);

        var postDriverNotificatedRequest = GenerateRandomPostDriverNotificatedRequest(createDriver.Id, createNotification.Id, null, false);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/entregadoresNotificados", postDriverNotificatedRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region Put

    [Fact]
    public async Task Put_ShouldReturnOk_WhenIsValid()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createOrder = await CreateOrder();

        var createNotification = await CreateNotification(createOrder.Id);

        var createDriverNotificated = await CreateDriverNotificated(createDriver.Id, createNotification.Id);

        var driverNotificated = await GetDriverNotificated(createDriverNotificated.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putDriverNotificatedRequest = GenerateRandomPutDriverNotificatedRequest(createDriver.Id, createNotification.Id, null, false);

        var response = await _client.PutAsJsonAsync($"api/v1/entregadoresNotificados/{driverNotificated.Id}", putDriverNotificatedRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenDriverIdIsMissing()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createOrder = await CreateOrder();

        var createNotification = await CreateNotification(createOrder.Id);

        var createDriverNotificated = await CreateDriverNotificated(createDriver.Id, createNotification.Id);

        var driverNotificated = await GetDriverNotificated(createDriverNotificated.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putDriverNotificatedRequest = GenerateRandomPutDriverNotificatedRequest(0, createNotification.Id, FieldEnum.DriverId, true);

        var response = await _client.PutAsJsonAsync($"api/v1/entregadoresNotificados/{driverNotificated.Id}", putDriverNotificatedRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenNotificationIdIsMissing()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createOrder = await CreateOrder();

        var createNotification = await CreateNotification(createOrder.Id);

        var createDriverNotificated = await CreateDriverNotificated(createDriver.Id, createNotification.Id);

        var driverNotificated = await GetDriverNotificated(createDriverNotificated.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putDriverNotificatedRequest = GenerateRandomPutDriverNotificatedRequest(createDriver.Id, 0, FieldEnum.NotificationId, true);

        var response = await _client.PutAsJsonAsync($"api/v1/entregadoresNotificados/{driverNotificated.Id}", putDriverNotificatedRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenIsDuplicate()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createOrder = await CreateOrder();

        var createNotification = await CreateNotification(createOrder.Id);

        var createDriverNotificated = await CreateDriverNotificated(createDriver.Id, createNotification.Id);

        var driverNotificated = await GetDriverNotificated(createDriverNotificated.Id);

        var createDriverDuplicate = await CreateDriver(CnhTypeEnum.A);

        var createOrderDuplicate = await CreateOrder();

        var createNotificationDuplicate = await CreateNotification(createOrderDuplicate.Id);

        var createDriverNotificatedDuplicate = await CreateDriverNotificated(createDriverDuplicate.Id, createNotificationDuplicate.Id);

        var driverNotificatedDuplicate = await GetDriverNotificated(createDriverNotificatedDuplicate.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putDriverNotificatedDuplicateRequest = new PutDriverNotificatedRequest();
        putDriverNotificatedDuplicateRequest.EntregadorId = driverNotificated.EntregadorId;
        putDriverNotificatedDuplicateRequest.NotificacaoId = driverNotificated.NotificacaoId;

        var response = await _client.PutAsJsonAsync($"api/v1/entregadoresNotificados/{driverNotificatedDuplicate.Id}", putDriverNotificatedDuplicateRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenDateIsMissing()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createOrder = await CreateOrder();

        var createNotification = await CreateNotification(createOrder.Id);

        var createDriverNotificated = await CreateDriverNotificated(createDriver.Id, createNotification.Id);

        var driverNotificated = await GetDriverNotificated(createDriverNotificated.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putDriverNotificatedRequest = GenerateRandomPutDriverNotificatedRequest(createDriver.Id, createNotification.Id, FieldEnum.Date, true);

        var response = await _client.PutAsJsonAsync($"api/v1/entregadoresNotificados/{driverNotificated.Id}", putDriverNotificatedRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenDriverIsDelivering()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createOrder = await CreateOrder();

        var createNotification = await CreateNotification(createOrder.Id);

        var createDriverNotificated = await CreateDriverNotificated(createDriver.Id, createNotification.Id);

        var driverNotificated = await GetDriverNotificated(createDriverNotificated.Id);

        var token = await LoginAdmin();

        await DriverSetDelivering(createDriver.Id, true);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putDriverNotificatedRequest = GenerateRandomPutDriverNotificatedRequest(createDriver.Id, createNotification.Id, null, false);

        var response = await _client.PutAsJsonAsync($"api/v1/entregadoresNotificados/{driverNotificated.Id}", putDriverNotificatedRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenDriverNotCnhTypeA()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createOrder = await CreateOrder();

        var createNotification = await CreateNotification(createOrder.Id);

        var createDriverNotificated = await CreateDriverNotificated(createDriver.Id, createNotification.Id);

        var driverNotificated = await GetDriverNotificated(createDriverNotificated.Id);

        var token = await LoginAdmin();

        await DriverSetCnhType(createDriver.Id, CnhTypeEnum.B);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putDriverNotificatedRequest = GenerateRandomPutDriverNotificatedRequest(createDriver.Id, createNotification.Id, null, false);

        var response = await _client.PutAsJsonAsync($"api/v1/entregadoresNotificados/{driverNotificated.Id}", putDriverNotificatedRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenDriverNotActive()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A, true);

        var createOrder = await CreateOrder();

        var createNotification = await CreateNotification(createOrder.Id);

        var createDriverNotificated = await CreateDriverNotificated(createDriver.Id, createNotification.Id);

        var driverNotificated = await GetDriverNotificated(createDriverNotificated.Id);

        var token = await LoginAdmin();

        var driver = await GetDriver(createDriver.Id);

        await UserSetActive(driver.UsuarioId.Value, false);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putDriverNotificatedRequest = GenerateRandomPutDriverNotificatedRequest(createDriver.Id, createNotification.Id, null, false);

        var response = await _client.PutAsJsonAsync($"api/v1/entregadoresNotificados/{driverNotificated.Id}", putDriverNotificatedRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region Authorize

    [Fact]
    public async Task AuthorizeDriverNotificated_Unauthorized_Missing_Token_Returns401()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createOrder = await CreateOrder();

        var createNotification = await CreateNotification(createOrder.Id);

        var createDriverNotificated = await CreateDriverNotificated(createDriver.Id, createNotification.Id);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", null);

        var response = await _client.GetAsync($"api/v1/entregadoresNotificados/{createDriverNotificated.Id}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task AuthorizeDriverNotificated_Forbidden_Invalid_Returns403()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var createOrder = await CreateOrder();

        var createNotification = await CreateNotification(createOrder.Id);

        var createDriverNotificated = await CreateDriverNotificated(createDriver.Id, createNotification.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/entregadoresNotificados/{createDriverNotificated.Id}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    #endregion
}