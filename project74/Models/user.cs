namespace project74.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class user
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public user()
        {
            user_cat = new HashSet<user_cat>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string username { get; set; }

        [Required]
        [StringLength(50)]
        public string fname { get; set; }

        [Required]
        [StringLength(50)]
        public string lname { get; set; }
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public int mobile { get; set; }

        [StringLength(225)]
        public string image_id { get; set; }

        public int? admin { get; set; }

        [Required]
        [StringLength(50)]
        public string password { get; set; }
        [NotMapped]
        [Compare("password", ErrorMessage = "password not match")]
        public string confrim_pass { set; get; }

        [StringLength(225)]
        public string loc { get; set; }

        [StringLength(225)]
        public string Adress { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<user_cat> user_cat { get; set; }
    }
}
