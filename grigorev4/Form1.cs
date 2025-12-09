using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using grigorev4.ModelEF;

namespace grigorev4
{
    public partial class Form1 : Form
    {

        Model1 db = new Model1();

        List<Pavilion> pavilionList;
        List<Pavilion> filteredList;

        public Form1()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            db.Pavilion.Load();
            pavilionList = db.Pavilion.Local.ToList();
            filteredList = pavilionList;

            pavilionBindingSource.DataSource = filteredList;

            LoadStatusList();
        }

        private void LoadStatusList()
        {
            var statuses = pavilionList
                .Select(p => p.Status)
            .Distinct()
            .ToList();

            comboBox1.DataSource = statuses;
        }


        private void ApplyFilters()
        {
            filteredList = pavilionList;


            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                string txt = textBox1.Text.ToLower();

                filteredList = filteredList
                    .Where(p =>
                        p.Num_pav.ToLower().Contains(txt) ||
                        p.Floor.ToString().Contains(txt) ||
                        p.Status.ToLower().Contains(txt) ||
                        p.Square.ToString().Contains(txt) ||
                        p.Cost_meter.ToString().Contains(txt)
                    ).ToList();
            }


            if (comboBox1.SelectedItem != null)
            {
                string st = comboBox1.SelectedItem.ToString();
                filteredList = filteredList
                    .Where(p => p.Status == st)
                    .ToList();
            }


            if (checkBox1.Checked)
            {
                filteredList = filteredList
                    .OrderBy(p => p.Status)
                    .ToList();
            }

            pavilionBindingSource.DataSource = filteredList;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
            ApplyFilters();
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void comboBoxStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void checkBoxSort_CheckedChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }
    }
}
