using BulletSharp;
using Genesis.Math;
using Genesis.Physics;
using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BulletSharp.Dbvt;

namespace Genesis.Core.Behaviors.Physics3D
{
    public class CapsuleRigidBody : Physics.PhysicsBehavior
    {
        public RigidBody RigidBody { get; set; }
        public Vec3 Offset { get; set; }

        public void CreateRigidBody(PhysicHandler physicsHandler, float radius, float height, float mass, Vec3 offset = null)
        {
            if (offset == null)
            {
                offset = Vec3.Zero();
            }

            this.Offset = offset;
            CapsuleShape capsuleShape = new CapsuleShape(radius, height);
            RigidBodyConstructionInfo constructionInfo = new RigidBodyConstructionInfo(mass, null, capsuleShape);
            
            var loaction = Utils.GetElementWorldLocation(Parent) + Offset;
            var rotation = Utils.GetElementWorldRotation(Parent);

            var btTranslation = System.Numerics.Matrix4x4.CreateTranslation(loaction.ToBulletVec3());
            var btRotation = System.Numerics.Matrix4x4.CreateRotationX(rotation.X) * System.Numerics.Matrix4x4.CreateRotationY(rotation.Y) * System.Numerics.Matrix4x4.CreateRotationZ(rotation.Z);
            var btTransform = btTranslation * btRotation;

            constructionInfo.MotionState = new DefaultMotionState(btTransform);

            RigidBody = new RigidBody(constructionInfo);
            RigidBody.UserObject = Parent;
            RigidBody.ApplyGravity();

            physicsHandler.ManageElement(this);
        }

        public void Rotate(Vec3 value)
        {
            this.Rotate(value.X, value.Y, value.Z);
        }

        public void Rotate(float x, float y, float z)
        {
            System.Numerics.Matrix4x4 transform = this.RigidBody.WorldTransform;
            System.Numerics.Quaternion rotation = System.Numerics.Quaternion.CreateFromYawPitchRoll(x, y, z);
            transform.SetRotation(rotation, out transform);
            this.RigidBody.WorldTransform = transform;
        }

        public void Translate(Vec3 value)
        {
            this.Rotate(value.X, value.Y, value.Z);
        }

        public void Translate(float x, float y, float z)
        {
            System.Numerics.Matrix4x4 translation = System.Numerics.Matrix4x4.CreateTranslation(x,y,z);
            System.Numerics.Quaternion rotation = System.Numerics.Quaternion.CreateFromRotationMatrix(RigidBody.WorldTransform);
            System.Numerics.Matrix4x4 rotationMatrix = System.Numerics.Matrix4x4.CreateFromQuaternion(rotation);
            this.RigidBody.WorldTransform = translation * rotationMatrix;
        }

        public void SetAngularVelocity(Vec3 value)
        {
            this.RigidBody.AngularVelocity = value.ToBulletVec3();
        }

        public override object GetPhysicsObject()
        {
            return RigidBody;    
        }

        public override T GetPhysicsObject<T>()
        {
            return (T)(object)RigidBody;
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
            System.Numerics.Vector3 position = RigidBody.WorldTransform.Translation;
            System.Numerics.Quaternion rotation = System.Numerics.Quaternion.CreateFromRotationMatrix(RigidBody.WorldTransform);
            vec3 rotationVector = (vec3)glm.EulerAngles(new quat(rotation.X, rotation.Y, rotation.Z, rotation.W));

            Vec3 newLocation = Utils.GetModelSpaceLocation(Parent, new Vec3(position.X, position.Y, position.Z));
            Vec3 newRotation = Utils.GetModelSpaceRotation(Parent, new Vec3(rotationVector));

            parent.Location = newLocation - Offset;
            parent.Rotation = new Vec3(Utils.ToDegrees(newRotation.X), Utils.ToDegrees(newRotation.Y), Utils.ToDegrees(newRotation.Z));
            RigidBody.Activate(true);
        }
    }
}
