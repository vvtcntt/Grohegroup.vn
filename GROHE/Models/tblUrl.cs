using System;
using System.Collections.Generic;

namespace GROHE.Models
{
    public partial class tblUrl
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public Nullable<int> Ord { get; set; }
        public Nullable<bool> Active { get; set; }
        public string Rel { get; set; }
        public Nullable<System.DateTime> DateCreate { get; set; }
        public Nullable<int> idUser { get; set; }
    }
}
