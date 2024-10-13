using Api.Tests.Common.Enums;
using Api.Tests.Common.Factories;
using CrossCutting.Common.Dtos.Request;
using CrossCutting.Const;
using CrossCutting.Dtos.OrderDelivered.Request;
using Domain.Common.Enums;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xunit;

namespace Api.Tests;

public class OrderDeliveredTests(
    WebApplicationFactory<Program> factory) : CustomWebApplicationFactory(factory)
{
    #region Delete

    [Fact]
    public async Task Delete_ShouldReturnOk_WhenIdIsValid()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createOrder = await CreateOrder();

        var createOrderAccepted = await CreateOrderAccepted(createDriver.Id, createOrder.Id);

        var createOrderDelivered = await CreateOrderDelivered(createDriver.Id, createOrder.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/pedidosEntregues/{createOrderDelivered.Id}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        response = await _client.GetAsync($"/pedidosEntregues/{createOrderDelivered.Id}");

        content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldReturnBadRequest_WhenIdIsInvalid()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/pedidosEntregues/invalid-id");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenIdDoesNotExist()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/pedidosEntregues/0");

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

        var createOrderAccepted = await CreateOrderAccepted(createDriver.Id, createOrder.Id);

        var createOrderDelivered = await CreateOrderDelivered(createDriver.Id, createOrder.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/pedidosEntregues/{createOrderDelivered.Id}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_ShouldReturnBadRequest_WhenIdIsInvalid()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/pedidosEntregues/invalid-id");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Get_ShouldReturnNotFound_WhenIdDoesNotExist()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/pedidosEntregues/0");

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

        var createOrderAccepted = await CreateOrderAccepted(createDriver.Id, createOrder.Id);

        var createOrderDelivered = await CreateOrderDelivered(createDriver.Id, createOrder.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/pedidosEntregues/listar");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetList_ShouldReturnNotFound_WhenNoResultsFound()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/pedidosEntregues/listar");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetListSearch_ShouldReturnOk_WhenSearchIsValid()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createOrder = await CreateOrder();

        var createOrderAccepted = await CreateOrderAccepted(createDriver.Id, createOrder.Id);

        var createOrderDelivered = await CreateOrderDelivered(createDriver.Id, createOrder.Id);

        var orderDelivered = await GetOrderDelivered(createOrderDelivered.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var listRelational = new List<Tuple<string, long>>();
        listRelational.Add(new Tuple<string, long>("orderId", createOrder.Id));

        var genericSearchPaginationRequest = new GenericSearchPaginationRequest();
        genericSearchPaginationRequest.ListaRelacionamento = listRelational;
        genericSearchPaginationRequest.PaginaAtual = 1;
        genericSearchPaginationRequest.TamanhoPagina = 100;
        genericSearchPaginationRequest.CampoOrdenacao = "orderId";
        genericSearchPaginationRequest.DirecaoOrdenacao = "asc";

        var response = await _client.PostAsJsonAsync($"api/v1/pedidosEntregues/consultar", genericSearchPaginationRequest);

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
        listRelational.Add(new Tuple<string, long>("orderId", 0));

        var genericSearchPaginationRequest = new GenericSearchPaginationRequest();
        genericSearchPaginationRequest.ListaRelacionamento = listRelational;
        genericSearchPaginationRequest.PaginaAtual = 1;
        genericSearchPaginationRequest.TamanhoPagina = 100;
        genericSearchPaginationRequest.CampoOrdenacao = "description";
        genericSearchPaginationRequest.DirecaoOrdenacao = "asc";

        var response = await _client.PostAsJsonAsync($"api/v1/pedidosEntregues/consultar", genericSearchPaginationRequest);

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
        listRelational.Add(new Tuple<string, long>("orderId", 0));

        var genericSearchPaginationRequest = new GenericSearchPaginationRequest();
        genericSearchPaginationRequest.ListaRelacionamento = listRelational;
        genericSearchPaginationRequest.PaginaAtual = 1;
        genericSearchPaginationRequest.TamanhoPagina = 100;
        genericSearchPaginationRequest.CampoOrdenacao = "orderId";
        genericSearchPaginationRequest.DirecaoOrdenacao = "asc";

        var response = await _client.PostAsJsonAsync($"api/v1/pedidosEntregues/consultar", genericSearchPaginationRequest);

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

        var createOrderAccepted = await CreateOrderAccepted(createDriver.Id, createOrder.Id);

        var postOrderDeliveredRequest = GenerateRandomPostOrderDeliveredRequest(createDriver.Id, createOrder.Id, null, false);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/pedidosEntregues", postOrderDeliveredRequest);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenDriverIdIsMissing()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createOrder = await CreateOrder();

        var createOrderAccepted = await CreateOrderAccepted(createDriver.Id, createOrder.Id);

        var postOrderDeliveredRequest = GenerateRandomPostOrderDeliveredRequest(0, createOrder.Id, FieldEnum.DriverId, true);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/pedidosEntregues", postOrderDeliveredRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenOrderIdIsMissing()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createOrder = await CreateOrder();

        var createOrderAccepted = await CreateOrderAccepted(createDriver.Id, createOrder.Id);

        var postOrderDeliveredRequest = GenerateRandomPostOrderDeliveredRequest(createDriver.Id, 0, FieldEnum.OrderId, true);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/pedidosEntregues", postOrderDeliveredRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenIsDuplicate()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createOrder = await CreateOrder();

        var createOrderAccepted = await CreateOrderAccepted(createDriver.Id, createOrder.Id);

        var postOrderDeliveredRequest = GenerateRandomPostOrderDeliveredRequest(createDriver.Id, createOrder.Id, null, false);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        await _client.PostAsJsonAsync("api/v1/pedidosEntregues", postOrderDeliveredRequest);

        var postOrderDeliveredDuplicateRequest = new PostOrderDeliveredRequest();
        postOrderDeliveredDuplicateRequest.EntregadorId = postOrderDeliveredRequest.EntregadorId;
        postOrderDeliveredDuplicateRequest.PedidoId = postOrderDeliveredRequest.PedidoId;

        var response = await _client.PostAsJsonAsync("api/v1/pedidosEntregues", postOrderDeliveredDuplicateRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenDateIsMissing()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createOrder = await CreateOrder();

        var createOrderAccepted = await CreateOrderAccepted(createDriver.Id, createOrder.Id);

        var postOrderDeliveredRequest = GenerateRandomPostOrderDeliveredRequest(createDriver.Id, createOrder.Id, FieldEnum.Date, true);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/pedidosEntregues", postOrderDeliveredRequest);

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

        var createOrderAccepted = await CreateOrderAccepted(createDriver.Id, createOrder.Id);

        var createOrderDelivered = await CreateOrderDelivered(createDriver.Id, createOrder.Id);

        var orderDelivered = await GetOrderDelivered(createOrderDelivered.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        await OrderRollback(createDriver.Id, createOrder.Id, SystemConst.OrderStatusAcceptedDefault);

        var putOrderDeliveredRequest = GenerateRandomPutOrderDeliveredRequest(createDriver.Id, createOrder.Id, null, false);

        var response = await _client.PutAsJsonAsync($"api/v1/pedidosEntregues/{orderDelivered.Id}", putOrderDeliveredRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenDriverIdIsMissing()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createOrder = await CreateOrder();

        var createOrderAccepted = await CreateOrderAccepted(createDriver.Id, createOrder.Id);

        var createOrderDelivered = await CreateOrderDelivered(createDriver.Id, createOrder.Id);

        var orderDelivered = await GetOrderDelivered(createOrderDelivered.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        await OrderRollback(createDriver.Id, createOrder.Id, SystemConst.OrderStatusAcceptedDefault);

        var putOrderDeliveredRequest = GenerateRandomPutOrderDeliveredRequest(0, createOrder.Id, FieldEnum.DriverId, true);

        var response = await _client.PutAsJsonAsync($"api/v1/pedidosEntregues/{orderDelivered.Id}", putOrderDeliveredRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenOrderIdIsMissing()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createOrder = await CreateOrder();

        var createOrderAccepted = await CreateOrderAccepted(createDriver.Id, createOrder.Id);

        var createOrderDelivered = await CreateOrderDelivered(createDriver.Id, createOrder.Id);

        var orderDelivered = await GetOrderDelivered(createOrderDelivered.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        await OrderRollback(createDriver.Id, createOrder.Id, SystemConst.OrderStatusAcceptedDefault);

        var putOrderDeliveredRequest = GenerateRandomPutOrderDeliveredRequest(createDriver.Id, 0, FieldEnum.OrderId, true);

        var response = await _client.PutAsJsonAsync($"api/v1/pedidosEntregues/{orderDelivered.Id}", putOrderDeliveredRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenIsDuplicate()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createOrder = await CreateOrder();

        var createOrderAccepted = await CreateOrderAccepted(createDriver.Id, createOrder.Id);

        var createOrderDelivered = await CreateOrderDelivered(createDriver.Id, createOrder.Id);

        var orderDelivered = await GetOrderDelivered(createOrderDelivered.Id);

        await OrderRollback(createDriver.Id, createOrder.Id, SystemConst.OrderStatusAcceptedDefault);

        var createDriverDuplicate = await CreateDriver(CnhTypeEnum.A);

        var createOrderDuplicate = await CreateOrder();

        var createOrderAcceptedDuplicate = await CreateOrderAccepted(createOrderDuplicate.Id, createOrderDuplicate.Id);

        var createOrderDeliveredDuplicate = await CreateOrderDelivered(createDriverDuplicate.Id, createOrderDuplicate.Id);

        var orderDeliveredDuplicate = await GetOrderDelivered(createOrderDeliveredDuplicate.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        await OrderRollback(createDriverDuplicate.Id, createOrderDuplicate.Id, SystemConst.OrderStatusAcceptedDefault);

        var putOrderDeliveredDuplicateRequest = new PutOrderDeliveredRequest();
        putOrderDeliveredDuplicateRequest.EntregadorId = orderDelivered.EntregadorId;
        putOrderDeliveredDuplicateRequest.PedidoId = orderDelivered.PedidoId;

        var response = await _client.PutAsJsonAsync($"api/v1/pedidosEntregues/{orderDeliveredDuplicate.Id}", putOrderDeliveredDuplicateRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenDateIsMissing()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createOrder = await CreateOrder();

        var createOrderAccepted = await CreateOrderAccepted(createDriver.Id, createOrder.Id);

        var createOrderDelivered = await CreateOrderDelivered(createDriver.Id, createOrder.Id);

        var orderDelivered = await GetOrderDelivered(createOrderDelivered.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        await OrderRollback(createDriver.Id, createOrder.Id, SystemConst.OrderStatusAcceptedDefault);

        var putOrderDeliveredRequest = GenerateRandomPutOrderDeliveredRequest(createDriver.Id, createOrder.Id, FieldEnum.Date, true);

        var response = await _client.PutAsJsonAsync($"api/v1/pedidosEntregues/{orderDelivered.Id}", putOrderDeliveredRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region Authorize

    [Fact]
    public async Task AuthorizeOrderDelivered_Unauthorized_Missing_Token_Returns401()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createOrder = await CreateOrder();

        var createOrderAccepted = await CreateOrderAccepted(createDriver.Id, createOrder.Id);

        var createOrderDelivered = await CreateOrderDelivered(createDriver.Id, createOrder.Id);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", null);

        var response = await _client.GetAsync($"api/v1/pedidosEntregues/{createOrderDelivered.Id}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task AuthorizeOrderDelivered_Forbidden_Invalid_Returns403()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var createOrder = await CreateOrder();

        var createOrderAccepted = await CreateOrderAccepted(createDriver.Id, createOrder.Id);

        var createOrderDelivered = await CreateOrderDelivered(createDriver.Id, createOrder.Id);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/pedidosEntregues/{createOrderDelivered.Id}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    #endregion
}