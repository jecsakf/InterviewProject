using MyProject.Model.Database;
using System;

namespace MyProject.Model.DTOs
{
    public class ScoreDTO : BaseDTO
    {
        public int ScoredNpuId { get; set; }
        public int Creativity { get; set; }
        public int Uniqueness { get; set; }

        public override Boolean Equals(Object obj)
        {
            return (obj is ScoreDTO dto) &&
                    ScoredNpuId == dto.ScoredNpuId;
        }

        public static explicit operator ScoreDTO(Score x) => new ScoreDTO
        {
            Id = x.Id,
            ScoredNpuId = x.ScoredNpuId,
            Creativity = x.Creativity,
            Uniqueness = x.Uniqueness
        };

        public static explicit operator Score(ScoreDTO dto) => new Score
        {
            Id = dto.Id,
            ScoredNpuId = dto.ScoredNpuId,
            Creativity = dto.Creativity,
            Uniqueness = dto.Uniqueness
        };
    }
}
