﻿using Api.Models;

namespace Api.Controllers.Models
{
    public struct CreateRobotQuery
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool Enabled { get; set; }
        public RobotStatus Status { get; set; }
    }
}