using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Knjiznica_AB.Entity;
using NHibernate.Cfg;
using NHibernate.Mapping.Attributes;
using NHibernate.Tool.hbm2ddl;
using NHibernate;

namespace Knjiznica_AB
{
    public partial class DodajGradivo : Form
    {
        private ISessionFactory m_SessionFactory = null;
        private ISession m_Session = null;
        public DodajGradivo()
        {
            InitializeComponent();
        }
        public void SetNhib(ISessionFactory isf, ISession iss)
        {
            m_SessionFactory = isf;
            m_Session = iss;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            {
                using (ISession m_Session = m_SessionFactory.OpenSession())
                {
                    using (ITransaction tx = m_Session.BeginTransaction())
                    {
                        try
                        {
                            Gradivo d = new Gradivo();

                            d.Naslov = textBox1.Text;
                            d.Avtor = textBox2.Text;
                            d.ZvrstGradiva = textBox3.Text;
                            d.LetoIzdaje = Int32.Parse(textBox4.Text);
                            d.NaZalogi = Int32.Parse(textBox5.Text);

                            m_Session.Save(d);
                            tx.Commit();
                            MessageBox.Show("Vnos gradiva je uspel");

                            DodajGradivo.ActiveForm.Close();

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message); ;
                        }
                    }
                }
            }
        }
    }
}
