using Compliadores_Form;

namespace Compliadores_Form
{
    public partial class Form1 : Form
    {
        Compiladores compiladores = new Compiladores();

        public Form1()
        {

            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        string archivoActual = ""; 

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Archivo de texto|*.txt|Todos los archivos|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                archivoActual = dlg.FileName;
                textBox2.Text = archivoActual; 
                richTextBox1.Text = System.IO.File.ReadAllText(archivoActual); 
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(archivoActual))
            {
                System.IO.File.WriteAllText(archivoActual, richTextBox1.Text);
                MessageBox.Show("Archivo guardado correctamente.");
            }
            else
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "Archivo de texto|*.txt|Todos los archivos|*.*";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    System.IO.File.WriteAllText(dlg.FileName, richTextBox1.Text);
                    archivoActual = dlg.FileName;
                    textBox2.Text = archivoActual;
                    MessageBox.Show("Archivo guardado correctamente.");
                }
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var resultado = compiladores.Analizar(richTextBox1.Text);

            dataGridView1.Rows.Clear();


            foreach (var token in resultado)
            {
                Console.WriteLine(token);
                dataGridView1.Rows.Add(token.Posicion, token.Lexema, token.Tipo); 
            }
        }

    }
}
