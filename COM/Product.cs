//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace COM
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        public int PID { get; set; }
        public string Name { get; set; }
        public int CatID { get; set; }
        public int SubCatID { get; set; }
        public int Price { get; set; }
        public int PriceOff { get; set; }
        public string Description { get; set; }
        public string specification { get; set; }
        public string Brand { get; set; }
        public Nullable<int> StockCount { get; set; }
    }
}