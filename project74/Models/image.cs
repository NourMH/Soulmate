namespace project74.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("image")]
    public partial class image
    {
        [Key]
        public int img_id { get; set; }

        [Required]
        [StringLength(225)]
        public string img_url { get; set; }

        public int? p_id { get; set; }

        public virtual package package { get; set; }
    }
}
