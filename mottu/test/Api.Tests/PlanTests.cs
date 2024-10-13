using Api.Tests.Common.Enums;
using Api.Tests.Common.Factories;
using CrossCutting.Common.Dtos.Request;
using CrossCutting.Const;
using CrossCutting.Dtos.Plan.Request;
using Domain.Common.Enums;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xunit;

namespace Api.Tests;

public class PlanTests(
    WebApplicationFactory<Program> factory) : CustomWebApplicationFactory(factory)
{
    #region Delete

    [Fact]
    public async Task Delete_ShouldReturnOk_WhenIdIsValid()
    {
        var createPlan = await CreatePlan();

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/planos/{createPlan.Id}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        response = await _client.GetAsync($"/planos/{createPlan.Id}");

        content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldReturnBadRequest_WhenIdIsInvalid()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/planos/invalid-id");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenIdDoesNotExist()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/planos/0");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldReturnBadRequest_WhenIdIsImmutable()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/planos/{SystemConst.PlanSevenDays}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region Get

    [Fact]
    public async Task Get_ShouldReturnOk_WhenIdIsValid()
    {
        var createPlan = await CreatePlan();

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/planos/{createPlan.Id}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_ShouldReturnBadRequest_WhenIdIsInvalid()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/planos/invalid-id");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Get_ShouldReturnNotFound_WhenIdDoesNotExist()
    {
        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/planos/0");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region Get List

    [Fact]
    public async Task GetList_ShouldReturnOk_WhenRequestIsValid()
    {
        var createPlan = await CreatePlan();

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/planos/listar");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetList_ShouldReturnNotFound_WhenNoResultsFound()
    {
        await ForceDeleteAllPlan();

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"api/v1/planos/listar");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetListSearch_ShouldReturnOk_WhenSearchIsValid()
    {
        var createPlan = await CreatePlan();

        var plan = await GetPlan(createPlan.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var genericSearchPaginationRequest = new GenericSearchPaginationRequest();
        genericSearchPaginationRequest.Texto = plan.Descricao;
        genericSearchPaginationRequest.PaginaAtual = 1;
        genericSearchPaginationRequest.TamanhoPagina = 100;
        genericSearchPaginationRequest.CampoOrdenacao = "description";
        genericSearchPaginationRequest.DirecaoOrdenacao = "asc";

        var response = await _client.PostAsJsonAsync($"api/v1/planos/consultar", genericSearchPaginationRequest);

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

        var response = await _client.PostAsJsonAsync($"api/v1/planos/consultar", genericSearchPaginationRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task GetListSearch_ShouldReturnNotFound_WhenNoResultsFound()
    {
        await ForceDeleteAllPlan();

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var genericSearchPaginationRequest = new GenericSearchPaginationRequest();
        genericSearchPaginationRequest.Texto = "Teste";
        genericSearchPaginationRequest.PaginaAtual = 1;
        genericSearchPaginationRequest.TamanhoPagina = 100;
        genericSearchPaginationRequest.CampoOrdenacao = "description";
        genericSearchPaginationRequest.DirecaoOrdenacao = "asc";

        var response = await _client.PostAsJsonAsync($"api/v1/planos/consultar", genericSearchPaginationRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region Post

    [Fact]
    public async Task Post_ShouldReturnCreated_WhenIsValid()
    {
        var postPlanRequest = GenerateRandomPostPlanRequest(null, false, false);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/planos", postPlanRequest);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenDescriptionIsDuplicate()
    {
        var postPlanRequest = GenerateRandomPostPlanRequest(null, false, false);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        await _client.PostAsJsonAsync("api/v1/planos", postPlanRequest);

        var postPlanDuplicateDescriptionRequest = GenerateRandomPostPlanRequest(null, false, false);
        postPlanDuplicateDescriptionRequest.Descricao = postPlanRequest.Descricao;

        var response = await _client.PostAsJsonAsync("api/v1/planos", postPlanDuplicateDescriptionRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenPlanTypeIdIsMissing()
    {
        var postPlanRequest = GenerateRandomPostPlanRequest(FieldEnum.PlanTypeId, true, false);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/planos", postPlanRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenDescriptionIsMissing()
    {
        var postPlanRequest = GenerateRandomPostPlanRequest(FieldEnum.Description, true, false);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/planos", postPlanRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenDescriptionSizeIsInvalid()
    {
        var postPlanRequest = GenerateRandomPostPlanRequest(FieldEnum.Description, false, true);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/planos", postPlanRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenDailyRateIsMissing()
    {
        var postPlanRequest = GenerateRandomPostPlanRequest(FieldEnum.DailyRate, true, false);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/planos", postPlanRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenAdditionalRateIsMissing()
    {
        var postPlanRequest = GenerateRandomPostPlanRequest(FieldEnum.AdditionalRate, true, false);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/planos", postPlanRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenDailyLateFeeIsMissing()
    {
        var postPlanRequest = GenerateRandomPostPlanRequest(FieldEnum.DailyLateFee, true, false);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/planos", postPlanRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenDurationInDaysIsMissing()
    {
        var postPlanRequest = GenerateRandomPostPlanRequest(FieldEnum.DurationInDays, true, false);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("api/v1/planos", postPlanRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region Put

    [Fact]
    public async Task Put_ShouldReturnOk_WhenIsValid()
    {
        var createPlan = await CreatePlan();

        var plan = await GetPlan(createPlan.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putPlanRequest = GenerateRandomPutPlanRequest(null, false, false);

        var response = await _client.PutAsJsonAsync($"api/v1/planos/{plan.Id}", putPlanRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnOk_WhenActiveIsValid()
    {
        var createPlan = await CreatePlan();

        var plan = await GetPlan(createPlan.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putPlanSetActiveRequest = new PutPlanSetActiveRequest();
        putPlanSetActiveRequest.Ativo = false;

        var response = await _client.PutAsJsonAsync($"api/v1/planos/{plan.Id}/ativo", putPlanSetActiveRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenDescriptionIsDuplicate()
    {
        var createPlan = await CreatePlan();

        var plan = await GetPlan(createPlan.Id);

        var createPlanDuplicateDescription = await CreatePlan();

        var planDuplicateDescription = await GetPlan(createPlanDuplicateDescription.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putPlanDuplicateDescriptionRequest = GenerateRandomPutPlanRequest(null, false, false);
        putPlanDuplicateDescriptionRequest.Descricao = plan.Descricao;

        var response = await _client.PutAsJsonAsync($"api/v1/planos/{planDuplicateDescription.Id}", putPlanDuplicateDescriptionRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenPlanTypeIdIsMissing()
    {
        var createPlan = await CreatePlan();

        var plan = await GetPlan(createPlan.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putPlanRequest = GenerateRandomPutPlanRequest(FieldEnum.PlanTypeId, true, false);

        var response = await _client.PutAsJsonAsync($"api/v1/planos/{plan.Id}", putPlanRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenDescriptionIsMissing()
    {
        var createPlan = await CreatePlan();

        var plan = await GetPlan(createPlan.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putPlanRequest = GenerateRandomPutPlanRequest(FieldEnum.Description, true, false);

        var response = await _client.PutAsJsonAsync($"api/v1/planos/{plan.Id}", putPlanRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenDescriptionSizeIsInvalid()
    {
        var createPlan = await CreatePlan();

        var plan = await GetPlan(createPlan.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putPlanRequest = GenerateRandomPutPlanRequest(FieldEnum.Description, false, true);

        var response = await _client.PutAsJsonAsync($"api/v1/planos/{plan.Id}", putPlanRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenDailyRateIsMissing()
    {
        var createPlan = await CreatePlan();

        var plan = await GetPlan(createPlan.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putPlanRequest = GenerateRandomPutPlanRequest(FieldEnum.DailyRate, true, false);

        var response = await _client.PutAsJsonAsync($"api/v1/planos/{plan.Id}", putPlanRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenAdditionalRateIsMissing()
    {
        var createPlan = await CreatePlan();

        var plan = await GetPlan(createPlan.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putPlanRequest = GenerateRandomPutPlanRequest(FieldEnum.AdditionalRate, true, false);

        var response = await _client.PutAsJsonAsync($"api/v1/planos/{plan.Id}", putPlanRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenDailyLateFeeIsMissing()
    {
        var createPlan = await CreatePlan();

        var plan = await GetPlan(createPlan.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putPlanRequest = GenerateRandomPutPlanRequest(FieldEnum.DailyLateFee, true, false);

        var response = await _client.PutAsJsonAsync($"api/v1/planos/{plan.Id}", putPlanRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenDurationInDaysIsMissing()
    {
        var createPlan = await CreatePlan();

        var plan = await GetPlan(createPlan.Id);

        var token = await LoginAdmin();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var putPlanRequest = GenerateRandomPutPlanRequest(FieldEnum.DailyLateFee, true, false);

        var response = await _client.PutAsJsonAsync($"api/v1/planos/{plan.Id}", putPlanRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region Authorize

    [Fact]
    public async Task Authorize_ShouldReturnUnauthorized_WhenTokenIsMissing()
    {
        var createPlan = await CreatePlan();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", null);

        var response = await _client.GetAsync($"api/v1/planos/{createPlan.Id}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Authorize_ShouldReturnForbidden_WhenRoleIsInvalid()
    {
        var createPlan = await CreatePlan();

        var createDriver = await CreateDriver(CnhTypeEnum.A);

        var driver = await GetDriver(createDriver.Id);

        var token = await LoginDriver(driver.UsuarioId.Value, driver.Nome);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"api/v1/planos/{createPlan.Id}");

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    #endregion
}