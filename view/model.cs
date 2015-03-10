using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Data.OleDb;
using System.IO;
using System.Windows;

namespace view
{
    class model
    {
        //connection
        OleDbConnection DBconnection = new OleDbConnection();
        //adapter
        OleDbDataAdapter DBAdapter;

        /// <summary>
        /// get the right database table 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public DataTable connectToDB(string table)
        {
            DataTable localDataTable = new DataTable();

            try
            {
                DBconnection.ConnectionString = ("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=projects4.accdb");

                DBconnection.Open();

                DBAdapter = new OleDbDataAdapter("select * from " + table, DBconnection);

                DBAdapter.Fill(localDataTable);

                DBconnection.Close();

                
            }
            catch
            {
                throw;
            }

            finally
            {
                DBconnection.Close();
            }
            return localDataTable;

        }
        /// <summary>
        /// add students to DB
        /// </summary>
        /// <param name="id"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="dep"></param>
        /// <param name="phone"></param>
        public void addStudToDB(string id, string firstName, string lastName, string email, string dep, string phone)
        {
            try
            {
                OleDbCommand comm = new OleDbCommand("INSERT INTO students (ID,First_Name,Last_Name,Email,Department,Phone_number) VALUES (" + "'" + id + "'" + ", " + "'" + firstName + "'" + ", " + "'" + lastName + "'" + ", " + "'" + email + "'" + ", " + "'" + dep + "'" + ", " + "'" + phone + "'" + ")", DBconnection);
                DBconnection.Open();
                comm.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                DBconnection.Close();
            }

        }

        /// <summary>
        /// get datatable of docs
        /// </summary>
        /// <returns></returns>
        public DataTable load_feedbacks()
        {
            DataTable localDataTable = new DataTable();

            try
            {
                DBconnection.ConnectionString = ("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=projects4.accdb");

                DBconnection.Open();

                DBAdapter = new OleDbDataAdapter("select * from Documents", DBconnection);

                DBAdapter.Fill(localDataTable);

                DBconnection.Close();


            }
            catch
            {
                throw;
            }

            finally
            {
                DBconnection.Close();
            }
            return localDataTable;
        }


        /// <summary>
        /// add feedback to the Documents table in the DB
        /// </summary>
        public void addDocToDB(string doc_id, string doc_name, string doc_desc, string proj_name, string feedback)
        {
            try
            {
                OleDbCommand comm = new OleDbCommand("INSERT INTO Documents (Document_ID,Document_Name,Document_Description,Project_Name,feedback) VALUES ('" + doc_id + "'" + ", " + "'" + doc_name + "'" + ", " + "'" + doc_desc + "'" + ", " + "'" + proj_name + "'" + ", " + "'" + feedback + "')", DBconnection);
                DBconnection.Open();
                comm.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                DBconnection.Close();
            }

        }
    }
}
