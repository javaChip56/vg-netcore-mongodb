using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using MongoDB.Driver;

using ClientAPI.Domain.Models;
using ClientAPI.Domain.Settings.Interfaces;
using ClientAPI.Domain.Queries.Interfaces;

namespace ClientAPI.Domain.Queries
{
    public class ClientQueries : IClientQueries
    {
        private IMongoDatabase _mongo = null;
        public ClientQueries(IMongoDatabase db)
        {
            this._mongo = db;
        }
        public async Task<Client> GetClientAsync(int id)
        {
           return await _mongo.GetCollection<Client>("client")
                .Find(c => c.ClientId == id)
                .FirstOrDefaultAsync();
        }
        public async Task<Client> GetClientAsync(string clientNo)
        {
            return await _mongo.GetCollection<Client>("client")
                .Find(c => c.ClientNo == clientNo)
                .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            return await _mongo.GetCollection<Client>("client")
                .Find(client => true).ToListAsync();
        }
    }
}
