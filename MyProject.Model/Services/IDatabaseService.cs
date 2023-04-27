using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProject.Model.Database;
using MyProject.Model.DTOs;

namespace MyProject.Model.Services
{
    public interface IDatabaseService
    {
        DatabaseContext GetContext();

        #region Npu
        Npu GetNpu(int id);
        List<Npu> GetAllNpu();
        List<Npu> GetAllNpuBasedOnElement(string elementName);
        Npu AddNpu(Npu npu, int userId);
        bool DeleteNpu(int id);
        #endregion

        #region User
        User GetUser(int id);
        User GetUserByUserName(string userName);
        List<User> GetAllUser();
        List<Npu> GetUserNpus(int id);
        #endregion

        #region Score
        bool ScoreNpu(ScoreDTO dto);
        #endregion
    }
}
