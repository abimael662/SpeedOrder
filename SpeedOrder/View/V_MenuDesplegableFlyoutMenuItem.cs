using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedOrder.View
{
    public class V_MenuDesplegableFlyoutMenuItem
    {
        public V_MenuDesplegableFlyoutMenuItem()
        {
            TargetType = typeof(V_MenuDesplegableFlyoutMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Icono { get; set; }

        public Type TargetType { get; set; }
    }
}