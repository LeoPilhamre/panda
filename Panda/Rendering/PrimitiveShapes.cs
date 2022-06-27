using System.Numerics;
using Silk.NET.Maths;
using Panda.Utils;


namespace Panda.Rendering
{

    internal static class PrimitiveShapes
    {

        internal class Shape
        {
            public float[] vertices;
            public uint[] indices;
        }

        internal enum ShapeType
        {
            Triangle,
            Rectangle,
            Hexagon,
            Octagon,
        }

        private static readonly Shape triangle = new Shape() {
            vertices = new float[]
            {
            //   X      Y     Z    
                -1.0f, -1.0f, 0.0f,
                 1.0f, -1.0f, 0.0f,
                 0.0f,  1.0f, 0.0f,
            },
            indices = new uint[]
            {
                1, 0, 2,
            }
        };

        private static readonly Shape rectangle = new Shape() {
            vertices = new float[]
            {
            //   X      Y     Z    
                -1.0f, -1.0f, 0.0f,
                 1.0f, -1.0f, 0.0f,
                 1.0f,  1.0f, 0.0f,
                -1.0f,  1.0f, 0.0f,
            },
            indices = new uint[]
            {
                2, 1, 0,
                0, 3, 2,
            }
        };

        private static readonly Shape hexagon = new Shape() {
            vertices = new float[]
            {
            //   X      Y     Z    
                 0.0f,  0.0f, 0.0f,
                 
                -1.0f,  0.0f, 0.0f,
                -0.5f, -1.0f, 0.0f,

                -0.5f, -1.0f, 0.0f,
                 0.5f, -1.0f, 0.0f,

                 0.5f, -1.0f, 0.0f,
                 1.0f,  0.0f, 0.0f,

                 0.5f,  1.0f, 0.0f,
                 1.0f,  0.0f, 0.0f,

                -0.5f,  1.0f, 0.0f,
                 0.5f,  1.0f, 0.0f,

                -1.0f,  0.0f, 0.0f,
                -0.5f,  1.0f, 0.0f,
            },
            indices = new uint[]
            {
                1, 2, 0,
                3, 4, 0,
                5, 6, 0,
                7, 8, 0,
                9, 10, 0,
                11, 12, 0,
            }
        };

        private static readonly Shape octagon = new Shape() {
            vertices = new float[]
            {
            //   X      Y     Z    
                 0.0f,  0.0f, 0.0f,
                 
                -1.0f, -0.5f, 0.0f,
                -0.5f, -1.0f, 0.0f,
                 0.5f, -1.0f, 0.0f,
                 1.0f, -0.5f, 0.0f,
                 1.0f,  0.5f, 0.0f,
                 0.5f,  1.0f, 0.0f,
                 0.5f,  1.0f, 0.0f,
                -0.5f,  1.0f, 0.0f,
                -1.0f,  0.5f, 0.0f,
            },
            indices = new uint[]
            {
                2, 1, 0,
                3, 2, 0,
                4, 3, 0,
                5, 4, 0,
                6, 5, 0,
                7, 6, 0,
                8, 7, 0,
                9, 8, 0,
                1, 9, 0,
            }
        };

        private static readonly Dictionary<ShapeType, Shape> shapes = new Dictionary<ShapeType, Shape>()
        {
            { ShapeType.Triangle, triangle },
            { ShapeType.Rectangle, rectangle },
            { ShapeType.Hexagon, hexagon },
            { ShapeType.Octagon, octagon },
        };


        public static Shape CreateShape(ShapeType type, Vector4 color, Vector2 size, Vector2 pos)
        {
            Vector2D<int> windowSize = Window.window.Size;

            float[] vertices = shapes[type].vertices;

            for (int i = 0; i < vertices.Length; i++)
            {
                if (i % 3 == 0)
                    vertices[i] = vertices[i] * Mathv.Remap(size.X, 0f, windowSize.X, 0f, 1f) + Mathv.Remap(pos.X, 0f, windowSize.X / 2, 0f, 1f);
                if ((i - 1) % 3 == 0)
                    vertices[i] = vertices[i] * Mathv.Remap(size.Y, 0f, windowSize.Y, 0f, 1f) + Mathv.Remap(pos.Y, 0f, windowSize.Y / 2, 0f, 1f);
            }

            List<float> verticesWColors = new List<float>();
            for (int i = 0; i < vertices.Length; i++)
            {
                verticesWColors.Add(vertices[i]);
                if ((i + 1) % 3 == 0 && i != 0)
                    verticesWColors.AddRange(new List<float>(){ color.X, color.Y, color.Z, color.W });
            }


            uint[] indices = shapes[type].indices;

            Shape shape = new Shape();
            shape.vertices = verticesWColors.ToArray();
            shape.indices = indices;

            return shape;
        }

    }

}