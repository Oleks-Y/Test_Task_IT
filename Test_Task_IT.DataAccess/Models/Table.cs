using System.Collections.Generic;
using System.Threading;

namespace Test_Task_IT.DataAccess.Models
{
    public class Table
    {
        public string Name { get; set; }
        public List<Column> Columns { get; set; }
        public List<string[]> Data { get; set; }

        public Table()
        {
            Columns = new List<Column>();
            Data = new List<string[]>();
        }
        public Table(int columnsCount)
        {
            Columns = new List<Column>();
            Data = new List<string[]>();
           
        }
    }
}