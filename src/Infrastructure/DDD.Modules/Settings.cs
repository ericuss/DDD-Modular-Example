using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Modules
{
    public class Settings
    {
        public Settings()
        {
            this.Modules = new List<Module>();
        }

        public Database Database{ get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public List<Module> Modules { get; set; }
    }

    public class Database
    {
        public bool UseInMemory { get; set; }
        public bool Regenerate { get; set; }
    }

    public class Module
    {
        public string Name { get; set; }
        public string Assembly { get; set; }
    }
    public class ConnectionStrings
    {
        public string Customer { get; set; }
        public string Users { get; set; }
        public string Storage { get; set; }
    }
}
