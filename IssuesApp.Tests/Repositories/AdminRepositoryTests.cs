using FluentAssertions;
using IssuesApp.Data;
using IssuesApp.Models;
using IssuesApp.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssuesApp.Tests.Repositories
{
    public class AdminRepositoryTests
    {
        private async Task<DataContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var databaseContext = new DataContext(options);
            databaseContext.Database.EnsureCreated();
            if(await databaseContext.Admins.CountAsync()<= 0)
            {
                for(int i = 0; i < 10; i++)
                {
                    databaseContext.Admins.Add(
                        new Admin()
                        {
                            FirstName = "Steve",
                            LastName = "Oketch",
                            Email = "oketch@gmail.com"
                        }
                    );
                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }

        [Fact]
        public async void AdminRepository_GetAdmin_ReturnsAdmin()
        {
            // Arrange
            var testAdminId = 1;
            var dbContext = await GetDatabaseContext();
            var adminRepository = new AdminRepository(dbContext);
            // Act
            var result = adminRepository.GetAdmin(testAdminId);
            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Admin>();
        }
    }
}
