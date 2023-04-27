using MyProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.APITest
{
    [Collection("Sequential")]
    public class UserControllerTest : IDisposable
    {
        private readonly List<UserDTO> _userDTOs;
        private readonly List<NpuDTO> _npuDTOs;
        private UserController _controller;
        private readonly DatabaseContext _context;

        public UserControllerTest()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;
            _context = new DatabaseContext(options);
            _context.Database.EnsureCreated();

            var userData = new List<User>
            {
                new User { Id = 1, Name = "TESTUSER" },
                new User { Id = 2, Name = "TESTUSER2" },
                new User { Id = 3, Name = "TESTUSER3" },
            };
            
            var user1 = new List<User>();
            user1.Add(userData[0]);

            var user2 = new List<User>();
            user2.Add(userData[1]);

            var user3 = new List<User>();
            user3.Add(userData[2]);

            var npuData = new List<Npu>
            {
                new Npu { OwnerId = 1, Description = "TESTDESCRIPTION1", ElementName = "TESTELEMENTNAME1" },
                new Npu { OwnerId = 2, Description = "TESTDESCRIPTION2", ElementName = "TESTELEMENTNAME2" },
                new Npu { OwnerId = 2, Description = "TESTDESCRIPTION3", ElementName = "TESTELEMENTNAME3" },
            };

            _context.Users.AddRange(userData);
            _context.Npus.AddRange(npuData);
            _context.SaveChanges();

            _userDTOs = userData.Select(user => new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
            }).ToList();

            _npuDTOs = npuData.Select(npu => new NpuDTO
            {
                Id = npu.Id,
                Description = npu.Description,
                ElementName = npu.ElementName,
            }).ToList();

            var userManager = new UserManager<User>(
                new UserStore<User, IdentityRole<int>, DatabaseContext, int>(_context), null,
                new PasswordHasher<User>(), null, null, null, null, null, null);

            var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, "testName"),
                new Claim(ClaimTypes.NameIdentifier, "12"),
            });

            _controller = new UserController(userManager,null, new DatabaseService(_context));

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "username")
                    }, "someAuthTypeName"))
                }
            };
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public void GetUsersTest()
        {
            var result = _controller.GetUsers();

            // Assert
            var objectResult = Assert.IsType<ActionResult<IEnumerable<UserDTO>>>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<UserDTO>>(objectResult.Value);
            Assert.Equal(_userDTOs, model);
        }

        [Fact]
        public void GetUserTest()
        {
            var result = _controller.GetUserById(1);

            // Assert
            var objectResult = Assert.IsType<ActionResult<UserDTO>>(result);
            var model = Assert.IsAssignableFrom<UserDTO>(objectResult.Value);
            Assert.Equal(_userDTOs[0], model);
        }

        [Fact]
        public void GetUserUploadedNpusTest()
        {
            var result = _controller.GetUserUploadedNpus(1);
            var npuResult = new List<NpuDTO>();
            npuResult.Add(_npuDTOs[0]);


            // Assert
            var objectResult = Assert.IsType<ActionResult<IEnumerable<NpuDTO>>>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<NpuDTO>>(objectResult.Value);
            Assert.Equal(npuResult, model);
        }
    }
}
