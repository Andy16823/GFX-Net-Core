using BulletSharp;
using Genesis.Core;
using Genesis.Math;
using GlmSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.Physics
{
    /// <summary>
    /// Abstract class representing a 2D rigid body behavior in the physics system.
    /// </summary>
    public abstract class RigidBodyBehavior2D : PhysicsBehavior
    {
        /// <summary>
        /// Gets or sets the rigid body associated with this behavior.
        /// </summary>
        public RigidBody RigidBody { get; set; }

        /// <summary>
        /// Gets or sets the offset for the rigid body.
        /// </summary>
        public Vec3 Offset { get; set; } = Vec3.Zero();

        /// <summary>
        /// Gets or sets the physics handler associated with this behavior.
        /// </summary>
        public PhysicHandler PhysicHandler { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RigidBodyBehavior2D"/> class.
        /// </summary>
        /// <param name="handler">The physics handler.</param>
        public RigidBodyBehavior2D(PhysicHandler handler)
        {
            this.PhysicHandler = handler;
        }

        /// <summary>
        /// Creates the rigid body with the specified mass.
        /// </summary>
        /// <param name="mass">The mass of the rigid body.</param>
        public abstract void CreateRigidBody(float mass, int collisionGroup = -1, int collisionMask = -1);

        public virtual void Rotate(Vec3 value)
        {
            this.Rotate(value.Z);
        }

        /// <summary>
        /// Rotates the rigid body by the specified vector.
        /// </summary>
        /// <param name="value">The rotation vector.</param>
        public virtual void Rotate(float z)
        {
            System.Numerics.Matrix4x4 transform = this.RigidBody.WorldTransform;
            System.Numerics.Quaternion rotation = System.Numerics.Quaternion.CreateFromYawPitchRoll(0, 0, z);
            transform.SetRotation(rotation, out transform);
            this.RigidBody.WorldTransform = transform;
        }

        /// <summary>
        /// Rotates the rigid body around the z-axis by the specified angle.
        /// </summary>
        /// <param name="z">The rotation angle in radians.</param>
        public virtual void Translate(Vec3 value)
        {
            this.Translate(value.X, value.Y);
        }

        /// <summary>
        /// Translates the rigid body by the specified vector.
        /// </summary>
        /// <param name="value">The translation vector.</param>
        public virtual void Translate(float x, float y)
        {
            System.Numerics.Matrix4x4 translation = System.Numerics.Matrix4x4.CreateTranslation(x, y, 0);
            System.Numerics.Quaternion rotation = this.RigidBody.WorldTransform.GetRotation();
            System.Numerics.Matrix4x4 rotaionMatrx = System.Numerics.Matrix4x4.CreateFromQuaternion(rotation);
            this.RigidBody.WorldTransform = rotaionMatrx * translation;
        }

        /// <summary>
        /// Translates the rigid body by the specified x and y values.
        /// </summary>
        /// <param name="x">The translation along the x-axis.</param>
        /// <param name="y">The translation along the y-axis.</param>
        public virtual void AngularVelocity(float z)
        {
            this.RigidBody.AngularVelocity = new System.Numerics.Vector3(0, 0, z);
        }

        /// <summary>
        /// Sets the angular velocity of the rigid body around the z-axis.
        /// </summary>
        /// <param name="z">The angular velocity around the z-axis.</param>
        public virtual void AngularVelocity(Vec3 value)
        {
            this.AngularVelocity(value.Z);
        }

        /// <summary>
        /// Sets the angular velocity of the rigid body based on the specified vector.
        /// </summary>
        /// <param name="value">The angular velocity vector.</param>
        public virtual void LinearVelocity(float x, float y)
        {
            this.RigidBody.LinearVelocity = new System.Numerics.Vector3(x, y, 0);
        }

        /// <summary>
        /// Sets the linear velocity of the rigid body.
        /// </summary>
        /// <param name="x">The velocity along the x-axis.</param>
        /// <param name="y">The velocity along the y-axis.</param>
        public virtual void LinearVelocity(Vec3 value)
        {
            this.LinearVelocity(value.X, value.Y);
        }

        /// <summary>
        /// Sets the linear velocity of the rigid body based on the specified vector.
        /// </summary>
        /// <param name="value">The linear velocity vector.</param>
        public override object GetPhysicsObject()
        {
            return RigidBody;
        }

        /// <summary>
        /// Gets the physics object associated with this behavior.
        /// </summary>
        /// <returns>The physics object.</returns>
        public override T GetPhysicsObject<T>()
        {
            return (T)(Object)RigidBody;
        }

        /// <summary>
        /// Gets the physics object associated with this behavior, cast to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to cast the physics object to.</typeparam>
        /// <returns>The physics object cast to the specified type.</returns>
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

        /// <summary>
        /// Updates the physics behavior for the specified game and parent element.
        /// </summary>
        /// <param name="game">The game instance.</param>
        /// <param name="parent">The parent game element.</param>
        public override void OnDestroy(Game game, GameElement parent)
        {
            this.RemoveRigidBody();
        }

        /// <summary>
        /// Removes the collision element from the physics world
        /// </summary>
        public virtual void RemoveRigidBody()
        {
            this.PhysicHandler.RemoveElement(this);
            this.RigidBody.CollisionShape.Dispose();
            this.RigidBody.MotionState.Dispose();
            this.RigidBody.Dispose();
        }

        /// <summary>
        /// Destroys the physics behavior for the specified game and parent element.
        /// </summary>
        /// <param name="game">The game instance.</param>
        /// <param name="parent">The parent game element.</param>
        public override void OnInit(Game game, GameElement parent)
        {

        }

        /// <summary>
        /// Initializes the physics behavior for the specified game and parent element.
        /// </summary>
        /// <param name="game">The game instance.</param>
        /// <param name="parent">The parent game element.</param>
        public override void OnRender(Game game, GameElement parent)
        {

        }

        public override void OnCollide(Collision collision, GameElement parent)
        {
            
        }
    }
}
