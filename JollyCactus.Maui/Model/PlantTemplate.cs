using SQLite;
using SQLiteNetExtensions.Attributes;

namespace JollyCactus.Maui.Model
{
    [Table("PlantTemplates")]
    public class PlantTemplate
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        [Unique]
        [Column("Name")]
        public string Name { get; set; } = string.Empty;

        [ForeignKey(typeof(FamilyTemplate))]
        public int FamilyId { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<PlantProperty> Properties { get; set; }

        public PlantTemplate()
        {
            Properties = new();
        }
    }
}