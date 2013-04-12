﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AmbleClient.custVendor.customerVendorMgr;

namespace AmbleClient.custVendor
{
    public partial class customerVendorMainFrame : Form
    {
        int customerOrVendor;
        DataTable userTable;
        DataTable orginateCustomerVendorTable;
        DataTable showTable;
        CustomerVendorMgr customerVendorMgr;
        private int selectedRow;
        
        
        public customerVendorMainFrame(int customerOrVendor)
        {
            InitializeComponent();
            this.customerOrVendor = customerOrVendor;//0:customer, 1:vendor
            if (customerOrVendor == 0)
            {
                this.Text = "View Customers";
                this.toolStripComboBox1.Items.Add("Include Subordinates' Customers");
                this.toolStripComboBox1.Items.Add("Only List My Customers");
            }  
            else
            {
                this.Text = "View Vendors";
                this.toolStripComboBox1.Items.Add("Include Subordinates' Vendors");
                this.toolStripComboBox1.Items.Add("Only List My Vendors");
            }

            customerVendorMgr = new CustomerVendorMgr();

        }

        private void customerVendorMainFrame_Load(object sender, EventArgs e)
        {
            userTable = new AmbleClient.Admin.AccountMgr.AccountMgr().ReturnWholeAccountTable();
                    
           
            showTable = new DataTable();
            showTable.Columns.AddRange(
                new DataColumn[]{
                    new DataColumn("Company Name",typeof(string)),
                    new DataColumn("Country",typeof(string)),
                    new DataColumn("Company Number",typeof(string)),
                    new DataColumn("Rate",typeof(int)),
                    new DataColumn("Term",typeof(string)),
                    new DataColumn("Contact 1",typeof(string)),
                    new DataColumn("Contact 2",typeof(string)),
                    new DataColumn("Phone 1",typeof(string)),
                    new DataColumn("Phone 2",typeof(string)),
                    new DataColumn("Cell Phone",typeof(string)),
                    new DataColumn("Fax",typeof(string)), 
                    new DataColumn("Email 1",typeof(string)),
                    new DataColumn("Email 2",typeof(string)),
                    new DataColumn("Owner Id",typeof(int)),
                    new DataColumn("Owner Name",typeof(string)),
                    new DataColumn("Last Modify Name",typeof(string)),
                    new DataColumn("Last Update Date",typeof(DateTime)),
                    new DataColumn("BlackListed",typeof(string)),
                    new DataColumn("Amount",typeof(int)),
                    new DataColumn("Notes",typeof(string))
                });

            this.toolStripComboBox1.SelectedIndex = 0; //auto call FillTheDataGrid();

        }
        private void FillTheDataGrid()
        {

            this.showTable.Clear();

            if (toolStripComboBox1.SelectedIndex==0)
            {
                orginateCustomerVendorTable = customerVendorMgr.GetTheCustomersOrVendorsICanSee(customerOrVendor, UserInfo.UserId);
            }
            else if (toolStripComboBox1.SelectedIndex==1)
            {
                orginateCustomerVendorTable = customerVendorMgr.GetMyCustomerOrVendor(customerOrVendor, UserInfo.UserId);
            
            }

            var showRows = from cvrow in orginateCustomerVendorTable.AsEnumerable()
                           join userrow in userTable.AsEnumerable() on cvrow["ownerName"] equals userrow["id"] 
                           join userrow2 in userTable.AsEnumerable() on cvrow["lastUpdateName"] equals userrow2["id"]
                       select new
                       {
                           name = cvrow["cvname"],
                           country = cvrow["country"],
                           number=cvrow["cvnumber"],
                           rate=cvrow["rate"],
                           term=cvrow["term"],
                           contact1=cvrow["contact1"],
                           contact2=cvrow["contact2"],
                           phone1=cvrow["phone1"],
                           phone2=cvrow["phone2"],
                           cellphone=cvrow["cellphone"],
                           fax=cvrow["fax"],
                           email1=cvrow["email1"],
                           email2=cvrow["email2"],
                           ownerId=cvrow["ownerName"],
                           ownerName=userrow["accountName"],
                           lastUpdateName=userrow2.Field<string>("accountName"),
                           lastUpdateDate=cvrow.Field<DateTime>("lastUpdateDate"),
                           blacklisted =((DBNull.Value.Equals(cvrow["blacklisted"])||Convert.ToInt32(cvrow["blacklisted"])==0)? "No" : "Yes"),
                           amount=cvrow["amount"],
                           notes=cvrow["notes"]
                       };
            
           foreach(var row in showRows)
           {
               showTable.Rows.Add(row.name, row.country,row.number, row.rate,row.term, row.contact1, row.contact2, row.phone1, row.phone2, row.cellphone,
                  row.fax, row.email1, row.email2,row.ownerId,row.ownerName, row.lastUpdateName,row.lastUpdateDate, row.blacklisted, row.amount, row.notes);

           }

            //remove unnecessary columns in the list view
           var tableForDataGrid = new DataTable();
           tableForDataGrid.Columns.AddRange(
               new DataColumn[]{
                    new DataColumn("Company Name",typeof(string)),
                    new DataColumn("Country",typeof(string)),
                    new DataColumn("Company Number",typeof(string)),
                    new DataColumn("Rate",typeof(int)),
                    new DataColumn("Term",typeof(string)),
                    new DataColumn("Contact 1",typeof(string)),
                    new DataColumn("Phone 1",typeof(string)),
                    new DataColumn("Cell Phone",typeof(string)),
                    new DataColumn("Fax",typeof(string)), 
                    new DataColumn("Email 1",typeof(string)),
                    new DataColumn("Owner Name",typeof(string)),
                    new DataColumn("Last Modify Name",typeof(string)),
                    new DataColumn("Last Update Date",typeof(DateTime)),
                    new DataColumn("Amount",typeof(int)),
                });

           foreach (var row in showRows)
           {
               tableForDataGrid.Rows.Add(row.name, row.country, row.number, row.rate, row.term, row.contact1, row.phone1,row.cellphone,
                  row.fax, row.email1,row.ownerName, row.lastUpdateName, row.lastUpdateDate,row.amount);

           }


           dataGridView1.DataSource = tableForDataGrid;
            //列宽自适应
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }


        }


        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return; //this happens when double click the column head.
            }
          customerVendorOperation ModifyOpFrame = new ModifyCustomerVendor(GetDataRowInShowTableFromIndex(e.RowIndex),customerOrVendor);
          ModifyOpFrame.ShowDialog();
          FillTheDataGrid();
          RestoreSelectedRow();
        }

        private DataRow GetDataRowInShowTableFromIndex(int rowIndex)
        { 
         //get the primary key in datagridview
            string companyName = dataGridView1["Company Name", rowIndex].Value.ToString();
            string ownerName=dataGridView1["Owner Name",rowIndex].Value.ToString();

            var dataRows = showTable.AsEnumerable().Where<DataRow>(row => (row["Company Name"].ToString() == companyName) && (row["Owner Name"].ToString()==ownerName));
            if (dataRows.Count<DataRow>() == 1)//如果只有一行纪录，就可以确定,大部分情况如此。
            {
                return dataRows.First<DataRow>();
            }
            else
            {
                MessageBox.Show("error, the select count is not 1");
                return null;
            
            }

        }


        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillTheDataGrid();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            customerVendorOperation addOpframe = new AddCustomerVendor(customerOrVendor);
            addOpframe.ShowDialog();
            FillTheDataGrid();
            RestoreSelectedRow();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            int rowIndex = dataGridView1.CurrentRow.Index;

            customerVendorOperation modifyOpFrame = new ModifyCustomerVendor(GetDataRowInShowTableFromIndex(rowIndex), customerOrVendor);
            modifyOpFrame.ShowDialog();
            FillTheDataGrid();
            RestoreSelectedRow();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
        if(DialogResult.Yes==MessageBox.Show("Delete the selected item?","",MessageBoxButtons.YesNo,MessageBoxIcon.Warning))
        {
            DataRow dr = GetDataRowInShowTableFromIndex(this.dataGridView1.CurrentRow.Index);
            customerVendorMgr.DeleteCustomerOrVendor(customerOrVendor, dr["Company Name"].ToString(), Convert.ToInt32(dr["Owner Id"]));
            FillTheDataGrid();
            RestoreSelectedRow();
        }
        }

        private void RestoreSelectedRow()
        {
            if (dataGridView1.Rows.Count == 0)
                return;
            dataGridView1.Rows[0].Selected = false;

            if (selectedRow > dataGridView1.Rows.Count - 1)
            {
                selectedRow = dataGridView1.Rows.Count - 1;
            }
            dataGridView1.Rows[selectedRow].Selected = true;
        
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
        }

    }
}
