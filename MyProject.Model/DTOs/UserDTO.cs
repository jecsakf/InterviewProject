using MyProject.Model.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Model.DTOs
{
    public class UserDTO : BaseDTO
    {
        public String Name { get; set; }
        public String UserName { get; set; }
        public List<NpuDTO> UploadedNpus { get; set; }

        private static List<NpuDTO> ConvertNpuToNpuDTO(ICollection<Npu> x) => new(x.ToList().Select(x => new NpuDTO
        {
            Id = x.Id,
            Picture = x.Picture,
            Description = x.Description,
            ElementName = x.ElementName
        }));

        private static List<Npu> ConvertNpuDTOToNpu(ICollection<NpuDTO> dtos) => new(dtos.ToList().Select(x => new Npu
        {
            Id = x.Id,
            Picture = x.Picture,
            Description = x.Description,
            ElementName = x.ElementName
        }));

        public static explicit operator UserDTO(User x) => new UserDTO
        {
            Id = x.Id,
            Name = x.Name,
            UserName = x.UserName,
            UploadedNpus = x.UploadedNpus != null ? ConvertNpuToNpuDTO(x.UploadedNpus) : new List<NpuDTO>(),
        };

        public static explicit operator User(UserDTO dto) => new User
        {
            Id = dto.Id,
            Name = dto.Name,
            UserName = dto.UserName,
            UploadedNpus = dto.UploadedNpus != null ? ConvertNpuDTOToNpu(dto.UploadedNpus) : new List<Npu>(),
        };

        public class LoginDTO
        {
            public String UserName { get; set; }
            public String Password { get; set; }
        }

        public override Boolean Equals(Object obj)
        {
            return (obj is UserDTO dto) &&
                   Id == dto.Id &&
                   Name == dto.Name;
        }
    }
}
