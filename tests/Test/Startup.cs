using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoFakeItEasy;
using AutoFixture.Xunit2;
using Castle.Core.Logging;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PlexClient;
using PlexClient.Client;

namespace Test
{
    public class Startup
    {

        public abstract class FakeHttpMessageHandler : HttpMessageHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return Task.FromResult(Send(request));
            }

            public abstract HttpResponseMessage Send(HttpRequestMessage request);
        }

        [AttributeUsage(AttributeTargets.Method)]
        public class AutoFakeDataAttribute : AutoDataAttribute
        {
            public AutoFakeDataAttribute() : base(FakeFixtureFactory.Create)
            {
            }
        }

        [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
        public class InlineAutoFakeDataAttribute : InlineAutoDataAttribute
        {
            public InlineAutoFakeDataAttribute(params object[] values)
                : base(new AutoFakeDataAttribute(), values)
            {
            }
        }

        public sealed class FakeFixtureFactory
        {
            public static IFixture Create()
            {
                var fixture = new Fixture()
                    .Customize(new AutoFakeItEasyCustomization());

                fixture.Register(() => A.Fake<FakeHttpMessageHandler>(x => x.Strict().CallsBaseMethods()));
                fixture.Register<FakeHttpMessageHandler, Uri, HttpClient>(
                    (handler, baseAdress) => new HttpClient(handler) { BaseAddress = baseAdress });

                fixture.Register(A.Fake<ILogger<PlexService>>);
                fixture.Register<IOptions<PlexOptions>>(() => new OptionsWrapper<PlexOptions>(new PlexOptions
                {
                    PlexToken = new Fixture().Create<string>()
                }));

                return fixture;
            }
        }
    }
}
