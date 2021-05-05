using System;
using System.Collections.Generic;

#nullable disable

namespace ClothesStore.Model.Model.EF
{
    public partial class Size
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? OrderBy { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? Status { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
