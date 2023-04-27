using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Model.Database
{
    public class DbBase
    {
        [Key]
        public virtual int Id { get; set; }
    }
}
