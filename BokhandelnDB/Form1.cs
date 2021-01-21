using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace BokhandelnDB
{
    public partial class Form1 : Form
    {
        private List<Böcker> böcker;
        //db = new BokhandelContext();
        private readonly BokhandelContext db;
        private Butiker activeButik = null;
        public Form1()
        {
            InitializeComponent();
            db = new BokhandelContext();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            if (db.Database.CanConnect())
            {
                böcker = db.Böckers.ToList();

                var butiker = db.Butikers
                    .Include(l => l.LagerSaldos)
                    .ThenInclude(i => i.IsbnNavigation)
                    .ThenInclude(f => f.Författare)
                    .ToList();

                foreach (Butiker butik in butiker)
                {
                    TreeNode butikNode = new TreeNode()
                    {
                        Text = $"{butik.Butiksnamn}",
                        Tag = butik

                    };
                    treeView1.Nodes.Add(butikNode);
                }
            }
            else Debug.WriteLine("Conncetion failed!");
        }

        public void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is Butiker butik)
            {
                activeButik = butik;
                
                textBoxButik.Text = butik.Butiksnamn;
                textBoxAdress.Text = butik.Address;

                dataGridView1.Rows.Clear();
                foreach (LagerSaldo lagersaldo in butik.LagerSaldos)
                {
                    int rowIndex = dataGridView1.Rows.Add();

                    dataGridView1.Rows[rowIndex].Tag = butik.LagerSaldos;

                    var comboBoxCell = dataGridView1.Rows[rowIndex].Cells["Titel"] as DataGridViewComboBoxCell;
                    comboBoxCell.ValueType = typeof(Böcker);
                    comboBoxCell.DisplayMember = "Titel";
                    comboBoxCell.ValueMember = "This";
                    foreach (Böcker böcker in böcker)
                    {
                        comboBoxCell.Items.Add(böcker);
                    }
                    
                    dataGridView1.Rows[rowIndex].Cells["Titel"].Value = lagersaldo.IsbnNavigation;
                    dataGridView1.Rows[rowIndex].Cells["ISBN"].Value = lagersaldo.IsbnNavigation.Isbn13;
                    dataGridView1.Rows[rowIndex].Cells["Språk"].Value = lagersaldo.IsbnNavigation.Språk;
                    dataGridView1.Rows[rowIndex].Cells["Pris"].Value = lagersaldo.IsbnNavigation.Pris + " kr";
                    dataGridView1.Rows[rowIndex].Cells["Utgivningsdatum"].Value = lagersaldo.IsbnNavigation.Utgivningsdatum;
                    dataGridView1.Rows[rowIndex].Cells["Författare"].Value = lagersaldo.IsbnNavigation.Författare.Förnamn + " " + lagersaldo.IsbnNavigation.Författare.Efternamn;
                    dataGridView1.Rows[rowIndex].Cells["Vikt"].Value = lagersaldo.IsbnNavigation.ViktGram;
                    dataGridView1.Rows[rowIndex].Cells["Sidor"].Value = lagersaldo.IsbnNavigation.Sidor;


                }

            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];

            if (cell is DataGridViewComboBoxCell comboBox)
            {
                var bok = comboBox.Value as Böcker;

                dataGridView1.Rows[e.RowIndex].Cells["ISBN"].Value = bok.Isbn13;
                dataGridView1.Rows[e.RowIndex].Cells["Språk"].Value = bok.Språk;
                dataGridView1.Rows[e.RowIndex].Cells["Pris"].Value = bok.Pris + " kr";
                dataGridView1.Rows[e.RowIndex].Cells["Utgivningsdatum"].Value = bok.Utgivningsdatum;
                dataGridView1.Rows[e.RowIndex].Cells["Författare"].Value = bok.Författare.Förnamn + " " + bok.Författare.Efternamn;
                dataGridView1.Rows[e.RowIndex].Cells["Vikt"].Value = bok.ViktGram;
                dataGridView1.Rows[e.RowIndex].Cells["Sidor"].Value = bok.Sidor;
                var lager = dataGridView1.Rows[e.RowIndex].Tag as LagerSaldo;


            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                var result = MessageBox.Show("Do you want to delete this row?", "Delete row", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    using (var db = new BokhandelContext())
                        foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
                        {
                            dataGridView1.Rows.RemoveAt(item.Index);
                            var bok = db.LagerSaldos.First();
                            db.LagerSaldos.Remove(bok);
                            db.SaveChanges();

                        }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (activeButik != null)
            {
                var lager = new LagerSaldo
                {
                    ButikId = activeButik.IdentityId,
                    
                    Isbn = 
                    
                   
                };

                activeButik.LagerSaldos.Add(lager);

                int rowIndex = dataGridView1.Rows.Add();
                dataGridView1.Rows[rowIndex].Tag = lager;

                var comboBoxCell = dataGridView1.Rows[rowIndex].Cells["Titel"] as DataGridViewComboBoxCell;
                comboBoxCell.ValueType = typeof(Böcker);
                comboBoxCell.DisplayMember = "Titel";
                comboBoxCell.ValueMember = "This";


                foreach (var item in böcker)
                {
                    comboBoxCell.Items.Add(item);
                }
            }

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            db.SaveChanges();
            db.Dispose();
        }
    }
}

