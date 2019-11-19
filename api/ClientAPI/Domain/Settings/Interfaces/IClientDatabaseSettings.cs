using System;

namespace ClientAPI.Domain.Settings.Interfaces
{
    public interface IClientDatabaseSettings
    {
        string ClientCollectionName { get; set; }
        string ConnectionString {get;set;}
        string DatabaseName {get;set;}
    }
}