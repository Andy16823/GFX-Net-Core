using Assimp;
using BulletSharp;
using Genesis.Core.GameElements;
using Genesis.Math;
using Genesis.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BulletSharp.Dbvt;

namespace Genesis.Core.Behaviors.Physics3D
{
    public class CompoundMeshCollider : ColliderBehavior3D
    {
        public CompoundMeshCollider(PhysicHandler physicHandler) : base(physicHandler)
        {
        }

        public void CreateCollider(String file, int collisionGroup = -1, int collisionMask = -1)
        {
            Element3D element = (Element3D)this.Parent;

            Assimp.AssimpContext importer = new Assimp.AssimpContext();
            var model = importer.ImportFile(file, Assimp.PostProcessPreset.TargetRealTimeQuality | Assimp.PostProcessSteps.PreTransformVertices);
            var compoundShape = new CompoundShape();

            var scale = Utils.GetElementWorldScale(element);
            var btStartTransform = Utils.GetBtTransform(element);

            foreach (var mesh in model.Meshes)
            {
                int[] indicies = mesh.GetIndices();
                float[] verticies = mesh.Vertices.SelectMany(v => new float[] { v.X, v.Y, v.Z }).ToArray();

                TriangleIndexVertexArray triangle = new TriangleIndexVertexArray(indicies, verticies);
                BvhTriangleMeshShape shape = new BvhTriangleMeshShape(triangle, true);

                compoundShape.AddChildShape(System.Numerics.Matrix4x4.Identity, shape);
            }

            compoundShape.LocalScaling = scale.ToVector3();
            compoundShape.CalculateLocalInertia(0f);

            Collider = new CollisionObject();
            Collider.UserObject = element;
            Collider.CollisionShape = compoundShape;
            Collider.WorldTransform = btStartTransform;
            PhysicHandler.ManageElement(this, collisionGroup, collisionMask);
        }

        public override void CreateCollider(int collisionGroup = -1, int collisionMask = -1)
        {
            throw new NotImplementedException();
        }

        private static Node FindMeshNode(Node root, String meshname)
        {
            return root.FindNode(meshname);
        }

        private static System.Numerics.Matrix4x4 GetMatrix(Assimp.Matrix4x4 matrix)
        {
            return new System.Numerics.Matrix4x4(
                matrix.A1, matrix.B1, matrix.C1, matrix.D1, // Erste Spalte
                matrix.A2, matrix.B2, matrix.C2, matrix.D2, // Zweite Spalte
                matrix.A3, matrix.B3, matrix.C3, matrix.D3, // Dritte Spalte
                matrix.A4, matrix.B4, matrix.C4, matrix.D4  // Vierte Spalte
            );
        }

        private static System.Numerics.Vector3 TransformVector(Vec3 vec)
        {
            return new System.Numerics.Vector3(vec.X, vec.Y, vec.Z);
        }
    }
}
