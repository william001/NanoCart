using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Principal;
using System.Web;
using System.Web.Routing;

namespace NanoCart.Tests
{
    /// <summary>
    /// HTTP session mockup.
    /// </summary>
    public class HttpSessionStateBaseProxy : HttpSessionStateBase
    {
        public virtual void SetItem(string key, object value)
        {
            base[key] = value;
        }

        public override object this[string name]
        {
            get
            {
                return base[name];
            }
            set
            {
                SetItem(name, value);
            }
        }

    }

    public static class TestUtilities
    {

        public static HttpContextBase GetMockedHttpContext()
        {
            var stateData = new Hashtable();

            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var session = new Mock<HttpSessionStateBaseProxy>();
            var server = new Mock<HttpServerUtilityBase>();
            var user = new Mock<IPrincipal>();
            var identity = new Mock<IIdentity>();

            session.CallBase = true;
            session.Setup(x => x.SetItem(It.IsAny<string>(), It.IsAny<object>()))
                    .Callback<string, object>((string index, object value) =>
                    {
                        if (!stateData.Contains(index)) stateData.Add(index, value);
                        else stateData[index] = value;


                    });

            session.Setup(s => s[It.IsAny<string>()]).Returns<string>(key =>
            {

                return stateData[key];

            });

            var requestContext = new Mock<RequestContext>();
            requestContext.Setup(x => x.HttpContext).Returns(context.Object);
            context.Setup(ctx => ctx.Request).Returns(request.Object);
            context.Setup(ctx => ctx.Response).Returns(response.Object);
            context.Setup(ctx => ctx.Session).Returns(session.Object);
            context.Setup(ctx => ctx.Server).Returns(server.Object);
            context.Setup(ctx => ctx.User).Returns(user.Object);
            user.Setup(ctx => ctx.Identity).Returns(identity.Object);
            identity.Setup(id => id.IsAuthenticated).Returns(true);
            identity.Setup(id => id.Name).Returns("Username");
            request.Setup(req => req.Url).Returns(new Uri("http://www.google.com"));
            request.Setup(req => req.RequestContext).Returns(requestContext.Object);
            requestContext.Setup(x => x.RouteData).Returns(new RouteData());
            request.SetupGet(req => req.Headers).Returns(new NameValueCollection());

            return context.Object;
        }

    }
}
