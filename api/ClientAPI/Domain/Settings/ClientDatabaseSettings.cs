using System;

namespace ClientAPI.Domain.Settings
{
    public class ClientDatabaseSettings
    {
        public string ClientCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}