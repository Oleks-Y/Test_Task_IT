using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Test_Task_IT.DataAccess;
using System.Configuration;
using System.Data.SqlClient;
using Test_Task_IT.DataAccess.Models;

namespace Test_Task_IT
{
    public partial class Form1 : Form
    {
        private readonly SQlDataAccess _db;
        private string TableName;
        public Form1()
        {
            InitializeComponent();
            // Database setup
            string connectionString = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            TableName = ConfigurationManager.AppSettings.Get("table_name");
            try
            {
                _db = new SQlDataAccess(connectionString);
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK);
            }
            
            
            // Setup
            SetupLayout();
            
        }

        private void SetupLayout()
        {
            
            this.Table.ColumnHeadersDefaultCellStyle
                .BackColor = Color.Navy;
            this.Table.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            
            var table = _db.SelectAllQuery(TableName);

            foreach (var column in table.Columns)
            {
                this.Table.Columns.Add(new DataGridViewTextBoxColumn() {Name = column.Name});
            }

            foreach (var row in table.Data)
            {
                this.Table.Rows.Add(row.ToArray());
            }

        }
        
        

       
    }
}