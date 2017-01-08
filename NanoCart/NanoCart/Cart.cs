using SessionCartPersistanceProvider.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NanoCart
{
    public interface INanoCart
    {
        Guid CartId { get; set; }

        DateTime DateCreated { get; set; }

        DateTime DateLastUpdate { get; set; }

        List<ICartItem> CartItems { get; set; }

        void AddCartItem(ICartItem cartItem);

        void RemoveCartItem(ICartItem cartItem);

        ICartItem GetCartItem(Guid cartItemId);

        INanoCart GetCart(Guid cartId);

        void Persist();

        void Delete();

    }

    public class StandardNanoCart : INanoCart
    {
        public Guid CartId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateLastUpdate { get; set; }

        public List<ICartItem> CartItems { get; set; }
        
        private ICartPersistanceProvider _persistanceProvider;

        public StandardNanoCart(ICartPersistanceProvider persistanceProvider)
        {
            _persistanceProvider = persistanceProvider;
            CartItems = new List<ICartItem>();
        }

        public void AddCartItem(ICartItem cartItem)
        {

            CartItems.Add(cartItem);
        }

        public void RemoveCartItem(ICartItem cartItem)
        {
            CartItems.RemoveAll(x => x.CartItemId == cartItem.CartItemId);
        }

        public ICartItem GetCartItem(Guid id)
        {
            return CartItems.FirstOrDefault(x => x.CartItemId == id);
        }

        public INanoCart GetCart(Guid id)
        {
            return _persistanceProvider.GetCartById(id);
        }

        public void Persist()
        {
            if (CartId == null)
            {
                CartId = Guid.NewGuid();
                DateCreated = DateTime.Now;
                DateLastUpdate = DateTime.Now;

                _persistanceProvider.Add(this);
            }
            else
            {
                DateLastUpdate = DateTime.Now;
                _persistanceProvider.Update(this);
            }
        }

        public void Delete()
        {
            _persistanceProvider.Delete(this);
        }

    }


    public interface ICartItem
    {
        Guid CartItemId { get; set; }

        int? Quantity { get; set; }

        decimal Amount { get; set; }

        decimal Total { get; }


    }

    public class StandardNanoCartItem : ICartItem
    {
        public Guid CartItemId { get; set; }

        public int? Quantity { get; set; }

        public decimal Amount { get; set; }

        public decimal Total => Quantity.GetValueOrDefault(0) * Amount;

        public StandardNanoCartItem()
        {
            CartItemId = Guid.NewGuid();
        }
    }


   

}
