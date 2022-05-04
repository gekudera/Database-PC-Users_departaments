using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DataBaseWork
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection = null;
        private SqlCommandBuilder sqlBuilder = null;
        private SqlDataAdapter sqlDataAdapter = null;
        private DataSet dataSet = null;
        private bool newRowAdding = false;
        private int indexOfCommand = 0;

        //при запуске по умолчанию отображаем таблицу departaments
        private string NameOfTable = "Users"; 

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(@"Data Source=DESKTOP-S9R3QU4\SQLEXPRESS;Initial Catalog=test_database;Integrated Security=True");
            sqlConnection.Open();

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                sqlDataAdapter = new SqlDataAdapter("SELECT *, 'Delete' AS [Command] FROM " + NameOfTable, sqlConnection);
                sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);
                sqlBuilder.GetInsertCommand();
                sqlBuilder.GetDeleteCommand();
                sqlBuilder.GetUpdateCommand();

                dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet, NameOfTable);

                dataGridView1.DataSource = dataSet.Tables[NameOfTable];

                indexOfCommand = dataGridView1.ColumnCount - 1;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[indexOfCommand, i] = linkCell;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReloadData()
        {
            try
            {
                dataSet.Tables[NameOfTable].Clear();
                sqlDataAdapter.Fill(dataSet, NameOfTable);

                dataGridView1.DataSource = dataSet.Tables[NameOfTable];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[indexOfCommand, i] = linkCell;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ReloadData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == indexOfCommand)
                {
                    string task = dataGridView1.Rows[e.RowIndex].Cells[indexOfCommand].Value.ToString();
                    if (task == "Delete")
                    {
                        if (MessageBox.Show("Удалить эту строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            int rowIndex = e.RowIndex;
                            dataGridView1.Rows.RemoveAt(rowIndex);
                            dataSet.Tables[NameOfTable].Rows[rowIndex].Delete();
                            sqlDataAdapter.Update(dataSet, NameOfTable);
                        }
                    }
                    else if (task == "Insert")
                    {
                        int rowIndex = dataGridView1.Rows.Count - 2;
                        DataRow row = dataSet.Tables[NameOfTable].NewRow();

                        //для разных таблиц нужно заполнять свои параметры. ВОЗМОЖНО ПЕРЕНОС ЗАПИСИ ПАРАМЕТРОВ В ФУНКЦИЮ
                        if (NameOfTable == "Departaments")
                        {
                            row["ID_depart"] = dataGridView1.Rows[rowIndex].Cells["ID_depart"].Value;
                            row["Name"] = dataGridView1.Rows[rowIndex].Cells["Name"].Value;
                        }
                        else if (NameOfTable == "PC")
                        {
                            row["ID_PC"] = dataGridView1.Rows[rowIndex].Cells["ID_PC"].Value;
                            row["cpu"] = dataGridView1.Rows[rowIndex].Cells["cpu"].Value;
                            row["memory"] = dataGridView1.Rows[rowIndex].Cells["memory"].Value;
                            row["hdd"] = dataGridView1.Rows[rowIndex].Cells["hdd"].Value;
                        }
                        else if (NameOfTable == "Users")
                        {
                            row["ID_users"] = dataGridView1.Rows[rowIndex].Cells["ID_Users"].Value;
                            row["Username"] = dataGridView1.Rows[rowIndex].Cells["Username"].Value;
                            row["Salary"] = dataGridView1.Rows[rowIndex].Cells["Salary"].Value;
                            row["ID_depart"] = dataGridView1.Rows[rowIndex].Cells["ID_depart"].Value;
                            row["ID_PC"] = dataGridView1.Rows[rowIndex].Cells["ID_PC"].Value;
                        }

                        dataSet.Tables[NameOfTable].Rows.Add(row);
                        dataSet.Tables[NameOfTable].Rows.RemoveAt(dataSet.Tables[NameOfTable].Rows.Count - 1);

                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);

                        dataGridView1.Rows[e.RowIndex].Cells[indexOfCommand].Value = "Delete";

                        sqlDataAdapter.Update(dataSet, NameOfTable);

                        newRowAdding = false;
                    }
                    else if (task == "Update")
                    {
                        int r = e.RowIndex;

                        if (NameOfTable == "Departaments")
                        {
                            dataSet.Tables[NameOfTable].Rows[r]["ID_depart"] = dataGridView1.Rows[r].Cells["ID_depart"].Value;
                            dataSet.Tables[NameOfTable].Rows[r]["Name"] = dataGridView1.Rows[r].Cells["Name"].Value;
                        }
                        else if (NameOfTable == "PC")
                        {
                            dataSet.Tables[NameOfTable].Rows[r]["ID_PC"] = dataGridView1.Rows[r].Cells["ID_PC"].Value;
                            dataSet.Tables[NameOfTable].Rows[r]["cpu"] = dataGridView1.Rows[r].Cells["cpu"].Value;
                            dataSet.Tables[NameOfTable].Rows[r]["memory"] = dataGridView1.Rows[r].Cells["memory"].Value;
                            dataSet.Tables[NameOfTable].Rows[r]["hdd"] = dataGridView1.Rows[r].Cells["hdd"].Value;
                        }
                        else if (NameOfTable == "Users")
                        {
                            dataSet.Tables[NameOfTable].Rows[r]["ID_users"] = dataGridView1.Rows[r].Cells["ID_users"].Value;
                            dataSet.Tables[NameOfTable].Rows[r]["Username"] = dataGridView1.Rows[r].Cells["Username"].Value;
                            dataSet.Tables[NameOfTable].Rows[r]["salary"] = dataGridView1.Rows[r].Cells["salary"].Value;
                            dataSet.Tables[NameOfTable].Rows[r]["ID_depart"] = dataGridView1.Rows[r].Cells["ID_depart"].Value;
                            dataSet.Tables[NameOfTable].Rows[r]["ID_PC"] = dataGridView1.Rows[r].Cells["ID_PC"].Value;
                        }

                        sqlDataAdapter.Update(dataSet, NameOfTable);
                        dataGridView1.Rows[e.RowIndex].Cells[indexOfCommand].Value = "Delete";
                    }
                    ReloadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    newRowAdding = true;
                    int lastRow = dataGridView1.Rows.Count - 2;
                    DataGridViewRow row = dataGridView1.Rows[lastRow];

                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[indexOfCommand, lastRow] = linkCell;
                    row.Cells["Command"].Value = "Insert";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (newRowAdding == false)
            {
                int rowIndex = dataGridView1.SelectedCells[0].RowIndex; //индекс строки выделенной ячейки
                DataGridViewRow editingRow = dataGridView1.Rows[rowIndex];

                DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                dataGridView1[indexOfCommand, rowIndex] = linkCell;
                editingRow.Cells["Command"].Value = "Update";
            }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(column_KeyPress);

            // 0 - потому что нам не нужно в колонку id вводить буквы
            if (dataGridView1.CurrentCell.ColumnIndex == 0)
            {
                TextBox textBox = e.Control as TextBox;

                if (textBox != null)
                {
                    textBox.KeyPress += new KeyPressEventHandler(column_KeyPress);
                }
            }
        }

        private void column_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            NameOfTable = "PC";
            LoadData();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            NameOfTable = "Users";
            LoadData();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            NameOfTable = "Departaments";
            LoadData();
        }
    }
}
