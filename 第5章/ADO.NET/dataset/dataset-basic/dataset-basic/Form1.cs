﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dataset_basic
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“adventureWorksDataSet.Product”中。您可以根据需要移动或删除它。
            this.productTableAdapter.Fill(this.adventureWorksDataSet.Product);
            // TODO: 这行代码将数据加载到表“adventureWorksDataSet.Product”中。您可以根据需要移动或删除它。
            this.productTableAdapter.Fill(this.adventureWorksDataSet.Product);

        }
    }
}
