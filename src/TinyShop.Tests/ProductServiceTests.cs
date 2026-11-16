using System.Net;
using System.Net.Http.Json;
using DataEntities;
using Store.Services;

namespace TinyShop.Tests;

[TestClass]
public class ProductServiceTests
{
    [TestMethod]
    public async Task GetProduct_WhenResponseIsSuccess_ReturnsProduct()
    {
        // Arrange
        var expected = new Product
        {
            Id = 7,
            Name = "Tent",
            Description = "2-person backpacking tent",
            Price = 129.99m,
            ImageUrl = "images/tent.jpg"
        };

        var handler = new StubHttpMessageHandler(_ =>
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(expected, ProductSerializerContext.Default.Product)
            });

        using var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://example.test")
        };

        var service = new ProductService(httpClient);

        // Act
        var product = await service.GetProduct(7);

        // Assert
        Assert.IsNotNull(product);
        Assert.AreEqual(expected.Id, product.Id);
        Assert.AreEqual(expected.Name, product.Name);
        Assert.AreEqual(expected.Description, product.Description);
        Assert.AreEqual(expected.Price, product.Price);
        Assert.AreEqual(expected.ImageUrl, product.ImageUrl);
    }

    [TestMethod]
    public async Task GetProduct_WhenResponseIsNotSuccess_ReturnsNull()
    {
        // Arrange
        var handler = new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.NotFound));
        using var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://example.test")
        };

        var service = new ProductService(httpClient);

        // Act
        var product = await service.GetProduct(999);

        // Assert
        Assert.IsNull(product);
    }

    [TestMethod]
    public async Task GetProduct_UsesExpectedApiPath()
    {
        // Arrange
        Uri? requestUri = null;

        var handler = new StubHttpMessageHandler(request =>
        {
            requestUri = request.RequestUri;
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        });

        using var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://example.test")
        };

        var service = new ProductService(httpClient);

        // Act
        await service.GetProduct(42);

        // Assert
        Assert.IsNotNull(requestUri);
        Assert.AreEqual("/api/Product/42", requestUri.AbsolutePath);
    }

    private sealed class StubHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> responseFactory)
        : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => Task.FromResult(responseFactory(request));
    }
}