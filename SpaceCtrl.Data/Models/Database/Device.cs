namespace SpaceCtrl.Data.Models.Database
{
    public partial class Device
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public int? OrderIndex { get; set; }
        public int TargetId { get; set; }

        public virtual TargetGroup Target { get; set; }
    }
}
