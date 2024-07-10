using BulletSharp;
using Genesis.Math;
using Genesis.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Genesis.Core.Behaviors.Physics2D
{
    /// <summary>
    /// Represents a 2D box collider behavior for physics simulations.
    /// </summary>
    /// <remarks>
    /// Provides functionality to create and manage a box-shaped collider in a 2D physics world.
    /// </remarks>
    public class BoxCollider : ColliderBehavior2D
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoxCollider"/> class with the specified physics handler.
        /// </summary>
        /// <param name="handler">The physics handler to associate with this box collider.</param>
        public BoxCollider(PhysicHandler handler) : base(handler)
        {
        }

        /// <summary>
        /// Creates a box collider with dimensions based on the parent's size and offset.
        /// </summary>
        public override void CreateCollider()
        {
            var shape = new Box2DShape(Parent.Size.ToVector3() / 2);

            Vec3 rotation = this.Parent.Rotation;
            System.Numerics.Matrix4x4 transformMatrix = System.Numerics.Matrix4x4.CreateTranslation(Parent.Location.ToVector3() + Offset.ToVector3());
            System.Numerics.Matrix4x4 rotationMatrix = System.Numerics.Matrix4x4.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z);
            System.Numerics.Matrix4x4 startTransform = transformMatrix * rotationMatrix;

            Collider = new BulletSharp.CollisionObject();
            Collider.CollisionShape = shape;
            Collider.UserObject = this.Parent;
            Collider.WorldTransform = startTransform;
            PhysicHandler.ManageElement(this);
        }
    }
}
