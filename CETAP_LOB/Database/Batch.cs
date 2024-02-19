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
    
    public partial class Batch
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Batch()
        {
            QAs = new HashSet<QA>();
            ScannedFiles = new HashSet<ScannedFile>();
        }
    
        public int BatchID { get; set; }
        public string BatchName { get; set; }
        public System.DateTime TestDate { get; set; }
        public string TestCombination { get; set; }
        public string BatchedBy { get; set; }
        public System.DateTime BatchDate { get; set; }
        public int RandTestNumber { get; set; }
        public int Count { get; set; }
        public int TestVenueID { get; set; }
        public int TestProfileID { get; set; }
        public string Description { get; set; }
        public System.DateTime DateModified { get; set; }
        public byte[] RowVersion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QA> QAs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ScannedFile> ScannedFiles { get; set; }
    }
}