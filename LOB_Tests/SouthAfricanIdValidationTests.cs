using System;
using System.Collections.Generic;
using System.Linq;
using CETAP_LOB.BDO;
using CETAP_LOB.Helper;
using CETAP_LOB.Model.venueprep;
using Xunit;

namespace LOB_Tests
{
    public class SouthAfricanIdValidationTests
    {
        [Theory]
        [InlineData("8001015009087", true)]
        [InlineData("8001015009088", false)]
        [InlineData("8001015009287", false)]
        [InlineData("8013015009087", false)]
        [InlineData("80010150090", false)]
        [InlineData("800101500908A", false)]
        public void HelperUtils_validates_sa_id_rules(string id, bool expectedValid)
        {
            Assert.Equal(expectedValid, HelperUtils.IsValidSAID(id));
        }

        [Fact]
        public void WebWriters_reports_database_differences_for_requested_fields()
        {
            string reference = BuildValidReference("310027017415");
            string said = BuildValidSaid("800101500908");
            DateTime dob = new DateTime(1980, 1, 1);

            var writer = new WritersBDO
            {
                NBT = long.Parse(reference),
                Name = "Tinyiko",
                Surname = "Shipalana",
                SAID = long.Parse(said),
                ForeignID = "PASS123",
                DOB = dob
            };

            var composit = new CompositBDO
            {
                RefNo = long.Parse(reference),
                Name = "Tinyiko",
                Surname = "Shipalana",
                SAID = long.Parse(said),
                ForeignID = "PASS123",
                DOB = dob
            };

            var record = new WebWriters
            {
                Reference = reference,
                FirstName = "Tinyiko",
                Surname = "Shipalana",
                SAID = said,
                ForeignID = "PASS123",
                DOB = dob
            };

            record.AttachWriterRecord(writer);
            record.AttachCompositRecord(composit);
            Assert.False(record.HasErrors);

            record.FirstName = "Different";
            Assert.Contains(GetErrors(record, "FirstName"), m => m.Contains("WriterList name differs from database"));
            Assert.Contains(GetErrors(record, "FirstName"), m => m.Contains("Composit name differs from database"));

            record.Surname = "Different";
            Assert.Contains(GetErrors(record, "Surname"), m => m.Contains("WriterList surname differs from database"));
            Assert.Contains(GetErrors(record, "Surname"), m => m.Contains("Composit surname differs from database"));

            record.SAID = BuildValidSaid("800101500918");
            Assert.Contains(GetErrors(record, "SAID"), m => m.Contains("WriterList SA ID differs from database"));
            Assert.Contains(GetErrors(record, "SAID"), m => m.Contains("Composit SA ID differs from database"));

            record.ForeignID = "FOREIGN-2";
            Assert.Contains(GetErrors(record, "ForeignID"), m => m.Contains("WriterList foreign ID differs from database"));
            Assert.Contains(GetErrors(record, "ForeignID"), m => m.Contains("Composit foreign ID differs from database"));

            record.DOB = new DateTime(1981, 2, 2);
            Assert.Contains(GetErrors(record, "DOB"), m => m.Contains("WriterList date of birth differs from database"));
            Assert.Contains(GetErrors(record, "DOB"), m => m.Contains("Composit date of birth differs from database"));

            record.Reference = BuildValidReference("310027017425");
            Assert.Contains(GetErrors(record, "Reference"), m => m.Contains("WriterList reference number differs from database"));
            Assert.Contains(GetErrors(record, "Reference"), m => m.Contains("Composit reference number differs from database"));
        }

        [Fact]
        public void WebWriters_can_accept_file_value_to_clear_selected_difference()
        {
            string reference = BuildValidReference("310027017415");
            string said = BuildValidSaid("800101500908");
            DateTime dob = new DateTime(1980, 1, 1);

            var writer = new WritersBDO
            {
                NBT = long.Parse(reference),
                Name = "Tinyiko",
                Surname = "Shipalana",
                SAID = long.Parse(said),
                ForeignID = "PASS123",
                DOB = dob
            };

            var record = new WebWriters
            {
                Reference = reference,
                FirstName = "Different",
                Surname = "Shipalana",
                SAID = said,
                ForeignID = "PASS123",
                DOB = dob
            };

            record.AttachWriterRecord(writer);
            Assert.True(record.IsWriterValueDifferent("Name"));
            Assert.Contains(GetErrors(record, "FirstName"), m => m.Contains("WriterList name differs from database"));

            record.AcceptFileValueForWriter("Name");

            Assert.False(record.IsWriterValueDifferent("Name"));
            Assert.DoesNotContain(GetErrors(record, "FirstName"), m => m.Contains("WriterList name differs from database"));
            Assert.Equal("Tinyiko", record.GetCompositValue("Name"));
        }

