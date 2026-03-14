using FluentAssertions;
using IntegrationTests.Fixture;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests.Api;

public class PaymentDetailsControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public PaymentDetailsControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task POST_Should_Return_201()
    {
        var response = await _client.PostAsJsonAsync("/payment-details", new
        {
            // test payload
        });

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}
