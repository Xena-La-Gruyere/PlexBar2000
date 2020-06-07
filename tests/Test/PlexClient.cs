using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PlexClient;
using PlexClient.Models;
using Xunit;

namespace Test
{
    public class PlexClient
    {
        [Fact]
        public void HttpClient_dont_have_BaseAdress_should_throw_ArgumentException()
        {
            // Arrange
            var logger = A.Fake<ILogger<PlexService>>();
            var handler = A.Fake<Startup.FakeHttpMessageHandler>();
            var client = new HttpClient(handler) {BaseAddress = null};
            var options = A.Fake<IOptions<PlexOptions>>();

            // Act
            Func<PlexService> action = () => new PlexService(client, options, logger);

            // Assert
            action.Should().Throw<ArgumentException>().Which.ParamName.Should().Be("client");
        }

        [Fact]
        public void Service_dont_have_PlexToken_should_throw_ArgumentException()
        {
            // Arrange
            var logger = A.Fake<ILogger<PlexService>>();
            var handler = A.Fake<Startup.FakeHttpMessageHandler>();
            var client = new HttpClient(handler) {BaseAddress = new Fixture().Create<Uri>()};
            var options = A.Fake<IOptions<PlexOptions>>();
            options.Value.PlexToken = null;

            // Act
            Func<PlexService> action = () => new PlexService(client, options, logger);

            // Assert
            action.Should().Throw<ArgumentException>().Which.ParamName.Should().Be("PlexToken");
        }

        #region GetSections

        [Theory]
        [Startup.InlineAutoFakeDataAttribute]
        public async Task GetSections_request_should_be_GET_method(
            Sections result,
            [Frozen] Startup.FakeHttpMessageHandler handler,
            PlexService sut)
        {
            // Arrange
            HttpRequestMessage request = null;
            A.CallTo(() => handler.Send(A<HttpRequestMessage>._))
                .Invokes((HttpRequestMessage r) => request = r)
                .Returns(
                    new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ByteArrayContent(JsonSerializer.SerializeToUtf8Bytes(result)),
                    });

            // Act
            var valueResult = await sut.GetSections();

            // Assert
            request.Should().NotBeNull();
            request.Method.Should().Be(HttpMethod.Get);
        }

        [Theory]
        [Startup.InlineAutoFakeDataAttribute]
        public async Task GetSections_request_should_have_PlexToken(
            Sections result,
            [Frozen] IOptions<PlexOptions> options,
            [Frozen] Startup.FakeHttpMessageHandler handler,
            PlexService sut)
        {
            // Arrange
            HttpRequestMessage request = null;
            A.CallTo(() => handler.Send(A<HttpRequestMessage>._))
                .Invokes((HttpRequestMessage r) => request = r)
                .Returns(
                    new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ByteArrayContent(JsonSerializer.SerializeToUtf8Bytes(result)),
                    });

            // Act
            var valueResult = await sut.GetSections();

