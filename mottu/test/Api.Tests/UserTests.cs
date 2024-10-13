using Api.Tests.Common.Enums;
using Api.Tests.Common.Factories;
using CrossCutting.Common.Dtos.Request;
using CrossCutting.Dtos.User.Request;
using Domain.Common.Enums;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xunit;

namespace Api.Tests;

public class UserTests(
    WebApplicationFactory<Program> factory) : CustomWebApplicationFactory(factory)
{
    #region Delete

    [Fact]
    public async Task Delete_ShouldReturnOk_WhenIdIsValid()
    {
        var createUser = await CreateUser();

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/usuarios/{createUser.Id}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        response = await _client.GetAsync($"/usuarios/{createUser.Id}");

        content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldReturnBadRequest_WhenIdIsInvalid()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/usuarios/invalid-id");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenIdDoesNotExist()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/usuarios/0");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldReturnBadRequest_WhenIdIsImmutable()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/usuarios/1");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region Get

    [Fact]
    public async Task Get_ShouldReturnOk_WhenIdIsValid()
    {
        var createUser = await CreateUser();

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/usuarios/{createUser.Id}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_ShouldReturnBadRequest_WhenIdIsInvalid()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/usuarios/invalid-id");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Get_ShouldReturnNotFound_WhenIdDoesNotExist()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/usuarios/0");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region Get List

    [Fact]
    public async Task GetList_ShouldReturnOk_WhenRequestIsValid()
    {
        var createUser = await CreateUser();

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/usuarios/listar");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetList_ShouldReturnNotFound_WhenNoResultsFound()
    {
        var token = await LoginAdmin();

        await ForceDeleteAllUser();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/usuarios/listar");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetListSearch_ShouldReturnOk_WhenSearchIsValid()
    {
        var createUser = await CreateUser();

        var user = await GetUser(createUser.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var genericSearchPaginationRequest = new GenericSearchPaginationRequest();
        genericSearchPaginationRequest.Texto = user.Nome;
        genericSearchPaginationRequest.PaginaAtual = 1;
        genericSearchPaginationRequest.TamanhoPagina = 100;
        genericSearchPaginationRequest.CampoOrdenacao = "name";
        genericSearchPaginationRequest.DirecaoOrdenacao = "asc";

        var response = await _client.PostAsJsonAsync($"api/v1/usuarios/consultar", genericSearchPaginationRequest);

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
        genericSearchPaginationRequest.CampoOrdenacao = "description";
        genericSearchPaginationRequest.DirecaoOrdenacao = "asc";

        var response = await _client.PostAsJsonAsync($"api/v1/usuarios/consultar", genericSearchPaginationRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task GetListSearch_ShouldReturnNotFound_WhenNoResultsFound()
    {
        var token = await LoginAdmin();

        await ForceDeleteAllUser();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var genericSearchPaginationRequest = new GenericSearchPaginationRequest();
        genericSearchPaginationRequest.Texto = "Teste";
        genericSearchPaginationRequest.PaginaAtual = 1;
        genericSearchPaginationRequest.TamanhoPagina = 100;
        genericSearchPaginationRequest.CampoOrdenacao = "name";
        genericSearchPaginationRequest.DirecaoOrdenacao = "asc";

        var response = await _client.PostAsJsonAsync($"api/v1/usuarios/consultar", genericSearchPaginationRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region Post

    [Fact]
    public async Task Post_ShouldReturnCreated_WhenIsValid()
    {
        var postUserRequest = GenerateRandomPostUserRequest(null, false, false);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/usuarios", postUserRequest);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenRoleIdIsMissing()
    {
        var postUserRequest = GenerateRandomPostUserRequest(FieldEnum.UserRoleId, true, false);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/usuarios", postUserRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenEmailIsDuplicate()
    {
        var postUserRequest = GenerateRandomPostUserRequest(null, false, false);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        await _client.PostAsJsonAsync("api/v1/usuarios", postUserRequest);

        var postUserDuplicateEmailRequest = GenerateRandomPostUserRequest(null, false, false);
        postUserDuplicateEmailRequest.Email = postUserRequest.Email;

        var response = await _client.PostAsJsonAsync("api/v1/usuarios", postUserDuplicateEmailRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenEmailIsMissing()
    {
        var postUserRequest = GenerateRandomPostUserRequest(FieldEnum.Email, true, false);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/usuarios", postUserRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenEmailSizeIsInvalid()
    {
        var postUserRequest = GenerateRandomPostUserRequest(FieldEnum.Email, false, true);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/usuarios", postUserRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenEmailFormatIsInvalid()
    {
        var postUserRequest = GenerateRandomPostUserRequest(null, false, false);
        postUserRequest.Email = "a@";

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/usuarios", postUserRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenNameIsMissing()
    {
        var postUserRequest = GenerateRandomPostUserRequest(FieldEnum.Name, true, false);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/usuarios", postUserRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenNameSizeIsInvalid()
    {
        var postUserRequest = GenerateRandomPostUserRequest(FieldEnum.Name, false, true);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/usuarios", postUserRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenPasswordIsMissing()
    {
        var postUserRequest = GenerateRandomPostUserRequest(FieldEnum.Password, true, false);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/usuarios", postUserRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region Put

    [Fact]
    public async Task Put_ShouldReturnOk_WhenIsValid()
    {
        var createUser = await CreateUser();

        var user = await GetUser(createUser.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putUserRequest = GenerateRandomPutUserRequest(null, false, false);

        var response = await _client.PutAsJsonAsync($"api/v1/usuarios/{user.Id}", putUserRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnOk_WhenActiveIsValid()
    {
        var createUser = await CreateUser();

        var user = await GetUser(createUser.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putUserSetActiveRequest = new PutUserSetActiveRequest();
        putUserSetActiveRequest.Ativo = true;

        var response = await _client.PutAsJsonAsync($"api/v1/usuarios/{user.Id}/ativo", putUserSetActiveRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenRoleIdIsMissing()
    {
        var createUser = await CreateUser();

        var user = await GetUser(createUser.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putUserRequest = GenerateRandomPutUserRequest(FieldEnum.UserRoleId, true, false);

        var response = await _client.PutAsJsonAsync($"api/v1/usuarios/{user.Id}", putUserRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenEmailIsDuplicate()
    {
        var createUser = await CreateUser();

        var user = await GetUser(createUser.Id);

        var createUserDuplicateEmail = await CreateUser();

        var userDuplicateEmail = await GetUser(createUserDuplicateEmail.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putUserDuplicateEmailRequest = GenerateRandomPutUserRequest(null, false, false);
        putUserDuplicateEmailRequest.Email = user.Email;

        var response = await _client.PutAsJsonAsync($"api/v1/usuarios/{userDuplicateEmail.Id}", putUserDuplicateEmailRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenEmailIsMissing()
    {
        var createUser = await CreateUser();

        var user = await GetUser(createUser.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putUserRequest = GenerateRandomPutUserRequest(FieldEnum.Email, true, false);

        var response = await _client.PutAsJsonAsync($"api/v1/usuarios/{user.Id}", putUserRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenEmailSizeIsInvalid()
    {
        var createUser = await CreateUser();

        var user = await GetUser(createUser.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putUserRequest = GenerateRandomPutUserRequest(FieldEnum.Email, false, true);

        var response = await _client.PutAsJsonAsync($"api/v1/usuarios/{user.Id}", putUserRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenEmailFormatIsInvalid()
    {
        var createUser = await CreateUser();

        var user = await GetUser(createUser.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putUserRequest = GenerateRandomPutUserRequest(null, false, false);
        putUserRequest.Email = "a@";

        var response = await _client.PutAsJsonAsync($"api/v1/usuarios/{user.Id}", putUserRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenNameIsMissing()
    {
        var createUser = await CreateUser();

        var user = await GetUser(createUser.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putUserRequest = GenerateRandomPutUserRequest(FieldEnum.Name, true, false);

        var response = await _client.PutAsJsonAsync($"api/v1/usuarios/{user.Id}", putUserRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenNameSizeIsInvalid()
    {
        var createUser = await CreateUser();

        var user = await GetUser(createUser.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putUserRequest = GenerateRandomPutUserRequest(FieldEnum.Name, false, true);

        var response = await _client.PutAsJsonAsync($"api/v1/usuarios/{user.Id}", putUserRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenPasswordIsMissing()
    {
        var createUser = await CreateUser();

        var user = await GetUser(createUser.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putUserRequest = GenerateRandomPutUserRequest(FieldEnum.Password, true, false);

        var response = await _client.PutAsJsonAsync($"api/v1/usuarios/{user.Id}", putUserRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region Authorize

    [Fact]
    public async Task AuthorizeUser_Unauthorized_Missing_Token_Returns401()
    {
        var createUser = await CreateUser();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", null);

        var response = await _client.GetAsync($"api/v1/usuarios/{createUser.Id}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task AuthorizeUser_Forbidden_Invalid_Returns403()
    {
        var createUser = await CreateUser();

        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/usuarios/{createUser.Id}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    #endregion
}