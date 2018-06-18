using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NHibernate;
using Knjiznica_AB.Entity;


namespace Knjiznica_AB
{
    public partial class Zaposleni : Form
    {
        private ISessionFactory m_SessionFactory = null;
        private ISession m_Session = null;
        private IList<Zaposlen> _users;
        private BindingSource _bs;
        public Zaposleni()
        {
            InitializeComponent();
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AllowUserToDeleteRows = true;
        }
        public void SetNhib(ISessionFactory isf, ISession iss)
        {
            m_SessionFactory = isf;
            m_Session = iss;
        }

        private void Zaposleni_Load(object sender, EventArgs e)
        {
            _users = m_Session.CreateCriteria(typeof(Zaposlen)).List<Zaposlen>();
            _bs = new BindingSource();
            _bs.DataSource = _users;
            _bs.AllowNew = true;
            dataGridView1.DataSource = _bs;
            _bs.ListChanged += new System.ComponentModel.ListChangedEventHandler(bs_ListChanged);
            dataGridView1.Columns.Remove("Izposoje");
            dataGridView1.Columns.Remove("Id");
        }
        
        void bs_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemChanged:
                    {
                        using (ITransaction tx = m_Session.BeginTransaction())
                        {
                            Zaposlen new_user = (Zaposlen)(_bs.List[e.NewIndex]);
                            if (new_user.UporabniskoIme == null)
                                new_user.UporabniskoIme = "";
                            if (new_user.Geslo == null)
                                new_user.Geslo = "";
                            m_Session.Save(new_user);
                            tx.Commit();
                        }
                        break;
                    }
            }
        }

        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            using (ITransaction tx = m_Session.BeginTransaction())
            {
                Zaposlen new_user = (Zaposlen)(e.Row.DataBoundItem);
                m_Session.Delete(new_user);
                tx.Commit();
            }
        }

    }
}
