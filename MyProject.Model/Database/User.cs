using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace MyProject.Model.Database
{
    public class User : IdentityUser<int>
    {
        public User()
        {
            UploadedNpus = new HashSet<Npu>();
        }

        public String Name { get; set; }
        public ICollection<Npu> UploadedNpus { get; set; }
        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                User u = (User)obj;
                return (Name == u.Name) && (Id == u.Id);
            }
        }
    }
}
