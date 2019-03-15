using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQLBR
{
    public partial class Form3 : Form
    {
	string database = "";
	public Form3()
	{
	    InitializeComponent();
	}

	public string GetDBName
	{
	    get
	    {
		return database;//textBox1.Text;
	    }
	    
	}

	private void textBox1_TextChanged(object sender, EventArgs e)
	{
	    //textBox1.Clear();
	    //textBox1.Text = "";
	    database = textBox1.Text;
	}

	private void label1_Click(object sender, EventArgs e)
	{

	}

    }
}
