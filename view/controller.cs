using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace view
{
    class controller
    {
        //testing commit
        //init the model
        model mod = new model();
        //init a data table
        DataTable localDataTable = new DataTable();
        //user premissions
        public string userType = "";
        public string _doc_id = "";
        public string _doc_name = "";
        public string _doc_desc = "";

        public string user_id = "";
        public string user_name = "";

        /// <summary>
        /// get the info from the login phase
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pass"></param>
        public bool login_access(string username, string pass)
        {
           localDataTable =  mod.connectToDB("User_Info");

           // look for the user name
           foreach (DataRow row in localDataTable.Rows)
           {
              string id =  row[2].ToString();
              if (id == username)
              {
                  //check if the password matches 
                  if (row[3].ToString() == pass)
                  {
                      //set the premission
                      userType = row[4].ToString();
                      user_id = row[0].ToString();
                      user_name = row[1].ToString();
                      return true;
                  }

              }
           }
           return false;
        }

        /// <summary>
        /// add students to DB using the model
        /// </summary>
        /// <param name="id"></param>
        /// <param name="first"></param>
        /// <param name="last"></param>
        /// <param name="email"></param>
        /// <param name="dep"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public bool add_stud(string id,string first, string last, string email, string dep, string phone)
        {
            //use the model to get the students data table
            localDataTable = mod.connectToDB("students");

            //check if the student exists in the DB
            foreach (DataRow row in localDataTable.Rows)
            {
                string id_inTable = row[0].ToString();
                if (id == id_inTable)
                {
                   return false;
                }
            }
            //if it does not exists, add it using the model
            mod.addStudToDB(id, first, last, email, dep, phone);
            return true;
        }

        /// <summary>
        /// get projects using the model and return them to the view
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public List<string> get_projects(string id, string pass)
        {
            List<string> projects = new List<string>();
            localDataTable = mod.connectToDB("Project_Group");

            foreach (DataRow row in localDataTable.Rows)
            {
                for (int i = 0; i < localDataTable.Columns.Count; i++)
                {
                    if (row[i].ToString() == id)
                    {
                        if (!projects.Contains(row[0].ToString()))
                            projects.Add(row[0].ToString());
                    }
                }
            }
            return projects;

        }

        /// <summary>
        /// load feedbacks from specific proj
        /// </summary>
        /// <param name="proj_name"></param>
        /// <returns></returns>
        public List<string> load_docs(string proj_name)
        {
            List<string> docs = new List<string>();
            localDataTable = mod.load_feedbacks();

            foreach (DataRow row in localDataTable.Rows)
            {
                if (row[3].ToString() == proj_name)
                {
                    if (!docs.Contains(row[1].ToString()))
                    {
                        docs.Add(row[1].ToString());
                    }
                }
            }
            return docs;
        }

        public List<string> load_feedbacks(string proj_name,string doc)
        {
            List<string> feedbacks = new List<string>();
            localDataTable = mod.load_feedbacks();

            foreach (DataRow row in localDataTable.Rows)
            {
                if (row[3].ToString() == proj_name)
                {
                    if (!feedbacks.Contains(row[1].ToString()) && doc == row[1].ToString())
                    {
                        feedbacks.Add(row[4].ToString());
                    }
                    _doc_id = row[0].ToString();
                    _doc_name = row[1].ToString();
                    _doc_desc = row[2].ToString();
                }
            }
            return feedbacks;
        }

        /// <summary>
        /// add feedback
        /// </summary>
        /// <returns></returns>
        public bool add_feedback(string feedback, string doc_id,string doc_desc,string doc_name,string proj_name)
        {
            try
            {
                mod.addDocToDB(doc_id, doc_name, doc_desc, proj_name, feedback);
                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}
