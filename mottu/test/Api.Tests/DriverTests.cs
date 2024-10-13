using Api.Tests.Common.Enums;
using Api.Tests.Common.Factories;
using CrossCutting.Common.Dtos.Request;
using CrossCutting.Dtos.Driver.Request;
using Domain.Common.Enums;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xunit;

namespace Api.Tests;

public class DriverTests(
    WebApplicationFactory<Program> factory) : CustomWebApplicationFactory(factory)
{
    #region Delete

    [Fact]
    public async Task Delete_ShouldReturnOk_WhenIdIsValid()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/entregadores/{createDriver.Id}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        response = await _client.GetAsync($"/entregadores/{createDriver.Id}");

        content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldReturnBadRequest_WhenIdIsInvalid()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/entregadores/invalid-id");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenIdDoesNotExist()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/entregadores/0");

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

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/entregadores/{createDriver.Id}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_ShouldReturnBadRequest_WhenIdIsInvalid()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/entregadores/invalid-id");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Get_ShouldReturnNotFound_WhenIdDoesNotExist()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/entregadores/0");

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

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/entregadores/listar");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetList_ShouldReturnNotFound_WhenNoResultsFound()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/entregadores/listar");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetListSearch_ShouldReturnOk_WhenSearchIsValid()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var genericSearchPaginationRequest = new GenericSearchPaginationRequest();
        genericSearchPaginationRequest.Texto = driver.Nome;
        genericSearchPaginationRequest.PaginaAtual = 1;
        genericSearchPaginationRequest.TamanhoPagina = 100;
        genericSearchPaginationRequest.CampoOrdenacao = "name";
        genericSearchPaginationRequest.DirecaoOrdenacao = "asc";

        var response = await _client.PostAsJsonAsync($"api/v1/entregadores/consultar", genericSearchPaginationRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetList_ShouldReturnOk_WhenCnhIsValid()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/entregadores/{driver.Numero_Cnh}/cnh");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetList_ShouldReturnNotFound_WhenCnhDoesNotExist()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/entregadores/12345678901/cnh");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetList_ShouldReturnOk_WhenCnpjIsValid()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/entregadores/{driver.Cnpj}/cnpj");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetList_ShouldReturnNotFound_WhenCnpjDoesNotExist()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/entregadores/68356795000109/cnpj");

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

        var response = await _client.PostAsJsonAsync($"api/v1/entregadores/consultar", genericSearchPaginationRequest);

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
        genericSearchPaginationRequest.CampoOrdenacao = "name";
        genericSearchPaginationRequest.DirecaoOrdenacao = "asc";

        var response = await _client.PostAsJsonAsync($"api/v1/entregadores/consultar", genericSearchPaginationRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region Post

    [Fact]
    public async Task Post_ShouldReturnCreated_WhenIsValid()
    {
        var postDriverRequest = GenerateRandomPostDriverRequest(null, false, false);

        var response = await _client.PostAsJsonAsync("api/v1/entregadores", postDriverRequest);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnOk_WhenCnhImageIsValid()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putDriverSetCnhImageRequest = new PostDriverSetCnhImageRequest();
        putDriverSetCnhImageRequest.Imagem_Cnh = ImageBase64Png;

        var response = await _client.PostAsJsonAsync($"api/v1/entregadores/{driver.Id}/cnh", putDriverSetCnhImageRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenCnhImageIsInvalid()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putDriverSetCnhImageRequest = new PostDriverSetCnhImageRequest();
        putDriverSetCnhImageRequest.Imagem_Cnh = ImageBase64InvalidJpeg;

        var response = await _client.PostAsJsonAsync($"api/v1/entregadores/{driver.Id}/cnh", putDriverSetCnhImageRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenCnpjIsDuplicate()
    {
        var postDriverRequest = GenerateRandomPostDriverRequest(null, false, false);

        await _client.PostAsJsonAsync("api/v1/entregadores", postDriverRequest);

        var postDriverDuplicateCnpjRequest = GenerateRandomPostDriverRequest(null, false, false);
        postDriverDuplicateCnpjRequest.Cnpj = postDriverRequest.Cnpj;

        var response = await _client.PostAsJsonAsync("api/v1/entregadores", postDriverDuplicateCnpjRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenCnhIsDuplicate()
    {
        var postDriverRequest = GenerateRandomPostDriverRequest(null, false, false);

        await _client.PostAsJsonAsync("api/v1/entregadores", postDriverRequest);

        var postDriverDuplicateCnhRequest = GenerateRandomPostDriverRequest(null, false, false);
        postDriverDuplicateCnhRequest.Numero_Cnh = postDriverRequest.Numero_Cnh;

        var response = await _client.PostAsJsonAsync("api/v1/entregadores", postDriverDuplicateCnhRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenIdentifierIsDuplicate()
    {
        var postDriverRequest = GenerateRandomPostDriverRequest(null, false, false);

        await _client.PostAsJsonAsync("api/v1/entregadores", postDriverRequest);

        var postDriverDuplicateIdentifierRequest = GenerateRandomPostDriverRequest(null, false, false);
        postDriverDuplicateIdentifierRequest.Identificador = postDriverRequest.Identificador;

        var response = await _client.PostAsJsonAsync("api/v1/entregadores", postDriverDuplicateIdentifierRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenIdentifierIsMissing()
    {
        var postDriverRequest = GenerateRandomPostDriverRequest(FieldEnum.Identifier, true, false);

        var response = await _client.PostAsJsonAsync("api/v1/entregadores", postDriverRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenIdentifierSizeIsInvalid()
    {
        var postDriverRequest = GenerateRandomPostDriverRequest(FieldEnum.Identifier, false, true);

        var response = await _client.PostAsJsonAsync("api/v1/entregadores", postDriverRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenNameIsMissing()
    {
        var postDriverRequest = GenerateRandomPostDriverRequest(FieldEnum.Name, true, false);

        var response = await _client.PostAsJsonAsync("api/v1/entregadores", postDriverRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenNameSizeIsInvalid()
    {
        var postDriverRequest = GenerateRandomPostDriverRequest(FieldEnum.Name, false, true);

        var response = await _client.PostAsJsonAsync("api/v1/entregadores", postDriverRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenCnpjIsMissing()
    {
        var postDriverRequest = GenerateRandomPostDriverRequest(FieldEnum.Cnpj, true, false);

        var response = await _client.PostAsJsonAsync("api/v1/entregadores", postDriverRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenCnpjIsInvalid()
    {
        var postDriverRequest = GenerateRandomPostDriverRequest(null, false, false);
        postDriverRequest.Cnpj = "2";

        var response = await _client.PostAsJsonAsync("api/v1/entregadores", postDriverRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenCnhIsMissing()
    {
        var postDriverRequest = GenerateRandomPostDriverRequest(FieldEnum.Cnh, true, false);

        var response = await _client.PostAsJsonAsync("api/v1/entregadores", postDriverRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenCnhIsInvalid()
    {
        var postDriverRequest = GenerateRandomPostDriverRequest(null, false, false);
        postDriverRequest.Numero_Cnh = "1";

        var response = await _client.PostAsJsonAsync("api/v1/entregadores", postDriverRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenTypeIsMissing()
    {
        var postDriverRequest = GenerateRandomPostDriverRequest(null, false, false);
        postDriverRequest.Tipo_Cnh = null;

        var response = await _client.PostAsJsonAsync("api/v1/entregadores", postDriverRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenTypeIsInvalid()
    {
        var postDriverRequest = GenerateRandomPostDriverRequest(null, false, false);
        postDriverRequest.Tipo_Cnh = "T";

        var response = await _client.PostAsJsonAsync("api/v1/entregadores", postDriverRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenDateOfBirthIsMissing()
    {
        var postDriverRequest = GenerateRandomPostDriverRequest(FieldEnum.DateOfBirth, true, false);

        var response = await _client.PostAsJsonAsync("api/v1/entregadores", postDriverRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenDateOfBirthIsInvalid()
    {
        var postDriverRequest = GenerateRandomPostDriverRequest(null, false, false);

        postDriverRequest.Data_Nascimento = DateTime.Today.AddYears(-3);

        var response = await _client.PostAsJsonAsync("api/v1/entregadores", postDriverRequest);

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

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putDriverRequest = GenerateRandomPutDriverRequest(null, false, false);

        var response = await _client.PutAsJsonAsync($"api/v1/entregadores/{driver.Id}", putDriverRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnOk_WhenDeliveringIsValid()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putDriverSetDeliveringRequest = new PutDriverSetDeliveringRequest();
        putDriverSetDeliveringRequest.Entregando = true;

        var response = await _client.PutAsJsonAsync($"api/v1/entregadores/{createDriver.Id}/entregando", putDriverSetDeliveringRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenCnpjIsDuplicate()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var createDriverDuplicateCnpj = await CreateDriver(CnhTypeEnum.A);

        var driverDuplicateCnpj = await GetDriver(createDriverDuplicateCnpj.Id);

        var token = await LoginDriver(driverDuplicateCnpj.UsuarioId.Value, driverDuplicateCnpj.Nome);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putDriverDuplicateCnpjRequest = GenerateRandomPutDriverRequest(null, false, false);
        putDriverDuplicateCnpjRequest.Cnpj = driver.Cnpj;

        var response = await _client.PutAsJsonAsync($"api/v1/entregadores/{driverDuplicateCnpj.Id}", putDriverDuplicateCnpjRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenCnhIsDuplicate()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var createDriverDuplicateCnh = await CreateDriver(CnhTypeEnum.A);

        var driverDuplicateCnh = await GetDriver(createDriverDuplicateCnh.Id);

        var token = await LoginDriver(driverDuplicateCnh.UsuarioId.Value, driverDuplicateCnh.Nome);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putDriverDuplicateCnhRequest = GenerateRandomPutDriverRequest(null, false, false);
        putDriverDuplicateCnhRequest.Numero_Cnh = driver.Numero_Cnh;

        var response = await _client.PutAsJsonAsync($"api/v1/entregadores/{driverDuplicateCnh.Id}", putDriverDuplicateCnhRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenIdentifierIsDuplicate()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var createDriverDuplicateIdentifier = await CreateDriver(CnhTypeEnum.A);

        var driverDuplicateIdentifier = await GetDriver(createDriverDuplicateIdentifier.Id);

        var token = await LoginDriver(driverDuplicateIdentifier.UsuarioId.Value, driverDuplicateIdentifier.Nome);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putDriverDuplicateIdentifierRequest = GenerateRandomPutDriverRequest(null, false, false);
        putDriverDuplicateIdentifierRequest.Identificador = driver.Identificador;

        var response = await _client.PutAsJsonAsync($"api/v1/entregadores/{driverDuplicateIdentifier.Id}", putDriverDuplicateIdentifierRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenIdentifierIsMissing()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putDriverRequest = GenerateRandomPutDriverRequest(FieldEnum.Identifier, true, false);

        var response = await _client.PutAsJsonAsync($"api/v1/entregadores/{driver.Id}", putDriverRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenIdentifierSizeIsInvalid()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putDriverRequest = GenerateRandomPutDriverRequest(FieldEnum.Identifier, false, true);

        var response = await _client.PutAsJsonAsync($"api/v1/entregadores/{driver.Id}", putDriverRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenNameIsMissing()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putDriverRequest = GenerateRandomPutDriverRequest(FieldEnum.Name, true, false);

        var response = await _client.PutAsJsonAsync($"api/v1/entregadores/{driver.Id}", putDriverRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenNameSizeIsInvalid()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putDriverRequest = GenerateRandomPutDriverRequest(FieldEnum.Name, false, true);

        var response = await _client.PutAsJsonAsync($"api/v1/entregadores/{driver.Id}", putDriverRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenCnpjIsMissing()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putDriverRequest = GenerateRandomPutDriverRequest(FieldEnum.Cnpj, true, false);

        var response = await _client.PutAsJsonAsync($"api/v1/entregadores/{driver.Id}", putDriverRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenCnpjIsInvalid()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putDriverRequest = GenerateRandomPutDriverRequest(null, false, false);
        putDriverRequest.Cnpj = "2";

        var response = await _client.PutAsJsonAsync($"api/v1/entregadores/{driver.Id}", putDriverRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenCnhIsMissing()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putDriverRequest = GenerateRandomPutDriverRequest(FieldEnum.Cnh, true, false);

        var response = await _client.PutAsJsonAsync($"api/v1/entregadores/{driver.Id}", putDriverRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenCnhIsInvalid()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putDriverRequest = GenerateRandomPutDriverRequest(null, false, false);
        putDriverRequest.Numero_Cnh = "1";

        var response = await _client.PutAsJsonAsync($"api/v1/entregadores/{driver.Id}", putDriverRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenTypeIsMissing()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putDriverRequest = GenerateRandomPutDriverRequest(null, false, false);
        putDriverRequest.Tipo_Cnh = null;

        var response = await _client.PutAsJsonAsync($"api/v1/entregadores/{driver.Id}", putDriverRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenTypeIsInvalid()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putDriverRequest = GenerateRandomPutDriverRequest(null, false, false);
        putDriverRequest.Tipo_Cnh = "T";

        var response = await _client.PutAsJsonAsync($"api/v1/entregadores/{driver.Id}", putDriverRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenDateOfBirthIsMissing()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putDriverRequest = GenerateRandomPutDriverRequest(FieldEnum.DateOfBirth, true, false);

        var response = await _client.PutAsJsonAsync($"api/v1/entregadores/{driver.Id}", putDriverRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenDateOfBirthIsInvalid()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putDriverRequest = GenerateRandomPostDriverRequest(null, false, false);
        putDriverRequest.Data_Nascimento = DateTime.Today.AddYears(-3);

        var response = await _client.PutAsJsonAsync($"api/v1/entregadores/{driver.Id}", putDriverRequest);

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

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", null);

        var response = await _client.GetAsync($"api/v1/entregadores/{createDriver.Id}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Authorize_ShouldReturnForbidden_WhenDriverIdIsDifferent()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/entregadores/10");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Authorize_ShouldReturnForbidden_WhenRoleIsInvalid()
    {
        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/entregadores/{createDriver.Id}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    #endregion
}