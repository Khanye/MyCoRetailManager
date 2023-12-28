
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;                                                                                                                             
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MRMDataManager.Library.Internal.DataAccess
{
    // Internal so that it cant be seen by anything outside the Library. Nothing outside thelibrary should be talking to the 
    //database but has to go through the SQLDataAccess class
    internal class SqlDataAccess : IDisposable
    {   // Method to get the connection string : Pass in the name of the connection and return a connection string
        public string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(storedProcedure, parameters,
                   commandType: CommandType.StoredProcedure).ToList();

                return rows;
            }
        }

        public void SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            using (IDbConnection cnn = new SqlConnection(connectionString))
            {
                cnn.Execute(storedProcedure, parameters,
                     commandType: CommandType.StoredProcedure);
            }
        }

        // TRANSACTION MAP
        // Open connect/start transaction method
        // Load using the transaction
        // Save using the transaction
        // Close connection/stop/end transaction method
        // Dispose

        private IDbConnection _connection;
        private IDbTransaction _transaction;

        // Open connect/start transaction method
        public void StartTransaction(string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            _connection = new SqlConnection(connectionString);
            _connection.Open(); 

            _transaction = _connection.BeginTransaction(); 

            isClosed = false;
        }

        // Save using the transaction
        public void SaveDataInTransaction<T>(string storedProcedure, T parameters)
        {
            _connection.Execute(storedProcedure, parameters,
                     commandType: CommandType.StoredProcedure, transaction: _transaction);
            
        }

        // Load using the transaction
        public List<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters)
        {
           List<T> rows = _connection.Query<T>(storedProcedure, parameters,
                   commandType: CommandType.StoredProcedure, transaction: _transaction).ToList();

           return rows;
            
        }

        private bool isClosed = false;

        // Close connection/stop/end transaction method
        public void CommitTransaction()
        {
            _transaction?.Commit();
            _connection?.Close();

            isClosed = true;
        }
        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _connection?.Close();

            isClosed = true;
        }

        // Dispose
        public void Dispose()
        {
            if (isClosed == false)
            {
                try
                {
                    CommitTransaction();
                }
                catch
                {
                    //TODO: Log this issue 
                }
            }

            _transaction  = null;
            _connection = null ;
        }
    }

}
 