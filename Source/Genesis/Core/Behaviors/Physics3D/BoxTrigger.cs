using BulletSharp;
using Genesis.Math;
using Genesis.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.Core.Behaviors.Physics3D
{
    /// <summary>
    /// Defines a box trigger behavior for 3D physics simulations.
    /// </summary>
    /// <remarks>
    /// Provides functionality to create and manage a box-shaped trigger for detecting collisions in a 3D physics world.
    /// </remarks>
    public class BoxTrigger : TriggerBehavior3D
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoxTrigger"/> class with the specified physics handler.
        /// </summary>
        /// <param name="handler">The physics handler to associate with this box trigger.</param>
        public BoxTrigger(PhysicHandler handler) : base(handler)
        {
        }

        /// <summary>
        /// Creates a box trigger with the default half extends (half of the parent's size).
        /// </summary>
        public override void CreateTrigger()
        {
            this.CreateTrigger(Parent.Size.Half());
        }

        /// <summary>
        /// Creates a box trigger with the specified half extends.
        /// </summary>
        /// <param name="boxHalfExtends">The half extends of the box trigger.</param>
        public void CreateTrigger(Vec3 boxHalfExtends)
        {
            var element = this.Parent;
            BoxShape boxShape = new BoxShape(boxHalfExtends.ToVector3());

            Vec3 location = Utils.GetElementWorldLocation(element) + Offset;
            Vec3 rotation = Utils.GetElementWorldRotation(element);

            var btTranslation = System.Numerics.Matrix4x4.CreateTranslation(location.ToVector3());
            var btRotation = System.Numerics.Matrix4x4.CreateRotationX(rotation.X) * System.Numerics.Matrix4x4.CreateRotationY(rotation.Y) * System.Numerics.Matrix4x4.CreateRotationZ(rotation.Z);
            var btStartTransform = btTranslation * btRotation;

            Trigger = new GhostObject();
            Trigger.CollisionShape = boxShape;
            Trigger.WorldTransform = btStartTransform;
            Trigger.CollisionFlags = CollisionFlags.NoContactResponse;

            PhysicHandler.ManageElement(this);
        }
    }
}
