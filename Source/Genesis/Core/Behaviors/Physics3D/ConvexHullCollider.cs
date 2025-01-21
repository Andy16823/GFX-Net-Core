using BulletSharp;
using Genesis.Core.GameElements;
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
    /// Represents a Convex Hull Collider behavior for 3D physics.
    /// </summary>
    public class ConvexHullCollider : ColliderBehavior3D
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConvexHullCollider"/> class with the specified physics handler.
        /// </summary>
        /// <param name="handler">The physics handler to associate with this convex hull collider.</param>
        public ConvexHullCollider(PhysicHandler handler) : base(handler)
        {
        }

        /// <summary>
        /// Creates a convex hull collider based on the shape of the parent element.
        /// </summary>
        public override void CreateCollider(int collisionGroup = -1, int collisionMask = -1) 
        {
            if (this.Parent.GetType() == typeof(Element3D))
            {
                //Create the shape 
                Element3D element = (Element3D)this.Parent;
                ConvexHullShape convexHull = new ConvexHullShape(element.GetShape());

                //Create the start matrix
                Vec3 location = Utils.GetElementWorldLocation(element);
                Vec3 rotation = Utils.GetElementWorldRotation(element);
                Vec3 scale = Utils.GetElementWorldScale(element);

                System.Numerics.Matrix4x4 btworldTransform = Utils.BuildPhysicsMatrix(location.ToVector3(), rotation.ToVector3(), scale.ToVector3());

                this.Collider = new CollisionObject();
                this.Collider.CollisionShape = convexHull;
                this.Collider.UserObject = this.Parent;
                this.Collider.WorldTransform = btworldTransform;
                this.Collider.CollisionShape.LocalScaling = scale.ToVector3();

                PhysicHandler.ManageElement(this, collisionGroup, collisionMask);
            }
            else
            {
                throw new InvalidOperationException("Invalid element for this Behavior");
            }
        }
    }
}
