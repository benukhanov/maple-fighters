﻿using System;
using Common.Components;
using CommonTools.Log;
using ServerCommon.Application.Components;
using ServerCommon.Configuration;
using ServerCommunicationHelper;
using ServerCommunicationInterfaces;

namespace ServerCommon.PeerLogic.Components
{
    public class ClientPeerLogicBase<TOperationCode, TEventCode> : PeerLogicBase<IClientPeer>
        where TOperationCode : IComparable, IFormattable, IConvertible
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        protected IOperationRequestHandlerRegister<TOperationCode> OperationHandlerRegister
        { 
            get;
            private set;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            AddCommonComponents();

            OperationHandlerRegister = ProvideOperationHandlerRegister();
        }

        protected override void OnDispose()
        {
            base.OnDispose();

            OperationHandlerRegister?.Dispose();
        }

        private void AddCommonComponents()
        {
            var executor = new FiberCoroutinesExecutor(
                Peer.Fiber,
                updateRateMilliseconds: 100);

            Components.Add(new CoroutinesManager(executor));
            Components.Add(new EventSenderProvider<TEventCode>(
                Peer, 
                Peer.EventSender));
        }

        private IOperationRequestHandlerRegister<TOperationCode> ProvideOperationHandlerRegister()
        {
            var coroutinesManager = Components.Get<ICoroutinesManager>().AssertNotNull();

            return new OperationRequestsHandler<TOperationCode>(
                Peer.OperationRequestNotifier,
                Peer.OperationResponseSender,
                ServerSettings.Peer.Operations.LogRequests,
                ServerSettings.Peer.Operations.LogResponses,
                coroutinesManager);
        }
    }
}