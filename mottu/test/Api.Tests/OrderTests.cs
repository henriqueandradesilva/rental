using Api.Tests.Common.Enums;
using Api.Tests.Common.Factories;
using CrossCutting.Common.Dtos.Request;
using CrossCutting.Const;
using CrossCutting.Dtos.Order.Request;
using Domain.Common.Enums;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xunit;

namespace Api.Tests;

public class OrderTests(
    WebApplicationFactory<Program> factory) : CustomWebApplicationFactory(factory)
{
    #region Delete

    [Fact]
    public async Task Delete_ShouldReturnOk_WhenIdIsValid()
    {
        var createOrder = await CreateOrder();

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/pedidos/{createOrder.Id}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        response = await _client.GetAsync($"/pedidos/{createOrder.Id}");

        content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldReturnBadRequest_WhenIdIsInvalid()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/pedidos/invalid-id");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenIdDoesNotExist()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/pedidos/0");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region Get

    [Fact]
    public async Task Get_ShouldReturnOk_WhenIdIsValid()
    {
        var createOrder = await CreateOrder();

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/pedidos/{createOrder.Id}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_ShouldReturnBadRequest_WhenIdIsInvalid()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/pedidos/invalid-id");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Get_ShouldReturnNotFound_WhenIdDoesNotExist()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/pedidos/0");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region Get List

    [Fact]
    public async Task GetList_ShouldReturnOk_WhenRequestIsValid()
    {
        var createOrder = await CreateOrder();

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/pedidos/listar");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetList_ShouldReturnNotFound_WhenNoResultsFound()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/pedidos/listar");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetListSearch_ShouldReturnOk_WhenSearchIsValid()
    {
        var createOrder = await CreateOrder();

        var order = await GetOrder(createOrder.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var genericSearchPaginationRequest = new GenericSearchPaginationRequest();
        genericSearchPaginationRequest.Texto = order.Descricao;
        genericSearchPaginationRequest.PaginaAtual = 1;
        genericSearchPaginationRequest.TamanhoPagina = 100;
        genericSearchPaginationRequest.CampoOrdenacao = "description";
        genericSearchPaginationRequest.DirecaoOrdenacao = "asc";

        var response = await _client.PostAsJsonAsync($"api/v1/pedidos/consultar", genericSearchPaginationRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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
        genericSearchPaginationRequest.CampoOrdenacao = "name";
        genericSearchPaginationRequest.DirecaoOrdenacao = "asc";

        var response = await _client.PostAsJsonAsync($"api/v1/pedidos/consultar", genericSearchPaginationRequest);

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
        genericSearchPaginationRequest.CampoOrdenacao = "description";
        genericSearchPaginationRequest.DirecaoOrdenacao = "asc";

        var response = await _client.PostAsJsonAsync($"api/v1/pedidos/consultar", genericSearchPaginationRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region Post

    [Fact]
    public async Task Post_ShouldReturnCreated_WhenIsValid()
    {
        var postOrderRequest = GenerateRandomPostOrderRequest(null, false, false);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/pedidos", postOrderRequest);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenDescriptionIsDuplicate()
    {
        var postOrderRequest = GenerateRandomPostOrderRequest(null, false, false);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        await _client.PostAsJsonAsync("api/v1/pedidos", postOrderRequest);

        var postOrderDuplicateDescriptionRequest = GenerateRandomPostOrderRequest(null, false, false);
        postOrderDuplicateDescriptionRequest.Descricao = postOrderRequest.Descricao;

        var response = await _client.PostAsJsonAsync("api/v1/pedidos", postOrderDuplicateDescriptionRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenDescriptionIsMissing()
    {
        var postOrderRequest = GenerateRandomPostOrderRequest(FieldEnum.Description, true, false);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/pedidos", postOrderRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenDescriptionSizeIsInvalid()
    {
        var postOrderRequest = GenerateRandomPostOrderRequest(FieldEnum.Description, false, true);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/pedidos", postOrderRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenValueIsMissing()
    {
        var postOrderRequest = GenerateRandomPostOrderRequest(FieldEnum.Value, true, false);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/pedidos", postOrderRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenValueIsInvalid()
    {
        var postOrderRequest = GenerateRandomPostOrderRequest(null, false, false);
        postOrderRequest.Valor = -5;

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/pedidos", postOrderRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenDateIsMissing()
    {
        var postOrderRequest = GenerateRandomPostOrderRequest(FieldEnum.Date, true, false);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/pedidos", postOrderRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region Put

    [Fact]
    public async Task Put_ShouldReturnOk_WhenIsValid()
    {
        var createOrder = await CreateOrder();

        var order = await GetOrder(createOrder.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putOrderRequest = GenerateRandomPutOrderRequest(null, false, false);

        var response = await _client.PutAsJsonAsync($"api/v1/pedidos/{order.Id}", putOrderRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnOk_WhenStatusIsValid()
    {
        var createOrder = await CreateOrder();

        var order = await GetOrder(createOrder.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putOrderRequest = new PutOrderSetStatusRequest();
        putOrderRequest.StatusId = SystemConst.OrderStatusDeliveredDefault;

        var response = await _client.PutAsJsonAsync($"api/v1/pedidos/{order.Id}/status", putOrderRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenDescriptionIsDuplicate()
    {
        var createOrder = await CreateOrder();

        var order = await GetOrder(createOrder.Id);

        var createOrderDuplicateDescription = await CreateOrder();

        var orderDuplicateDescription = await GetOrder(createOrderDuplicateDescription.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putOrderDuplicateDescriptionRequest = GenerateRandomPutOrderRequest(null, false, false);
        putOrderDuplicateDescriptionRequest.Descricao = order.Descricao;

        var response = await _client.PutAsJsonAsync($"api/v1/pedidos/{orderDuplicateDescription.Id}", putOrderDuplicateDescriptionRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenDescriptionIsMissing()
    {
        var createOrder = await CreateOrder();

        var order = await GetOrder(createOrder.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putOrderRequest = GenerateRandomPutOrderRequest(FieldEnum.Description, true, false);

        var response = await _client.PutAsJsonAsync($"api/v1/pedidos/{order.Id}", putOrderRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenDescriptionSizeIsInvalid()
    {
        var createOrder = await CreateOrder();

        var order = await GetOrder(createOrder.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putOrderRequest = GenerateRandomPutOrderRequest(FieldEnum.Description, false, true);

        var response = await _client.PutAsJsonAsync($"api/v1/pedidos/{order.Id}", putOrderRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenValueIsMissing()
    {
        var createOrder = await CreateOrder();

        var order = await GetOrder(createOrder.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putOrderRequest = GenerateRandomPutOrderRequest(FieldEnum.Value, true, false);

        var response = await _client.PutAsJsonAsync($"api/v1/pedidos/{order.Id}", putOrderRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenDateIsMissing()
    {
        var createOrder = await CreateOrder();

        var order = await GetOrder(createOrder.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putOrderRequest = GenerateRandomPutOrderRequest(FieldEnum.Date, true, false);

        var response = await _client.PutAsJsonAsync($"api/v1/pedidos/{order.Id}", putOrderRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region Authorize

    [Fact]
    public async Task Authorize_ShouldReturnUnauthorized_WhenTokenIsMissing()
    {
        var createOrder = await CreateOrder();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", null);

        var response = await _client.GetAsync($"api/v1/pedidos/{createOrder.Id}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Authorize_ShouldReturnForbidden_WhenRoleIsInvalid()
    {
        var createOrder = await CreateOrder();

        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/pedidos/{createOrder.Id}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    #endregion
}