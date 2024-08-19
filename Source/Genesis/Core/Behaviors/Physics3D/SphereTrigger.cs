using BulletSharp;
using Genesis.Math;
using Genesis.Physics;
using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.Core.Behaviors.Physics3D
{
    /// <summary>
    /// Represents a sphere trigger behavior for 3D physics.
    /// </summary>
    /// <remarks>
    /// Provides functionality to create and manage a sphere-shaped trigger for detecting collisions in a 3D physics world.
    /// </remarks>
    public class SphereTrigger : TriggerBehavior3D
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SphereTrigger"/> class with the specified physics handler.
        /// </summary>
        /// <param name="handler">The physics handler to associate with this sphere trigger.</param>
        public SphereTrigger(PhysicHandler handler) : base(handler)
        {
        }

        /// <summary>
        /// Creates a trigger with a sphere shape using the default radius (half of the parent's size).
        /// </summary>
        public override void CreateTrigger(int collisionGroup = -1, int collisionMask = -1)
        {
            this.CreateTrigger(this.Parent.Size.X / 2, collisionGroup, collisionMask);
        }

        /// <summary>
        /// Creates a trigger with a sphere shape at the origin.
        /// </summary>
        /// <param name="radius">The radius of the sphere.</param>
        public void CreateTrigger(float radius, int collisionGroup = -1, int collisionMask = -1)
        {
            this.CreateTrigger(Vec3.Zero(), radius, collisionGroup, collisionMask);
        }

        /// <summary>
        /// Creates a trigger with a sphere shape at the specified offset.
        /// </summary>
        /// <param name="offset">The offset from the parent element's location.</param>
        /// <param name="radius">The radius of the sphere.</param>
        public void CreateTrigger(Vec3 offset, float radius, int collisionGroup = -1, int collisionMask = -1)
        {
            this.Offset = offset;

            var element = this.Parent;
            SphereShape sphereShape = new SphereShape(radius);

            Vec3 location = Utils.GetElementWorldLocation(element) + Offset;
            Vec3 rotation = Utils.GetElementWorldRotation(element);

            var btTranslation = System.Numerics.Matrix4x4.CreateTranslation(location.ToVector3());
            var btRotation = System.Numerics.Matrix4x4.CreateRotationX(rotation.X) * System.Numerics.Matrix4x4.CreateRotationY(rotation.Y) * System.Numerics.Matrix4x4.CreateRotationZ(rotation.Z);
            var btStartTransform = btTranslation * btRotation;

            Trigger = new GhostObject();
            Trigger.UserObject = element;
            Trigger.CollisionShape = sphereShape;
            Trigger.WorldTransform = btStartTransform;
            PhysicHandler.ManageElement(this, collisionGroup, collisionMask);
        }
    }
}
