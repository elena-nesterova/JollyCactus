using SQLite;
using SQLiteNetExtensions.Attributes;

namespace JollyCactus.Maui.Model
{
    [Table("Plants")]
    public class Plant
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        [ForeignKey(typeof(Location))]
        public int LocationId { get; set; }
        
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<PlantProperty> Properties { get; set; }

        public Plant()
        {
            Properties = new List<PlantProperty>();            
        }
    }
}
