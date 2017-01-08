using NanoCart;
using System;

namespace SessionCartPersistanceProvider.Interfaces
{
    public interface ICartPersistanceProvider
    {
        void Add(INanoCart cart);

        void Update(INanoCart cart);

        void Delete(INanoCart cart);

        INanoCart GetCartById(Guid cartId);
    }
}
