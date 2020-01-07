﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace WorkerCatalog
{
    public partial class Post : Form
    {
        public Post()
        {
            InitializeComponent();
        }
        SqlConnection conn;
        DataTable Visualisation()
        {
            string query = "Select ID_Post, Name as Наименование From Post";
            SqlCommand command = new SqlCommand(query, conn);
            SqlDataAdapter dataadapter = new SqlDataAdapter(command);
            SqlCommandBuilder CommandBuilder = new SqlCommandBuilder(dataadapter);
            DataTable dt = new DataTable();
            dataadapter.Fill(dt);
            return dt;
        }
        Authorization auth;
        private void Post_Load(object sender, EventArgs e)
        {
            auth = (Authorization)Application.OpenForms[0];
            conn = auth.conn;
            dataGridView1.DataSource = Visualisation();
            dataGridView1.Columns[0].Visible = false;
        }

        private void Post_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dataGridView1.Visible == false)
            {
                if (groupBox1.Visible == true)
                {
                    MessageBox.Show("Вы не можете закрыть форму в режиме добавления данных!");
                }
                if (groupBox2.Visible == true)
                    MessageBox.Show("Вы не можете закрыть форму в режиме добавления данных!");
                e.Cancel = true;
            }
        }

        int ToInt(string value)
        {
            return Convert.ToInt32(value);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.RowCount != 0)
                {
                    if (dataGridView1.SelectedRows.Count != 0)
                    {
                        string query = "DELETE FROM Post where ID_Post=" + ToInt(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                        SqlCommand command = conn.CreateCommand();
                        command.CommandText = query;
                        command.ExecuteNonQuery();
                        dataGridView1.DataSource = Visualisation();
                        MessageBox.Show("Запись удалена!");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Невозможно удалить,так как в таблице Сотрудники имеются связаннные записи!");
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            dataGridView1.Visible = false;
            groupBox1.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button1.Enabled = !false;
            button2.Enabled = !false;
            button3.Enabled = !false;
            dataGridView1.Visible = !false;
            groupBox1.Visible = !true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Заполните все поля!");
            }
            else
            {
                string Name = textBox1.Text;
                Name = Name.Trim(new char[] { ' ' });
                Name = Regex.Replace(Name, @"\s+", " ");
                if (Name.Length == 0)
                {
                    MessageBox.Show("Заполните все поля!");
                }
                else
                {
                    string query = "Insert Into Post Values('" + Name + "')";
                    SqlCommand command = conn.CreateCommand();
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                    dataGridView1.DataSource = Visualisation();
                    dataGridView1.CurrentCell = dataGridView1[1, dataGridView1.RowCount - 1];
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = true;
                    button1.Enabled = !false;
                    button2.Enabled = !false;
                    button3.Enabled = !false;
                    dataGridView1.Visible = !false;
                    groupBox1.Visible = !true;
                    textBox1.Text = "";
                }
            }
        }
        int RedIndex;
        private void button2_Click(object sender, EventArgs e)
        {
            string query = "Select Name from Post where ID_Post=" + ToInt(dataGridView1.CurrentRow.Cells[0].Value.ToString());
            SqlCommand command = new SqlCommand(query, conn);
            SqlDataAdapter da = new SqlDataAdapter(command);
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            DataTable dt = new DataTable();
            da.Fill(dt);
            textBox2.Text = dt.Rows[0][0].ToString();
            RedIndex = dataGridView1.CurrentRow.Index;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            dataGridView1.Visible = false;
            groupBox2.Visible = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button1.Enabled = !false;
            button2.Enabled = !false;
            button3.Enabled = !false;
            dataGridView1.Visible = !false;
            groupBox2.Visible = !true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("Заполните все поля!");
            }
            else
            {
                string Name = textBox2.Text;
                Name = Name.Trim(new char[] { ' ' });
                Name = Regex.Replace(Name, @"\s+", " ");
                if (Name.Length == 0)
                {
                    MessageBox.Show("Заполните все поля!");
                }
                else
                {
                    string query = "UPDATE Post SET Name='" + Name + "' WHERE ID_Post=" + ToInt(dataGridView1[0, RedIndex].Value.ToString());
                    SqlCommand command = conn.CreateCommand();
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                    dataGridView1.DataSource = Visualisation();
                    dataGridView1.CurrentCell = dataGridView1[1, RedIndex];
                    dataGridView1.Rows[RedIndex].Selected = true;
                    button1.Enabled = !false;
                    button2.Enabled = !false;
                    button3.Enabled = !false;
                    dataGridView1.Visible = !false;
                    groupBox2.Visible = !true;
                    textBox2.Text = "";
                }
            }
        }
        bool IsFormOpened<TForm>() where TForm : Form
        {
            return Application.OpenForms.OfType<TForm>().Any();
        }
        Worker worker;
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (IsFormOpened<Worker>())
            {
                this.Close();
                worker = (Worker)Application.OpenForms["Worker"];
                worker.Focus();
            }           
        }
    }
}
