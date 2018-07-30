﻿using System;
using System.Collections.Generic;
using LiveHTS.Core.Model.Config;
using LiveHTS.Core.Model.Subject;

namespace LiveHTS.Core.Interfaces.Services.Config
{
    public interface IDeviceSetupService
    {
        bool IsSetup();
        bool HasPulledData();
        Device GetDefault(Guid deviceId);
        Device GetDefault(string serial="");
        ServerConfig GetCentral();
        ServerConfig GetLocal();
        void SaveCentral(ServerConfig config);
        void SaveLocal(ServerConfig config);
        void Register(Device device);
        void CheckRegister(Device device);
        void UpdateCode(string prefix);
        void SavePractce(Practice practice);
        void SaveUsers(List<User> users);
    }
}