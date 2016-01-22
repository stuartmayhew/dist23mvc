﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Dist23MVC.Models
{
    public class PaymentSetup
    {
        [Key]
        public int pKey { get; set; }
        public int DistKey { get; set; }
        public int EventKey { get; set; }
        public Decimal? Amount { get; set; }
        public bool hasSpecial { get; set; }
        public string SpecialLable { get; set; }
        public List<PaymentSpecValues> paymentSpecValues { get; set; }
    }

    public class PaymentSpecValues
    {
        [Key]
        public int pKey { get; set; }
        public int SpecialKey { get; set; }
        public string SpecialValue { get; set; }
        public decimal SpecialAmount { get; set; }
    }



}