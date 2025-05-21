using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using ContractorsApp.Models;

namespace ContractorsApp.Data
{
    internal interface IContractorRepository
    {
        Task AddContractorAsync(Contractor contractor);
        Task<Contractor?> FetchContractorByIdAsync(int contractorId);
        Task<List<Contractor>> FetchContractorsByNameOrTaxNumberAsync(string name, string taxNumber);
        Task ModifyContractorAsync(Contractor contractor);
        Task RemoveAddressByIdAsync(int addressId);
        Task RemoveContractorByIdAsync(int contractorId);
    }

    internal class ContractorRepository : IContractorRepository
    {
        #region GET
        //get all record + filtration
        public async Task<List<Contractor>> FetchContractorsByNameOrTaxNumberAsync(string name, string taxNumber)
        {
            var contractors = new Dictionary<int, Contractor>();

            try
            {
                using (var conn = DatabaseFactory.CreateConnection())
                {
                    await conn.OpenAsync();
                    var cmd = new SqlCommand("sp_GetContractorsByNameOrTaxNumber", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CompanyName", string.IsNullOrEmpty(name) ? (object)DBNull.Value : name);
                    cmd.Parameters.AddWithValue("@TaxNumber", string.IsNullOrEmpty(taxNumber) ? (object)DBNull.Value : taxNumber);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            int contractorId = (int)reader["ContractorId"];
                            if (!contractors.ContainsKey(contractorId))
                            {
                                contractors[contractorId] = new Contractor
                                {
                                    ContractorId = contractorId,
                                    CompanyName = reader["CompanyName"].ToString(),
                                    TaxNumber = reader["TaxNumber"].ToString(),
                                    REGON = reader["REGON"].ToString(),
                                    Addresses = new List<Address>()
                                };
                            }

                            if (reader["AddressID"] != DBNull.Value)
                            {
                                contractors[contractorId].Addresses.Add(new Address
                                {
                                    AddressID = (int)reader["AddressID"],
                                    Street = reader["Street"].ToString(),
                                    City = reader["City"].ToString(),
                                    PostalCode = reader["PostalCode"].ToString(),
                                    IsMainAddress = (bool)reader["IsMainAddress"]
                                });
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                // TODO
                // Adding Log the exception
                // Log the exception
                Console.WriteLine($"Error in FetchContractorsByNameOrTaxNumberAsync: {ex.Message}");
                throw;
            }
            

            return contractors.Values.ToList();
        }

        //get single record by id
        public async Task<Contractor?> FetchContractorByIdAsync(int contractorId)
        {
            Contractor? contractor = null;
            try
            {
                using (var conn = DatabaseFactory.CreateConnection())
                {
                    await conn.OpenAsync();
                    var cmd = new SqlCommand("sp_GetContractorById", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@ContractorId", contractorId);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            if (contractor == null)
                            {
                                contractor = new Contractor
                                {
                                    ContractorId = contractorId,
                                    CompanyName = reader["CompanyName"].ToString(),
                                    TaxNumber = reader["TaxNumber"].ToString(),
                                    REGON = reader["REGON"].ToString(),
                                    Addresses = new List<Address>()
                                };
                            }

                            if (reader["AddressID"] != DBNull.Value)
                            {
                                contractor.Addresses.Add(new Address
                                {
                                    AddressID = (int)reader["AddressID"],
                                    Street = reader["Street"].ToString(),
                                    City = reader["City"].ToString(),
                                    PostalCode = reader["PostalCode"].ToString(),
                                    IsMainAddress = (bool)reader["IsMainAddress"]
                                });
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                // TODO
                // Adding Log the exception
                // Log the exception
                Console.WriteLine($"Error in FetchContractorByIdAsync: {ex.Message}");
                throw;
            }
            return contractor;
        }

        #endregion

        #region POST
        //add new record 
        public async Task AddContractorAsync(Contractor contractor)
        {
            try
            {
                await DatabaseHelper.ExecuteWithTransactionAsync(async transaction =>
                {
                    var contractorId = await InsertContractorAsync(transaction.Connection, transaction, contractor);
                    await InsertAddressesAsync(transaction.Connection, transaction, contractorId, contractor.Addresses);
                    return contractorId;
                });
            }
            catch (Exception ex)
            {
                // TODO
                // Adding Log the exception
                // Log the exception
                Console.WriteLine($"Error in AddContractorAsync: {ex.Message}");
                throw;
            }
        }

        #region Private Methods
        private async Task<int> InsertContractorAsync(SqlConnection conn, SqlTransaction transaction, Contractor contractor)
        {
            using var cmd = new SqlCommand("sp_InsertContractor", conn, transaction)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@CompanyName", contractor.CompanyName);
            cmd.Parameters.AddWithValue("@TaxNumber", (object?)contractor.TaxNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@REGON", (object?)contractor.REGON ?? DBNull.Value);

            return (int)await cmd.ExecuteScalarAsync();
        }
        private async Task InsertAddressesAsync(SqlConnection conn, SqlTransaction transaction, int contractorId, List<Address> addresses)
        {
            foreach (var address in addresses)
            {
                var cmd = new SqlCommand("sp_InsertAddress", conn, transaction)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ContractorId", contractorId);
                cmd.Parameters.AddWithValue("@Street", address.Street ?? "");
                cmd.Parameters.AddWithValue("@City", address.City ?? "");
                cmd.Parameters.AddWithValue("@PostalCode", address.PostalCode ?? "");
                cmd.Parameters.AddWithValue("@IsMainAddress", address.IsMainAddress);

                await cmd.ExecuteNonQueryAsync();
            }
        }
        #endregion
        #endregion

        #region PUT
        //update existing record
        public async Task ModifyContractorAsync(Contractor contractor)
        {
            try
            {
                await DatabaseHelper.ExecuteWithTransactionAsync(async transaction =>
                {
                    await ModifyContractorDetailsAsync(transaction.Connection, transaction, contractor);
                    await RemoveAddressesByContractorIdAsync(transaction.Connection, transaction, contractor.ContractorId);
                    await InsertAddressesAsync(transaction.Connection, transaction, contractor.ContractorId, contractor.Addresses);
                    return Task.CompletedTask;
                });
            }
            catch (Exception ex)
            {
                // TODO
                // Adding Log the exception
                // Log the exception
                Console.WriteLine($"Error in ModifyContractorAsync: {ex.Message}");
                throw;
            }
        }

        #region Private Methods
        private async Task ModifyContractorDetailsAsync(SqlConnection conn, SqlTransaction transaction, Contractor contractor)
        {
            var cmd = new SqlCommand("sp_UpdateContractorDetails", conn, transaction)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@ContractorId", contractor.ContractorId);
            cmd.Parameters.AddWithValue("@CompanyName", contractor.CompanyName);
            cmd.Parameters.AddWithValue("@TaxNumber", (object?)contractor.TaxNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@REGON", (object?)contractor.REGON ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        #endregion
        #endregion

        #region DELETE
        //delete existing record Contractor + Addresses
        public async Task RemoveContractorByIdAsync(int contractorId)
        {
            try
            {
                using (var conn = DatabaseFactory.CreateConnection())
                {
                    await conn.OpenAsync();
                    var cmd = new SqlCommand("sp_DeleteContractorById", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@ContractorId", contractorId);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                // TODO
                // Adding Log the exception
                // Log the exception
                Console.WriteLine($"Error in RemoveContractorByIdAsync: {ex.Message}");
                throw;
            }
        }

        //delete address by id
        public async Task RemoveAddressByIdAsync(int addressId)
        {
            try
            {
                using (var conn = DatabaseFactory.CreateConnection())
                {
                    await conn.OpenAsync();
                    var cmd = new SqlCommand("sp_DeleteAddressById", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@AddressID", addressId);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                // TODO
                // Adding Log the exception
                // Log the exception
                Console.WriteLine($"Error in RemoveAddressByIdAsync: {ex.Message}");
                throw;
            }  
        }

        //delete all addresses by contractor id
        private static async Task RemoveAddressesByContractorIdAsync(SqlConnection conn, SqlTransaction transaction, int contractorId)
        {
            try
            {
                var cmd = new SqlCommand("sp_DeleteAddressesByContractorId", conn, transaction)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ContractorId", contractorId);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                // TODO
                // Adding Log the exception
                // Log the exception
                Console.WriteLine($"Error in RemoveAddressesByContractorIdAsync: {ex.Message}");
                throw;
            } 
        }
        #endregion
    }
}
