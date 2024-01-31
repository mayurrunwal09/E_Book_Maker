using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Data.DBModel
{
    public class BaseEntityClass
    {
        public int Id { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Created { get; set; }

        [StringLength(100)]
        public string? CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? Updated { get; set; }

        [StringLength(100)]
        public string? UpdatedBy { get; set; }
    }
}
