using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using MyProject.Model.Database;
using MyProject.Model.DTOs;

namespace MyProject.Model.Services
{
    public class DatabaseService : IDatabaseService
    {
        private DatabaseContext _context;

        public DatabaseService(DatabaseContext dc)
        {
            _context = dc;
        }
        public DatabaseContext GetContext() => this._context;

        #region Npus

        public Npu GetNpu(int id) => _context.Npus.Include(x => x.Owner)
                                                  .Include(x => x.Scores)
                                                  .FirstOrDefault(s => s.Id == id);

        public List<Npu> GetAllNpu() => _context.Npus.Include(x => x.Owner)
                                                     .Include(x => x.Scores)
                                                     .ToList();

        public List<Npu> GetAllNpuBasedOnElement(string elementName) => _context.Npus.Include(x => x.Owner)
                                                                                     .Include(x => x.Scores)
                                                                                     .Where(x => x.ElementName.Contains(elementName))
                                                                                     .ToList();

        public Npu AddNpu(Npu npu, int userId)
        {
            try
            {
                _context.Npus.Add(npu);
                _context.Users.FirstOrDefault(x => x.Id == userId).UploadedNpus.Add(npu);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return null;
            }
            catch (DbUpdateException)
            {
                return null;
            }
            return npu;
        }

        public bool DeleteNpu(int id)
        {
            Npu item = _context.Npus.Find(id);

            try
            {
                _context.Npus.Remove(item);
                _context.Users.FirstOrDefault(x => x.UploadedNpus.Contains(item)).UploadedNpus.Remove(item);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            catch (DbUpdateException)
            {
                return false;
            }
            return true;
        }

        #endregion

        #region User

        public User GetUser(int id) => _context.Users.Include(x => x.UploadedNpus)
                                                     .FirstOrDefault(s => s.Id == id);

        public User GetUserByUserName(string userName) => _context.Users.Include(x => x.UploadedNpus)
                                                                        .FirstOrDefault(s => s.UserName.Equals(userName));

        public List<User> GetAllUser() => _context.Users.Include(x => x.UploadedNpus)
                                                        .ToList();

        public List<Npu> GetUserNpus(int id) => _context.Users.Include(x => x.UploadedNpus)
                                                              .ThenInclude(x => x.Scores)
                                                              .FirstOrDefault(x => x.Id == id).UploadedNpus.ToList();

        #endregion

        #region Score

        public bool ScoreNpu(ScoreDTO dto)
        {
            Npu scoredNpu = _context.Npus.Find(dto.ScoredNpuId);
            User owner = _context.Users.Include(x => x.UploadedNpus)
                                       .FirstOrDefault(x => x.UploadedNpus.Contains(scoredNpu));

            try
            {
                scoredNpu.Scores.Add((Score)dto);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            catch (DbUpdateException)
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
