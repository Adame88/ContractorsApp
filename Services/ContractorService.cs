using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ContractorsApp.Data;
using ContractorsApp.Models;

namespace ContractorsApp.Services
{
    internal interface IContractorService
    {
        Task AddOrUpdateContractorAsync(Contractor contractor);
        Task DeleteAddressAsync(int addressId);
        Task DeleteContractorAsync(int contractorId);
        Task<Contractor?> FetchContractorByIdAsync(int contractorId);
        Task<List<Contractor>> FetchContractorsAsync(string name, string taxNumber);
    }

    internal class ContractorService : IContractorService
    {
        private readonly IContractorRepository _repository;

        public ContractorService(IContractorRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Contractor>> FetchContractorsAsync(string name, string taxNumber)
        {
            try
            {
                return await _repository.FetchContractorsByNameOrTaxNumberAsync(name, taxNumber);
            }
            catch (Exception ex)
            {
                // TODO
                // Adding Log the exception
                // Log the exception
                Console.WriteLine($"Error in FetchContractorsAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<Contractor?> FetchContractorByIdAsync(int contractorId)
        {
            try
            {
                return await _repository.FetchContractorByIdAsync(contractorId);
            }
            catch (Exception ex)
            {
                // TODO
                // Adding Log the exception
                // Log the exception
                Console.WriteLine($"Error in FetchContractorByIdAsync: {ex.Message}");
                throw;
            }
            
        }

        public async Task AddOrUpdateContractorAsync(Contractor contractor)
        {
            try
            {
                await DatabaseHelper.ExecuteWithTransactionAsync(async transaction =>
                {
                    if (contractor.ContractorId == 0)
                    {
                        await _repository.AddContractorAsync(contractor);
                    }
                    else
                    {
                        await _repository.ModifyContractorAsync(contractor);
                    }
                    return Task.CompletedTask;
                });
            }
            catch (Exception ex)
            {
                // TODO
                // Adding Log the exception
                // Log the exception
                Console.WriteLine($"Error in AddOrUpdateContractorAsync: {ex.Message}");
                throw;
            }

        }

        public async Task DeleteContractorAsync(int contractorId)
        {
            try
            {
                await DatabaseHelper.ExecuteWithTransactionAsync(async transaction =>
                {
                    await _repository.RemoveContractorByIdAsync(contractorId);
                    return Task.CompletedTask;
                });
            }
            catch (Exception ex)
            {
                // TODO
                // Adding Log the exception
                // Log the exception
                Console.WriteLine($"Error in DeleteContractorAsync: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteAddressAsync(int addressId)
        {
            try
            {
                await DatabaseHelper.ExecuteWithTransactionAsync(async transaction =>
                {
                    await _repository.RemoveAddressByIdAsync(addressId);
                    return Task.CompletedTask;
                });
            }
            catch (Exception ex)
            {
                // TODO
                // Adding Log the exception
                // Log the exception
                Console.WriteLine($"Error in DeleteAddressAsync: {ex.Message}");
                throw;
            }  
        }
    }
}
