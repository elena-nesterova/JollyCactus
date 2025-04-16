using SQLite;
using SQLiteNetExtensions.Attributes;

namespace JollyCactus.Maui.Model
{
    [Table("PlantProperties")]
    public class PlantProperty
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        [ForeignKey(typeof(Plant))]
        public int PlantId { get; set; }

        [Column("PropertyName")]
        public string PropertyName { get; set; }

        [Column("SubName")]
        public string SubName { get; set; }

        public PlantPropertyType PropertyType { get; set; }

        public string DBValue { get; set; }

        
    }
}
