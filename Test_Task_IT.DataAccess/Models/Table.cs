using System.Collections.Generic;
using System.Threading;

namespace Test_Task_IT.DataAccess.Models
{
    public class Table
    {
        public string Name { get; set; }
        public List<Column> Columns { get; set; }
        public List<List<string>> Data { get; set; }

        public Table()
        {
            Columns = new List<Column>();
            Data = new List<List<string>>();
        }
        public Table(int columnsCount)
        {
            Columns = new List<Column>();
            Data = new List<List<string>>();
            for (int i = 0; i < columnsCount; i++)
            {
                Data.Add( new List<string>());
            }
        }
    }
}