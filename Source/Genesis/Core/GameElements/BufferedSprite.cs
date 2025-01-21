using Genesis.Graphics;
using Genesis.Math;
using GlmSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static Genesis.Core.WindowUtilities;

namespace Genesis.Core.GameElements
{
    public class BufferedSpriteInstance : GameElement
    {
        public Vec3 Location { get; set; }
        public Vec3 Rotation { get; set; }
        public Vec3 Size { get; set; }
        public Color Color { get; set; }
        public Vector4 UVTransform { get; set; }
        public bool Visible { get; set; }

        public float[] GetMatrixArray()
        {
            var t_mat = mat4.Translate(Location.ToGlmVec3());
            var r_mat = mat4.RotateX(Utils.ToRadians(Rotation.X)) * mat4.RotateY(Utils.ToRadians(Rotation.Y)) * mat4.RotateZ(Utils.ToRadians(Rotation.Z));
            var s_mat = mat4.Scale(Size.ToGlmVec3());

            var matrix = t_mat * r_mat * s_mat;
            return matrix.ToArray();
        }

        public BufferedSpriteInstance()
        {
            this.Location = new Vec3(0.0f, 0.0f, 0.0f);
            this.Rotation = new Vec3(0.0f, 0.0f, 0.0f);
            this.Size = new Vec3(1.0f, 1.0f, 1.0f);
            this.Color = Color.White;
            this.UVTransform = DefaultUVTransform();
            this.Visible = false;
        }

        public static Vector4 DefaultUVTransform()
        {
            return new Vector4(1.0f, 1.0f, 0.0f, 0.0f);
        }

        public float[] GetColorArray()
        {
            return Utils.ConvertColor(Color);
        }

        public float[] GetUVTransformArray()
        {
            return [UVTransform.X, UVTransform.Y, UVTransform.Z, UVTransform.W];
        }

        public float[] GetExtrasArray()
        {
            return [this.Visible ? 1.0f : 0.0f, 0.0f, 0.0f, 0.0f];
        }
    }

    /// <summary>
    /// Represents a game element that creates a buffered sprite with vertices, colors, and texture coordinates.
    /// </summary>
    public class BufferedSprite : GameElement
    {
        public List<BufferedSpriteInstance> Instances { get; set; }
        public Texture Texture { get; set; }


        public BufferedSprite()
        {
            this.Instances = new List<BufferedSpriteInstance>();
        }

        public void BakeInstances(int count)
        {
            for (int i = 0; i < count; i++)
            {
                this.Instances.Add(new BufferedSpriteInstance());
            }
        }

        public int CreateInstance(Vec3 location, Vec3 rotation, Vec3 size, Color color)
        {
            this.Instances.Add(new BufferedSpriteInstance
            {
                Location = location,
                Rotation = rotation,
                Size = size,
                Color = color,
                UVTransform = BufferedSpriteInstance.DefaultUVTransform(),
                Visible = true
            });
            return this.Instances.Count;
        }

        /// <summary>
        /// Initializes the game element.
        /// </summary>
        /// <param name="game">The game instance.</param>
        /// <param name="renderDevice">The render device used for rendering.</param>
        public override void Init(Game game, IRenderDevice renderDevice)
        {
            Debug.WriteLine("Initializing BufferedSprite"); 
            base.Init(game, renderDevice);
        }

        /// <summary>
        /// Renders the game element.
        /// </summary>
        /// <param name="game">The game instance.</param>
        /// <param name="renderDevice">The render device used for rendering.</param>
        public override void OnRender(Game game, IRenderDevice renderDevice)
        {
            base.OnRender(game, renderDevice);
            renderDevice.DrawBufferedSprite(this);
        }

        /// <summary>
        /// Updates the game element.
        /// </summary>
        /// <param name="game">The game instance.</param>
        /// <param name="renderDevice">The render device used for rendering.</param>
        public override void OnUpdate(Game game, IRenderDevice renderDevice)
        {
            base.OnUpdate(game, renderDevice);
        }

        /// <summary>
        /// Clears the GPU memory
        /// </summary>
        /// <param name="game"></param>
        public override void OnDestroy(Game game)
        {
            base.OnDestroy(game);
            game.RenderDevice.DisposeElement(this);
        }

        public static float[] GetVertexBuffer()
        {
            return
            [
                -0.5f, -0.5f, 0.0f,
                -0.5f, 0.5f, 0.0f,
                0.5f, 0.5f, 0.0f,
                0.5f, -0.5f, 0.0f
            ];
        }

        public static int[] GetIndexBuffer()
        {
            return [
                0, 1, 3,
                3, 1, 2
            ];
        }

        public static float[] GetUVBuffer()
        {
            return [
                0.0f, 0.0f,
                0.0f, 1.0f,
                1.0f, 1.0f,
                1.0f, 0.0f
            ];
        }
    }
}
