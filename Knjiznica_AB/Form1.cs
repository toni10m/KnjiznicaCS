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
using CsvHelper;
using System.IO;
using NHibernate.Mapping;
using System.Threading;
using System.Globalization;

namespace Knjiznica_AB
{
    public partial class Form1 : Form
    {
        private ISessionFactory m_SessionFactory = null;
        private ISession m_Session = null;
        private DodajIzposojevalca dDodajIzposojevalca;
        private DodajGradivo dDodajGradivo;
        private IList<Izposojevalec> _izposojevalci;
        private BindingSource _bs;
        private IList<Gradivo> _gradiva;
        private BindingSource _gbs;
        private IList<Izposoja> _izposoje;
        private BindingSource _ibs;
        private Login fLogin;
        private Zaposleni fZaposleni;
        
        private MyLogger logger;

        public Form1()
        {
            InitializeComponent();
            ConfigureLog4Net();
            ConfigureNHibernate(false);

            //inicializacija prvega uporabnika
            //initUser();

            logger = new MyLogger(MyLogger.typeOfLogger.MAIN);
            Login();
        }
        private void Login()
        {
            int count = 3;
            fLogin = new Login();
            
            
            while (count > 0)
            {
                if (fLogin.ShowDialog() == DialogResult.OK)
                {
                    
                    if (CheckUser(fLogin.Controls["textBox1"].Text, fLogin.Controls["textBox2"].Text))
                    {
                        count = -1;
                        this.Text = "User: " + fLogin.Controls["textBox1"].Text;
                    }
                    else
                    {
                        MessageBox.Show("Vnešeni prijavni podatki so bili napačni!");
                        count--;
                    }
                }
            }
            fLogin.Dispose();
            if (count == 0)
                System.Environment.Exit(0);
        }
        //preverjanje ujemanja gesla z uporabnikom
        private bool CheckUser(string user, string pass)
        {
            bool result = false;
            try
            {
                ITransaction tx = m_Session.BeginTransaction();
                IQuery query = m_Session.CreateQuery("from Zaposlen d where d.UporabniskoIme = ?");
                Zaposlen res = query.SetString(0, user).UniqueResult<Zaposlen>();
                tx.Commit();
                if (res != null && res.Geslo.Equals(pass))
                    result = true;
            }
            catch (Exception ex)
            {
                ResetSession();
            }
            return result;
        }

        private void ConfigureLog4Net()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        private void ConfigureNHibernate(bool bDrop)
        {
            Configuration config = new Configuration();
            config.Configure();
            HbmSerializer.Default.Validate = true;//enable validation (optional)
            config.AddInputStream(HbmSerializer.Default.Serialize(System.Reflection.Assembly.GetExecutingAssembly()));

            //ustvarjanje nove tabele, brisanje tabel

            //prvič je ta sklop odkomentiran nato jo zakomentiram
            //config.AddAssembly(typeof(Izposoja).Assembly);
            //config.AddAssembly(typeof(Zaposlen).Assembly);
            //new SchemaExport(config).Execute(true, true, bDrop);



            //create session factory from configuration object
            m_SessionFactory = config.BuildSessionFactory();
            m_Session = m_SessionFactory.OpenSession();
        }
        private void ResetSesion()
        {
            m_Session.Close();
            m_Session.Dispose();
            m_Session = m_SessionFactory.OpenSession();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_Session.Close();
            m_Session.Dispose();
            m_SessionFactory.Close();
            m_SessionFactory.Dispose();
        }

        //create table

