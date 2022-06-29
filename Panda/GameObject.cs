using System;
using System.Collections.Generic;


namespace Panda.Utils
{

    internal class Component<T>
    {

    }


    internal sealed class GameObject<T> where T : Entity
    {

        public string name = "unnamed";
        
        public List<Component<T>> components;


        public GameObject()
        {
            components = new List<Component<T>>();

            // Entity entity = new T() as Entity;
        }

        public GameObject(string name)
        {
            this.name = name;
            components = new List<Component<T>>();
        }


        public void AddComponent<T>()
        {
            // components.Add(new Component<T>());
        }

    }

}