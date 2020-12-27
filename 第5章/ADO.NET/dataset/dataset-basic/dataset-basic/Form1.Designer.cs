namespace dataset_basic
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.adventureWorksDataSet = new dataset_basic.AdventureWorksDataSet();
            this.productBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.productTableAdapter = new dataset_basic.AdventureWorksDataSetTableAdapters.ProductTableAdapter();
            this.productIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.makeFlagDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.finishedGoodsFlagDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.safetyStockLevelDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reorderPointDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.standardCostDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.listPriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizeUnitMeasureCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.weightUnitMeasureCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.weightDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.daysToManufactureDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productLineDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.classDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.styleDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productSubcategoryIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productModelIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sellStartDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sellEndDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.discontinuedDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rowguidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modifiedDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.adventureWorksDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.productIDDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.productNumberDataGridViewTextBoxColumn,
            this.makeFlagDataGridViewCheckBoxColumn,
            this.finishedGoodsFlagDataGridViewCheckBoxColumn,
            this.colorDataGridViewTextBoxColumn,
            this.safetyStockLevelDataGridViewTextBoxColumn,
            this.reorderPointDataGridViewTextBoxColumn,
            this.standardCostDataGridViewTextBoxColumn,
            this.listPriceDataGridViewTextBoxColumn,
            this.sizeDataGridViewTextBoxColumn,
            this.sizeUnitMeasureCodeDataGridViewTextBoxColumn,
            this.weightUnitMeasureCodeDataGridViewTextBoxColumn,
            this.weightDataGridViewTextBoxColumn,
            this.daysToManufactureDataGridViewTextBoxColumn,
            this.productLineDataGridViewTextBoxColumn,
            this.classDataGridViewTextBoxColumn,
            this.styleDataGridViewTextBoxColumn,
            this.productSubcategoryIDDataGridViewTextBoxColumn,
            this.productModelIDDataGridViewTextBoxColumn,
            this.sellStartDateDataGridViewTextBoxColumn,
            this.sellEndDateDataGridViewTextBoxColumn,
            this.discontinuedDateDataGridViewTextBoxColumn,
            this.rowguidDataGridViewTextBoxColumn,
            this.modifiedDateDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.productBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(800, 450);
            this.dataGridView1.TabIndex = 0;
            // 
            // bindingSource1
            // 
            this.bindingSource1.AllowNew = true;
            this.bindingSource1.DataSource = this.adventureWorksDataSet;
            this.bindingSource1.Position = 0;
            // 
            // adventureWorksDataSet
            // 
            this.adventureWorksDataSet.DataSetName = "AdventureWorksDataSet";
            this.adventureWorksDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // productBindingSource
            // 
            this.productBindingSource.DataMember = "Product";
            this.productBindingSource.DataSource = this.bindingSource1;
            // 
            // productTableAdapter
            // 
            this.productTableAdapter.ClearBeforeFill = true;
            // 
            // productIDDataGridViewTextBoxColumn
            // 
            this.productIDDataGridViewTextBoxColumn.DataPropertyName = "ProductID";
            this.productIDDataGridViewTextBoxColumn.HeaderText = "ProductID";
            this.productIDDataGridViewTextBoxColumn.Name = "productIDDataGridViewTextBoxColumn";
            this.productIDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            // 
            // productNumberDataGridViewTextBoxColumn
            // 
            this.productNumberDataGridViewTextBoxColumn.DataPropertyName = "ProductNumber";
            this.productNumberDataGridViewTextBoxColumn.HeaderText = "ProductNumber";
            this.productNumberDataGridViewTextBoxColumn.Name = "productNumberDataGridViewTextBoxColumn";
            // 
            // makeFlagDataGridViewCheckBoxColumn
            // 
            this.makeFlagDataGridViewCheckBoxColumn.DataPropertyName = "MakeFlag";
            this.makeFlagDataGridViewCheckBoxColumn.HeaderText = "MakeFlag";
            this.makeFlagDataGridViewCheckBoxColumn.Name = "makeFlagDataGridViewCheckBoxColumn";
            // 
            // finishedGoodsFlagDataGridViewCheckBoxColumn
            // 
            this.finishedGoodsFlagDataGridViewCheckBoxColumn.DataPropertyName = "FinishedGoodsFlag";
            this.finishedGoodsFlagDataGridViewCheckBoxColumn.HeaderText = "FinishedGoodsFlag";
            this.finishedGoodsFlagDataGridViewCheckBoxColumn.Name = "finishedGoodsFlagDataGridViewCheckBoxColumn";
            // 
            // colorDataGridViewTextBoxColumn
            // 
            this.colorDataGridViewTextBoxColumn.DataPropertyName = "Color";
            this.colorDataGridViewTextBoxColumn.HeaderText = "Color";
            this.colorDataGridViewTextBoxColumn.Name = "colorDataGridViewTextBoxColumn";
            // 
            // safetyStockLevelDataGridViewTextBoxColumn
            // 
            this.safetyStockLevelDataGridViewTextBoxColumn.DataPropertyName = "SafetyStockLevel";
            this.safetyStockLevelDataGridViewTextBoxColumn.HeaderText = "SafetyStockLevel";
            this.safetyStockLevelDataGridViewTextBoxColumn.Name = "safetyStockLevelDataGridViewTextBoxColumn";
            // 
            // reorderPointDataGridViewTextBoxColumn
            // 
            this.reorderPointDataGridViewTextBoxColumn.DataPropertyName = "ReorderPoint";
            this.reorderPointDataGridViewTextBoxColumn.HeaderText = "ReorderPoint";
            this.reorderPointDataGridViewTextBoxColumn.Name = "reorderPointDataGridViewTextBoxColumn";
            // 
            // standardCostDataGridViewTextBoxColumn
            // 
            this.standardCostDataGridViewTextBoxColumn.DataPropertyName = "StandardCost";
            this.standardCostDataGridViewTextBoxColumn.HeaderText = "StandardCost";
            this.standardCostDataGridViewTextBoxColumn.Name = "standardCostDataGridViewTextBoxColumn";
            // 
            // listPriceDataGridViewTextBoxColumn
            // 
            this.listPriceDataGridViewTextBoxColumn.DataPropertyName = "ListPrice";
            this.listPriceDataGridViewTextBoxColumn.HeaderText = "ListPrice";
            this.listPriceDataGridViewTextBoxColumn.Name = "listPriceDataGridViewTextBoxColumn";
            // 
            // sizeDataGridViewTextBoxColumn
            // 
            this.sizeDataGridViewTextBoxColumn.DataPropertyName = "Size";
            this.sizeDataGridViewTextBoxColumn.HeaderText = "Size";
            this.sizeDataGridViewTextBoxColumn.Name = "sizeDataGridViewTextBoxColumn";
            // 
            // sizeUnitMeasureCodeDataGridViewTextBoxColumn
            // 
            this.sizeUnitMeasureCodeDataGridViewTextBoxColumn.DataPropertyName = "SizeUnitMeasureCode";
            this.sizeUnitMeasureCodeDataGridViewTextBoxColumn.HeaderText = "SizeUnitMeasureCode";
            this.sizeUnitMeasureCodeDataGridViewTextBoxColumn.Name = "sizeUnitMeasureCodeDataGridViewTextBoxColumn";
            // 
            // weightUnitMeasureCodeDataGridViewTextBoxColumn
            // 
            this.weightUnitMeasureCodeDataGridViewTextBoxColumn.DataPropertyName = "WeightUnitMeasureCode";
            this.weightUnitMeasureCodeDataGridViewTextBoxColumn.HeaderText = "WeightUnitMeasureCode";
            this.weightUnitMeasureCodeDataGridViewTextBoxColumn.Name = "weightUnitMeasureCodeDataGridViewTextBoxColumn";
            // 
            // weightDataGridViewTextBoxColumn
            // 
            this.weightDataGridViewTextBoxColumn.DataPropertyName = "Weight";
            this.weightDataGridViewTextBoxColumn.HeaderText = "Weight";
            this.weightDataGridViewTextBoxColumn.Name = "weightDataGridViewTextBoxColumn";
            // 
            // daysToManufactureDataGridViewTextBoxColumn
            // 
            this.daysToManufactureDataGridViewTextBoxColumn.DataPropertyName = "DaysToManufacture";
            this.daysToManufactureDataGridViewTextBoxColumn.HeaderText = "DaysToManufacture";
            this.daysToManufactureDataGridViewTextBoxColumn.Name = "daysToManufactureDataGridViewTextBoxColumn";
            // 
            // productLineDataGridViewTextBoxColumn
            // 
            this.productLineDataGridViewTextBoxColumn.DataPropertyName = "ProductLine";
            this.productLineDataGridViewTextBoxColumn.HeaderText = "ProductLine";
            this.productLineDataGridViewTextBoxColumn.Name = "productLineDataGridViewTextBoxColumn";
            // 
            // classDataGridViewTextBoxColumn
            // 
            this.classDataGridViewTextBoxColumn.DataPropertyName = "Class";
            this.classDataGridViewTextBoxColumn.HeaderText = "Class";
            this.classDataGridViewTextBoxColumn.Name = "classDataGridViewTextBoxColumn";
            // 
            // styleDataGridViewTextBoxColumn
            // 
            this.styleDataGridViewTextBoxColumn.DataPropertyName = "Style";
            this.styleDataGridViewTextBoxColumn.HeaderText = "Style";
            this.styleDataGridViewTextBoxColumn.Name = "styleDataGridViewTextBoxColumn";
            // 
            // productSubcategoryIDDataGridViewTextBoxColumn
            // 
            this.productSubcategoryIDDataGridViewTextBoxColumn.DataPropertyName = "ProductSubcategoryID";
            this.productSubcategoryIDDataGridViewTextBoxColumn.HeaderText = "ProductSubcategoryID";
            this.productSubcategoryIDDataGridViewTextBoxColumn.Name = "productSubcategoryIDDataGridViewTextBoxColumn";
            // 
            // productModelIDDataGridViewTextBoxColumn
            // 
            this.productModelIDDataGridViewTextBoxColumn.DataPropertyName = "ProductModelID";
            this.productModelIDDataGridViewTextBoxColumn.HeaderText = "ProductModelID";
            this.productModelIDDataGridViewTextBoxColumn.Name = "productModelIDDataGridViewTextBoxColumn";
            // 
            // sellStartDateDataGridViewTextBoxColumn
            // 
            this.sellStartDateDataGridViewTextBoxColumn.DataPropertyName = "SellStartDate";
            this.sellStartDateDataGridViewTextBoxColumn.HeaderText = "SellStartDate";
            this.sellStartDateDataGridViewTextBoxColumn.Name = "sellStartDateDataGridViewTextBoxColumn";
            // 
            // sellEndDateDataGridViewTextBoxColumn
            // 
            this.sellEndDateDataGridViewTextBoxColumn.DataPropertyName = "SellEndDate";
            this.sellEndDateDataGridViewTextBoxColumn.HeaderText = "SellEndDate";
            this.sellEndDateDataGridViewTextBoxColumn.Name = "sellEndDateDataGridViewTextBoxColumn";
            // 
            // discontinuedDateDataGridViewTextBoxColumn
            // 
            this.discontinuedDateDataGridViewTextBoxColumn.DataPropertyName = "DiscontinuedDate";
            this.discontinuedDateDataGridViewTextBoxColumn.HeaderText = "DiscontinuedDate";
            this.discontinuedDateDataGridViewTextBoxColumn.Name = "discontinuedDateDataGridViewTextBoxColumn";
            // 
            // rowguidDataGridViewTextBoxColumn
            // 
            this.rowguidDataGridViewTextBoxColumn.DataPropertyName = "rowguid";
            this.rowguidDataGridViewTextBoxColumn.HeaderText = "rowguid";
            this.rowguidDataGridViewTextBoxColumn.Name = "rowguidDataGridViewTextBoxColumn";
            // 
            // modifiedDateDataGridViewTextBoxColumn
            // 
            this.modifiedDateDataGridViewTextBoxColumn.DataPropertyName = "ModifiedDate";
            this.modifiedDateDataGridViewTextBoxColumn.HeaderText = "ModifiedDate";
            this.modifiedDateDataGridViewTextBoxColumn.Name = "modifiedDateDataGridViewTextBoxColumn";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.adventureWorksDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource bindingSource1;
        private AdventureWorksDataSet adventureWorksDataSet;
        private System.Windows.Forms.BindingSource productBindingSource;
        private AdventureWorksDataSetTableAdapters.ProductTableAdapter productTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn productIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn makeFlagDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn finishedGoodsFlagDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn safetyStockLevelDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn reorderPointDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn standardCostDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn listPriceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizeUnitMeasureCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn weightUnitMeasureCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn weightDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn daysToManufactureDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productLineDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn classDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn styleDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productSubcategoryIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productModelIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sellStartDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sellEndDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn discontinuedDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rowguidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modifiedDateDataGridViewTextBoxColumn;
    }
}

