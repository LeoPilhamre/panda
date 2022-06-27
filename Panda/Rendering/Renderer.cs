using System;
using System.IO;
using System.Reflection;
using System.Numerics;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Utils.Console;


namespace Panda.Rendering
{

    public sealed class Renderer : IDisposable
    {

        private GL gl;

        private BufferObject<float> vbo;
        private BufferObject<uint> ebo;
        private VertexArrayObject<float, uint> vao;
        private Shader shader;

        private PrimitiveShapes.Shape shape;


        public Renderer()
        {

        }


        public unsafe void OnOpen(IWindow window)
        {
            shape = PrimitiveShapes.CreateShape(PrimitiveShapes.ShapeType.Octagon, new Vector4(1, 0, 0, 1), new Vector2(200f, 200f), new Vector2(-100f, -100f));

            gl = GL.GetApi(window);

            //Instantiating our new abstractions
            ebo = new BufferObject<uint>(gl, shape.indices, BufferTargetARB.ElementArrayBuffer);
            vbo = new BufferObject<float>(gl, shape.vertices, BufferTargetARB.ArrayBuffer);
            vao = new VertexArrayObject<float, uint>(gl, vbo, ebo);

            //Telling the VAO object how to lay out the attribute pointers
            vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 7, 0);
            vao.VertexAttributePointer(1, 4, VertexAttribPointerType.Float, 7, 3);

            string basePath = "..\\Panda\\Rendering";
            shader = new Shader(gl, $"{basePath}\\vertexShader.vert", $"{basePath}\\fragmentShader.frag");
        }

        //Method needs to be unsafe due to draw elements.
        public unsafe void OnRender()
        {
            gl.Clear((uint) ClearBufferMask.ColorBufferBit);

            //Binding and using our VAO and shader.
            vao.Bind();
            shader.Use();

            gl.DrawElements(PrimitiveType.Triangles, (uint) shape.indices.Length, DrawElementsType.UnsignedInt, null);
        }

        public void Dispose()
        {
            vbo.Dispose();
            ebo.Dispose();
            vao.Dispose();
            shader.Dispose();
        }
    }

}