        [Fact]
        public void WebWriters_propagates_writer_value_to_file_and_composit()
        {
            string reference = BuildValidReference("310027017415");
            string said = BuildValidSaid("800101500908");
            DateTime dob = new DateTime(1980, 1, 1);

            var writer = new WritersBDO
            {
                NBT = long.Parse(reference),
                Name = "Tinyiko",
                Surname = "Shipalana",
                SAID = long.Parse(said),
                ForeignID = "PASS123",
                DOB = dob
            };

            var composit = new CompositBDO
            {
                RefNo = long.Parse(reference),
                Name = "Tinyiko",
                Surname = "Different",
                SAID = long.Parse(said),
                ForeignID = "PASS123",
                DOB = dob
            };

            var record = new WebWriters
            {
                Reference = reference,
                FirstName = "Tinyiko",
                Surname = "Shipalana",
                SAID = said,
                ForeignID = "PASS123",
                DOB = dob
            };

            record.AttachWriterRecord(writer);
            record.AttachCompositRecord(composit);
            record.Surname = "FileSurname";

            record.ApplyWriterValue("Surname");

            Assert.Equal("Shipalana", record.Surname);
            Assert.Equal("Shipalana", record.GetCompositValue("Surname"));
            Assert.False(record.IsCompositValueDifferent("Surname"));
        }

        [Fact]
        public void WebWriters_replaces_walk_in_reference_with_non_walk_in_reference()
        {
            string walkInReference = BuildValidReference("310027017415");
            string canonicalReference = BuildValidReference("310027017425");

            var writer = new WritersBDO
            {
                NBT = long.Parse(canonicalReference),
                Name = "Tinyiko",
                Surname = "Shipalana"
            };

            var record = new WebWriters
            {
                Reference = walkInReference,
                FirstName = "Tinyiko",
                Surname = "Shipalana"
            };

            record.AttachWriterRecord(writer);
            record.ApplyWriterValue("Name");

            Assert.Equal(canonicalReference, record.Reference);
            Assert.Equal(canonicalReference, record.GetWriterValue("Reference"));
        }

        [Fact]
        public void WebWriters_only_updates_sources_that_differ_from_the_accepted_value()
        {
            string reference = BuildValidReference("310027017415");
            string said = BuildValidSaid("800101500908");
            DateTime dob = new DateTime(1980, 1, 1);

            var writer = new WritersBDO
            {
                NBT = long.Parse(reference),
                Name = "Tinyiko",
                Surname = "Shipalana",
                SAID = long.Parse(said),
                ForeignID = "PASS123",
                DOB = dob
            };

            var composit = new CompositBDO
            {
                RefNo = long.Parse(reference),
                Name = "Tinyiko",
                Surname = "Different",
                SAID = long.Parse(said),
                ForeignID = "PASS999",
                DOB = dob
            };

            var record = new WebWriters
            {
                Reference = reference,
                FirstName = "Tinyiko",
                Surname = "Shipalana",
                SAID = said,
                ForeignID = "PASS123",
                DOB = dob
            };

            record.AttachWriterRecord(writer);
            record.AttachCompositRecord(composit);

            record.ApplyWriterValue("Surname");

            Assert.Equal("Shipalana", record.Surname);
            Assert.Equal("Shipalana", record.GetCompositValue("Surname"));
            Assert.Equal("PASS999", record.GetCompositValue("ForeignID"));

            record.AcceptFileValueForComposit("ForeignID");

            Assert.Equal("PASS123", record.GetWriterValue("ForeignID"));
            Assert.Equal("PASS123", record.GetCompositValue("ForeignID"));
        }

        private static IEnumerable<string> GetErrors(WebWriters record, string property)
        {
            return record.GetErrors(property)?.Cast<string>() ?? Enumerable.Empty<string>();
        }

        private static string BuildValidReference(string body12)
        {
            string body13 = BuildValidChecksumBody(body12);
            string reference = "9" + body13;

            if (HelperUtils.IsValidChecksum(reference.Substring(1, 13)))
                return reference;

            throw new InvalidOperationException("Unable to build a valid reference number for the test.");
        }

        private static string BuildValidChecksumBody(string body12)
        {
            for (int digit = 0; digit <= 9; digit++)
            {
                string candidate = body12 + digit;
                if (HelperUtils.IsValidChecksum(candidate))
                    return candidate;
            }

            throw new InvalidOperationException("Unable to build a valid checksum body for the test.");
        }

        private static string BuildValidSaid(string body12)
        {
            for (int digit = 0; digit <= 9; digit++)
            {
                string candidate = body12 + digit;
                if (HelperUtils.IsValidSAID(candidate))
                    return candidate;
            }

            throw new InvalidOperationException("Unable to build a valid SA ID for the test.");
        }
    }
}
