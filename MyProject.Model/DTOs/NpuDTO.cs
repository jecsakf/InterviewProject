using System;
using System.Collections.Generic;
using System.Linq;
using MyProject.Model.Database;

namespace MyProject.Model.DTOs
{
    public class NpuDTO : BaseDTO
    {
        public int OwnerId { get; set; }
        public Byte[] Picture { get; set; }
        public string Description { get; set; }
        public string ElementName { get; set; }
        public List<ScoreDTO> Scores { get; set; }

        public override Boolean Equals(Object obj)
        {
            return (obj is NpuDTO dto) && Id == dto.Id;
        }

        private static List<ScoreDTO> ConvertScoreToScoreDTO(ICollection<Score> x) => new(x.ToList().Select(x => new ScoreDTO
        {
            Id = x.Id,
            ScoredNpuId = x.ScoredNpuId,
            Creativity = x.Creativity,
            Uniqueness = x.Uniqueness
        }));

        private static List<Score> ConvertScoreDTOToScore(ICollection<ScoreDTO> dtos) => new(dtos.ToList().Select(x => new Score
        {
            Id = x.Id,
            ScoredNpuId = x.ScoredNpuId,
            Creativity = x.Creativity,
            Uniqueness = x.Uniqueness
        }));

        public static explicit operator NpuDTO(Npu x) => new NpuDTO
        {
            OwnerId = x.OwnerId,
            Id = x.Id,
            Picture = x.Picture,
            Description = x.Description,
            ElementName = x.ElementName,
            Scores = x.Scores != null ? ConvertScoreToScoreDTO(x.Scores) : new List<ScoreDTO>(),
        };

        public static explicit operator Npu(NpuDTO dto) => new Npu
        {
            OwnerId = dto.OwnerId,
            Id = dto.Id,
            Picture = dto.Picture,
            Description = dto.Description,
            ElementName = dto.ElementName,
            Scores = dto.Scores != null ? ConvertScoreDTOToScore(dto.Scores) : new List<Score>(),
        };
    }
}
