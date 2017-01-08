using System;
using NUnit.Framework;
using FluentAssertions;
using System.Web;
using Moq;
using System.Web.Routing;
using System.Security.Principal;
using System.Collections.Specialized;

namespace NanoCart.Tests
{
    [TestFixture]
    public class CartTests
    {
        [Test]
        public void TestMethod1()
        {
            var context = TestUtilities.GetMockedHttpContext();
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