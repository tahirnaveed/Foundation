using EPiServer.ServiceLocation;
using FluentAssertions;
using Foundation.Cms.Services;
using Moq;
using System;
using System.Web;
using Xunit;

namespace Foundation.Cms.Tests.Services
{
    public class CookieServiceTests
    {
        [Fact]
        public void Get_WhenSuccess()
        {
            _httpRequestBaseMock.Setup(x => x.Cookies).Returns(SetupCollection("id", "Cookie"));
            var result = _subject.Get("id");
            result.Should().Be("Cookie");
        }

        [Fact]
        public void Get_WhenHttpContextBaseIsNull()
        {
            _subject = new CookieService(Setup_ServiceAccessors<HttpContextBase>(null));
            var result = _subject.Get("id");
            result.Should().BeNull();
        }

        [Fact]
        public void Set_WhenHttpContextBaseIsNull()
        {
            _subject = new CookieService(Setup_ServiceAccessors<HttpContextBase>(null));
            _subject.Set("test", "value");
            _httpContextBase.Verify(x => x.Response, Times.Never);
            _httpContextBase.Verify(x => x.Request, Times.Never);
        }

        [Fact]
        public void Set_WhenSuccess()
        {
            var requestCookies = SetupCollection("id", "Cookie");
            _httpRequestBaseMock.Setup(x => x.Cookies).Returns(requestCookies);
            var response = new Mock<HttpResponseBase>();
            var responseCookies = SetupCollection("id", "Cookie");
            _httpContextBase.Setup(x => x.Response).Returns(response.Object);
            response.Setup(x => x.Cookies).Returns(responseCookies);
            _subject.Set("test", "value");
            responseCookies.Should().BeEquivalentTo(responseCookies);
        }

        [Fact]
        public void Set_WhenSuccessAndIsSessionCookie()
        {
            var requestCookies = SetupCollection("id", "Cookie");
            _httpRequestBaseMock.Setup(x => x.Cookies).Returns(requestCookies);
            var response = new Mock<HttpResponseBase>();
            var responseCookies = SetupCollection("id", "Cookie");
            _httpContextBase.Setup(x => x.Response).Returns(response.Object);
            response.Setup(x => x.Cookies).Returns(responseCookies);
            _subject.Set("test", "value");
            responseCookies["test"].Expires.Should().BeAfter(DateTime.Now.AddMonths(11));
        }

        [Fact]
        public void Remove_WhenHttpContextBaseIsNull()
        {
            _subject = new CookieService(Setup_ServiceAccessors<HttpContextBase>(null));
            _subject.Remove("test");
            _httpContextBase.Verify(x => x.Response, Times.Never);
            _httpContextBase.Verify(x => x.Request, Times.Never);
        }

        [Fact]
        public void Remove_WhenSuccess()
        {
            var requestCookies = SetupCollection("id", "Cookie");
            _httpRequestBaseMock.Setup(x => x.Cookies).Returns(requestCookies);
            var response = new Mock<HttpResponseBase>();
            var responseCookies = SetupCollection("id", "Cookie");
            _httpContextBase.Setup(x => x.Response).Returns(response.Object);
            response.Setup(x => x.Cookies).Returns(responseCookies);
            _subject.Remove("test");
            responseCookies["test"].Expires.Should().BeBefore(DateTime.Now.AddHours(-23));
        }

        public CookieServiceTests()
        {
            _httpContextBase = new Mock<HttpContextBase>();
            _httpRequestBaseMock = new Mock<HttpRequestBase>();
            _httpContextBase.SetupGet(x => x.Request).Returns(_httpRequestBaseMock.Object);
            _subject = new CookieService(Setup_ServiceAccessors(_httpContextBase.Object));
        }

        private CookieService _subject;
        private Mock<HttpContextBase> _httpContextBase;
        private Mock<HttpRequestBase> _httpRequestBaseMock;

        private ServiceAccessor<T> Setup_ServiceAccessors<T>(T value)
        {
            return new ServiceAccessor<T>(() => value);
        }

        private HttpCookieCollection SetupCollection(string name, string value)
        {
            var collection = new HttpCookieCollection();
            collection.Add(new HttpCookie(name, value));
            return collection;
        }
    }
}
