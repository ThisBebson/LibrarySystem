using System.Collections.Generic;

public interface IClientService
{
    void AddClient(Client client);
    List<Client> GetClients();
    List<Client> SearchClients(string keyword);
    string GenerateNextClientId();

}