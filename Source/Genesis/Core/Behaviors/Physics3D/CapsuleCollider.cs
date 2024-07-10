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
        /// Initializes a new instance of the <see cref="CapsuleCollider"/> class with the specified physics handler.
        /// </summary>
        /// <param name="handler">The physics handler to associate with this capsule collider.</param>
        public CapsuleCollider(PhysicHandler handler) : base(handler)
        {
        }

        /// <summary>
        /// Creates a collider with a capsule shape using default dimensions.
        /// </summary>
        public override void CreateCollider()
        {
            this.CreateCollider(1.0f, 2.0f);
        }

        /// <summary>
        /// Creates a collider with a capsule shape at the origin.
        /// </summary>
        /// <param name="radius">The radius of the capsule.</param>
        /// <param name="height">The height of the capsule.</param>
        public void CreateCollider(float radius, float height)
        {
            this.CreateCollider(Vec3.Zero(), radius, height);
        }

        /// <summary>
        /// Creates a collider with a capsule shape at the specified offset.
        /// </summary>
        /// <param name="offset">The offset from the parent element's location.</param>
        /// <param name="radius">The radius of the capsule.</param>
        /// <param name="height">The height of the capsule.</param>
        public void CreateCollider(Vec3 offset, float radius, float height)
        {
            this.Offset = offset;
            var element = this.Parent;
            CapsuleShape capsuleShape = new CapsuleShape(radius, height);

            Vec3 location = Utils.GetElementWorldLocation(element) + Offset;
            Vec3 rotation = Utils.GetElementWorldRotation(element);

            var btTranslation = System.Numerics.Matrix4x4.CreateTranslation(location.ToVector3());
            var btRotation = System.Numerics.Matrix4x4.CreateRotationX(rotation.X) * System.Numerics.Matrix4x4.CreateRotationY(rotation.Y) * System.Numerics.Matrix4x4.CreateRotationZ(rotation.Z);
            var btStartTransform = btTranslation * btRotation;

            Collider = new CollisionObject();
            Collider.UserObject = element;
            Collider.CollisionShape = capsuleShape;
            Collider.WorldTransform = btStartTransform;

            PhysicHandler.ManageElement(this);
        }
    }
}
