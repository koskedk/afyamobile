﻿using LiveHTS.SharedKernel.Model;

namespace LiveHTS.Core.Model.Config
{
    public class ProviderType:Entity<string>
    {
        public string Name { get; set; }

        public ProviderType()
        {
        }
    }
}