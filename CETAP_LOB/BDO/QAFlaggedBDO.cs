// Type: LOB.BDO.QAFlaggedBDO
// A record flagged on the QA grid whose details could not be confirmed, held in
// [NBT_Production].[Process].[QA_Flagged] while the administrators trace the writer.

using System;

namespace CETAP_LOB.BDO
{
  /// <summary>
  /// One row of Process.QA_Flagged: the details of a QA record that could not be confirmed.
  /// The barcode identifies the row (it is the table's primary key).
  /// </summary>
  public class QAFlaggedBDO
  {
    /// <summary>The scanned barcode, the table's primary key.</summary>
    public long Barcode { get; set; }

    /// <summary>The NBT reference of the record as it was flagged.</summary>
    public long NBT { get; set; }

    public string Surname { get; set; }

    public string Name { get; set; }

    /// <summary>South African ID when the record had one.</summary>
    public long? SAID { get; set; }

    /// <summary>Foreign ID or passport number when the record had one.</summary>
    public string ForeignID { get; set; }

    public DateTime DOB { get; set; }

    /// <summary>
    /// The venue code of the file the record came from: a venue is identified by its code
    /// throughout the database (TestVenues and Composit have no separate VenueID column), so
    /// this holds the code itself.
    /// </summary>
    public int VenueID { get; set; }

    /// <summary>Date of test - the date the flagged list is reported by.</summary>
    public DateTime DOT { get; set; }

    /// <summary>The QA file name without its extension.</summary>
    public string Batch { get; set; }

    public string CreatedBy { get; set; }

    public DateTime DateCreated { get; set; }

    public string ModifiedBy { get; set; }

    public DateTime DateModified { get; set; }

    /// <summary>How the record reads on the flagged list.</summary>
    public string Display
    {
      get
      {
        return Barcode + "  " + Surname + ", " + Name + "  NBT " + NBT + "  " + (SAID.HasValue ? SAID.Value.ToString("D13") : (ForeignID ?? ""))
             + "  " + DOB.ToString("yyyy-MM-dd") + "  " + Batch;
      }
    }
  }
}
