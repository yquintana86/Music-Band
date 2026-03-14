using Domain.Entities;
using FluentAssertions;
using Infrastructure.Persistence.DataContext;
using Infrastructure.Persistence.Repositories;
using IntegrationTests.Fixture;

namespace IntegrationTests.Repositories
{
    public class MusicianRepositoryTests
    : IClassFixture<DatabaseFixture>
    {
        private readonly AppDbContext _context;

        public MusicianRepositoryTests(DatabaseFixture fixture)
        {
            _context = fixture.DbContext;
        }

        [Fact]
        public async Task Should_Save_Musician()
        {
            var repo = new MusicianRepository(_context);

            repo.Add(new Musician
            {
                FirstName = "Test",
                LastName = "Test",
                MiddleName = "Test",
                Age = 35,
                BasicSalary = 100,
                Experience = 20
            });
            await _context.SaveChangesAsync();

            (await repo.GetByIdAsync(1)).Should().NotBeNull();
        }
    }
}
