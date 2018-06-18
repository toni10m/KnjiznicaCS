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
using log4net;

namespace Knjiznica_AB
{

    public partial class DodajIzposojevalca : Form
    {
        private ISessionFactory m_SessionFactory = null;
        private ISession m_Session = null;
        private MyLogger logger;

        public DodajIzposojevalca()
        {
            InitializeComponent();
            logger = new MyLogger(MyLogger.typeOfLogger.MAIN);
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
                            Izposojevalec d = new Izposojevalec();

                            d.IdentifikacijskaStevilka = Int32.Parse(textBox1.Text);
                            d.Ime = textBox2.Text;
                            d.Priimek = textBox3.Text;
                            d.TelSt = textBox4.Text;
                            
                            //beleženje vnosov članov v log-datoteko
                            logger.Debug("id:" + textBox1.Text + ",ime:" + textBox2.Text+",priimek:" + textBox3.Text + ",telSt:" + textBox4.Text);

                            
                            m_Session.Save(d);
                            tx.Commit();
                            

                            MessageBox.Show("Vnos člana je uspel");
                            
                            DodajIzposojevalca.ActiveForm.Close();

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
