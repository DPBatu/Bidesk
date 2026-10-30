namespace Bidesk.Domain.Entities.Public
{
    public class WorkCenterEnt: BaseEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ProgramName { get; set; } = "_G_XXXXXX";
        public bool IsServiceActive { get; set; } = false;
        public double CycleTime { get; set; } = 0;
        public bool IsHasContinuousCounter { get; set; }
        public int OldMachineID { get; set; } = 0;
        public long ProductionLineID { get; set; }
        public ProductionLineEnt? ProductionLine { get; set; }
        public long? LastWorkingMaterialID { get; set; }
        public MaterialEnt? LastWorkingMaterial { get; set; }
        public bool IsWorking { get; set; }
        public bool IsStopped { get; set; }
        public bool IsWarmUp { get; set; }
    }
}
