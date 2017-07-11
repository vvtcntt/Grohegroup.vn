using System;
using System.Collections.Generic;

namespace GROHE.Models
{
    public partial class tblOrder
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public Nullable<int> Mobile { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public Nullable<double> Tolar { get; set; }
        public Nullable<System.DateTime> DateByy { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<bool> Active { get; set; }
    }
}
