using MyProject.Model;

namespace MyProject.APITest
{
    [Collection("Sequential")]
    public class NpuControllerTest : IDisposable
    {
        private readonly List<NpuDTO> _npuDTOs;
        private readonly List<ScoreDTO> _scoreDTOs;

        private NpuController _controller;
        private readonly DatabaseContext _context;

        
        public NpuControllerTest()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;
            _context = new DatabaseContext(options);
            _context.Database.EnsureCreated();

            var npuData = new List<Npu>
            {
                new Npu { OwnerId = 1, Description = "TESTDESCRIPTION1", ElementName = "TESTELEMENTNAME1" },
                new Npu { OwnerId = 2, Description = "TESTDESCRIPTION2", ElementName = "TESTELEMENTNAME2" },
                new Npu { OwnerId = 2, Description = "TESTDESCRIPTION3", ElementName = "TESTELEMENTNAME3" },
            };

            var scoreData = new List<Score>
            {
                new Score { ScoredNpuId = 1, Creativity = 5, Uniqueness = 5 },
                new Score { ScoredNpuId = 1, Creativity = 4, Uniqueness = 5 },
                new Score { ScoredNpuId = 2, Creativity = 4, Uniqueness = 3 }
            };

            var userData = new List<User>
            {
                new User { Id = 1, Name = "TESTUSER" },
                new User { Id = 2, Name = "TESTUSER2" },
            };

            _context.Npus.AddRange(npuData);
            _context.Users.AddRange(userData);
            _context.SaveChanges();

            _npuDTOs = npuData.Select(npu => new NpuDTO
            {
                Id = npu.Id,
                Description = npu.Description,
                ElementName = npu.ElementName,
            }).ToList();

            _scoreDTOs = scoreData.Select(score => new ScoreDTO
            {
                Id = score.Id,
                Creativity = score.Creativity,
                Uniqueness = score.Uniqueness
            }).ToList();

            _controller = new NpuController(new DatabaseService(_context));
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public void GetAllNpuTest()
        {
            var result = _controller.GetAllNpu();

            // Assert
            var objectResult = Assert.IsType < ActionResult<IEnumerable<NpuDTO>>>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<NpuDTO>>(objectResult.Value);
            Assert.Equal(_npuDTOs, model);
        }

        [Fact]
        public void GetNpuByIdTest()
        {
            var result = _controller.GetNpuById(1);

            // Assert
            var objectResult = Assert.IsType<ActionResult<NpuDTO>>(result);
            var model = Assert.IsAssignableFrom<NpuDTO>(objectResult.Value);
            Assert.Equal(_npuDTOs[0], model);
        }

        [Fact]
        public void PostNpuTest()
        {
            var newNpu = new NpuDTO
            {
                Id = 4,
                OwnerId = 2,
                Description = "TESTDESCRIPTION4",
                ElementName = "TESTELEMENTNAME4"
            };

            var result = _controller.PostNpu(2, newNpu);

            // Assert
            var objectResult = Assert.IsType<CreatedAtActionResult>(result);
            var model = Assert.IsAssignableFrom<NpuDTO>(objectResult.Value);
            Assert.Equal(_npuDTOs.Count + 1, _context.Npus.Count());
            Assert.Equal(newNpu, model);
        }
    }
}