using Bidesk.Domain.Entities.Public;

namespace Bidesk.Application.ViewModels.WorkCenter
{
    public class WorkCenterVM
    {
        public long Value { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ProgramName { get; set; } = "_G_XXXXXX";
        public bool IsServiceActive { get; set; } = false;
        public double CycleTime { get; set; } = 0;
        public int OldMachineID { get; set; } = 0;
        public string? ProductionLineCode { get; set; }
        public string? ProductionLineDescription { get; set; }
        public long ProductionLineId { get; set; }
        public bool IsHasContinuousCounter { get; set; }
        public WorkCenterVM GetValueFromEntity(WorkCenterEnt ent)
        {
            Value = ent.ID;
            Code = ent.Code;
            Name = ent.Name;
            //ProductionLineCode = ent.ProductionLine.Code;
            //ProductionLineDescription = ent.ProductionLine.Name;
            ProductionLineId = ent.ProductionLineID;
            ProgramName = ent.ProgramName;
            IsServiceActive = ent.IsServiceActive;
            CycleTime = ent.CycleTime;
            OldMachineID = ent.OldMachineID;
            IsHasContinuousCounter = ent.IsHasContinuousCounter;
            return this;
        }

        public WorkCenterEnt ToEntity()
        {
            return new WorkCenterEnt
            {
                ID = Value,
                Name = Name,
                Code = Code,
                ProductionLineID = ProductionLineId,
                ProgramName = ProgramName,
                IsServiceActive = IsServiceActive,
                CycleTime = CycleTime,
                OldMachineID = OldMachineID,
                IsHasContinuousCounter= IsHasContinuousCounter
            };
        }
    }

}
