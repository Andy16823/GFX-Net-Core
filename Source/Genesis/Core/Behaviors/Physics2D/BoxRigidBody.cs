using BulletSharp;
using Genesis.Math;
using Genesis.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.Core.Behaviors.Physics2D
{
    /// <summary>
    /// Represents a 2D box-shaped rigid body behavior in the physics system.
    /// </summary>
    public class BoxRigidBody : RigidBodyBehavior2D
    {
        /// <summary>
        /// Gets or sets the linear factor of the rigid body.
        /// </summary>
        public Vec3 LinearFactor { get; set; } = new Vec3(1, 1, 0);

        /// <summary>
        /// Gets or sets the angular factor of the rigid body.
        /// </summary>
        public Vec3 AngularFactor { get; set; } = new Vec3(0, 0, 0);

        /// <summary>
        /// Initializes a new instance of the <see cref="BoxRigidBody"/> class.
        /// </summary>
        /// <param name="handler">The physics handler.</param>
        public override void CreateRigidBody(float mass, int collisionGroup = -1, int collisionMask = -1)
        {
            this.CreateRigidBody(mass, Parent.Size.Half(), collisionGroup, collisionMask);
        }

        /// <summary>
        /// Creates the rigid body with the specified mass.
        /// </summary>
        /// <param name="mass">The mass of the rigid body.</param>
        public BoxRigidBody(PhysicHandler handler) : base(handler)
        {
        }

        /// <summary>
        /// Creates the rigid body with the specified mass and half extents.
        /// </summary>
        /// <param name="mass">The mass of the rigid body.</param>
        /// <param name="halfextends">The half extents of the box shape.</param>
        public void CreateRigidBody(float mass, Vec3 halfextends, int collisionGroup = -1, int collisionMask = -1)
        {
            //var capsuleShape = new CapsuleShape(Parent.Size.X / 2, 1.1f);
            var boxShape = new Box2DShape(halfextends.ToVector3());
            //var shape = new Box2DShape(Parent.Size.ToVector3() / 2);
            RigidBodyConstructionInfo info = new RigidBodyConstructionInfo(mass, null, boxShape, boxShape.CalculateLocalInertia(mass));


            System.Numerics.Matrix4x4 transform = System.Numerics.Matrix4x4.CreateTranslation(Parent.Location.ToVector3() + this.Offset.ToVector3());
            System.Numerics.Matrix4x4 rotationMatrix = System.Numerics.Matrix4x4.CreateFromYawPitchRoll(Parent.Rotation.X, Parent.Rotation.Y, Parent.Rotation.Z);
            System.Numerics.Matrix4x4 startTransform = transform * rotationMatrix;

            info.MotionState = new DefaultMotionState(startTransform);
            RigidBody = new BulletSharp.RigidBody(info);
            RigidBody.LinearFactor = this.LinearFactor.ToVector3();
            RigidBody.AngularFactor = this.AngularFactor.ToVector3();
            RigidBody.UserObject = this.Parent;
            this.RigidBody.ApplyGravity();
            PhysicHandler.ManageElement(this, collisionGroup, collisionMask);
        }
    }
}
