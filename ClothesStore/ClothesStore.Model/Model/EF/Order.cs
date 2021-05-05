using System;
using System.Collections.Generic;

#nullable disable

namespace ClothesStore.Model.Model.EF
{
    public partial class Order
    {
        public int Id { get; set; }
        public int? EmployeeId { get; set; }
        public int? CustomerId { get; set; }
        public string Note { get; set; }
        public string Address { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? Status { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
