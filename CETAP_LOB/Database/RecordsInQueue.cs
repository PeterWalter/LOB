//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CETAP_LOB.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class RecordsInQueue
    {
        public long RecID { get; set; }
        public long Barcode { get; set; }
        public long NBT { get; set; }
        public Nullable<long> SAID { get; set; }
        public string FID { get; set; }
        public string Reason { get; set; }
        public System.DateTime DateModified { get; set; }
    }
}