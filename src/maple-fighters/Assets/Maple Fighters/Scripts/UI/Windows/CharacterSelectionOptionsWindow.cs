﻿using System;
using UI.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Windows
{
    [RequireComponent(typeof(UIFadeAnimation))]
    public class CharacterSelectionOptionsWindow : UIElement
    {
        public event Action StartButtonClicked;

        public event Action CreateCharacterButtonClicked;

        public event Action DeleteCharacterButtonClicked;

        [SerializeField]
        private Button startButton;

        [SerializeField]
        private Button createCharacterButton;

        [SerializeField]
        private Button deleteCharacterButton;

        private void Start()
        {
            if (startButton != null)
            {
                startButton.onClick.AddListener(OnStartButtonClicked);
            }

            if (createCharacterButton != null)
            {
                createCharacterButton.onClick.AddListener(
                    OnCreateCharacterButtonClicked);
            }

            if (deleteCharacterButton != null)
            {
                deleteCharacterButton.onClick.AddListener(
                    OnDeleteCharacterButtonClicked);
            }
        }

        private void OnDestroy()
        {
            if (startButton != null)
            {
                startButton.onClick.RemoveListener(OnStartButtonClicked);
            }

            if (createCharacterButton != null)
            {
                createCharacterButton.onClick.RemoveListener(
                    OnCreateCharacterButtonClicked);
            }

            if (deleteCharacterButton != null)
            {
                deleteCharacterButton.onClick.RemoveListener(
                    OnDeleteCharacterButtonClicked);
            }
        }
        
        private void OnStartButtonClicked()
        {
            Hide();

            StartButtonClicked?.Invoke();
        }

        private void OnCreateCharacterButtonClicked()
        {
            Hide();

            CreateCharacterButtonClicked?.Invoke();
        }

        private void OnDeleteCharacterButtonClicked()
        {
            Hide();

            DeleteCharacterButtonClicked?.Invoke();
        }

        public void StartButtonInteraction(bool interctable)
        {
            if (startButton != null)
            {
                startButton.interactable = interctable;
            }
        }

        public void CreateCharacterButtonInteraction(bool interctable)
        {
            if (createCharacterButton != null)
            {
                createCharacterButton.interactable = interctable;
            }
        }

        public void DeleteCharacterButtonInteraction(bool interctable)
        {
            if (deleteCharacterButton != null)
            {
                deleteCharacterButton.interactable = interctable;
            }
        }
    }
}