        ////drop table
        //private void button2_Click(object sender, EventArgs e)
        //{
        //    ConfigureNHibernate(true);
        //}
        //dodajanje članov oz. izposojevalcev
        private void button3_Click(object sender, EventArgs e)
        {
            dDodajIzposojevalca = new DodajIzposojevalca();
            dDodajIzposojevalca.SetNhib(m_SessionFactory, m_Session);
            dDodajIzposojevalca.ShowDialog();
            Form1_Load(null, null);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //izposojevalci
            _izposojevalci = m_Session.CreateCriteria(typeof(Izposojevalec)).List<Izposojevalec>();
            _bs = new BindingSource();
            _bs.DataSource = _izposojevalci;
            _bs.AllowNew = true;
            dataGridView1.DataSource = _bs;
            dataGridView1.Columns.Remove("Izposoje");
            dataGridView1.Columns.Remove("Id");
            _bs.ListChanged += new System.ComponentModel.ListChangedEventHandler(bs_ListChanged);
            

            //gradivaypeof(Gradivo
            _gradiva = m_Session.CreateCriteria(typeof(Gradivo)).List<Gradivo>();
            _gbs = new BindingSource();
            _gbs.DataSource = _gradiva;
            _gbs.AllowNew = true;
            dataGridView2.DataSource = _gbs;
            dataGridView2.Columns.Remove("Izposoje");
            dataGridView2.Columns.Remove("Id");
            _gbs.ListChanged += new System.ComponentModel.ListChangedEventHandler(gbs_ListChanged);

            //izposoje
            
            _izposoje = m_Session.CreateCriteria(typeof(Izposoja)).List<Izposoja>();
            _ibs = new BindingSource();
            _ibs.DataSource = _izposoje;
            //_ibs.DataSource = query;
            _ibs.AllowNew = true;
            
            dataGridView3.DataSource = _ibs;
            dataGridView3.Columns.Remove("Id");
            dataGridView3.Columns.Remove("Zaposlen");
            dataGridView3.Columns.Remove("Izposojevalec");
            dataGridView3.Columns.Remove("Gradivo");
            dataGridView3.Columns.Remove("DatumIzposoje");
            dataGridView3.Columns.Remove("DatumVracila");

            
            dataGridView3.Columns[0].DataPropertyName = "Naslov";
            dataGridView3.Columns[1].DataPropertyName = "Avtor";
            dataGridView3.Columns[2].DataPropertyName = "Ime";
            dataGridView3.Columns[3].DataPropertyName = "Priimek";
            dataGridView3.Columns[4].DataPropertyName = "DatumIzposoje";
            dataGridView3.Columns[5].DataPropertyName = "DatumVracila";
           
            //IQuery query = m_Session.CreateQuery("from Car d where d.Brand = ?");
            //Car res = query.SetString(0, textBox1.Text).UniqueResult<Car>();
            //tx.Commit();
            //textBox4.Text = "Rezultat: " + System.Environment.NewLine + "Znamka: " + res.Brand
            //                           + System.Environment.NewLine + "Model: " + res.Model
            //                           + System.Environment.NewLine + "Letnik: " + res.Year


        }
        //edit za člane
        void bs_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemChanged:
                    {
                        using (ITransaction tx = m_Session.BeginTransaction())
                        {
                            Izposojevalec new_user = (Izposojevalec)(_bs.List[e.NewIndex]);
                            if (new_user.Ime == null)
                                new_user.Ime = "";
                            if (new_user.Priimek == null)
                                new_user.Priimek = "";
                            if (new_user.TelSt == null)
                                new_user.TelSt = "";
                            m_Session.Save(new_user);
                            tx.Commit();
                        }
                        break;
                    }
            }
        }

        //edit za gradiva
        void gbs_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemChanged:
                    {
                        using (ITransaction tx = m_Session.BeginTransaction())
                        {
                            Gradivo new_gradivo = (Gradivo)(_gbs.List[e.NewIndex]);
                            if (new_gradivo.Naslov == null)
                                new_gradivo.Naslov = "";
                            if (new_gradivo.Avtor == null)
                                new_gradivo.Avtor = "";
                            if (new_gradivo.ZvrstGradiva == null)
                                new_gradivo.ZvrstGradiva = "";
                            
                            m_Session.Save(new_gradivo);
                            tx.Commit();
                        }
                        break;
                    }
            }
        }

        //dodajanje gradiv
        private void button4_Click(object sender, EventArgs e)
        {
            dDodajGradivo = new DodajGradivo();
            dDodajGradivo.SetNhib(m_SessionFactory, m_Session);
            dDodajGradivo.ShowDialog();
            Form1_Load(null, null);
        }

        //izposoja
        private void button1_Click(object sender, EventArgs e)
        {
            //ConfigureNHibernate(false);
            {
                using (ISession m_Session = m_SessionFactory.OpenSession())
                {
                    using (ITransaction tx = m_Session.BeginTransaction())
                    {
                        try
                        {
                            Izposoja d = new Izposoja();

                            d.Izposojevalec = (Izposojevalec)dataGridView1.CurrentRow.DataBoundItem;
                            d.Gradivo = (Gradivo)dataGridView2.CurrentRow.DataBoundItem;
                            d.DatumIzposoje = DateTime.Now;

                            m_Session.Save(d);
                            tx.Commit();
                            MessageBox.Show("Gradivo je bilo uspešno izposojeno");

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message); ;
                        }
                    }
                }
            }
            Form1_Load(null, null);
        }
        //dodajanje in spreminjanje dostopov do aplikacije
        private void zaposleniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fZaposleni = new Zaposleni();
            fZaposleni.SetNhib(m_SessionFactory, m_Session);
            fZaposleni.ShowDialog();
        }

        
        private void initUser()
        {
            try
            {
                ITransaction tx = m_Session.BeginTransaction();
                Zaposlen usr = new Zaposlen();
                usr.UporabniskoIme = "test";
                usr.Geslo = "test";
                m_Session.Save(usr);
                tx.Commit();
            }
            catch (Exception ex)
            {
                ResetSession();
            }
        }
        private void ResetSession()
        {
            m_Session.Close();
            m_Session.Dispose();
            m_Session = m_SessionFactory.OpenSession();
        }

        
        //izvoz gradiv
        private void button7_Click(object sender, EventArgs e)
        {
            SaveToCSV(dataGridView2);

           

        }
        //csv
        private void SaveToCSV(DataGridView DGV)
        {
            string filename = "";
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV (*.csv)|*.csv";
            sfd.FileName = "Output.csv";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Ko bodo podatki pripravljeni na izvoz boste obveščeni.");
                if (File.Exists(filename))
                {
                    try
                    {
                        File.Delete(filename);
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show("Podatkov ni bilo mogoče izvoziti." + ex.Message);
                    }
                }
                int columnCount = DGV.ColumnCount;
                string columnNames = "";
                string[] output = new string[DGV.RowCount + 1];
                for (int i = 0; i < columnCount; i++)
                {
                    columnNames += DGV.Columns[i].Name.ToString() + ",";
                }
                output[0] += columnNames;
                for (int i = 1; (i - 1) < DGV.RowCount; i++)
                {
                    for (int j = 0; j < columnCount; j++)
                    {
                        output[i] += DGV.Rows[i - 1].Cells[j].Value.ToString() + ",";
                    }
                }
                System.IO.File.WriteAllLines(sfd.FileName, output, System.Text.Encoding.UTF8);
                MessageBox.Show("Podatki so bili uspešno izvoženi in so pripravljeni na uporabo.");
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {

            Printanje _Print = new Printanje(dataGridView3, "Izpis Podatkov");
            
            _Print.PrintForm();

        }
        
        
        //lokalizacija
        private void angleskoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(angleskoToolStripMenuItem.Text == "Angleško")
            {
                ChangeLanguage("en");
                zaposleniToolStripMenuItem.Text = "Employees";
                angleskoToolStripMenuItem.Text = "Slovene";

            }
            else if(angleskoToolStripMenuItem.Text == "Slovene")
            {
                ChangeLanguage("sl-SI");
                zaposleniToolStripMenuItem.Text = "Zaposleni";
                angleskoToolStripMenuItem.Text = "Angleško";
            }
        }
        private void ChangeLanguage(string lang)
        {
            foreach (Control c in this.Controls)
            {
                ComponentResourceManager resources = new ComponentResourceManager(typeof(Form1));
                resources.ApplyResources(c, c.Name, new CultureInfo(lang));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            //ConfigureNHibernate(false);
            {
                using (ISession m_Session = m_SessionFactory.OpenSession())
                {
                    using (ITransaction tx = m_Session.BeginTransaction())
                    {
                        try
                        {
                            Izposoja d = (Izposoja)dataGridView3.CurrentRow.DataBoundItem;
                            d.DatumVracila = DateTime.Today;
                            //d.Izposojevalec = (Izposojevalec)dataGridView1.CurrentRow.DataBoundItem;
                            //d.Gradivo = (Gradivo)dataGridView2.CurrentRow.DataBoundItem;
                            

                            m_Session.Update(d);
                            tx.Commit();
                            MessageBox.Show("Gradivo je bilo vrnjeno");

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message); ;
                        }
                    }
                }
            }
            Form1_Load(null, null);
        }
    }
}

