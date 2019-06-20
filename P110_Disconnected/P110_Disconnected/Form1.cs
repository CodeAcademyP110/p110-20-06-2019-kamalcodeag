using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace P110_Disconnected
{
    public partial class Form1 : Form
    {
        private readonly string baseConnString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
        private int update_emp_id;

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            string commandText = "select * from EmployeesReport";

            using (SqlConnection conn = new SqlConnection(baseConnString))
            {
                conn.Open();

                //fill DataGridView
                using (SqlDataAdapter adapter = new SqlDataAdapter(commandText, conn))
                {
                    DataSet employees = new DataSet();
                    adapter.Fill(employees);

                    dgwEmployees.DataSource = employees.Tables[0];
                    dgwEmployees.Columns[0].Visible = false;
                }
                    
                //fill ComboBox
                commandText = "select Id, Name from Positions";

                using (SqlCommand command = new SqlCommand(commandText, conn))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);

                            ComboBoxItem item = new ComboBoxItem(id, name);
                            cmbPositions.Items.Add(item);
                        }
                    }
                }
            }
        }

        private async void dgwEmployees_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            update_emp_id = (int)dgwEmployees[0, e.RowIndex].Value;

            string commandText = "select * from Employees where Id = @Id";

            using (SqlConnection conn = new SqlConnection(baseConnString))
            {
                using (SqlCommand comm = new SqlCommand(commandText, conn))
                {
                    comm.Parameters.AddWithValue("@Id", update_emp_id);

                    await conn.OpenAsync();

                    using (SqlDataReader reader = await comm.ExecuteReaderAsync())
                    {
                        if (!reader.HasRows)
                        {
                            MessageBox.Show("Employee not found");
                            return;
                        }

                        reader.Read();

                        txtFirstname.Text = reader["Firstname"].ToString();
                        txtLastname.Text = reader["Lastname"].ToString();
                        txtEmail.Text = reader["Email"].ToString();
                        nmSalary.Value = reader.GetDecimal(5);

                        int positionId = reader.GetInt32(6);

                        //select employee position from ComboBox
                        for(int i = 0; i < cmbPositions.Items.Count; i++)
                        {
                            ComboBoxItem item = cmbPositions.Items[i] as ComboBoxItem;
                            if(item.Id == positionId)
                            {
                                cmbPositions.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private async void btnEdit_Click(object sender, EventArgs e)
        {
            string fname = txtFirstname.Text;
            string lname = txtLastname.Text;
            string email = txtEmail.Text;
            decimal salary = nmSalary.Value;
            int positionId = ((ComboBoxItem)cmbPositions.SelectedItem).Id;

            string commandText =
                "update Employees  set Firstname = @fname, Lastname = @lname, Email = @email, Salary = @salary, PositionId = @posId where Id = @Id";

            using (SqlConnection conn = new SqlConnection(baseConnString))
            {
                using (SqlCommand comm = new SqlCommand(commandText, conn))
                {
                    comm.Parameters.AddWithValue("@fname", fname);
                    comm.Parameters.AddWithValue("@lname", lname);
                    comm.Parameters.AddWithValue("@email", email);
                    comm.Parameters.AddWithValue("@salary", salary);
                    comm.Parameters.AddWithValue("@posId", positionId);
                    comm.Parameters.AddWithValue("@Id", update_emp_id);

                    await conn.OpenAsync();

                    int rowAffected = await comm.ExecuteNonQueryAsync();

                    if(rowAffected > 0)
                    {
                        MessageBox.Show("Successfull Update", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetUpdatePanel();
                        Form1_Load(null, null);
                    }
                    else
                    {
                        MessageBox.Show("Unknown error occurred", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

        }

        private void ResetUpdatePanel()
        {
            txtFirstname.Text = txtLastname.Text = txtEmail.Text = string.Empty;
            nmSalary.Value = 0;
            cmbPositions.SelectedIndex = 0;
        }
    }
}
