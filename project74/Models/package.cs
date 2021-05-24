namespace project74.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class package
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public package()
        {
            images = new HashSet<image>();
        }

        [Key]
        public int p_id { get; set; }

        [StringLength(50)]
        public string p_name { get; set; }

        [StringLength(225)]
        public string p_desc { get; set; }

        public int? u_id { get; set; }

        public int? cat_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<image> images { get; set; }

        public virtual user_cat user_cat { get; set; }
    }
}
