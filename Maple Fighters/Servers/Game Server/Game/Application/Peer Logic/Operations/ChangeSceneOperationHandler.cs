﻿using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Application.Components;
using Game.Application.PeerLogic.Components;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;
using Shared.Game.Common;

namespace Game.Application.PeerLogic.Operations
{
    internal class ChangeSceneOperationHandler : IOperationRequestHandler<ChangeSceneRequestParameters, EmptyParameters>
    {
        private readonly GameObjectGetter gameObjectGetter;
        private readonly SceneContainer sceneContainer;

        public ChangeSceneOperationHandler(GameObjectGetter gameObjectGetter)
        {
            this.gameObjectGetter = gameObjectGetter;

            sceneContainer = Server.Entity.Container.GetComponent<SceneContainer>().AssertNotNull();
        }

        public EmptyParameters? Handle(MessageData<ChangeSceneRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var sceneId = messageData.Parameters.Map;
            var scene = sceneContainer.GetScene(sceneId).AssertNotNull();

            var gameObject = gameObjectGetter.GetGameObject();
            gameObject.SetScene(scene);
            return new EmptyParameters();
        }
    }
}