using System;
using System.Collections.Generic;

#nullable disable

namespace ClothesStore.Model.Model.EF
{
    public partial class ImportDetail
    {
        public int Id { get; set; }
        public int? ImportId { get; set; }
        public int? ConfigProductId { get; set; }
        public int? Stock { get; set; }
        public decimal? Price { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? Status { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
