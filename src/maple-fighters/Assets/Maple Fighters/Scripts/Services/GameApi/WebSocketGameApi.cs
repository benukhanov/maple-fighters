using System;
using System.Text;
using Game.Messages;
using Game.MessageTools;
using NativeWebSocket;
using UnityEngine;

namespace Scripts.Services.GameApi
{
    public class WebSocketGameApi : MonoBehaviour, IGameApi
    {
        public static WebSocketGameApi GetInstance()
        {
            if (instance == null)
            {
                var gameApi = new GameObject("WebSocket Game Api");
                instance = gameApi.AddComponent<WebSocketGameApi>();
            }

            return instance;
        }

        private static WebSocketGameApi instance;

        public Action Connected { get; set; }

        public Action<WebSocketCloseCode> Disconnected { get; set; }

        public UnityEvent<EnteredSceneMessage> SceneEntered { get; set; }

        public UnityEvent<SceneChangedMessage> SceneChanged { get; set; }

        public UnityEvent<GameObjectsAddedMessage> GameObjectsAdded { get; set; }

        public UnityEvent<GameObjectsRemovedMessage> GameObjectsRemoved { get; set; }

        public UnityEvent<PositionChangedMessage> PositionChanged { get; set; }

        public UnityEvent<AnimationStateChangedMessage> AnimationStateChanged { get; set; }

        public UnityEvent<AttackedMessage> Attacked { get; set; }

        public UnityEvent<BubbleNotificationMessage> BubbleMessageReceived { get; set; }

        private IJsonSerializer jsonSerializer;
        private MessageHandlerCollection collection;
        private WebSocket webSocket;

        private void Awake()
        {
            // Properties
            SceneEntered = new UnityEvent<EnteredSceneMessage>();
            SceneChanged = new UnityEvent<SceneChangedMessage>();
            GameObjectsAdded = new UnityEvent<GameObjectsAddedMessage>();
            GameObjectsRemoved = new UnityEvent<GameObjectsRemovedMessage>();
            PositionChanged = new UnityEvent<PositionChangedMessage>();
            AnimationStateChanged = new UnityEvent<AnimationStateChangedMessage>();
            Attacked = new UnityEvent<AttackedMessage>();
            BubbleMessageReceived = new UnityEvent<BubbleNotificationMessage>();

            // Variables
            jsonSerializer = new UnityJsonSerializer();
            collection = new MessageHandlerCollection(jsonSerializer);
        }

        private async void Start()
        {
            var url = UserData.GameServerUrl;

            webSocket = new WebSocket(url);
            webSocket.OnOpen += OnOpen;
            webSocket.OnClose += OnClose;
            webSocket.OnMessage += OnMessage;

            if (webSocket != null)
            {
                await webSocket.Connect();
            }
        }

        private void Update()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            webSocket?.DispatchMessageQueue();
#endif
        }

        private async void OnDestroy()
        {
            if (webSocket != null)
            {
                await webSocket.Close();
            }

            ApiProvider.RemoveGameApiProvider();
        }

        private async void OnApplicationQuit()
        {
            if (webSocket != null)
            {
                await webSocket.Close();
            }
        }

        async void IGameApi.SendMessage<T, M>(T code, M message)
        {
            if (webSocket != null && webSocket.State == WebSocketState.Open)
            {
                var data = jsonSerializer.Serialize(new MessageData()
                {
                    Code = Convert.ToByte(code),
                    Data = jsonSerializer.Serialize(message)
                });

                if (webSocket != null)
                {
                    await webSocket.SendText(data);
                }
            }
        }

        private void OnOpen()
        {
            collection.Set(MessageCodes.EnteredScene, SceneEntered.ToMessageHandler());
            collection.Set(MessageCodes.SceneChanged, SceneChanged.ToMessageHandler());
            collection.Set(MessageCodes.GameObjectsAdded, GameObjectsAdded.ToMessageHandler());
            collection.Set(MessageCodes.GameObjectsRemoved, GameObjectsRemoved.ToMessageHandler());
            collection.Set(MessageCodes.PositionChanged, PositionChanged.ToMessageHandler());
            collection.Set(MessageCodes.AnimationStateChanged, AnimationStateChanged.ToMessageHandler());
            collection.Set(MessageCodes.Attacked, Attacked.ToMessageHandler());
            collection.Set(MessageCodes.BubbleNotification, BubbleMessageReceived.ToMessageHandler());

            Connected?.Invoke();
        }

        private void OnClose(WebSocketCloseCode code)
        {
            collection.Unset(MessageCodes.EnteredScene);
            collection.Unset(MessageCodes.SceneChanged);
            collection.Unset(MessageCodes.GameObjectsAdded);
            collection.Unset(MessageCodes.GameObjectsRemoved);
            collection.Unset(MessageCodes.PositionChanged);
            collection.Unset(MessageCodes.AnimationStateChanged);
            collection.Unset(MessageCodes.Attacked);
            collection.Unset(MessageCodes.BubbleNotification);

            Disconnected?.Invoke(code);
        }

        private void OnMessage(byte[] bytes)
        {
            var message = Encoding.UTF8.GetString(bytes);
            var messageData = jsonSerializer.Deserialize<MessageData>(message);
            var code = messageData.Code;
            var data = messageData.Data;

            if (collection.TryGet(code, out var handler))
            {
                handler?.Invoke(data);
            }
        }
    }
}