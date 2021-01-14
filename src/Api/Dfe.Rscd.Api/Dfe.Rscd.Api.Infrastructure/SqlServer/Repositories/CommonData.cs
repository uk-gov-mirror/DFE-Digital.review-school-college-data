using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;

namespace Dfe.Rscd.Api.Infrastructure.SqlServer.Repositories
{
    public class CommonData : ICommonData
    {
        private readonly IDbConnection _connection;

        public CommonData(IDbConnection connection)
        {
            _connection = connection;
        }

        public List<T> Get<T>(string table)
        {
            try
            {
                _connection.Open();
                var list = _connection.Query<T>($"SELECT * FROM {table}").ToList();
                _connection.Close();

                return list;
            }
            catch(Exception ex)
            {
                throw new Exception("Reference Data fetching failed", ex);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
            
        }

        public T GetById<T>(string id, string idColumn, string table)
        {
            try
            {
                _connection.Open();
                var item = _connection.QuerySingleOrDefault<T>($"SELECT * FROM {table} WHERE {idColumn} = {id}");

                if (item == null)
                    item = _connection.QuerySingleOrDefault<T>($"SELECT * FROM {table} WHERE {idColumn} = '{id}'");

                _connection.Close();

                return item;
            }
            catch (Exception ex)
            {
                throw new Exception("Reference Data fetching failed", ex);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
            
        }
    }
}