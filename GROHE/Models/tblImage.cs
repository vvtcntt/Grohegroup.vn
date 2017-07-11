using System;
using System.Collections.Generic;

namespace GROHE.Models
{
    public partial class tblImage
    {
        public int id { get; set; }
        public Nullable<int> idMenu { get; set; }
        public Nullable<int> idCate { get; set; }
        public string Name { get; set; }
        public string Images { get; set; }
        public string Url { get; set; }
        public Nullable<int> Ord { get; set; }
        public string Link { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<System.DateTime> DateCreate { get; set; }
        public Nullable<int> idUser { get; set; }
    }
}
