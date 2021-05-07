using ClothesStore.Model.Model.EF;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClothesStore.Model.ModelView
{
    public class ConfigProductModelView
    {
        public ConfigProduct config { get; set; }
        public Size size { get; set; }
        public Color color { get; set; }
    }
}
