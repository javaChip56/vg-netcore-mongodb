using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using MongoDB.Bson;
using MongoDB.Driver;

using ClientAPI.Domain.Models;
using ClientAPI.Domain.Settings.Interfaces;
using ClientAPI.Domain.Queries.Interfaces;

namespace ClientAPI.Domain.Queries
{
    public class ClientQueries : IClientQueries
    {
        private IMongoDatabase _context = null;
        public ClientQueries(IMongoDatabase db)
        {
            this._context = db;
        }
        public async Task<Client> GetClientAsync(int id)
        {
           return await _context.GetCollection<Client>("client")
                .Find(c => c.ClientId == id)
                .FirstOrDefaultAsync();
        }
        public async Task<Client> GetClientAsync(string clientNo)
        {
            return await _context.GetCollection<Client>("client")
                .Find(c => c.ClientNo == clientNo)
                .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            return await _context.GetCollection<Client>("client")
                .Find(client => true).ToListAsync();
        }
        public async Task<bool> InsertClientAsync(Client client)
        {
            _context.GetCollection<Client>("client")
                .InsertOne(client);
        
            return await Task.FromResult(
                !string.IsNullOrEmpty(client.Id));
        }
        public async Task<bool> UpdateClientAsync(Client client)
        {
            var result = await _context.GetCollection<Client>("client")
                .ReplaceOneAsync(
                    Builders<Client>.Filter.Eq("ClientId", client.ClientId),
                    client);

            return result.IsAcknowledged && result.IsModifiedCountAvailable;
        }
        public async Task<bool> DeleteClientAsync(int clientId)
        {
            var result = await _context.GetCollection<Client>("client")
                .DeleteOneAsync(
                    Builders<Client>.Filter.Eq("clientId", clientId));

            return result.IsAcknowledged && result.DeletedCount >= 1;
        }

        public async Task<bool> DeleteClientByNoAsync(string clientNo)
        {
            var result = await _context.GetCollection<Client>("client")
                .DeleteOneAsync(
                    Builders<Client>.Filter.Eq("clientNo", clientNo));

            return result.IsAcknowledged && result.DeletedCount >= 1;
        }

    }
}
