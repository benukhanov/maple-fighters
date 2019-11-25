﻿using Game.Common;
using Scripts.Constants;
using UnityEngine;

namespace Scripts.Gameplay.Player
{
    [RequireComponent(typeof(PlayerController), typeof(Collider2D))]
    public class RopeInteractor : ClimbInteractor
    {
        [SerializeField]
        private KeyCode key = KeyCode.LeftControl;

        private PlayerController playerController;
        private ColliderInteraction colliderInteraction;

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();

            var collider = GetComponent<Collider2D>();
            colliderInteraction = new ColliderInteraction(collider);
        }

        protected override void SetPlayerToClimbState()
        {
            playerController.ChangePlayerState(GetClimbState());
        }

        protected override void UnsetPlayerFromClimbState()
        {
            var isGrounded =
                playerController.IsGrounded()
                    ? PlayerState.Idle
                    : PlayerState.Falling;

            playerController.ChangePlayerState(isGrounded);
        }

        protected override PlayerState GetPlayerState()
        {
            return playerController.PlayerState;
        }

        protected override KeyCode GetKey()
        {
            return key;
        }

        protected override ColliderInteraction GetColliderInteraction()
        {
            return colliderInteraction;
        }

        protected override string GetTagName()
        {
            return GameTags.RopeTag;
        }

        protected override PlayerState GetClimbState()
        {
            return PlayerState.Rope;
        }
    }
}