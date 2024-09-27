using Application.Models.Requests;
using Application.Models;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Interfaces;

namespace Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public List<ClientDto?> GetAllClients()
        {
            var clientsList = _clientRepository.GetAll();

            return ClientDto.CreateList(clientsList);
        }

        public ClientDto? GetClientById(int id)
        {
            var client = _clientRepository.GetById(id);

            if (client == null)
                throw new NotFoundException(nameof(Client), id);

            return ClientDto.Create(client);
        }

        public ClientDto CreateNewClient(ClientCreateRequest clientCreateRequest)
        {
            var newClient = new Client();
            newClient.Name = clientCreateRequest.Name;
            newClient.Email = clientCreateRequest.Email;
            newClient.Password = clientCreateRequest.Password;
            newClient.Type = UserType.Client;

            return ClientDto.Create(_clientRepository.Add(newClient));
        }

        public void ModifyClientData(int id, ClientUpdateRequest clientUpdateRequest)
        {
            var client = _clientRepository.GetById(id);

            if (client == null)
                throw new NotFoundException(nameof(Client), id);

            if (clientUpdateRequest.Name != string.Empty) client.Name = clientUpdateRequest.Name;

            if (clientUpdateRequest.Password != string.Empty) client.Password = clientUpdateRequest.Password;

            _clientRepository.Update(client);
        }

        public void DeleteClient(int id)
        {
            var client = _clientRepository.GetById(id);

            if (client == null)
                throw new NotFoundException(nameof(Client), id);

            _clientRepository.Delete(client);
        }
    }
}
