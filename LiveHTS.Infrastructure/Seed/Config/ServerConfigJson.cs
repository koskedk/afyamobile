﻿using System.Collections.Generic;
using LiveHTS.Core.Interfaces.Repository;
using LiveHTS.Core.Model.Config;
using Newtonsoft.Json;

namespace LiveHTS.Infrastructure.Seed.Config
{
  public  class ServerConfigJson : ISeedJson<ServerConfig>
    {
        public  List<ServerConfig> Read()
        {
            string raw = @"
[
  {
    ^Id^: ^hapi.central^,    
    ^Address^: ^http://data.kenyahmis.org:6000/^,
    ^Code^: ^1^,
    ^Name^: ^^,
    ^Instance^: ^00000000-0000-0000-0000-000000000000^,
    ^Voided^: 0
  },
  {
    ^Id^: ^hapi.local^,    
    ^Address^: ^http://DESKTOP-VO0N4SS:6000/^,
    ^Code^: ^^,
    ^Name^: ^^,
    ^Instance^: ^00000000-0000-0000-0000-000000000000^,
    ^Voided^: 0
  }
]
";
            return JsonConvert.DeserializeObject<List<ServerConfig>>(raw.Replace("^","\""));
        }
    }
}
