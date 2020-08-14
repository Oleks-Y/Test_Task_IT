namespace Test_Task_IT.DataAccess.Models
{
    public class Column
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public bool IsNull { get; set; }
        public bool IsPk { get; set; }

        public Column(string name)
        {
            Name = name;
        }
    }
}