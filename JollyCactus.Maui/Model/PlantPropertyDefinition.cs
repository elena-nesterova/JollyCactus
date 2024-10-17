using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace JollyCactus.Maui.Model
{
    public enum PlantPropertyType
    {
        PlantPropertyString,
        PlantPropertyDate,
        PlantPropertyPicture,
        PlantPropertyPictureSet,
        PlantPropertyFlags,
        PlantPropertyStringFromList,
        PlantPropertyComposite
    }


    [Table("PlantPropertyGroups")]
    public class PlantPropertyGroup
    {
        [PrimaryKey]
        [Column("Number")]
        public int GroupNumber { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [OneToMany]
        public List<PlantPropertyDefinition> Properties { get; set; }

        public PlantPropertyGroup()
        {
            Properties = new List<PlantPropertyDefinition>();
        }
    }

    [Table("PlantProperties")]
    public class PlantPropertyDefinition
    {
        [PrimaryKey]
        [Column("Name")]
        public string Name { get; set; }

        [Column("SubName")]
        public string SubName { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [Column("CanBeEmpty")]
        public bool CanBeEmpty { get; set; } // TODO:

        public string DefaultValue { get; set; } = string.Empty;

        [Column("Type")]
        public PlantPropertyType Type { get; set; }

        [OneToMany]
        public List<PlantPropertyDefinition> SubProperties { get; set; }
    }
}
