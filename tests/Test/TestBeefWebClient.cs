using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using BeefWebClient;
using BeefWebClient.Models;
using BeefWebClient.Parameters;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Test
{
    public class TestBeefWebClient
    {
        [Fact]
        public void HttpClient_dont_have_BaseAdress_should_throw_ArgumentException()
        {
            // Arrange
            var logger = A.Fake<ILogger<BeefWebService>>();
            var handler = A.Fake<Startup.FakeHttpMessageHandler>();
            var client = new HttpClient(handler);

            // Act
            Func<BeefWebService> action = () => new BeefWebService(client, logger);

            // Assert
            action.Should().Throw<ArgumentException>().Which.ParamName.Should().Be("client");
        }

        #region GetPlayerState

        [Theory]
        [Startup.InlineAutoFakeData]
        public async Task GetPlayerState_request_should_be_GET_method(
            PlayerStateResult result,
            TrackInfoFields[] fields,
            [Frozen] Startup.FakeHttpMessageHandler handler,
            BeefWebService sut)
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
            var valueResult = await sut.GetPlayerState(fields);

            // Assert
            request.Should().NotBeNull();
            request.Method.Should().Be(HttpMethod.Get);
        }

        [Theory]
        [Startup.InlineAutoFakeData]
        public async Task GetPlayerState_request_resource_should_be_player(
            PlayerStateResult result,
            TrackInfoFields[] fields,
            [Frozen] Startup.FakeHttpMessageHandler handler,
            BeefWebService sut)
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
            var valueResult = await sut.GetPlayerState(fields);

            // Assert
            request.RequestUri.AbsolutePath.Should().EndWith("/player");
        }

        [Theory]
        [Startup.InlineAutoFakeData]
        public async Task GetPlayerState_request_query_should_contains_columns_parameter(
            PlayerStateResult result,
            TrackInfoFields[] fields,
            [Frozen] Startup.FakeHttpMessageHandler handler,
            BeefWebService sut)
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
            var valueResult = await sut.GetPlayerState(fields);

            // Assert
            QueryHelpers.ParseQuery(request.RequestUri.Query)
                .Should()
                .ContainKey("columns");
        }

        [Theory]
        [Startup.InlineAutoFakeData]
        public async Task GetPlayerState_request_query_parameter_columns_should_contains_all_fields_with_coma_separator(
            PlayerStateResult result,
            TrackInfoFields[] fields,
            [Frozen] Startup.FakeHttpMessageHandler handler,
            BeefWebService sut)
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
            var valueResult = await sut.GetPlayerState(fields);

            // Assert
            var query = QueryHelpers.ParseQuery(request.RequestUri.Query);
            var allFields = query["columns"][0].Split(',');

            allFields.Should().Contain(fields.Select(f => BeefWebService.GetEnumDescription(f)));
        }

        [Theory]
        [Startup.InlineAutoFakeData]
        [Startup.InlineAutoFakeData]
        [Startup.InlineAutoFakeData]
        public async Task GetPlayerState_result_should_be_correctly_parsed(
            PlayerStateResult result,
            TrackInfoFields[] fields,
            [Frozen] Startup.FakeHttpMessageHandler handler,
            BeefWebService sut)
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
            var valueResult = await sut.GetPlayerState(fields);

            // Assert
            valueResult.Should().BeEquivalentTo(result);
        }

        #endregion

        #region SetPlayerState

        [Theory]
        [Startup.InlineAutoFakeData]
        public async Task SetPlayerState_request_should_be_POST(
            SetPlayerStateParameters parameters,
            [Frozen] Startup.FakeHttpMessageHandler handler,
            BeefWebService sut)
        {
            // Arrange
            HttpRequestMessage request = null;
            A.CallTo(() => handler.Send(A<HttpRequestMessage>._))
                .Invokes((HttpRequestMessage r) => request = r)
                .Returns(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(string.Empty)});

            // Act
            await sut.SetPlayerState(parameters);

            // Assert
            request.Should().NotBeNull();
            request.Method.Should().Be(HttpMethod.Post);
        }

        [Theory]
        [Startup.InlineAutoFakeData]
        public async Task SetPlayerState_request_resource_should_be_player(
            SetPlayerStateParameters parameters,
            [Frozen] Startup.FakeHttpMessageHandler handler,
            BeefWebService sut)
        {
            // Arrange
            HttpRequestMessage request = null;
            A.CallTo(() => handler.Send(A<HttpRequestMessage>._))
                .Invokes((HttpRequestMessage r) => request = r)
                .Returns(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(string.Empty) });

            // Act
            await sut.SetPlayerState(parameters);

            // Assert
            request.RequestUri.AbsolutePath.Should().EndWith("/player");
        }


        [Theory]
        [Startup.InlineAutoFakeData]
        public async Task SetPlayerState_request_should_not_contains_volume(
            SetPlayerStateParameters parameters,
            [Frozen] Startup.FakeHttpMessageHandler handler,
            BeefWebService sut)
        {
            // Arrange
            parameters.Volume = null;

            HttpRequestMessage request = null;
            A.CallTo(() => handler.Send(A<HttpRequestMessage>._))
                .Invokes((HttpRequestMessage r) => request = r)
                .Returns(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(string.Empty) });

            // Act
            await sut.SetPlayerState(parameters);

            // Assert
            QueryHelpers.ParseQuery(request.RequestUri.Query)
                .Should()
                .NotContainKey("volume");
        }

        [Theory]
        [Startup.InlineAutoFakeData]
        public async Task SetPlayerState_request_should_contains_volume_with_correct_format_F2(
            SetPlayerStateParameters parameters,
            [Frozen] Startup.FakeHttpMessageHandler handler,
            BeefWebService sut)
        {
            // Arrange
            HttpRequestMessage request = null;
            A.CallTo(() => handler.Send(A<HttpRequestMessage>._))
                .Invokes((HttpRequestMessage r) => request = r)
                .Returns(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(string.Empty) });

            // Act
            await sut.SetPlayerState(parameters);

            // Assert
            QueryHelpers.ParseQuery(request.RequestUri.Query)
                .Should()
                .ContainKey("volume")
                .WhichValue.Should().Equal(parameters.Volume.Value.ToString("F2", CultureInfo.InvariantCulture));
        }

        [Theory]
        [Startup.InlineAutoFakeData]
        public async Task SetPlayerState_request_should_not_contains_isMuted(
            SetPlayerStateParameters parameters,
            [Frozen] Startup.FakeHttpMessageHandler handler,
            BeefWebService sut)
        {
            // Arrange
            parameters.IsMuted = null;

            HttpRequestMessage request = null;
            A.CallTo(() => handler.Send(A<HttpRequestMessage>._))
                .Invokes((HttpRequestMessage r) => request = r)
                .Returns(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(string.Empty) });

            // Act
            await sut.SetPlayerState(parameters);

            // Assert
            QueryHelpers.ParseQuery(request.RequestUri.Query)
                .Should()
                .NotContainKey("isMuted");
        }

        [Theory]
        [Startup.InlineAutoFakeData]
        public async Task SetPlayerState_request_should_contains_isMuted_with_correct_value(
            SetPlayerStateParameters parameters,
            [Frozen] Startup.FakeHttpMessageHandler handler,
            BeefWebService sut)
        {
            // Arrange
            HttpRequestMessage request = null;
            A.CallTo(() => handler.Send(A<HttpRequestMessage>._))
                .Invokes((HttpRequestMessage r) => request = r)
                .Returns(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(string.Empty) });

            // Act
            await sut.SetPlayerState(parameters);

            // Assert
            QueryHelpers.ParseQuery(request.RequestUri.Query)
                .Should()
                .ContainKey("isMuted")
                .WhichValue.Should().Equal(parameters.IsMuted.ToString());
        }

        [Theory]
        [Startup.InlineAutoFakeData]
        public async Task SetPlayerState_request_should_not_contains_position(
            SetPlayerStateParameters parameters,
            [Frozen] Startup.FakeHttpMessageHandler handler,
            BeefWebService sut)
        {
            // Arrange
            parameters.Position = null;

            HttpRequestMessage request = null;
            A.CallTo(() => handler.Send(A<HttpRequestMessage>._))
                .Invokes((HttpRequestMessage r) => request = r)
                .Returns(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(string.Empty) });

            // Act
            await sut.SetPlayerState(parameters);

            // Assert
            QueryHelpers.ParseQuery(request.RequestUri.Query)
                .Should()
                .NotContainKey("position");
        }

        [Theory]
        [Startup.InlineAutoFakeData]
        public async Task SetPlayerState_request_should_contains_position_with_correct_value(
            SetPlayerStateParameters parameters,
            [Frozen] Startup.FakeHttpMessageHandler handler,
            BeefWebService sut)
        {
            // Arrange
            HttpRequestMessage request = null;
            A.CallTo(() => handler.Send(A<HttpRequestMessage>._))
                .Invokes((HttpRequestMessage r) => request = r)
                .Returns(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(string.Empty) });

            // Act
            await sut.SetPlayerState(parameters);

            // Assert
            QueryHelpers.ParseQuery(request.RequestUri.Query)
                .Should()
                .ContainKey("position")
                .WhichValue.Should().Equal(parameters.Position.ToString());
        }

        [Theory]
        [Startup.InlineAutoFakeData]
        public async Task SetPlayerState_request_should_not_contains_relativePosition(
            SetPlayerStateParameters parameters,
            [Frozen] Startup.FakeHttpMessageHandler handler,
            BeefWebService sut)
        {
            // Arrange
            parameters.RelativePosition = null;

            HttpRequestMessage request = null;
            A.CallTo(() => handler.Send(A<HttpRequestMessage>._))
                .Invokes((HttpRequestMessage r) => request = r)
                .Returns(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(string.Empty) });

            // Act
            await sut.SetPlayerState(parameters);

            // Assert
            QueryHelpers.ParseQuery(request.RequestUri.Query)
                .Should()
                .NotContainKey("relativePosition");
        }

        [Theory]
        [Startup.InlineAutoFakeData]
        public async Task SetPlayerState_request_should_contains_relativePosition_with_correct_value(
            SetPlayerStateParameters parameters,
            [Frozen] Startup.FakeHttpMessageHandler handler,
            BeefWebService sut)
        {
            // Arrange
            HttpRequestMessage request = null;
            A.CallTo(() => handler.Send(A<HttpRequestMessage>._))
                .Invokes((HttpRequestMessage r) => request = r)
                .Returns(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(string.Empty) });

            // Act
            await sut.SetPlayerState(parameters);

            // Assert
            QueryHelpers.ParseQuery(request.RequestUri.Query)
                .Should()
                .ContainKey("relativePosition")
                .WhichValue.Should().Equal(parameters.RelativePosition.ToString());
        }

        [Theory]
        [Startup.InlineAutoFakeData]
        public async Task SetPlayerState_request_should_not_contains_playbackMode(
            SetPlayerStateParameters parameters,
            [Frozen] Startup.FakeHttpMessageHandler handler,
            BeefWebService sut)
        {
            // Arrange
            parameters.PlaybackMode = null;

            HttpRequestMessage request = null;
            A.CallTo(() => handler.Send(A<HttpRequestMessage>._))
                .Invokes((HttpRequestMessage r) => request = r)
                .Returns(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(string.Empty) });

            // Act
            await sut.SetPlayerState(parameters);

            // Assert
            QueryHelpers.ParseQuery(request.RequestUri.Query)
                .Should()
                .NotContainKey("playbackMode");
        }

        [Theory]
        [Startup.InlineAutoFakeData]
        public async Task SetPlayerState_request_should_contains_playbackMode_with_correct_value(
            SetPlayerStateParameters parameters,
            [Frozen] Startup.FakeHttpMessageHandler handler,
            BeefWebService sut)
        {
            // Arrange
            HttpRequestMessage request = null;
            A.CallTo(() => handler.Send(A<HttpRequestMessage>._))
                .Invokes((HttpRequestMessage r) => request = r)
                .Returns(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(string.Empty) });

            // Act
            await sut.SetPlayerState(parameters);

            // Assert
            QueryHelpers.ParseQuery(request.RequestUri.Query)
                .Should()
                .ContainKey("playbackMode")
                .WhichValue.Should().Equal(parameters.PlaybackMode.ToString());
        }
        #endregion

        ///// <summary>
        ///// Exemple de test de Query Parameters
        ///// </summary>
        ///// <param name="search">Valeur du parametre, injecté automatique par AutoFixture</param>
        ///// <param name="value">Valeur de retour, injecté automatique par AutoFixture</param>
        ///// <param name="handler">Handler http permettant le mock du serveur, injecté automatiquement par AutoFixture</param>
        ///// <param name="sut">Client à tester, injecté automatiquement par AutoFixture</param>
        ///// <returns>Résultat du test</returns>
        //[Theory]
        //[Startup.InlineAutoFakeDataAttribute]
        //[Startup.InlineAutoFakeDataAttribute]
        //[Startup.InlineAutoFakeDataAttribute]
        //public async Task Si_SearchValues_AvecQueryParameters(
        //    string search,
        //    Value[] values,
        //    [Frozen] Startup.FakeHttpMessageHandler handler,
        //    ValueServiceClient sut)
        //{
        //    // Arrange
        //    HttpRequestMessage request = null;
        //    A.CallTo(() => handler.Send(A<HttpRequestMessage>._))
        //        .Invokes((HttpRequestMessage r) => request = r)
        //        .Returns(
        //            new HttpResponseMessage(HttpStatusCode.OK)
        //            {
        //                Content = new ByteArrayContent(JsonSerializer.SerializeToUtf8Bytes(values))
        //            });

        //    // Act
        //    var valueResult = await sut.SearchValues(search);

        //    // Assert
        //    QueryHelpers.ParseQuery(request.RequestUri.Query)
        //        .Should()
        //        .ContainKey("search")
        //        .WhichValue.Should()
        //        .Equal(search);
        //}


        ///// <summary>
        ///// Exemple de test de Body
        ///// </summary>
        ///// <param name="id">Id pour construire l'uri de post, injecté automatiquement par AutoFixture</param>
        ///// <param name="value">Valeur posté par le client, injecté automatiquement par AutoFixture</param>
        ///// <param name="valueReturn">Valeur retourné par le client http mock</param>
        ///// <param name="handler">Handler http permettant le mock du serveur, injecté automatiquement par AutoFixture</param>
        ///// <param name="sut">Client à tester, injecté automatiquement par AutoFixture</param>
        ///// <returns>Resultat du test</returns>
        //[Theory]
        //[Startup.InlineAutoFakeDataAttribute]
        //[Startup.InlineAutoFakeDataAttribute]
        //[Startup.InlineAutoFakeDataAttribute]
        //public async Task Si_AddValue_CorpsRequeteValide(
        //    string id,
        //    int value,
        //    Value valueReturn,
        //    [Frozen] Startup.FakeHttpMessageHandler handler,
        //    ValueServiceClient sut)
        //{
        //    // Arrange
        //    HttpRequestMessage request = null;
        //    A.CallTo(() => handler.Send(A<HttpRequestMessage>._))
        //        .Invokes((HttpRequestMessage r) => request = r)
        //        .Returns(
        //            new HttpResponseMessage(HttpStatusCode.OK)
        //            {
        //                Content = new ByteArrayContent(JsonSerializer.SerializeToUtf8Bytes(valueReturn))
        //            });

        //    // Act
        //    var idResult = await sut.AddValue(id, value);

        //    // Assert
        //    var contentPost = JsonSerializer.Deserialize<int>(await request.Content.ReadAsStringAsync());
        //    contentPost.Should()
        //        .Be(value);
        //}


        ///// <summary>
        ///// Exemple de test de Header Scheme
        ///// </summary>
        ///// <param name="values">Valeurs retournés par le serveur mocké, injecté automatiquement par FakeItEasy</param>
        ///// <param name="token">Token dans le header passé par le client, injecté automatiquement par FakeItEasy</param>
        ///// <param name="handler">Handler http permettant le mock du serveur, injecté automatiquement par FakeItEasy</param>
        ///// <param name="sut">Client à tester, injecté automatiquement par FakeItEasy</param>
        ///// <returns>Resultat du test</returns>
        //[Theory]
        //[Startup.InlineAutoFakeDataAttribute]
        //[Startup.InlineAutoFakeDataAttribute]
        //[Startup.InlineAutoFakeDataAttribute]
        //public async Task Si_GetValuesWithToken_WithTokenBearerScheme(
        //    Value[] values,
        //    string token,
        //    [Frozen] Startup.FakeHttpMessageHandler handler,
        //    ValueServiceClient sut)
        //{
        //    // Arrange
        //    HttpRequestMessage request = null;
        //    A.CallTo(() => handler.Send(A<HttpRequestMessage>._))
        //        .Invokes((HttpRequestMessage r) => request = r)
        //        .Returns(
        //            new HttpResponseMessage(HttpStatusCode.OK)
        //            {
        //                Content = new ByteArrayContent(JsonSerializer.SerializeToUtf8Bytes(values))
        //            });

        //    // Act
        //    var valuesResult = await sut.GetValuesWithToken(token);

        //    // Assert
        //    request.Headers.Authorization.Scheme.Should()
        //        .Be("Bearer");
        //}

        ///// <summary>
        ///// Exemple de test de Header value
        ///// </summary>
        ///// <param name="values">Valeurs retournés par le serveur mocké, injecté automatiquement par FakeItEasy</param>
        ///// <param name="token">Token dans le header passé par le client, injecté automatiquement par FakeItEasy</param>
        ///// <param name="handler">Handler http permettant le mock du serveur, injecté automatiquement par FakeItEasy</param>
        ///// <param name="sut">Client à tester, injecté automatiquement par FakeItEasy</param>
        ///// <returns>Resultat du test</returns>
        //[Theory]
        //[Startup.InlineAutoFakeDataAttribute]
        //[Startup.InlineAutoFakeDataAttribute]
        //[Startup.InlineAutoFakeDataAttribute]
        //public async Task Si_GetValuesWithToken_WithTokenBearerValue(
        //    Value[] values,
        //    string token,
        //    [Frozen] Startup.FakeHttpMessageHandler handler,
        //    ValueServiceClient sut)
        //{
        //    // Arrange
        //    HttpRequestMessage request = null;
        //    A.CallTo(() => handler.Send(A<HttpRequestMessage>._))
        //        .Invokes((HttpRequestMessage r) => request = r)
        //        .Returns(
        //            new HttpResponseMessage(HttpStatusCode.OK)
        //            {
        //                Content = new ByteArrayContent(JsonSerializer.SerializeToUtf8Bytes(values))
        //            });

        //    // Act
        //    var valuesResult = await sut.GetValuesWithToken(token);

        //    // Assert
        //    request.Headers.Authorization.Parameter.Should()
        //        .Be(token);
        //}
    }
}
