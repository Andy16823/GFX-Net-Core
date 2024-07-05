using BulletSharp;
using Genesis.Math;
using Genesis.Physics;
using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.Core.Behaviors.Physics3D
{
    /// <summary>
    /// Represents a Capsule Collider behavior for 3D physics.
    /// </summary>
    public class CapsuleCollider : ColliderBehavior3D
    {
        /// <summary>
        /// Creates a collider with a capsule shape using default dimensions.
        /// </summary>
        /// <param name="physicHandler">The physics handler to manage this element.</param>
        public override void CreateCollider(PhysicHandler physicHandler)
        {
            this.CreateCollider(physicHandler, 1.0f, 2.0f);
        }
        /// <summary>
        /// Creates a collider with a capsule shape at the origin.
        /// </summary>
        /// <param name="handler">The physics handler to manage this element.</param>
        /// <param name="radius">The radius of the capsule.</param>
        /// <param name="height">The height of the capsule.</param>
        public void CreateCollider(PhysicHandler handler, float radius, float height)
        {
            this.CreateCollider(handler, Vec3.Zero(), radius, height);
        }

        /// <summary>
        /// Creates a collider with a capsule shape at the specified offset.
        /// </summary>
        /// <param name="handler">The physics handler to manage this element.</param>
        /// <param name="offset">The offset from the parent element's location.</param>
        /// <param name="radius">The radius of the capsule.</param>
        /// <param name="height">The height of the capsule.</param>
        public void CreateCollider(PhysicHandler handler, Vec3 offset, float radius, float height)
        {
            this.Offset = offset;
            var element = this.Parent;
            CapsuleShape capsuleShape = new CapsuleShape(radius, height);

            Vec3 location = Utils.GetElementWorldLocation(element);
            Vec3 rotation = Utils.GetElementWorldRotation(element);

            var btTranslation = System.Numerics.Matrix4x4.CreateTranslation(location.ToVector3() + Offset.ToVector3());
            var btRotation = System.Numerics.Matrix4x4.CreateRotationX(rotation.X) * System.Numerics.Matrix4x4.CreateRotationY(rotation.Y) * System.Numerics.Matrix4x4.CreateRotationZ(rotation.Z);
            var btStartTransform = btTranslation * btRotation;

            Collider = new CollisionObject();
            Collider.UserObject = element;
            Collider.CollisionShape = capsuleShape;
            Collider.WorldTransform = btStartTransform;

            handler.ManageElement(this);
        }
    }
}
