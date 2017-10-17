﻿using ServerApplication.Common.ComponentModel;

namespace Shared.ServerApplication.Common.PeerLogic
{
    public class PeerEntity : IPeerEntity
    {
        public int Id { get; }
        public IContainer<IPeerEntity> Container { get; }

        public PeerEntity(int id)
        {
            Id = id;
            Container = new Container<IPeerEntity>(this);
        }

        public void Dispose()
        {
            Container.Dispose();
        }
    }
}