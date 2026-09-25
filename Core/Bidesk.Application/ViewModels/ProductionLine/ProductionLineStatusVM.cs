using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bidesk.Application.ViewModels.ProductionLine
{
    public class ProductionLineStatusVM
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public int ActiveWorkCenterCount { get; set; } = 0;
        public int NotActiveWorkCenterCount { get; set; } = 0;
        public int MesWorkCenterCount { get; set; } = 0;
        public int WorkCenterCount { get; set; } = 0;

    }
}
