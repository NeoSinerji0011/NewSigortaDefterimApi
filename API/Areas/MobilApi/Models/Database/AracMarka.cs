﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.MobilApi.Models.Database
{
    public class AracMarka
    {
        [Key]

        public string MarkaKodu { get; set; }
        public string MarkaAdi { get; set; }
    }
}