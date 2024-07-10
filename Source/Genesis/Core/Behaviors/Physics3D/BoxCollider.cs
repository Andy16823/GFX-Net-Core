using BulletSharp;
using BulletSharp.SoftBody;
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
    /// Defines a box collider behavior for 3D physics simulations.
    /// </summary>
    public class BoxCollider : ColliderBehavior3D
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoxCollider"/> class with the specified physics handler.
        /// </summary>
        /// <param name="handler">The physics handler to associate with this box collider.</param>
        public BoxCollider(PhysicHandler handler) : base(handler)
        {
        }

        /// <summary>
        /// Creates a box collider with default half extends.
        /// </summary>
        public override void CreateCollider()
        {
            this.CreateCollider(this.Parent.Size.Half());
        }

        /// <summary>
        /// Creates a box collider with specified half extends.
        /// </summary>
        /// <param name="boxHalfExtends">Half extends of the box collider.</param>
        public void CreateCollider(Vec3 boxHalfExtends)
        {
            var element = this.Parent;
            BoxShape boxShape = new BoxShape(boxHalfExtends.ToVector3());

            Vec3 location = Utils.GetElementWorldLocation(element) + Offset;
            Vec3 rotation = Utils.GetElementWorldRotation(element);

            var btTranslation = System.Numerics.Matrix4x4.CreateTranslation(location.ToVector3());
            var btRotation = System.Numerics.Matrix4x4.CreateRotationX(rotation.X) * System.Numerics.Matrix4x4.CreateRotationY(rotation.Y) * System.Numerics.Matrix4x4.CreateRotationZ(rotation.Z);
            var btStartTransform = btTranslation * btRotation;

            Collider = new CollisionObject();
            Collider.UserObject = element;
            Collider.CollisionShape = boxShape;
            Collider.WorldTransform = btStartTransform;

            PhysicHandler.ManageElement(this);
        }
    }
}
