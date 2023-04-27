using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Model.Database
{
    public class Score : DbBase
    {
        public int ScoredNpuId { get; set; }
        public Npu ScoredNpu { get; set; }
        public int Creativity { get; set; }
        public int Uniqueness { get; set; }
    }
}
