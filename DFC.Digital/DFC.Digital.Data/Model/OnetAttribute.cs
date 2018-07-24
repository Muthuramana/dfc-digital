﻿namespace DFC.Digital.Data.Model
{
    public class OnetAttribute
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public AttributeType Type { get; set; }

        public string OnetOccupationalCode { get; set; }

        public string SocCode { get; set; }

        public decimal Score { get; set; }
    }
}