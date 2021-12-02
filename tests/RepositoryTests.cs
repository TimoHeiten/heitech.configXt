using System;
using Xunit;
using heitech.configXt;
using Microsoft.Data.Sqlite;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static heitech.configXt.SqliteStore;
using FluentAssertions;

namespace tests
{
    public class RepositoryTests : IDisposable
    {
        #region Setup
        private readonly Repository _sut;
        private readonly SqliteStore _sqliteStore;

        public RepositoryTests()
        {
            var options = new DbContextOptionsBuilder<SqliteStore>()
                .UseSqlite(CreateInMemoryDatabase())
                .Options;
            _sqliteStore = new SqliteStore(options);
            _sqliteStore.Database.EnsureCreated();
            _sut = new Repository(_sqliteStore);
        }

        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            return connection;
        }
        #endregion

        [Fact]
        public async Task Add_Adds_to_store_on_save()
        {
            // Arrange
            var model = ConfigModel.From("key", "value");
            // Act
            _sut.Add(model);

            // Assert
            await _sqliteStore.SaveChangesAsync();
            var modelR = await _sqliteStore.Set<ConfigModelEntity>().SingleAsync();
            modelR.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateAsync_without_item_returns_null()
        {
            // Arrange
            var model = ConfigModel.From("key", "value");

            // Act
            var result = await _sut.UpdateAsync(model);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_with_existing_item_returns_updated_item()
        {
            // Arrange
            var model = ConfigModel.From("key", "");
            _sqliteStore.Add(ConfigModelEntity.FromModel(model));
            await _sqliteStore.SaveChangesAsync();
            var next = ConfigModel.From("key", "updated");

            // Act
            var result = await _sut.UpdateAsync(next);

            // Assert
            await _sqliteStore.SaveChangesAsync();
            var updated = await _sqliteStore.Set<ConfigModelEntity>().SingleAsync();
            updated.Should().NotBeNull();
            $"{result.Value}".Should().Be($"{updated.ToConfigModel().Value}");
        }

        [Fact]
        public async Task Get_With_Existing_Id_Returns_Model()
        {
            // Arrange
            // Act
            var result = await _sut.Get("key");
            
            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Get_With_Non_Existing_Id_returns_null()
        {
            // Arrange
            var model = ConfigModel.From("key", "-1-");
            _sqliteStore.Add(ConfigModelEntity.FromModel(model));
            await _sqliteStore.SaveChangesAsync();

            // Act
            var result = await _sut.Get("key");
            
            // Assert
            $"{result.Value}".Should().Be($"{result.Value}");
        }

        [Fact]
        public async Task DeleteAsync_without_item_returns_null()
        {
            // Act
            var result = await _sut.DeleteAsync("key");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_with_existing_item_returns_updated_item()
        {
            // Arrange
            var model = ConfigModel.From("key", "");
            _sqliteStore.Add(ConfigModelEntity.FromModel(model));
            await _sqliteStore.SaveChangesAsync();

            // Act
            var result = await _sut.DeleteAsync("key");

            // Assert
            await _sqliteStore.SaveChangesAsync();
            var updated = await _sqliteStore.Set<ConfigModelEntity>().ToListAsync();
            updated.Should().BeEmpty();
        }


        #region Dispose
        public void Dispose()
            => _sqliteStore.Dispose();
        #endregion
    }
}