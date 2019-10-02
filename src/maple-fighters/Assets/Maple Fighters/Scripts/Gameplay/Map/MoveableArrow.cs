﻿using System.Collections;
using Scripts.Constants;
using UnityEngine;

namespace Scripts.Gameplay.Map
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class MoveableArrow : MonoBehaviour
    {
        [SerializeField]
        private float moveTime;

        [SerializeField]
        private float moveSpeed;

        private new UnityEngine.Camera camera;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            var minimapCamera =
                GameObject.FindGameObjectWithTag(GameTags.MinimapCameraTag);
            if (minimapCamera != null)
            {
                camera = minimapCamera.GetComponent<UnityEngine.Camera>();
            }

            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            StartCoroutine(MoveableArrowCoroutine());
        }

        private void Update()
        {
            var isInLayerMask = 
                Utils.IsInLayerMask(gameObject.layer, camera.cullingMask);

            spriteRenderer.enabled = isInLayerMask;
        }

        private IEnumerator MoveableArrowCoroutine()
        {
            while (true)
            {
                while (Utils.IsInLayerMask(gameObject.layer, camera.cullingMask))
                {
                    yield return StartCoroutine(MoveArrowUp());
                    yield return StartCoroutine(MoveArrowDown());
                }

                yield return null;
            }
        }

        private IEnumerator MoveArrowUp()
        {
            var currentTime = Time.time;

            do
            {
                transform.position += Vector3.up * moveSpeed * Time.deltaTime;
                yield return null;
            }
            while (Time.time < currentTime + moveTime);
        }

        private IEnumerator MoveArrowDown()
        {
            var currentTime = Time.time;

            do
            {
                transform.position += Vector3.down * moveSpeed * Time.deltaTime;
                yield return null;
            }
            while (Time.time < currentTime + moveTime);
        }
    }
}