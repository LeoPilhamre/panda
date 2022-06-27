using System;
using System.Collections.Generic;
using Panda.Entities;


namespace Panda.Utils
{

    internal class Component<T>
    {

    }


    internal sealed class GameObject<T> where T : Entity
    {

        public string name = "unnamed";
        
        public List<Component> components;


        public GameObject()
        {
            components = new List<Component>();

            Entity entity = new T() as Entity;
        }

        public GameObject(string name)
        {
            this.name = name;
            components = new List<Component>();
        }


        public void AddComponent<T>() where T : Component
        {
            components.Add(new Component<T>());
        }

    }

}