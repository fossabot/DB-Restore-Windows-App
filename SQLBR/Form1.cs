/*
 * LDM-AVINASH = Local SQL Server
 * LDM-AVINASH\SQLEXPRESS = Network Server
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Threading;
using System.Windows.Forms;

namespace SQLBR
{
   public partial class Restore : Form
   {
      public Restore()
      {
         InitializeComponent();
      }
      private void Form1_Load(object sender, EventArgs e)
      {
         Thread th = new Thread(new ThreadStart(FillServers));
         th.IsBackground = true;
         th.Start();
      }

      private void FillServers()
      {
         this.Invoke((MethodInvoker)delegate
         {
            comboBox1.Enabled = false;
            comboBox1.Items.Clear();
            comboBox1.Items.Add("Please wait...");
            comboBox1.SelectedIndex = 0;
            Application.DoEvents();
	    
	    SqlDataSourceEnumerator instance = SqlDataSourceEnumerator.Instance;
	    DataTable servers = instance.GetDataSources();
            for(int i = 0; i < servers.Rows.Count; i++)
            {
               if((servers.Rows[i]["InstanceName"] as string) != null)
                  comboBox1.Items.Add(servers.Rows[i]["ServerName"] + "\\" + servers.Rows[i]["InstanceName"]);
               else
                  comboBox1.Items.Add(servers.Rows[i]["ServerName"]);
            }
            comboBox1.Items.RemoveAt(0);
            comboBox1.Sorted = true;
            if(comboBox1.Items.Count > 0)
            {
               comboBox1.SelectedIndex = 0;
               comboBox1.Enabled = true;
            }
         });
      }
	
      public List<string> GetDatabaseList()
      {
         List<string> list = new List<string>();
         // Open connection to the database
         string conString;
         if(cbIntergrated.Checked)
            conString = "server=" + textBox3.Text + ";Integrated Security=true";
         else
            conString = "server=" + textBox3.Text + ";uid=" + textBox1.Text + ";pwd=" + textBox2.Text;
         using(SqlConnection con = new SqlConnection(conString))
         {
            try
            {
               con.Open();
               // Set up a command with the given query and associate
               // this with the current connection.
               using(SqlCommand cmd = new SqlCommand("SELECT name from sys.databases", con))
               {
                  using(IDataReader dr = cmd.ExecuteReader())
                  {
                     while(dr.Read())
                     {
                        list.Add(dr[0].ToString());
                     }
                  }
               }

            }
            catch(Exception exception)
            {
               MessageBox.Show(exception.Message);
            }
         }
         return list;
      }

      private void comboBox2_DropDown(object sender, EventArgs e)
      {
         comboBox2.Items.Clear();
         foreach(string s in GetDatabaseList())
         {
            comboBox2.Items.Add(s);
         }
      }
      private void cbIntergrated_Click(object sender, EventArgs e)
      {
         textBox1.Enabled = !cbIntergrated.Checked;
         textBox2.Enabled = !cbIntergrated.Checked;
      }
      private void comboBox1_TextChanged(object sender, EventArgs e)
      {
         comboBox2.Items.Clear();
         comboBox2.Text = "";
      }
      private void AddMemo(string inMemo)
      {
         memoEdit1.Text += DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss") + "\r\n";
         memoEdit1.Text += inMemo + "\r\n";
         Application.DoEvents();

      }

      private void DoBackup()
      {
         string bkfile = "";
         Form2 f = new Form2(textBox3.Text, textBox1.Text, textBox2.Text, comboBox2.Text, cbIntergrated.Checked);
         if(f != null)
         {
            if(f.ShowDialog() == DialogResult.OK)
               bkfile = f.FullBackupPath;
            else
               return;
         }
         else
            return;
         if(bkfile == "")
            return;
         SqlConnection con = new SqlConnection();
         SqlCommand sqlcmd = new SqlCommand();
         SqlDataAdapter da = new SqlDataAdapter();
         DataTable dt = new DataTable();

         if(cbIntergrated.Checked)
            con.ConnectionString = "Data Source=" + textBox3.Text + ";" +
                                    "Initial Catalog=" + comboBox2.Text + ";" +
                                    "Integrated Security=true";
         else
            con.ConnectionString = "Data Source=" + textBox3.Text + ";" +
                                    "Initial Catalog=" + comboBox2.Text + ";" +
                                    "User id=" + textBox1.Text + ";" +
                                    "Password=" + textBox2.Text + ";";
         try
         {
            con.Open();
            string tSQL = @"backup database " + comboBox2.Text + " to disk='" + bkfile + "' with init";
            AddMemo(tSQL);
            sqlcmd = new SqlCommand(tSQL, con);
            sqlcmd.CommandTimeout = 0;
            sqlcmd.ExecuteNonQuery();
            Application.DoEvents();
            con.Close();
            Application.DoEvents();
            AddMemo("Success");
	 }
         catch(Exception ex)
         {
            AddMemo("!!! Error - " + ex.Message);
            MessageBox.Show(ex.Message);
         }
      }
      private void DoRestore()
      {
         string bkfile = "";
	 string database = null;
	 

	    Form2 f = new Form2(textBox3.Text, textBox1.Text, textBox2.Text, comboBox2.Text, cbIntergrated.Checked);
	 Form3 f3 = new Form3();
	 if (f3 != null)
	 {
	    if (f3.ShowDialog() == DialogResult.OK)
		    database = f3.GetDBName; 
	    else
		return;
	 }
	 else
	    return;

         if(f != null)
         {
            if(f.ShowDialog() == DialogResult.OK)
               bkfile = f.FullBackupPath;
            else
               return;
         }
         else
            return;
         if(bkfile == "")
            return;
         SqlConnection con = new SqlConnection();
         SqlCommand sqlcmd = new SqlCommand();
         SqlDataAdapter da = new SqlDataAdapter();
         DataTable dt = new DataTable();

         if(cbIntergrated.Checked)
            con.ConnectionString = "Data Source=" + textBox3.Text + ";" +
                                    "Initial Catalog=" + comboBox2.Text + ";" +
                                    "Integrated Security=true";
         else
            con.ConnectionString = "Data Source=" + textBox3.Text + ";" +
                                    "Initial Catalog=" + comboBox2.Text + ";" +
                                    "User id=" + textBox1.Text + ";" +
                                    "Password=" + textBox2.Text + ";";
         try
         {
            con.Open();
		string tSQL = @"RESTORE database "
				+ database +
				" from disk='"
				+ bkfile +
				"'";
				//+ "WITH MOVE '"
				//+ database +
				//"_data' TO "
				//+ @"'C:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\"
				//+ database +
				//".mdf'"; 
		AddMemo(tSQL);
            sqlcmd = new SqlCommand(tSQL, con);
            sqlcmd.CommandTimeout = 0;
            sqlcmd.ExecuteNonQuery();
            Application.DoEvents();
            con.Close();
            Application.DoEvents();
            AddMemo("Success");
	    AddMemo("Triggering Stored Procedure....");
		// Trigger Stored Procedure after restore.
		String connectionString = "Server=" + textBox3.Text + "; DataBase=" + database + ";Integrated Security=SSPI";
		triggerSP(connectionString);
	 }
         catch(Exception ex)
         {
            AddMemo("!!! Error - " + ex.Message);
            MessageBox.Show(ex.Message);
         }
      }

      private void triggerSP(String connectionStr)
	{
	    // This doesn't open the Connection. conn.Open() has to be explicitly called.
	    SqlConnection conn = new SqlConnection(connectionStr);
	    try
	    {

		conn.Open();
		AddMemo("Inside the Trigger....");
		// 1.  create a command object identifying the stored procedure
		SqlCommand cmd = new SqlCommand("usp_NestedSP", conn);

		// 2. set the command object so it knows to execute a stored procedure
		cmd.CommandType = CommandType.StoredProcedure;

		// Add a check here as well.
		// execute the command
		SqlDataReader rdr = cmd.ExecuteReader();

		// Since we are not using - using block we have to explicitly call Close() to close the connection.
		//conn.Close();
	    }
	    catch (SqlException SqlEx)
	    {
		string[] error = new string[3];

		string msg1 = "Errors Count:" + SqlEx.Errors.Count;
		string msg2 = null;

		foreach (SqlError myError in SqlEx.Errors)
		    msg2 += myError.Number + " - " + myError.Message + "/";

		error[0] = msg1;
		error[1] = msg2;
		AddMemo("Something went wrong......IDk what Debugg it." + msg2);
		//WriteToFile(error);
	    }

	    finally
	    {
		//call this if exception occurs or not
		//in this example, dispose the WebClient
		conn.Close();
		AddMemo("Connection Closed after executing the SPs....");
	    }

	}

      private void button1_Click(object sender, EventArgs e)
      {
         if(radioButton1.Checked)
            DoBackup();
         else
            DoRestore();

      }
      private void label1_Click(object sender, EventArgs e)
	{

	}
   }

}
