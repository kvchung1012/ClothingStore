using System;
using System.Collections.Generic;

#nullable disable

namespace ClothesStore.Model.Model.EF
{
    public partial class ProductImage
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public string Image { get; set; }
    }
}
