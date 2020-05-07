namespace SpaceCtrl.Front.Models.Common
{
    public class Dropdown
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Dropdown(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}