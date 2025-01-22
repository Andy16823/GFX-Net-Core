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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace Genesis.Core.Behaviors.Physics2D
{
    /// <summary>
    /// Represents a 2D physics behavior for game elements.
    /// </summary>
    public class Rigidbody2D : RigidBodyBehavior2D
    {
        /// <summary>
        /// Gets or sets the linear factor for the RigidBody's motion.
        /// </summary>
        public Vec3 LinearFactor { get; set; } = new Vec3(1, 1, 0);

        /// <summary>
        /// Gets or sets the angular factor for the RigidBody's rotation.
        /// </summary>
        public Vec3 AngularFactor { get; set; } = new Vec3(0, 1, 0);

        /// <summary>
        /// Gets or sets whether physics is enabled.
        /// </summary>
        public bool EnablePhysic { get; set; } = true;

        public Rigidbody2D(PhysicHandler handler) : base(handler)
        {
        }

        public override void CreateRigidBody(float mass, int collisionGroup = -1, int collisionMask = -1)
        {
            this.CreateRigidbody(mass, 0.5f, 1.0f, collisionGroup, collisionMask);
        }

        /// <summary>
        /// Creates a RigidBody with the specified mass using the provided PhysicHandler.
        /// </summary>
        /// <param name="handler">The PhysicHandler responsible for managing physics elements.</param>
        /// <param name="mass">The mass of the RigidBody.</param>
        /// <param name="capsuleRadius">The radius of the capsule shape of the RigidBody.</param>
        /// <param name="capsuleHeight">The height of the capsule shape of the RigidBody.</param>
        public void CreateRigidbody(float mass, float capsuleRadius, float capsuleHeight, int collisionGroup = -1, int collisionMask = -1)
        {
            var capsuleShape = new CapsuleShape(capsuleRadius, capsuleHeight);
            var shape = new Convex2DShape(capsuleShape);
            RigidBodyConstructionInfo info = new RigidBodyConstructionInfo(mass, null, shape, shape.CalculateLocalInertia(mass));
            var startTransform = Utils.GetBtTransform(this.Parent, Offset);

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
