using SQLite;
using SQLiteNetExtensions.Attributes;

namespace JollyCactus.Maui.Model
{
    [Table("Settings")]
    class Settings
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }               

        [Column("ColdSeasonStart")]
        public string ColdSeasonStart { get; set; } = string.Empty;

        [Column("ColdSeasonEnd")]
        public string ColdSeasonEnd { get; set; } = string.Empty;
    }
}
