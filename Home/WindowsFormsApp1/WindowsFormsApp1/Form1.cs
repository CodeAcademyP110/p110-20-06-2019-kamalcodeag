using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string baseConnection = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
        int playerId;

        private void ResetInputs()
        {
            textBox1.Text = textBox2.Text = String.Empty;
            comboBox1.SelectedIndex = -1;
        }
        private async void LoadDataBase()
        {
            using (SqlConnection connection = new SqlConnection(baseConnection))
            {
                await connection.OpenAsync();
                string sqlText = "select players.id,players.name,players.surname,countries.name country from players join countries on players.countryId = countries.id";
                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlText, connection))
                {
                    DataSet dataset = new DataSet();
                    adapter.Fill(dataset);
                    dataGridView1.DataSource = dataset.Tables[0];
                    dataGridView1.Columns[0].Visible = false;

                    sqlText = "select name from countries";

                    using (SqlCommand command = new SqlCommand(sqlText, connection))
                    {
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                comboBox1.Items.Add(reader["name"].ToString());
                            }
                        }
                    }
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadDataBase();
        }

        private async void DataGridView1_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            using(SqlConnection connection = new SqlConnection(baseConnection))
            {
                await connection.OpenAsync();
                playerId = (int)dataGridView1[0, e.RowIndex].Value;
                //MessageBox.Show(playerId.ToString());
                string sqlText = $"select * from DraftPlayersList where DraftPlayersList.Id = '{playerId}'";

                using(SqlCommand command = new SqlCommand(sqlText,connection))
                {
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    if(reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            textBox1.Text = reader["name"].ToString();
                            textBox2.Text = reader["surname"].ToString();

                            for(int i = 0; i<comboBox1.Items.Count; i++)
                            {
                                if((string)dataGridView1[3,e.RowIndex].Value == comboBox1.Items[i].ToString())
                                {
                                    comboBox1.SelectedIndex = i;
                                }
                            }
                        }
                    }
                }
            }
        }

        private async void Button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(baseConnection))
            {
                await connection.OpenAsync();
                string nameText = textBox1.Text;
                string surnameText = textBox2.Text;
                string sqlText = $"update players set name = '{nameText}', surname = '{surnameText}' where id = '{playerId}'";

                using (SqlCommand command = new SqlCommand(sqlText, connection))
                {
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    reader.Close();
                    if (command.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Succesfully updated");
                        ResetInputs();
                        LoadDataBase();
                    }
                }
            }
        }

        private async void Button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(baseConnection))
            {
                await connection.OpenAsync();
                string sqlText = $"delete from players where id = '{playerId}'";

                using (SqlCommand command = new SqlCommand(sqlText, connection))
                {
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    reader.Close();
                    //if (command.ExecuteNonQuery() > 0) // ???
                    //{
                        MessageBox.Show("Succesfully deleted");
                        ResetInputs();
                        LoadDataBase();
                    //}
                }


            }
        }
    }
}
