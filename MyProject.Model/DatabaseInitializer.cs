using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using MyProject.Model.Database;

namespace MyProject.Model
{
	public class DatabaseInitializer
	{
		private static DatabaseContext _context;
		private static UserManager<User> _userManager;

		public static void Initialize(IServiceProvider serviceProvider)
		{
			_context = serviceProvider.GetRequiredService<DatabaseContext>();
			_userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            List<User> users = new List<User>()
            {
                new User
                {
                    Name = "John Doe",
                    UserName = "john.doe",
                },
                new User
                {
                    Name = "Jane Doe",
                    UserName = "jane.doe",
                },
                new User
                {
                    Name = "Sam Smith",
                    UserName = "sam.smith",
                },
            };

            string password = "John1234";
            string password2 = "Jane1234";
            string password3 = "Sam1234";

            _ = _userManager.CreateAsync(users[0]).Result;
            _ = _userManager.CreateAsync(users[1]).Result;
            _ = _userManager.CreateAsync(users[2]).Result;

            _ = _userManager.AddPasswordAsync(users[0], password).Result;
            _ = _userManager.AddPasswordAsync(users[1], password2).Result;
            _ = _userManager.AddPasswordAsync(users[2], password3).Result;

            List<Npu> npus = new List<Npu>()
			{
				new Npu
				{
                    Owner = users[0],
                    Picture = new byte[1],
					Description = "The minifigure has a double-barreled gun which body is a small tyre.",
					ElementName = "small tyre"
                },
                new Npu
                {
                    Owner = users[1],
                    Picture = new byte[1],
                    Description = "It is a space shuttle that main part is a Technic remote controller.",
                    ElementName = "remote controller"
                },
                new Npu
                {
                    Owner = users[0],
                    Picture = new byte[1],
                    Description = "The frog element acts as a leaf on the bonsai tree.",
                    ElementName = "frog",
                }
            };

            List<Score> scores = new List<Score>()
            {
                new Score
                {
                    ScoredNpu = npus[0],
                    Creativity = 5,
                    Uniqueness = 5,
                },
                new Score
                {
                    ScoredNpu = npus[0],
                    Creativity = 5,
                    Uniqueness = 4,
                },
                new Score
                {
                    ScoredNpu = npus[1],
                    Creativity = 4,
                    Uniqueness = 5,
                },
                new Score
                {
                    ScoredNpu = npus[1],
                    Creativity = 5,
                    Uniqueness = 3,
                },
                new Score
                {
                    ScoredNpu = npus[2],
                    Creativity = 5,
                    Uniqueness = 5,
                }
            };

            _context.Npus.AddRange(npus);
            _context.Scores.AddRange(scores);

            _context.SaveChanges();
		}
	}
}
