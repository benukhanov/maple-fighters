﻿using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Scripts.Gameplay.Entity
{
    public class EntityCollection : IEntityCollection
    {
        private readonly Dictionary<int, IEntity> collection;

        public EntityCollection()
        {
            collection = new Dictionary<int, IEntity>();
        }

        public IEntity Add(int id, string name, Vector2 position)
        {
            if (collection.ContainsKey(id))
            {
                Debug.LogWarning($"The entity with id #{id} already exists.");
            }
            else
            {
                var gameObject = Utils.CreateGameObject(name, position);
                if (gameObject != null)
                {
                    var entity = gameObject.GetComponent<IEntity>();
                    if (entity != null)
                    {
                        entity.Id = id;

                        collection.Add(id, entity);

                        Debug.Log($"Added a new entity with id #{id}");
                    }
                }
            }

            return TryGet(id);
        }

        public void Remove(int id)
        {
            var entity = TryGet(id);
            if (entity != null)
            {
                Object.Destroy(entity.GameObject);

                collection.Remove(id);

                Debug.Log($"Removed an entity with id #{id}");
            }
        }

        public IEntity TryGet(int id)
        {
            if (!collection.TryGetValue(id, out var entity))
            {
                Debug.LogWarning($"Could not find an entity with id #{id}");
            }

            return entity;
        }
    }
}