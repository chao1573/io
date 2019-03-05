using System;
using Entitas;
using Entitas.Unity;
using UnityEngine;
using Entitas.Unity;
namespace IO
{
    public class View : MonoBehaviour, IView, IPositionListener, IDestroyedListener, IRotationListener
    {
        public void Link(IEntity entity, IContext context)
        {
            gameObject.Link(entity,context);
            var gameEntity = (GameEntity) entity;
            gameEntity.AddPositionListener(this);
            gameEntity.AddDestroyedListener(this);
            gameEntity.AddRotationListener(this);
        }

        public virtual void OnPosition(GameEntity entity, Microsoft.Xna.Framework.Vector3 value)
        {
            transform.position = new Vector3(value.X,value.Y, -value.Z);
        }
        
        public virtual void OnRotation(GameEntity entity, Microsoft.Xna.Framework.Quaternion value)
        {
            transform.rotation = new Quaternion(value.X, value.Y, value.Z, value.W);
        }

        public virtual void OnDestroyed(GameEntity entity)
        {
            destroy();
        }
        
        protected void destroy()
        {
            gameObject.Unlink();
            Destroy(gameObject);
        }


    }
}