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
    /// <summary>
    /// Represents a 2D collider behavior for physics interactions.
    /// </summary>
    /// <remarks>
    /// Provides functionality to create and manage 2D colliders for detecting collisions in a physics simulation.
    /// </remarks>
    public abstract class ColliderBehavior2D : PhysicsBehavior
    {
        /// <summary>
        /// Gets or sets the collider object associated with this 2D collider behavior.
        /// </summary>
        public CollisionObject Collider { get; set; }

        /// <summary>
        /// Gets or sets the offset of the collider relative to its parent's location.
        /// </summary>
        public Vec3 Offset { get; set; } = Vec3.Zero();

        /// <summary>
        /// Gets or sets the physics handler associated with this 2D collider behavior.
        /// </summary>
        public PhysicHandler PhysicHandler { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColliderBehavior2D"/> class with the specified physics handler.
        /// </summary>
        /// <param name="handler">The physics handler to associate with this 2D collider behavior.</param>
        public ColliderBehavior2D(PhysicHandler handler)
        {
            this.PhysicHandler = handler;
        }

        /// <summary>
        /// Creates the collider.
        /// </summary>
        public abstract void CreateCollider(int collisionGroup = -1, int collisionMask = -1);

        /// <summary>
        /// Translates the collider by the specified vector.
        /// </summary>
        /// <param name="value">The translation vector.</param>
        public void Translate(Vec3 value)
        {
            this.Translate(value.X, value.Y);
        }

        /// <summary>
        /// Translates the collider by the specified x and y values.
        /// </summary>
        /// <param name="x">The translation in the X-axis.</param>
        /// <param name="y">The translation in the Y-axis.</param>
        public void Translate(float x, float y)
        {
            var translationMatrix = System.Numerics.Matrix4x4.CreateTranslation(x, y, 0);
            var rotation = this.Collider.WorldTransform.GetRotation();
            var rotationMatrix = System.Numerics.Matrix4x4.CreateFromQuaternion(rotation);
            this.Collider.WorldTransform = rotationMatrix * translationMatrix;
        }

        /// <summary>
        /// Rotates the collider by the specified vector.
        /// </summary>
        /// <param name="value">The rotation vector.</param>
        public void Rotate(Vec3 value)
        {
            this.Rotate(value.Z);
        }

        /// <summary>
        /// Rotates the collider by the specified angle around the Z-axis.
        /// </summary>
        /// <param name="z">The rotation angle in degrees.</param>
        public void Rotate(float z)
        {
            var transformMatrix = this.Collider.WorldTransform;
            transformMatrix.SetRotation(System.Numerics.Quaternion.CreateFromYawPitchRoll(0, 0, z), out transformMatrix);
            this.Collider.WorldTransform = transformMatrix;
        }

        /// <summary>
        /// Retrieves the collider object.
        /// </summary>
        /// <returns>The collider object.</returns>
        public override object GetPhysicsObject()
        {
            return this.Collider;
        }

        /// <summary>
        /// Retrieves the collider object of type T.
        /// </summary>
        /// <typeparam name="T">The type of collider object.</typeparam>
        /// <returns>The collider object casted to type T.</returns>
        public override T GetPhysicsObject<T>()
        {
            return (T)(object)this.Collider;
        }

        /// <summary>
        /// Updates the game element's position and rotation based on the collider's state.
        /// </summary>
        /// <param name="game">The game instance.</param>
        /// <param name="parent">The parent game element.</param>
        public override void OnUpdate(Game game, GameElement parent)
        {
            System.Numerics.Vector3 position = Collider.WorldTransform.Translation;

            System.Numerics.Quaternion rotation = System.Numerics.Quaternion.CreateFromRotationMatrix(Collider.WorldTransform);
            vec3 rotationVector = (vec3)glm.EulerAngles(new quat(rotation.X, rotation.Y, rotation.Z, rotation.W));

            parent.Location = new Vec3(position.X, position.Y, position.Z) - Offset;
            parent.Rotation = new Vec3(Utils.ToDegrees(rotationVector.x), Utils.ToDegrees(rotationVector.y), Utils.ToDegrees(rotationVector.z));
            Collider.Activate(true);
        }

        /// <summary>
        /// Removes the collider from the physics simulation.
        /// </summary>
        /// <param name="game">The game instance.</param>
        /// <param name="parent">The parent game element.</param>
        public override void OnDestroy(Game game, GameElement parent)
        {
            this.RemoveCollider();
        }

        /// <summary>
        /// Removes the collision element from the physics world
        /// </summary>
        public virtual void RemoveCollider()
        {
            this.PhysicHandler.RemoveElement(this);
            this.Collider.CollisionShape.Dispose();
            this.Collider.Dispose();
        }

        /// <summary>
        /// Initializes the collider behavior.
        /// </summary>
        /// <param name="game">The game instance.</param>
        /// <param name="parent">The parent game element.</param>
        public override void OnInit(Game game, GameElement parent)
        {

        }

        /// <summary>
        /// Renders the collider behavior.
        /// </summary>
        /// <param name="game">The game instance.</param>
        /// <param name="parent">The parent game element.</param>
        public override void OnRender(Game game, GameElement parent)
        {

        }
    }
}
