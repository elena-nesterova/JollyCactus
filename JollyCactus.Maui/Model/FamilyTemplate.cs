using SQLite;
using SQLiteNetExtensions.Attributes;

namespace JollyCactus.Maui.Model
{
    [Table("FamilyTemplates")]
    public class FamilyTemplate
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        [Unique]
        [Column("Name")]
        public string Name { get; set; } = string.Empty;

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<PlantTemplate> PlantTemplates { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<PlantProperty> Properties { get; set; }

        public FamilyTemplate()
        {
            PlantTemplates = new();
            Properties = new();
        }
    }
}
