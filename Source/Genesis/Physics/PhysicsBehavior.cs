﻿using BulletSharp;
using Genesis.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.Physics
{
    /// <summary>
    /// Represents a delegate for handling physics behavior events.
    /// </summary>
    /// <param name="scene">The scene involved in the physics behavior event</param>
    /// <param name="game">The game involved in the physics behavior event</param>
    /// <param name="collision">The information from the collision</param>
    public delegate void PhysicsBehaviorEvent(Scene scene, Game game, Collision collision);

    /// <summary>
    /// Represents an abstract class for defining physics behavior in the game.
    /// </summary>
    public abstract class PhysicsBehavior : IGameBehavior
    {
        /// <summary>
        /// Gets the physics object associated with this behavior.
        /// </summary>
        /// <returns>The physics object</returns>
        public abstract object GetPhysicsObject();

        /// <summary>
        /// Gets the physics object associated with this behavior, cast to type T.
        /// </summary>
        /// <typeparam name="T">Type to cast the physics object to</typeparam>
        /// <returns>The physics object cast to type T</returns>
        public abstract T GetPhysicsObject<T>();

        public override void OnCollide(Collision collision, GameElement parent)
        {
            
        }
    }
}
