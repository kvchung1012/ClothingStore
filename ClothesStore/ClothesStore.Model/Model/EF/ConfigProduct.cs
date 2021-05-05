using System;
using System.Collections.Generic;

#nullable disable

namespace ClothesStore.Model.Model.EF
{
    public partial class ConfigProduct
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? SizeId { get; set; }
        public int? ColorId { get; set; }
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
