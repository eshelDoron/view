using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace view
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        controller control = new controller();
        string active_user = "";
        string active_user_password = "";
        string doc_id = "";
        string doc_name = "";
        string doc_desc = "";

        public MainWindow()
        {
            InitializeComponent();
            main.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// login button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //checking that details were inserted
                if (userName.Text != "" && password.Password != "")
                {
                    bool accessGranted = false;
                    accessGranted = control.login_access(userName.Text, password.Password);

                    if (!accessGranted)
                    {
                        MessageBox.Show("The users details you entered are incorrect");
                        password.Password = "";
                        userName.Text = "";
                    }
                    else
                    {
                        //set global user and password
                        active_user = control.user_id;
                        active_user_password = password.Password;

                        //set the right windows to be visible / not visible
                        middel_screen.Visibility = System.Windows.Visibility.Visible;
                        main.Visibility = System.Windows.Visibility.Hidden;
                        add_stud_win.Visibility = System.Windows.Visibility.Hidden;

                        //reset the fields
                        id.Text = "";
                        firstName.Text = "";
                        lastName.Text = "";
                        phone.Text = "";
                        dep.Text = "";
                        email.Text = "";

                        visiter.Content = control.user_name + " שלום";

                        //check premissions, in order to know which button to make usable
                        if (control.userType == "sec")
                        {
                            feedback.IsEnabled = false;
                            add.IsEnabled = true;
                        }
                        else
                        {
                            feedback.IsEnabled = true;
                            add.IsEnabled = false;
                        }


                    }
                }
                else
                {
                    MessageBox.Show("You need to enter an Email and a Password");
                }

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
        /// <summary>
        /// go to the prev screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void back_Click(object sender, RoutedEventArgs e)
        {
            //go to the previous screen
            main.Visibility = System.Windows.Visibility.Hidden;
            add_stud_win.Visibility = System.Windows.Visibility.Hidden;
            middel_screen.Visibility = System.Windows.Visibility.Visible;
            password.Password = "";
            userName.Text = "";
        }
        /// <summary>
        /// add student
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void add_student_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //check that all the details were inserted
                if (id.Text != "" && firstName.Text != "" && lastName.Text != "" && email.Text != "" && dep.Text != "" && phone.Text != "")
                {
                    int number = 0;
                    if (id.Text.Length != 9 || !int.TryParse(id.Text, out number))
                    {
                        MessageBox.Show("ID must be 9 digits");
                    }
                    else if (!email.Text.Contains("@") || email.Text.IndexOf('@') == 0 || email.Text.IndexOf('@') == email.Text.Length - 1 || !email.Text.Contains(".") || email.Text.IndexOf('.') == 0 || email.Text.IndexOf('.') == email.Text.Length - 1)
                    {
                        MessageBox.Show("Email is invalid");
                    }
                    else if (!int.TryParse(phone.Text, out number))
                    {
                        MessageBox.Show("Phone number is invalid");
                    }
                    else
                    {

                        bool accessGranted = false;
                        accessGranted = control.add_stud(id.Text, firstName.Text, lastName.Text, email.Text, dep.Text, phone.Text);

                        if (!accessGranted)
                        {
                            MessageBox.Show("The ID already exists in the database");
                        }
                        else
                        {
                            MessageBox.Show("Student was added successfully, a mail was sent");

                        }
                    }
                }
                else
                {
                    MessageBox.Show("All fields must have value");
                }
            }

            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        /// <summary>
        /// go to the prev screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void back_main_Click(object sender, RoutedEventArgs e)
        {
            //go to the previous screen
            main.Visibility = System.Windows.Visibility.Visible;
            middel_screen.Visibility = System.Windows.Visibility.Hidden;
            password.Password = "";
            userName.Text = "";
            visiter.Content = "שלום אורח";
        }

        /// <summary>
        /// go to the add student screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void add_Click(object sender, RoutedEventArgs e)
        {
            main.Visibility = System.Windows.Visibility.Hidden;
            middel_screen.Visibility = System.Windows.Visibility.Hidden;
            add_stud_win.Visibility = System.Windows.Visibility.Visible;
            email.Text = "";
            firstName.Text = "";
            lastName.Text = "";
            dep.Text = "";
            phone.Text = "";
            id.Text = "";
        }

        /// <summary>
        /// go to the feedback screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void feedback_Click(object sender, RoutedEventArgs e)
        {
            //set the right windows to be visible / not visible
            middel_screen.Visibility = System.Windows.Visibility.Hidden;
            feedback_screen.Visibility = System.Windows.Visibility.Visible;
            projects_lv.Items.Clear();
            docs_lv.Items.Clear();

            try
            {

                List<string> projects = new List<string>();
                projects = control.get_projects(active_user, active_user_password);

                if (projects.Count == 0)
                {
                    MessageBox.Show("You have no open projects");
                    //set the right windows to be visible / not visible
                    middel_screen.Visibility = System.Windows.Visibility.Visible;
                    main.Visibility = System.Windows.Visibility.Hidden;
                    add_stud_win.Visibility = System.Windows.Visibility.Hidden;

                    id.Text = "";
                    firstName.Text = "";
                    lastName.Text = "";
                    phone.Text = "";
                    dep.Text = "";
                    email.Text = "";
                    if (control.userType == "sec")
                    {
                        feedback.IsEnabled = false;
                        add.IsEnabled = true;
                    }
                    else
                    {
                        feedback.IsEnabled = true;
                        add.IsEnabled = false;
                    }
                }
                else
                {
                    foreach (string project in projects)
                    {
                        projects_lv.Items.Add(project);
                    }
                }

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        /// <summary>
        /// after a project was selceted, view the feedback screen of the project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void select_p_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (projects_lv.SelectedItem != null && docs_lv.SelectedItem != null)
                {
                    feedback_screen.Visibility = System.Windows.Visibility.Hidden;
                    leave_feedback.Visibility = System.Windows.Visibility.Visible;
                    project_name.Content = projects_lv.SelectedItem;

                    List<string> feedbacks = new List<string>();
                    feedbacks = control.load_feedbacks(project_name.Content.ToString(), docs_lv.SelectedItem.ToString());
                    doc_id = control._doc_id;
                    doc_desc = control._doc_desc;
                    doc_name = control._doc_name;

                    foreach (string feedback in feedbacks)
                    {
                        feebback_history_lb.Items.Add(feedback);
                    }
                }
                else
                {
                    MessageBox.Show("You must pick a project and a document");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        /// <summary>
        /// after the add feedback button was clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void add_feedback_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string new_feedback = new_feedback_tb.Text;
                new_feedback_tb.Text = "";

                control.add_feedback(new_feedback, doc_id, doc_desc, doc_name, project_name.Content.ToString());
                feebback_history_lb.Items.Add(new_feedback);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

        }

        /// <summary>
        /// go to previous screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void feedback_back_Click(object sender, RoutedEventArgs e)
        {
            leave_feedback.Visibility = System.Windows.Visibility.Hidden;
            feedback_screen.Visibility = System.Windows.Visibility.Visible;
            new_feedback_tb.Text = "";
            feebback_history_lb.Items.Clear();
        }

        private void feedback_screen_back_btn_Click(object sender, RoutedEventArgs e)
        {
            middel_screen.Visibility = System.Windows.Visibility.Visible;
            feedback_screen.Visibility = System.Windows.Visibility.Hidden;
            projects_lv.Items.Clear();
        }


        private void projects_lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (projects_lv.SelectedItem != null)
                {
                    project_name.Content = projects_lv.SelectedItem;
                    List<string> docs = new List<string>();
                    docs = control.load_docs(project_name.Content.ToString());

                    foreach (string feedback in docs)
                    {
                        docs_lv.Items.Add(feedback);
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
    }
}
