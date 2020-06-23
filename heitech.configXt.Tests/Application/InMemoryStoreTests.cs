using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using heitech.configXt.Application;
using heitech.configXt.Core.Commands;
using heitech.configXt.Core.Entities;
using Xunit;

namespace heitech.configXt.Tests.Application
{
    public class InMemoryStoreTests : IDisposable
    {
        private const string FOUND = "found";
        private const string OTHER = "other";
        private const string NOT_FOUND = "not-found";
        private readonly Spy _store = new Spy("AdminName", "pwHash", "app-1");

        public InMemoryStoreTests()
        {
            _store.Entities.Add(FOUND, new ConfigEntity { Name = FOUND, Value = FOUND+"-1"});
            _store.Entities.Add(OTHER, new ConfigEntity { Name = OTHER, Value = OTHER+"-1"});
        }

        [Fact]
        public async Task AllEntitiesReturnsAllItems()
        {
            var result = await _store.AllEntitesAsync();
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task AllEntitiesEmptyReturnsEmpty()
        {
            _store.Entities.Clear();
            var result = await _store.AllEntitesAsync();
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public async Task ByNameNotFoundReturnsNull()
        {
            var result = await _store.GetEntityByNameAsync(NOT_FOUND);
            Assert.Null(result);
        }

        [Fact]
        public async Task ByNameFoundReturnsEntity()
        {
            var result = await _store.GetEntityByNameAsync(FOUND);

            Assert.NotNull(result);
            Assert.Equal(FOUND, result.Name);
            Assert.Equal(FOUND+"-1", result.Value);
        }

        [Fact]
        public async Task TryCreateNonExistingEntityReturnsTrueAndAddsToCount()
        {
            var entity = new ConfigEntity { Name = "Added", Value = "val"};
            entity.CrudOperationName = CommandTypes.Create;

            bool result = await _store.StoreEntityAsync(entity);

            Assert.True(result);
            Assert.Equal(3, _store.Entities.Count);
        }

        [Fact]
        public async Task TryCreateExistingEntityReturnsFalse() // needs to use operation updated instead
        {
            var entity = new ConfigEntity { Name = FOUND, Value = "val"};
            entity.CrudOperationName = CommandTypes.Create;

            bool result = await _store.StoreEntityAsync(entity);

            Assert.False(result);
            Assert.Equal(2, _store.Entities.Count);
        }

        [Fact]
        public async Task TryUpdateWithExistingReturnsTrue()
        {
            var entity = new ConfigEntity { Name = FOUND, Value = "updated"};
            entity.CrudOperationName = CommandTypes.UpdateValue;

            bool result = await _store.StoreEntityAsync(entity);

            Assert.True(result);
            Assert.Equal(2, _store.Entities.Count);

            var updated = _store.Entities[FOUND];
            Assert.Equal(updated.Value, "updated");
        }

        [Fact]
        public async Task TryUpdateNonExistingReturnsFalse()
        {
            var entity = new ConfigEntity { Name = NOT_FOUND, Value = "updated"};
            entity.CrudOperationName = CommandTypes.UpdateValue;

            bool result = await _store.StoreEntityAsync(entity);

            Assert.False(result);
            Assert.Equal(2, _store.Entities.Count);
        }

        [Fact]
        public async Task StoreDeleteRemoves()
        {
            var entity = new ConfigEntity { Name = FOUND, Value = null};
            entity.CrudOperationName = CommandTypes.Delete;

            bool result = await _store.StoreEntityAsync(entity);

            Assert.True(result);
            Assert.Equal(1, _store.Entities.Count);

            bool exists = _store.Entities.ContainsKey(FOUND);
            Assert.False(exists);
        }

        [Fact]
        public async Task DeleteWithNonExistentReturnsFalse()
        {
            var entity = new ConfigEntity { Name = NOT_FOUND, Value = null};
            entity.CrudOperationName = CommandTypes.Delete;

            bool result = await _store.StoreEntityAsync(entity);

            Assert.False(result);
            Assert.Equal(2, _store.Entities.Count);
        }

        public void Dispose()
        {
            _store.Entities.Clear();
        }

        public class Spy : InMemoryStore
        {
            public Spy(string name, string passwordHash, string appName) 
                : base(name, passwordHash, appName)
            { }

            public Dictionary<string, ConfigEntity> Entities => _entities;
        }
    }
}