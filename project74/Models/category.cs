namespace project74.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("category")]
    public partial class category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public category()
        {
            user_cat = new HashSet<user_cat>();
        }

        [Key]
        public int cat_id { get; set; }

        [Required]
        [StringLength(50)]
        public string cat_name { get; set; }

        [StringLength(225)]
        public string image { get; set; }

        [StringLength(225)]
        public string description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<user_cat> user_cat { get; set; }
    }
}
