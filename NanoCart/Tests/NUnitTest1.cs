using System;
using NUnit.Framework;
using FluentAssertions;
using NanoCart;
using System.Web;
using Moq;
using System.Web.Routing;
using System.Security.Principal;
using System.Collections.Specialized;

namespace Tests
{
    [TestFixture]
    public class NUnitTest1
    {
        private HttpContextBase GetMockedHttpContext()
        {
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var session = new Mock<HttpSessionStateBase>();
            var server = new Mock<HttpServerUtilityBase>();
            var user = new Mock<IPrincipal>();
            var identity = new Mock<IIdentity>();

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

        [Test]
        public void TestMethod1()
        {
            var context = GetMockedHttpContext();
            var persistanceProvider = new NanoCart.Providers.SessionCartPersistanceProvider(context.Session);
            var cart = new StandardNanoCart(persistanceProvider);

            var cartItem = new StandardNanoCartItem
            {
                Quantity = 1,
                Amount = 10,
                CartItemId = Guid.NewGuid()
            };

            // add cart item
            cart.AddCartItem(cartItem);

            // add another cart item to increase quantity
            cart.AddCartItem(cartItem);

            var cartItem2 = new StandardNanoCartItem
            {
                Quantity = 1,
                Amount = 10,
                CartItemId = Guid.NewGuid()
            };

            // add another cart item to ensure multiples are accepted
            cart.AddCartItem(cartItem2);

            // Save to Session
            cart.Persist();

            // Retrieve from Session     
            cart.GetCart(cart.CartId);

            cart.RemoveCartItem(cartItem2);

            // Get Cart Item Total Amount

            // Get Total Cart Items Count

            // Get Total Cart quantity

            // Get Discount

            // Get Shipping

            // Get Tax 1

            // Get Tax 2


            // Delete cart
            cart.Delete();
        }
    }
}