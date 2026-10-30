using Bidesk.Domain.Entities.Public;
using System.ComponentModel.DataAnnotations;

namespace Bidesk.Application.DTOs.WorkCenter
{
    public class WorkCenterDto
    {
        [MaxLength(20)]
        public string Code { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public string ProgramName { get; set; }
        public bool IsServiceActive { get; set; } = false;
        public double CycleTime { get; set; } = 0;
        public ProductionLineEnt ProductionLine { get; set; }
        public WorkCenterDto GetValueEntity(WorkCenterEnt ent)
        {
            Code = ent.Code;
            Name = ent.Name;
            ProgramName = ent.ProgramName;
            IsServiceActive = ent.IsServiceActive;
            CycleTime = ent.CycleTime;
            ProductionLine = ent.ProductionLine;
            return this;
        }
    }
}
