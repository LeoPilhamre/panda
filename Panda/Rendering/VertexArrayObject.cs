using Silk.NET.OpenGL;
using System;

namespace Panda.Rendering
{

    //The vertex array object abstraction.
    public class VertexArrayObject<TVertexType, TIndexType> : IDisposable
        where TVertexType : unmanaged
        where TIndexType : unmanaged
    {

        //Our handle and the GL instance this class will use, these are private because they have no reason to be public.
        //Most of the time you would want to abstract items to make things like this invisible.
        private uint handle;
        private GL gl;

        public VertexArrayObject(GL gl, BufferObject<TVertexType> vbo, BufferObject<TIndexType> ebo)
        {
            //Saving the GL instance.
            this.gl = gl;

            //Setting out handle and binding the VBO and EBO to this VAO.
            handle = gl.GenVertexArray();
            Bind();
            vbo.Bind();
            ebo.Bind();
        }

        public unsafe void VertexAttributePointer(uint index, int count, VertexAttribPointerType type, uint vertexSize, int offSet)
        {
            //Setting up a vertex attribute pointer
            gl.VertexAttribPointer(index, count, type, false, vertexSize * (uint) sizeof(TVertexType), (void*) (offSet * sizeof(TVertexType)));
            gl.EnableVertexAttribArray(index);
        }

        public void Bind() => gl.BindVertexArray(handle);

        public void Dispose() => gl.DeleteVertexArray(handle);

    }

}