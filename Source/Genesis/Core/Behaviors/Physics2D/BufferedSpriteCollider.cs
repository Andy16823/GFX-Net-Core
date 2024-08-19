using BulletSharp;
using Genesis.Core.GameElements;
using Genesis.Graphics.Shapes;
using Genesis.Math;
using Genesis.Physics;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static BulletSharp.Dbvt;

namespace Genesis.Core.Behaviors.Physics2D
{
    /// <summary>
    /// Represents a behavior for creating a physics collider for 2D sprites using BulletSharp.
    /// </summary>
    public class BufferedSpriteCollider : ColliderBehavior2D
    {
        public BufferedSpriteCollider(PhysicHandler physicHandler) : base(physicHandler)
        {
        }

        /// <summary>
        /// Creates a rigid body for the collider.
        /// </summary>
        public override void CreateCollider(int collisionGroup = -1, int collisionMask = -1)
        {
            if (this.Parent.GetType() == typeof(BufferedSprite))
            {
                var bufferedSprite = (BufferedSprite)this.Parent;
                CompoundShape compoundShape = new CompoundShape(true);

                foreach (var deffinition in bufferedSprite.ShapeDeffinitions)
                {
                    Box2DShape box2DShape = new Box2DShape(new Vector3(deffinition.sizeX, deffinition.sizeY, 0f) / 2);
                    System.Numerics.Matrix4x4 boxtransform = System.Numerics.Matrix4x4.CreateTranslation(new Vector3(deffinition.locX, deffinition.locY, 0f));
                    compoundShape.AddChildShape(boxtransform, box2DShape);
                }
                System.Numerics.Matrix4x4 transform = System.Numerics.Matrix4x4.CreateTranslation(bufferedSprite.Location.ToVector3());


                Collider = new CollisionObject();
                Collider.CollisionShape = compoundShape;
                Collider.WorldTransform = transform;
                Collider.UserObject = this.Parent;
                //RigidBody.CollisionFlags = CollisionFlags.StaticObject;

                PhysicHandler.ManageElement(this, collisionGroup, collisionMask);
            }
            else
            {
                throw new InvalidOperationException("Invalid element for this Behavior");
            }
        }

        public override void OnUpdate(Game game, GameElement parent)
        {
            Collider.Activate(true);
        }
    }
}
