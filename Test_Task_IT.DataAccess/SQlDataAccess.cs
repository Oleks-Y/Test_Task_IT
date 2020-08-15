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

                     _connection.Close();
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
                        string[] row = new string[reader.VisibleFieldCount];
                        foreach (var col in table.Columns)
                        {
                            var item = reader[col.Name].ToString();
                            row[k] = item;
                            k++;
                        }
                        table.Data.Add(row);
                    }

                    _connection.Close();
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

        public Table SelectGrouped(List<string> groupBy, List<string> sumBy, string tableName)
        {
            // TODO 
            // Check if arrays null
            // in Different cases will be diffent queries
            if(sumBy.Count == 0)
            {
                return ExecuteQueryResult($@"SELECT {string.Join(",", groupBy)}
                                        FROM {tableName}    
                                        GROUP BY {string.Join(",", groupBy)};
                                        ");
            }
            var sumByFormated = sumBy.Select(s => $"SUM({s}) AS {s}");

            if (groupBy.Count == 0)
            {
                return ExecuteQueryResult($@"SELECT  {string.Join(",", sumByFormated)}
                                            FROM {tableName};
                                            ");
            }
            
            return ExecuteQueryResult($@"SELECT {string.Join(",", groupBy)}, {string.Join(",", sumByFormated)}
                                        FROM {tableName}    
                                        GROUP BY {string.Join(",", groupBy)};
                                        ");
        } 

    }
}