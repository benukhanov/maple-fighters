﻿using Common.ComponentModel;
using Game.Application.Messages;

namespace Game.Application.Objects.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class PlayerAttackedMessageSender : ComponentBase, IPlayerAttacked
    {
        private IMessageSender messageSender;

        protected override void OnAwake()
        {
            messageSender = Components.Get<IMessageSender>();
        }

        public void Attack(int attackerId)
        {
            var messageCode = (byte)MessageCodes.Attacked;
            var message = new AttackedMessage()
            {
                AttackerId = attackerId
            };

            messageSender.SendMessage(messageCode, message);
        }
    }
}