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

        //[Column("Name")]
        //public string Name { get; set; }

        //[Column("BotanicalName")]
        //public string BotanicalName { get; set; }

        //[Column("Family")]
        //public string Family { get; set; }

        //public int Amount { get; set; }

        //[Column("AdoptionDate")]
        //public DateTime AdoptionDate { get; set; }

        //[Column("Note")]
        //public string Note { get; set; }

        //[Column("PicturePath")]
        //public string PicturePath { get; set; }

        //[OneToMany]
        //public List<string> Gallery {  get; set; }

        

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<PlantProperty> Properties { get; set; }

        public Plant()
        {
            Properties = new List<PlantProperty>();
            //Gallery = new List<string>();
            //Properties = new List<PlantProperty>();
            //AdoptionDate = DateTime.Now;
            //Name = "Plant";
        }
    }
}
