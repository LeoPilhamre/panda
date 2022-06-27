using System;
using System.Collections.Generic;


namespace Panda.Utils
{

    internal class Component
    {

    }


    internal sealed class GameObject
    {

        public string name = "unnamed";
        
        public List<Component> components;


        public GameObject()
        {
            components = new List<Component>();
        }

        public GameObject(string name)
        {
            this.name = name;
            components = new List<Component>();
        }


        public void AddComponent<T>() where T : Component
        {
            // components.Add();
        }

    }

}