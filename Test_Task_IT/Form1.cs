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
            SetupCheckboxes();
            
        }

        private void SetupCheckboxes()
        {
            var groupColumnsAllowed = ConfigurationManager.AppSettings.Get("columns_groupby");
            var sumColumnsAllowed = ConfigurationManager.AppSettings.Get("columns_sum_by");
            string[] group_names = groupColumnsAllowed.Split(',');
            string[] sum_names = sumColumnsAllowed.Split(',');
            int location_Y = 0;
            foreach(var name in group_names)
            {
                this.CheckBoxesPanel.Controls.Add(new CheckBox() { Text = name, Location = new Point(0,location_Y) });
                location_Y += 30;
            }
            location_Y = 0;
            foreach (var name in sum_names)
            {
                this.SumByCheckBoxesPanel.Controls.Add(new CheckBox() { Text = name, Location = new Point(0, location_Y) });
                location_Y += 30;
            }
        }

        private void SetupLayout()
        {

            Table table = new Table();
            try
            {
                table = _db.SelectAllQuery(TableName);
            }
            catch(SqlException e)
            {
                MessageBox.Show("Invalid table name", "Error", MessageBoxButtons.OK);
                this.Dispose();

            }
            
            WriteLayout(table);

        }
        private void WriteLayout(Table table)
        {
            this.Table.Columns.Clear();
            this.Table.Rows.Clear();

            this.Table.ColumnHeadersDefaultCellStyle
                .BackColor = Color.Navy;
            this.Table.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            

            foreach (var column in table.Columns)
            {

                this.Table.Columns.Add(new DataGridViewTextBoxColumn() { Name = column.Name });

            }

            foreach (var row in table.Data)
            {
                this.Table.Rows.Add(row.ToArray());
            }
        }
        private void Reset_Button_Click(object sender, EventArgs e)
        {
            SetupLayout();
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            List<string> group_by = new List<string>();
            List<string> sum_by = new List<string>();
            foreach(var checkBox in CheckBoxesPanel.Controls)
            {
                var check = (CheckBox)checkBox;
                if (check.Checked)
                {
                    group_by.Add(check.Text);
                }
            }
            foreach (var checkBox in SumByCheckBoxesPanel.Controls)
            {
                var check = (CheckBox)checkBox;
                if (check.Checked)
                {
                    sum_by.Add(check.Text);
                }
            }
            if(group_by.Count == 0 && sum_by.Count == 0)
            {
                MessageBox.Show("You must choose one option!", "Error", MessageBoxButtons.OK);
                return;
            }

            // Query here 
            Table table = new Table();
            try
            {
                table = _db.SelectGrouped(group_by, sum_by, TableName);
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);

            }

            WriteLayout(table);

        }
    }
}