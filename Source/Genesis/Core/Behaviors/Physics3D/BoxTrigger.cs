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
    public class BoxTrigger : TriggerBehavior3D
    {
        public override void CreateTrigger(PhysicHandler handler)
        {
            this.CreateTrigger(handler, Parent.Size.Half());
        }

        /// <summary>
        /// Creates a box trigger with the specified parameters.
        /// </summary>
        /// <param name="handler">The physics handler managing this trigger.</param>
        /// <param name="boxHalfExtends">The half extends of the box trigger.</param>
        public void CreateTrigger(PhysicHandler handler, Vec3 boxHalfExtends)
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

            handler.ManageElement(this);
        }
    }
}
