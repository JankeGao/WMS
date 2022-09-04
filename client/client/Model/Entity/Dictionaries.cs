﻿using System;

namespace wms.Client.Model.Entity
{
    [Serializable]
    public class Dictionaries : BaseEntity
    {
        public string TypeCode { get; set; }

        public string DataCode { get; set; }

        public string NativeName { get; set; }

        public string EnglishName { get; set; }

        public DateTime CreateTime { get; set; }

        public string CreateBy { get; set; }

        public DateTime LastTime { get; set; }

        public string LastTimeBy { get; set; }
    }
}
