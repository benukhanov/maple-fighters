using System;

namespace Game.Network
{
    public interface IMessageHandlerCollection
    {
        void Set<T, M>(T messageCode, IMessageHandler<M> handler)
            where T : IComparable, IFormattable, IConvertible
            where M : class;

        void Unset<T>(T messageCode)
            where T : IComparable, IFormattable, IConvertible;

        bool TryGet<T>(T messageCode, out Action<string> handler)
            where T : IComparable, IFormattable, IConvertible;
    }
}