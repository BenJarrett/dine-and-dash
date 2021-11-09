﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DineNDash.Models
{
    public class DetailedOrderView
    {
        public Guid ProductOrderId { get; set; }
        public int ProductQuantity { get; set; }
        public string ProductName { get; set; }
        public decimal IndividualProductPrice { get; set; }
        public string ProductDescription { get; set; }
        public string Image { get; set; }
        public ProductType ProductType { get; set; }
        public string PaymentType { get; set; }
        public string AccountNumber { get; set; }

    }
}