            // Assert
            QueryHelpers.ParseQuery(request.RequestUri.Query)
                .Should()
                .ContainKey("X-Plex-Token")
                .WhichValue.Should().Equal(options.Value.PlexToken);
        }

        [Theory]
        [Startup.InlineAutoFakeDataAttribute]
        public async Task GetSections_request_resource_should_be_library_sectons(
            Sections result,
            [Frozen] Startup.FakeHttpMessageHandler handler,
            PlexService sut)
        {
            // Arrange
            HttpRequestMessage request = null;
            A.CallTo(() => handler.Send(A<HttpRequestMessage>._))
                .Invokes((HttpRequestMessage r) => request = r)
                .Returns(
                    new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ByteArrayContent(JsonSerializer.SerializeToUtf8Bytes(result)),
                    });

            // Act
            var valueResult = await sut.GetSections();

            // Assert
            request.RequestUri.AbsolutePath.Should().EndWith("/library/sections");
        }

        [Theory]
        [Startup.InlineAutoFakeDataAttribute]
        public async Task GetSections_should_be_correctly_parsed(
            Sections result,
            [Frozen] Startup.FakeHttpMessageHandler handler,
            PlexService sut)
        {
            // Arrange
            HttpRequestMessage request = null;
            A.CallTo(() => handler.Send(A<HttpRequestMessage>._))
                .Invokes((HttpRequestMessage r) => request = r)
                .Returns(
                    new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ByteArrayContent(JsonSerializer.SerializeToUtf8Bytes(result)),
                    });

            // Act
            var valueResult = await sut.GetSections();

            // Assert
            valueResult.Should().BeEquivalentTo(result);
        }

        #endregion


        #region GetAllArtists

        [Theory]
        [Startup.InlineAutoFakeDataAttribute]
        public async Task GetAllArtists_request_should_be_GET_method(
            AllArtists result,
            string sectionKey,
            [Frozen] Startup.FakeHttpMessageHandler handler,
            PlexService sut)
        {
            // Arrange
            HttpRequestMessage request = null;
            A.CallTo(() => handler.Send(A<HttpRequestMessage>._))
                .Invokes((HttpRequestMessage r) => request = r)
                .Returns(
                    new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ByteArrayContent(JsonSerializer.SerializeToUtf8Bytes(result)),
                    });

            // Act
            var valueResult = await sut.GetAllArtists(sectionKey);

            // Assert
            request.Should().NotBeNull();
            request.Method.Should().Be(HttpMethod.Get);
        }

        [Theory]
        [Startup.InlineAutoFakeDataAttribute]
        public async Task GetAllArtists_request_should_have_PlexToken(
            AllArtists result,
            string sectionKey,
            [Frozen] IOptions<PlexOptions> options,
            [Frozen] Startup.FakeHttpMessageHandler handler,
            PlexService sut)
        {
            // Arrange
            HttpRequestMessage request = null;
            A.CallTo(() => handler.Send(A<HttpRequestMessage>._))
                .Invokes((HttpRequestMessage r) => request = r)
                .Returns(
                    new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ByteArrayContent(JsonSerializer.SerializeToUtf8Bytes(result)),
                    });

            // Act
            var valueResult = await sut.GetAllArtists(sectionKey);

            // Assert
            QueryHelpers.ParseQuery(request.RequestUri.Query)
                .Should()
                .ContainKey("X-Plex-Token")
                .WhichValue.Should().Equal(options.Value.PlexToken);
        }

        [Theory]
        [Startup.InlineAutoFakeDataAttribute]
        public async Task GetAllArtists_request_resource_should_be_library_sectons_key_all(
            AllArtists result,
            string sectionKey,
            [Frozen] Startup.FakeHttpMessageHandler handler,
            PlexService sut)
        {
            // Arrange
            HttpRequestMessage request = null;
            A.CallTo(() => handler.Send(A<HttpRequestMessage>._))
                .Invokes((HttpRequestMessage r) => request = r)
                .Returns(
                    new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ByteArrayContent(JsonSerializer.SerializeToUtf8Bytes(result)),
                    });

            // Act
            var valueResult = await sut.GetAllArtists(sectionKey);

            // Assert
            request.RequestUri.AbsolutePath.Should().EndWith($"/library/sections/{sectionKey}/all");
        }

        [Theory]
        [Startup.InlineAutoFakeDataAttribute]
        public async Task GetAllArtists_should_be_correctly_parsed(
            AllArtists result,
            string sectionKey,
            [Frozen] Startup.FakeHttpMessageHandler handler,
            PlexService sut)
        {
            // Arrange
            HttpRequestMessage request = null;
            A.CallTo(() => handler.Send(A<HttpRequestMessage>._))
                .Invokes((HttpRequestMessage r) => request = r)
                .Returns(
                    new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ByteArrayContent(JsonSerializer.SerializeToUtf8Bytes(result)),
                    });

            // Act
            var valueResult = await sut.GetAllArtists(sectionKey);

            // Assert
            valueResult.Should().BeEquivalentTo(result);
        }

        #endregion
    }
}
