using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OCTOBER.Shared.DTO
{
    public class GradeConversionDTO
    {
        [Precision(8)]
        public int SchoolId { get; set; }
        [StringLength(2)]
        [Unicode(false)]
        public string LetterGrade { get; set; } = null!;
        public decimal GradePoint { get; set; }
        [Precision(3)]
        public byte MaxGrade { get; set; }
        [Precision(3)]
        public byte MinGrade { get; set; }
        [StringLength(30)]
        [Unicode(false)]
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        [StringLength(30)]
        [Unicode(false)]
        public string ModifiedBy { get; set; } = null!;
        public DateTime ModifiedDate { get; set; }
    }
}
