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
    
    public partial class TestVenue
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TestVenue()
        {
            WriterLists = new HashSet<WriterList>();
        }
    
        public int VenueCode { get; set; }
        public string VenueName { get; set; }
        public string ShortName { get; set; }
        public string WebsiteName { get; set; }
        public string Room { get; set; }
        public string VenueType { get; set; }
        public int ProvinceID { get; set; }
        public Nullable<int> Capacity { get; set; }
        public string Comments { get; set; }
        public bool Available { get; set; }
        public byte[] RowVersion { get; set; }
        public System.Guid RowGuid { get; set; }
        public System.DateTime DateModified { get; set; }
        public System.Data.Entity.Spatial.DbGeography Place { get; set; }
    
        public virtual Province Province { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WriterList> WriterLists { get; set; }
    }
}
