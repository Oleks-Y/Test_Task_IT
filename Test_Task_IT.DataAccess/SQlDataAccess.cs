using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Test_Task_IT.DataAccess.Models;

namespace Test_Task_IT.DataAccess
{
    public class SQlDataAccess
    {
        private readonly SqlConnection _connection;
        public static string ConnectionString { get; set; }

        public SQlDataAccess(string cs)
        { 
            _connection = new SqlConnection(cs);
            ConnectionString = cs;
        }
        public void ExecuteQuery(string query)
        {
            using (var cmd = new SqlCommand(query, _connection))
            {
                    _connection.Open();

                    var result = cmd.ExecuteScalar();
            }
        }

        public Table ExecuteQueryResult(string query)
        {
            using (var cmd = new SqlCommand(query, _connection))
            {
                _connection.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    int count = reader.FieldCount;
                    int numberRecord = 0;
                    var table = new Table(reader.VisibleFieldCount);
                    for (int k = 0; k < count; k++)
                    {
                        table.Columns.Add(new Column(reader.GetName(k)));
                    }

                    while (reader.Read())
                    {
                        int k = 0;
                        
                        foreach (var col in table.Columns)
                        {
                            table.Data[k].Add(reader[col.Name].ToString());
                            k++;
                        }
                    }

                    return table;

                }
            }
        }

        public void InsertQuery(Dictionary<string, string> data, string tableName)
        {
            string query = 
                $"INSERT INTO {tableName} VALUES('{String.Join("','",data.Values.ToArray())}')";

        }

        public Table SelectAllQuery(string tableName)
        {
            return ExecuteQueryResult($"SELECT * FROM {tableName}");
        }


    }
}