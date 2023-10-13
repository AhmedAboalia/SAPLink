using System.Text.Json;
using SAPLink.Core.Models.SAP.Sales;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SAPLink.Utilities.Forms
{
    public static class InboundData
    {
        public static void BindDepartments(this Guna2DataGridView gridView, ref BindingList<ItemGroups>? bindingList, ItemGroups department)
        {
            if (bindingList == null)
            {
                bindingList = new BindingList<ItemGroups>();
                var source = new BindingSource(bindingList, null);
                gridView.DataSource = source;

                // Add the count column as the first column.
                DataGridViewTextBoxColumn countColumn = new DataGridViewTextBoxColumn();
                countColumn.Name = "#";
                countColumn.HeaderText = "#";
                gridView.Columns.Insert(0, countColumn); // Insert the count column at the first position.
            }

            if (!bindingList.Contains(department))
            {
                bindingList.Add(department);
                //gridView.Refresh(); // Refresh the grid after adding the new row

                // Set the count value.
                gridView.Rows[gridView.Rows.Count - 1].Cells["#"].Value = gridView.Rows.Count.ToString();
            }
        }
        public static void BindVendors(this Guna2DataGridView gridView, ref BindingList<BusinessPartner>? bindingList, BusinessPartner vendor)
        {
            if (bindingList == null)
            {
                bindingList = new BindingList<BusinessPartner>();
                var source = new BindingSource(bindingList, null);
                gridView.DataSource = source;

                // Add the count column as the first column.
                DataGridViewTextBoxColumn countColumn = new DataGridViewTextBoxColumn();
                countColumn.Name = "#";
                countColumn.HeaderText = "#";
                gridView.Columns.Insert(0, countColumn); // Insert the count column at the first position.
            }

            if (!bindingList.Contains(vendor))
            {
                bindingList.Add(vendor);
                //gridView.Refresh(); // Refresh the grid after adding the new row

                // Set the count value.
                gridView.Rows[gridView.Rows.Count - 1].Cells["#"].Value = gridView.Rows.Count.ToString();
            }
        }
        public static void BindItems(this Guna2DataGridView gridView, ref BindingList<ItemMasterData>? bindingList, ItemMasterData item)
        {
            if (bindingList == null)
            {
                bindingList = new BindingList<ItemMasterData>();
                var source = new BindingSource(bindingList, null);
                gridView.DataSource = source;

                // Add the count column as the first column.
                DataGridViewTextBoxColumn countColumn = new DataGridViewTextBoxColumn();
                countColumn.Name = "#";
                countColumn.HeaderText = "#";
                gridView.Columns.Insert(0, countColumn); // Insert the count column at the first position.
            }

            if (!bindingList.Contains(item))
            {
                bindingList.Add(item);
                //gridView.Refresh(); // Refresh the grid after adding the new row

                // Set the count value.
                gridView.Rows[gridView.Rows.Count - 1].Cells["#"].Value = gridView.Rows.Count.ToString();
            }
        }

        public static void BindGoodsReceiptPo(this Guna2DataGridView gridView, ref BindingList<Goods>? bindingList, Goods item)
        {
            if (bindingList == null)
            {
                bindingList = new BindingList<Goods>();
                var source = new BindingSource(bindingList, null);
                gridView.DataSource = source;

                // Add the count column as the first column.
                DataGridViewTextBoxColumn countColumn = new DataGridViewTextBoxColumn();
                countColumn.Name = "#";
                countColumn.HeaderText = "#";
                gridView.Columns.Insert(0, countColumn); // Insert the count column at the first position.
            }

            if (!bindingList.Contains(item))
            {
                bindingList.Add(item);
                //gridView.Refresh(); // Refresh the grid after adding the new row

                // Set the count value.
                gridView.Rows[gridView.Rows.Count - 1].Cells["#"].Value = gridView.Rows.Count.ToString();
            }
        }
    }
}
