﻿using BulletSharp;
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
    /// <summary>
    /// Represents a Capsule RigidBody behavior for 3D physics.
    /// </summary>
    public class CapsuleRigidBody : Physics.RigidBodyBehavior3D
    {
        /// <summary>
        /// Creates a RigidBody with default capsule dimensions.
        /// </summary>
        /// <param name="physicHandler">The physics handler to manage this element.</param>
        /// <param name="mass">The mass of the capsule.</param>
        public override void CreateRigidBody(PhysicHandler physicHandler, float mass)
        {
            this.CreateRigidBody(physicHandler, 1.0f, 2.0f, mass);
        }

        /// <summary>
        /// Creates a RigidBody with a capsule shape.
        /// </summary>
        /// <param name="physicsHandler">The physics handler to manage this element.</param>
        /// <param name="radius">The radius of the capsule.</param>
        /// <param name="height">The height of the capsule.</param>
        /// <param name="mass">The mass of the capsule.</param>
        /// <param name="offset">The offset of the capsule. Default is Vec3.Zero().</param>
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

            var btTranslation = System.Numerics.Matrix4x4.CreateTranslation(loaction.ToVector3());
            var btRotation = System.Numerics.Matrix4x4.CreateRotationX(rotation.X) * System.Numerics.Matrix4x4.CreateRotationY(rotation.Y) * System.Numerics.Matrix4x4.CreateRotationZ(rotation.Z);
            var btTransform = btTranslation * btRotation;

            constructionInfo.MotionState = new DefaultMotionState(btTransform);

            RigidBody = new RigidBody(constructionInfo);
            RigidBody.UserObject = Parent;
            RigidBody.ApplyGravity();

            physicsHandler.ManageElement(this);
        }
    }
}