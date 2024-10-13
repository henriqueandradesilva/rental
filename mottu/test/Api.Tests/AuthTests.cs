using Api.Tests.Common.Factories;
using CrossCutting.Dtos.Auth.Request;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Api.Tests;

public class AuthTests(
    WebApplicationFactory<Program> factory) : CustomWebApplicationFactory(factory)
{
    [Fact]
    public async Task Post_ShouldReturnOk_WhenCredentialsAreValid()
    {
        var postAuthRequest = new PostAuthRequest("admin@mottu.app", "1");

        var response = await _client.PostAsJsonAsync("api/v1/login", postAuthRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
    {
        var postAuthRequest = new PostAuthRequest("admin@mottu.app", "Sizepassword");

        var response = await _client.PostAsJsonAsync("api/v1/login", postAuthRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenEmailIsMissing()
    {
        var postAuthRequest = new PostAuthRequest(null, "1234");

        var response = await _client.PostAsJsonAsync("api/v1/login", postAuthRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenPasswordIsMissing()
    {
        var postAuthRequest = new PostAuthRequest("admin@mottu.app", null);

        var response = await _client.PostAsJsonAsync("api/v1/login", postAuthRequest);

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenAccountIsNotActive()
    {
        var postDriverRequest = GenerateRandomPostDriverRequest(null, false, false);

        var response = await _client.PostAsJsonAsync("api/v1/entregadores", postDriverRequest);

        var content = await response.Content.ReadAsStringAsync();

        var postAuthRequest = new PostAuthRequest($"{postDriverRequest.Nome}@mottu.app", "1234");

        response = await _client.PostAsJsonAsync("api/v1/login", postAuthRequest);

        content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}