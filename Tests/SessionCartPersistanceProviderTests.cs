using System;
using System.Web;
using NUnit.Framework;
using FluentAssertions;
using Moq;

namespace NanoCart.Tests
{
    [TestFixture]
    public class SessionCartPersistanceProviderTests
    {
        private String _cartId = "fb622491-d7aa-4d94-bb08-3ada030c9330";

        [Test]
        public void GetSessionKey_Returns_Valid_CartSpecific_Key()
        {
            var context = TestUtilities.GetMockedHttpContext();
            var persistanceProvider = new Providers.SessionCartPersistanceProvider(context.Session);

            var sessionKey = persistanceProvider.GetSessionKey(cartId);

            sessionKey.Should().Be("nanocart_fb622491-d7aa-4d94-bb08-3ada030c9330");
        }

        [Test]
        public void Add_Will_Set_Cart_If_None_Already_Present()
        {
            var context = TestUtilities.GetMockedHttpContext();
            var persistanceProvider = new Providers.SessionCartPersistanceProvider(context.Session);
            var cart = new StandardNanoCart(persistanceProvider) { CartId = new Guid(_cartId) };

            persistanceProvider.Add(cart);

            var sessionKey = persistanceProvider.GetSessionKey(cart.CartId);
            var retrievedCart = context.Session[sessionKey];

            retrievedCart.Should().NotBeNull("The cart object was not saved to the session");
        }

        [Test]
        public void Add_Will_Throw_Exception_If_Cart_Already_Present()
        {
            var context = TestUtilities.GetMockedHttpContext();
            var persistanceProvider = new Providers.SessionCartPersistanceProvider(context.Session);
            var cart = new StandardNanoCart(persistanceProvider) { CartId = new Guid(_cartId) };

            persistanceProvider.Add(cart);

            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => { persistanceProvider.Add(cart); });

            ex.Message.Should().Be("An existing cart has already been saved. Use Update instead.");
        }

        [Test]
        public void TestMethod1()
        {
            var context = TestUtilities.GetMockedHttpContext();
            var persistanceProvider = new Providers.SessionCartPersistanceProvider(context.Session);
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