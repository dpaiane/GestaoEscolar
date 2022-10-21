using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;       /* Biblioteca pronta pacote nuget */

namespace cadastro
{
    public partial class Form1 : Form
    {
        private MySqlConnection Conexao;
        private string data_source = "datasource=localhost;username=root;password=;database=db_agenda";

        private int ?id_contato_selecionado = null;
        public Form1()
        {
            InitializeComponent();
            lst_contatos.View = View.Details;
            lst_contatos.LabelEdit = true;
            lst_contatos.AllowColumnReorder = true;
            lst_contatos.FullRowSelect = true;  
            lst_contatos.GridLines = true;

            lst_contatos.Columns.Add("ID", 30, HorizontalAlignment.Left);
            lst_contatos.Columns.Add("Nome", 150, HorizontalAlignment.Left);
            lst_contatos.Columns.Add("E-mail", 150, HorizontalAlignment.Left);
            lst_contatos.Columns.Add("Telefone", 150, HorizontalAlignment.Left);

            carregar_contatos();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            { 
                Conexao = new MySqlConnection(data_source);
                Conexao.Open();                             /* Abre uma conexao com o banco de dados */
                MySqlCommand cmd = new MySqlCommand();      /* Instanciamento da conexao */
                cmd.Connection = Conexao;                   /* Cria conexao */
                /* ou insert ou update */
                if(id_contato_selecionado == null)
                {
                    cmd.Parameters.AddWithValue("@nome", txtNome.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@telefone", txtTelefone.Text);
                    cmd.CommandText = "INSERT INTO contato (nome, email, telefone) VALUES (@nome, @email, @telefone)";  /* Formata os dados a serem enviados */
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();          /* Envia os dados para o banco de dados */

                    MessageBox.Show("Contato inserido com sucesso!", "Sucesso!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@nome", txtNome.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@telefone", txtTelefone.Text);
                    cmd.Parameters.AddWithValue("@id", id_contato_selecionado);
                    cmd.CommandText = "UPDATE contato SET nome=@nome, email=@email, telefone=@telefone WHERE id=@id ";  /* Formata os dados a serem enviados */
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();          /* Envia os dados para o banco de dados */

                    MessageBox.Show("Contato atualizado com sucesso!", "Sucesso!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                id_contato_selecionado = null;
                zerar_formulario();
                carregar_contatos(); 
            }
            catch(MySqlException ex)
            {
                MessageBox.Show("Erro " + ex.Number + " ocorreu: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch(Exception ex)
            {
                MessageBox.Show("ocorreu: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Conexao.Close();
            }

        }

        private void txtNome_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void txtTelefone_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string q = "'%" + txtBuscar.Text + "%'";
                Conexao = new MySqlConnection(data_source);
                Conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
               
                cmd.Connection = Conexao;
                cmd.CommandText = "SELECT * FROM contato WHERE nome LIKE @q OR email LIKE @q ";
                cmd.Parameters.AddWithValue("@q", "%" + txtBuscar.Text + "%");

                MySqlDataReader reader = cmd.ExecuteReader();
                lst_contatos.Items.Clear();

                while(reader.Read())
                {
                    string[] row =
                    {
                        reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)
                    };
                    lst_contatos.Items.Add(new ListViewItem(row));
                }

            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erro " + ex.Number + " ocorreu: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ocorreu: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Conexao.Close();
            }
        }
        private void carregar_contatos()
        {
            try
            {
                string q = "'%" + txtBuscar.Text + "%'";
                Conexao = new MySqlConnection(data_source);
                Conexao.Open();
                MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = Conexao;
                cmd.CommandText = "SELECT * FROM contato ORDER BY id DESC ";
                cmd.Prepare();

                MySqlDataReader reader = cmd.ExecuteReader();
                lst_contatos.Items.Clear();

                while (reader.Read())
                {
                    string[] row =
                    {
                        reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)
                    };
                    lst_contatos.Items.Add(new ListViewItem(row));
                }

            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erro " + ex.Number + " ocorreu: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ocorreu: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Conexao.Close();
            }
        }

        private void lst_contatos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lst_contatos_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            /* ListView sao as linhas,  o laco percorre os dados da linha */
            ListView.SelectedListViewItemCollection itens_selecionados = lst_contatos.SelectedItems;

            /* Percorre a colecao */
            foreach(ListViewItem item in itens_selecionados)
            {
                id_contato_selecionado = Convert.ToInt32(item.SubItems[0].Text);
                txtNome.Text = item.SubItems[1].Text;
                txtEmail.Text = item.SubItems[2].Text;
                txtTelefone.Text = item.SubItems[3].Text;
                button4.Visible = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zerar_formulario();
        }
        private void zerar_formulario()
        {
            id_contato_selecionado = null;
            txtNome.Text = "";
            txtEmail.Text = "";
            txtTelefone.Text = "";
            txtNome.Focus();
            button4.Visible = false;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            excluir_contato(); 
        }
        private void button4_Click(object sender, EventArgs e)
        {
            excluir_contato();
        }
        private void excluir_contato()
        {
            try
            {
                DialogResult conf = MessageBox.Show("Tem certeza que deseja excluir o registro?", "Ops, tem certeza", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (conf == DialogResult.Yes)
                {
                    Conexao = new MySqlConnection(data_source);
                    MySqlCommand cmd = new MySqlCommand();
                    Conexao.Open();
                    cmd.Connection = Conexao;
                    cmd.CommandText = "DELETE FROM contato WHERE id=@id";

                    cmd.Parameters.AddWithValue("@id", id_contato_selecionado);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Contato Excluído com sucesso!", "Sucesso!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    carregar_contatos();
                    zerar_formulario();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erro" + ex.Number + " ocorreu " + ex.Message, "Erro ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(" ocorreu " + ex.Message, "Erro ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Conexao.Close();
            }
        }
    }
}
