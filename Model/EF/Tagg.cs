namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Tagg")]
    public partial class Tagg
    {
        [StringLength(50)]
        public string ID { get; set; }

        [StringLength(250)]
        public string Name { get; set; }
    }
}
