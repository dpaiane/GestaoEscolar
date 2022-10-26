using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace cadastro
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent(); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUsuario.Text.Equals("diego") && txtSenha.Text.Equals("123"))
                {
                    this.Visible = false;
                    //MessageBox.Show("Logado");
                }
                else
                {
                    MessageBox.Show("Usuário ou senha incorretos", "Desculpe", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtUsuario.Focus();
                    txtUsuario.Text = "";
                    txtSenha.Text = "";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Algo deu errado", "Desculpe", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
