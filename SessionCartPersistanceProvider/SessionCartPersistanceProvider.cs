using NanoCart;
using System;
using System.Web;
using SessionCartPersistanceProvider.Interfaces;

namespace NanoCart.Providers
{
    public class SessionCartPersistanceProvider : ICartPersistanceProvider
    {
        private HttpSessionStateBase _sessionBase;

        public SessionCartPersistanceProvider(HttpSessionStateBase sessionBase)
        {
            _sessionBase = sessionBase;
        }

        public void Add(INanoCart cart)
        {
            if (cart == null)
                throw new ArgumentNullException("The cart cannot be null");

            if (_sessionBase == null)
                throw new InvalidOperationException("Session cannot be null");

            var existingCart = GetCartById(cart.CartId);
            if (existingCart != null)
                throw new InvalidOperationException("An existing cart has already been saved. Use Update instead.");

            var sessionKey = GetSessionKey(cart.CartId);
            _sessionBase[sessionKey] = cart;
        }

        public void Update(INanoCart cart)
        {
            if (cart == null)
                throw new ArgumentNullException("The cart cannot be null");

            if (_sessionBase == null)
                throw new InvalidOperationException("Session cannot be null");

            var sessionKey = GetSessionKey(cart.CartId);
            _sessionBase[sessionKey] = cart;
        }

        public void Delete(INanoCart cart)
        {
            if (cart == null)
                throw new ArgumentNullException("The cart cannot be null");

            if (_sessionBase == null)
                throw new InvalidOperationException("Session cannot be null");

            var sessionKey = GetSessionKey(cart.CartId);
            if (_sessionBase[sessionKey] != null)
            {
                // This feels like it should be .Remove() but I can't seem to get the mock working in the unit tests
                _sessionBase[sessionKey] = null; 
            }
        }

        public INanoCart GetCartById(Guid id)
        {
            if (_sessionBase == null)
                throw new InvalidOperationException("Session cannot be null");

            var sessionKey = GetSessionKey(id);
            if (_sessionBase[sessionKey] == null)
                return null;

            if (!(_sessionBase[sessionKey] is INanoCart))
                return null;

            return (INanoCart)_sessionBase[sessionKey];
        }

        public String GetSessionKey(Guid id)
        {
            return "nanocart_" + id;
        }
    }
}
