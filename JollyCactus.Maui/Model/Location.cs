using SQLite;
using SQLiteNetExtensions.Attributes;

namespace JollyCactus.Maui.Model
{
    [Table("Locations")]
    public class Location
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Name")]
        public string Name { get; set; } = string.Empty;

        [Column("Note")]
        public string Note { get; set; } = string.Empty;

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Plant> Plants { get; set; } = new List<Plant>();
    }
}
