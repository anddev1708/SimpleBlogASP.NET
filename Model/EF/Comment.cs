namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Comment
    {
        public long ID { get; set; }

        public long? BlogId { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        [Column("Comment", TypeName = "ntext")]
        public string Content { get; set; }

        public DateTime? DatePosted { get; set; }

        public virtual Blog Blog { get; set; }
    }
}
