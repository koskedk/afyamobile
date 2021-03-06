﻿using LiveHTS.SharedKernel.Model;

namespace LiveHTS.Core.Model.Config
{
    public class County:Entity<int>
    {
        public string Name { get; set; }

        public County()
        {
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}