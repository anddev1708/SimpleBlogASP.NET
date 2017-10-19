namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    public partial class Blog
    {
        [Key]
        public long ID { get; set; }

        public long? CategoryId { get; set; }

        [StringLength(250)]
        public string Subject { get; set; }

        [Column(TypeName = "ntext")]
        [AllowHtml]
        public string Body { get; set; }

        public DateTime? DatePosted { get; set; }

        public int? Type { get; set; }

        public int? OtherType { get; set; }

        public virtual Category Category { get; set; }
    }
}
