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
    public class BoxCollider : ColliderBehavior2D
    {
        /// <summary>
        /// Creates a collider using the provided PhysicHandler.
        /// </summary>
        /// <param name="handler">The PhysicHandler responsible for managing physics elements.</param>
        public override void CreateCollider(PhysicHandler physicHandler)
        {
            //var shape = new CapsuleShape(Parent.Size.X / 2, 1.5f);
            var shape = new Box2DShape(Parent.Size.ToVector3() / 2);

            Vec3 rotation = this.Parent.Rotation;
            System.Numerics.Matrix4x4 transformMatrix = System.Numerics.Matrix4x4.CreateTranslation(Parent.Location.ToVector3());
            System.Numerics.Matrix4x4 rotationMatrix = System.Numerics.Matrix4x4.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z);
            System.Numerics.Matrix4x4 startTransform = transformMatrix * rotationMatrix;

            Collider = new BulletSharp.CollisionObject();
            Collider.CollisionShape = shape;
            Collider.UserObject = this.Parent;
            Collider.WorldTransform = startTransform;
            physicHandler.ManageElement(this);
        }
    }
}
