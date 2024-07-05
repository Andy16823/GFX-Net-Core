using BulletSharp;
using Genesis.Core;
using Genesis.Math;
using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.Physics
{
    public abstract class ColliderBehavior2D : PhysicsBehavior
    {
        public CollisionObject Collider { get; set; }
        public Vec3 Offset { get; set; } = Vec3.Zero();

        public abstract void CreateCollider(PhysicHandler physicHandler);

        public void Translate(Vec3 value)
        {
            this.Translate(value.X, value.Y);
        }

        public void Translate(float x, float y)
        {
            var translationMatrix = System.Numerics.Matrix4x4.CreateTranslation(x, y, 0);
            var rotation = this.Collider.WorldTransform.GetRotation();
            var rotationMatrix = System.Numerics.Matrix4x4.CreateFromQuaternion(rotation);
            this.Collider.WorldTransform = translationMatrix * rotationMatrix;
        }

        public void Rotate(Vec3 value)
        {
            this.Rotate(value.Z);
        }

        public void Rotate(float z)
        {
            var transformMatrix = this.Collider.WorldTransform;
            transformMatrix.SetRotation(System.Numerics.Quaternion.CreateFromYawPitchRoll(0, 0, z), out transformMatrix);
            this.Collider.WorldTransform = transformMatrix;
        }

        public override object GetPhysicsObject()
        {
            return this.Collider;
        }

        public override T GetPhysicsObject<T>()
        {
            return (T)(object)this.Collider;
        }

        public override void OnDestroy(Game game, GameElement parent)
        {

        }

        public override void OnInit(Game game, GameElement parent)
        {

        }

        public override void OnRender(Game game, GameElement parent)
        {

        }

        public override void OnUpdate(Game game, GameElement parent)
        {
            System.Numerics.Vector3 position = Collider.WorldTransform.Translation;

            System.Numerics.Quaternion rotation = System.Numerics.Quaternion.CreateFromRotationMatrix(Collider.WorldTransform);
            vec3 rotationVector = (vec3)glm.EulerAngles(new quat(rotation.X, rotation.Y, rotation.Z, rotation.W));

            parent.Location = new Vec3(position.X, position.Y, position.Z) - Offset;
            parent.Rotation = new Vec3(Utils.ToDegrees(rotationVector.x), Utils.ToDegrees(rotationVector.y), Utils.ToDegrees(rotationVector.z));
            Collider.Activate(true);
        }
    }
}
