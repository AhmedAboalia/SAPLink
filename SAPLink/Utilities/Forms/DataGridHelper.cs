using System.Text.Json;
using SAPLink.Core.Models.Prism.StockManagement;
using SAPLink.Core.Models.SAP.Sales;
using InventoryPosting = SAPLink.Core.Models.Prism.StockManagement.InventoryPosting;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SAPLink.Utilities.Forms
{
    public static class DataGridHelper
    {
        public static void BindInvoices(this Guna2DataGridView gridView, ref BindingList<SAPInvoice>? bindingList, SAPInvoice invoice)
        {
            if (bindingList == null)
            {
                bindingList = new BindingList<SAPInvoice>();
                var source = new BindingSource(bindingList, null);
                gridView.DataSource = source;

                // Add the count column as the first column.
                DataGridViewTextBoxColumn countColumn = new DataGridViewTextBoxColumn();
                countColumn.Name = "#";
                countColumn.HeaderText = "#";
                gridView.Columns.Insert(0, countColumn); // Insert the count column at the first position.
            }

            if (!bindingList.Contains(invoice))
            {
                bindingList.Add(invoice);
                //gridView.Refresh(); // Refresh the grid after adding the new row

                // Set the count value.
                gridView.Rows[gridView.Rows.Count - 1].Cells["#"].Value = gridView.Rows.Count.ToString();
            }
        }
        public static void BindInvoices(this TreeView treeView, ref BindingList<SAPInvoice>? bindingList, SAPInvoice invoice)
        {
            //if (bindingList == null)
            //{
            //    bindingList = new BindingList<Core.Models.SAP.Sales.Invoice>();
            //}

            //if (!bindingList.Contains(invoice))
            //{
            //bindingList.Add(invoice);
            treeView.Nodes.Clear();

            TreeNode invoiceNode = new TreeNode($"Invoice ({invoice.DocNum}) Customer ({invoice.CardCode}: {invoice.CardName}) Doc. Total ({invoice.DocTotal})");
            treeView.Nodes.Add(invoiceNode);

            //Add DocumentLines
            foreach (DocumentLine line in invoice.DocumentLines)
            {
                TreeNode lineNode = new TreeNode($"Line - Item Code: {line.ItemCode} - Quantity: {line.Quantity} - Unit Price: {line.UnitPrice}");
                invoiceNode.Nodes.Add(lineNode);

                // Add Quantity, UnitPrice details under lineNode
                TreeNode quantityNode = new TreeNode($"Quantity: {line.Quantity}");
                TreeNode unitPriceNode = new TreeNode($"Unit Price: {line.UnitPrice}");
                lineNode.Nodes.Add(quantityNode);
                lineNode.Nodes.Add(unitPriceNode);
            }

            //Add DocumentAdditionalExpenses
            foreach (DocumentAdditionalExpense expense in invoice.DocumentAdditionalExpenses)
            {
                TreeNode expenseNode = new TreeNode($"Additional Expenses: {expense.Remarks} Total: ({expense.LineGross}) - Amount: ({expense.LineTotal}) - Tax Total: ({expense.TaxSum})\"");
                invoiceNode.Nodes.Add(expenseNode);

                // Add Net Amount under expenseNode
                TreeNode lineTotalNode = new TreeNode($"Amount: {expense.LineTotal}");
                expenseNode.Nodes.Add(lineTotalNode);


                TreeNode netAmountNode = new TreeNode($"Tax Total: {expense.TaxSum}");
                expenseNode.Nodes.Add(netAmountNode);
            }
            //}
        }

        public static void BindVerifiedVouchers(this Guna2DataGridView gridView, ref BindingList<VerifiedVoucher>? bindingList, VerifiedVoucher verifiedVoucher)
        {
            if (bindingList == null)
            {
                bindingList = new BindingList<VerifiedVoucher>();
                var source = new BindingSource(bindingList, null);
                gridView.DataSource = source;

                // Add the count column as the first column.
                DataGridViewTextBoxColumn countColumn = new DataGridViewTextBoxColumn();
                countColumn.Name = "#";
                countColumn.HeaderText = "#";
                gridView.Columns.Insert(0, countColumn); // Insert the count column at the first position.
            }

            if (!bindingList.Contains(verifiedVoucher))
            {
                bindingList.Add(verifiedVoucher);
                //gridView.Refresh(); // Refresh the grid after adding the new row

                // Set the count value.
                gridView.Rows[gridView.Rows.Count - 1].Cells["#"].Value = gridView.Rows.Count.ToString();
            }
        }
        public static void BindVerifiedVouchers(this TreeView treeView, ref BindingList<VerifiedVoucher>? bindingList, VerifiedVoucher verifiedVoucher)
        {
            treeView.Nodes.Clear();

            TreeNode invoiceNode = new TreeNode($"Stock Transfer From Whs. : {verifiedVoucher.Origstorename} To Whs: {verifiedVoucher.Storename}");
            treeView.Nodes.Add(invoiceNode);

            //Add DocumentLines
            foreach (Recvitem line in verifiedVoucher.Recvitem)
            {
                TreeNode lineNode = new TreeNode($"Line - Item Code: {line.Alu} - Item Name: {line.Description1} - {line.Description2} - Qty: {line.Qty}");
                invoiceNode.Nodes.Add(lineNode);
            }
        }


        public static void BindInventoryPosting(this Guna2DataGridView gridView, ref BindingList<InventoryPosting>? bindingList, InventoryPosting inventoryCounting)
        {
            if (bindingList == null)
            {
                bindingList = new BindingList<InventoryPosting>();
                var source = new BindingSource(bindingList, null);
                gridView.DataSource = source;

                // Add the count column as the first column.
                DataGridViewTextBoxColumn countColumn = new DataGridViewTextBoxColumn();
                countColumn.Name = "#";
                countColumn.HeaderText = "#";
                gridView.Columns.Insert(0, countColumn); // Insert the count column at the first position.
            }

            if (!bindingList.Contains(inventoryCounting))
            {
                bindingList.Add(inventoryCounting);
                //gridView.Refresh(); // Refresh the grid after adding the new row

                // Set the count value.
                gridView.Rows[gridView.Rows.Count - 1].Cells["#"].Value = gridView.Rows.Count.ToString();
            }
        }
        public static void BindInventoryCounting(this TreeView treeView, ref BindingList<InventoryPosting>? bindingList, InventoryPosting count)
        {
            //if (bindingList == null)
            //{
            //    bindingList = new BindingList<Core.Models.SAP.Sales.Invoice>();
            //}

            //if (!bindingList.Contains(invoice))
            //{
            //bindingList.Add(invoice);
            treeView.Nodes.Clear();

            TreeNode invoiceNode = new TreeNode($"Adjustment No. : {count.Adjno} - Adjustment Sid: {count.Sid}");
            treeView.Nodes.Add(invoiceNode);

            //Add DocumentLines
            foreach (Adjitem line in count.Adjitem)
            {
                TreeNode lineNode = new TreeNode($"Line - Item Code: {line.Alu} - In-Whse Qty on Count Date: {line
                } - Counted Qty: {line.Adjvalue}");
                invoiceNode.Nodes.Add(lineNode);
            }
        }

    }
}
