using System;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Utils.Console;


namespace Panda.Rendering
{

    internal sealed class Shader
    {

        //Our handle and the GL instance this class will use, these are private because they have no reason to be public.
        //Most of the time you would want to abstract items to make things like this invisible.
        private uint handle;
        private GL gl;

        public Shader(GL gl, string vertexPath, string fragmentPath)
        {
            this.gl = gl;

            //Load the individual shaders.
            uint vertex = LoadShader(ShaderType.VertexShader, vertexPath);
            uint fragment = LoadShader(ShaderType.FragmentShader, fragmentPath);
            //Create the shader program.
            handle = gl.CreateProgram();
            //Attach the individual shaders.
            gl.AttachShader(handle, vertex);
            gl.AttachShader(handle, fragment);
            gl.LinkProgram(handle);
            //Check for linking errors.
            gl.GetProgram(handle, GLEnum.LinkStatus, out var status);
            if (status == 0)
                throw new Exception($"Program failed to link with error: {gl.GetProgramInfoLog(handle)}");

            //Detach and delete the shaders
            gl.DetachShader(handle, vertex);
            gl.DetachShader(handle, fragment);
            gl.DeleteShader(vertex);
            gl.DeleteShader(fragment);
        }

        public void Use()
        {
            //Using the program
            gl.UseProgram(handle);
        }

        //Uniforms are properties that applies to the entire geometry
        public void SetUniform(string name, int value)
        {
            //Setting a uniform on a shader using a name.
            int location = gl.GetUniformLocation(handle, name);
            if (location == -1) //If GetUniformLocation returns -1 the uniform is not found.
                throw new Exception($"{name} uniform not found on shader.");

            gl.Uniform1(location, value);
        }

        public void SetUniform(string name, float value)
        {
            int location = gl.GetUniformLocation(handle, name);
            if (location == -1)
                throw new Exception($"{name} uniform not found on shader.");

            gl.Uniform1(location, value);
        }

        public void Dispose()
        {
            //Remember to delete the program when we are done.
            gl.DeleteProgram(handle);
        }

        private uint LoadShader(ShaderType type, string path)
        {
            //To load a single shader we need to:
            //1) Load the shader from a file.
            //2) Create the handle.
            //3) Upload the source to opengl.
            //4) Compile the shader.
            //5) Check for errors.
            string src = File.ReadAllText(path);
            uint handle = gl.CreateShader(type);
            gl.ShaderSource(handle, src);
            gl.CompileShader(handle);
            string infoLog = gl.GetShaderInfoLog(handle);
            if (!string.IsNullOrWhiteSpace(infoLog))
                throw new Exception($"Error compiling shader of type {type}, failed with error {infoLog}");

            return handle;
        }

    }

}