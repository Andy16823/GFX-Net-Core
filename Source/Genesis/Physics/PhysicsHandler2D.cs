﻿using BulletSharp;
using Genesis.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.Physics
{
    /// <summary>
    /// Represents a 2D physics handler responsible for managing physics simulation in a game.
    /// </summary>
    public class PhysicsHandler2D : PhysicHandler
    {
        /// <summary>
        /// Gets or sets the 2D physics world used for simulation.
        /// </summary>
        public DiscreteDynamicsWorld PhysicsWorld { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether physics simulation should be processed.
        /// </summary>
        public bool ProcessPhysics { get; set; } = true;

        /// <summary>
        /// Gets or sets the tick rate for physics simulation.
        /// </summary>
        public float TickRate { get; set; } = 100;

        /// <summary>
        /// Gets ore sets the substepps for physic simulation
        /// </summary>
        public int Substepps { get; set; } = 10;

        /// <summary>
        /// Initializes a new instance of the PhysicsHandler2D class with specified gravity values.
        /// </summary>
        /// <param name="gravityX">The X component of gravity.</param>
        /// <param name="gravityY">The Y component of gravity.</param>
        public PhysicsHandler2D(float gravityX, float gravityY)
        {
            var CollisionConfiguration = new DefaultCollisionConfiguration();
            var Dispatcher = new CollisionDispatcher(CollisionConfiguration);
            var Broadphase = new DbvtBroadphase();
            this.PhysicsWorld = new DiscreteDynamicsWorld(Dispatcher, Broadphase, null, CollisionConfiguration);
            this.PhysicsWorld.Gravity = new Vector3(gravityX, gravityY, 0f);
        }

        /// <summary>
        /// Processes the physics simulation for the given scene and game.
        /// </summary>
        /// <param name="scene">The current game scene.</param>
        /// <param name="game">The current game instance.</param>
        public override void Process(Scene scene, Game game)
        {
            if(this.ProcessPhysics && this.PhysicsWorld != null)
            {
                this.OnBeforePhysicsUpdate(scene, game);
                this.PhysicsWorld.StepSimulation((float)(game.DeltaTime / TickRate), Substepps);
                //this.PhysicsWorld.StepSimulation(1.0f / 60.0f, 10);
                int numManifolds = PhysicsWorld.Dispatcher.NumManifolds;
                for (int i = 0; i < numManifolds; i++)
                {
                    PersistentManifold contactManifold = PhysicsWorld.Dispatcher.GetManifoldByIndexInternal(i);

                    CollisionObject obA = contactManifold.Body0 as CollisionObject;
                    var elementA = (GameElement)obA.UserObject;

                    CollisionObject obB = contactManifold.Body1 as CollisionObject;
                    var elementB = (GameElement)obB.UserObject;

                    Collision collisionA = new Collision()
                    {
                        collidingElement = elementB,
                        initiator = elementA,
                        contacts = contactManifold.NumContacts
                    };
                    elementA.OnCollide(collisionA);

                    Collision collisionB = new Collision()
                    {
                        collidingElement = elementA,
                        initiator = elementB,
                        contacts = contactManifold.NumContacts
                    };
                    elementB.OnCollide(collisionB); 
                }
            }
        }

        /// <summary>
        /// Manages a physics behavior element by adding its RigidBody to the physics world.
        /// </summary>
        /// <param name="physicsBehavior">The PhysicsBehavior representing the collision object.</param>
        public override void ManageElement(PhysicsBehavior physicsBehavior, int collisionGroup = -1, int collisionMask = -1)
        {
            PhysicsWorld.AddCollisionObject((CollisionObject)physicsBehavior.GetPhysicsObject(), collisionGroup, collisionMask);
        }

        /// <summary>
        /// Removes the specified physics behavior from the physics world.
        /// </summary>
        /// <param name="physicsBehavior">The physics behavior to be removed.</param>
        /// <remarks>
        /// This method removes the collision object associated with the provided physics behavior from the physics world.
        /// </remarks>
        public override void RemoveElement(PhysicsBehavior physicsBehavior)
        {
            var element = physicsBehavior.GetPhysicsObject();
            PhysicsWorld.RemoveCollisionObject((CollisionObject)element);
        }
    }
}
