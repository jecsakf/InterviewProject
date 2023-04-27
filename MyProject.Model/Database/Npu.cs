using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Model.Database
{
    public class Npu : DbBase
    {
        public Npu()
        {
            Scores = new List<Score>();
        }
        public int OwnerId { get; set; }
        public User Owner { get; set; }
        public Byte[] Picture { get; set; }
        public string Description { get; set; }
        public string ElementName { get; set; }
        public ICollection<Score> Scores { get; set; }
    }
}
