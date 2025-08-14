using System;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using System.Data;

public class InventoryManagementForm : Form
{
    private DataGridView dataGridView;
    private Button addButton, updateButton, deleteButton;

    public InventoryManagementForm()
    {
        Text = "Inventory Management";
        Size = new System.Drawing.Size(600, 400);
        StartPosition = FormStartPosition.CenterScreen;

        this.BackColor = InventoryManagementSystem.ThemeColors.Background;
        this.ForeColor = InventoryManagementSystem.ThemeColors.Foreground;

        this.FormBorderStyle = FormBorderStyle.FixedDialog;

        dataGridView = new DataGridView { Dock = DockStyle.Top, Height = 300 };
        addButton = new Button { Text = "Add", Location = new System.Drawing.Point(178, 320) };
        updateButton = new Button { Text = "Update", Location = new System.Drawing.Point(263, 320) };
        deleteButton = new Button { Text = "Delete", Location = new System.Drawing.Point(348, 320) };

        addButton.Click += AddButton_Click;
        updateButton.Click += UpdateButton_Click;
        deleteButton.Click += DeleteButton_Click;

        dataGridView.BorderStyle = BorderStyle.None;
        dataGridView.GridColor = InventoryManagementSystem.ThemeColors.ControlBackground;
        dataGridView.DefaultCellStyle.SelectionBackColor = InventoryManagementSystem.ThemeColors.Accent;
        dataGridView.DefaultCellStyle.SelectionForeColor = InventoryManagementSystem.ThemeColors.ControlForeground;
        dataGridView.ColumnHeadersDefaultCellStyle.BackColor = InventoryManagementSystem.ThemeColors.ControlBackground;
        dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = InventoryManagementSystem.ThemeColors.ControlForeground;
        dataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
        dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor = InventoryManagementSystem.ThemeColors.ControlBackground;
        dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor = InventoryManagementSystem.ThemeColors.ControlForeground;
        dataGridView.RowHeadersDefaultCellStyle.BackColor = InventoryManagementSystem.ThemeColors.ControlBackground;
        dataGridView.RowHeadersDefaultCellStyle.ForeColor = InventoryManagementSystem.ThemeColors.ControlForeground;
        dataGridView.RowHeadersDefaultCellStyle.SelectionBackColor = InventoryManagementSystem.ThemeColors.ControlBackground;
        dataGridView.RowHeadersDefaultCellStyle.SelectionForeColor = InventoryManagementSystem.ThemeColors.ControlForeground;
        dataGridView.AlternatingRowsDefaultCellStyle.BackColor = InventoryManagementSystem.ThemeColors.ControlBackground;
        dataGridView.AlternatingRowsDefaultCellStyle.ForeColor = InventoryManagementSystem.ThemeColors.ControlForeground;

        addButton.BackColor = InventoryManagementSystem.ThemeColors.Accent;
        addButton.ForeColor = InventoryManagementSystem.ThemeColors.ControlForeground;
        addButton.FlatStyle = FlatStyle.Flat;
        addButton.FlatAppearance.BorderColor = InventoryManagementSystem.ThemeColors.Accent;

        updateButton.BackColor = InventoryManagementSystem.ThemeColors.Accent;
        updateButton.ForeColor = InventoryManagementSystem.ThemeColors.ControlForeground;
        updateButton.FlatStyle = FlatStyle.Flat;
        updateButton.FlatAppearance.BorderColor = InventoryManagementSystem.ThemeColors.Accent;

        deleteButton.BackColor = InventoryManagementSystem.ThemeColors.Accent;
        deleteButton.ForeColor = InventoryManagementSystem.ThemeColors.ControlForeground;
        deleteButton.FlatStyle = FlatStyle.Flat;
        deleteButton.FlatAppearance.BorderColor = InventoryManagementSystem.ThemeColors.Accent;

        this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

        addButton.MouseEnter += (s, e) => addButton.BackColor = InventoryManagementSystem.ThemeColors.ButtonHover;
        addButton.MouseLeave += (s, e) => addButton.BackColor = InventoryManagementSystem.ThemeColors.Accent;

        updateButton.MouseEnter += (s, e) => updateButton.BackColor = InventoryManagementSystem.ThemeColors.ButtonHover;
        updateButton.MouseLeave += (s, e) => updateButton.BackColor = InventoryManagementSystem.ThemeColors.Accent;

        deleteButton.MouseEnter += (s, e) => deleteButton.BackColor = InventoryManagementSystem.ThemeColors.ButtonHover;
        deleteButton.MouseLeave += (s, e) => deleteButton.BackColor = InventoryManagementSystem.ThemeColors.Accent;

        Controls.Add(dataGridView);
        Controls.Add(addButton);
        Controls.Add(updateButton);
        Controls.Add(deleteButton);

        LoadProducts();
    }

    private void LoadProducts()
    {
        try
        {
            using (var connection = new SqliteConnection("Data Source=inventory.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Name, Quantity, Price FROM Products";
                using (var reader = command.ExecuteReader())
                {
                    var table = new DataTable();
                    table.Load(reader);
                    dataGridView.DataSource = table;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load products: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}", "Error Loading Products", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void AddButton_Click(object? sender, EventArgs e)
    {
        using (var form = new ProductForm("Add Product"))
        {
            if (form.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var connection = new SqliteConnection("Data Source=inventory.db"))
                    {
                        connection.Open();
                        var command = connection.CreateCommand();
                        command.CommandText = "INSERT INTO Products (Name, Quantity, Price) VALUES ($name, $quantity, $price)";
                        command.Parameters.AddWithValue("$name", form.nameTextBox.Text);
                        command.Parameters.AddWithValue("$quantity", int.Parse(form.quantityTextBox.Text));
                        command.Parameters.AddWithValue("$price", decimal.Parse(form.priceTextBox.Text));
                        command.ExecuteNonQuery();
                    }
                    LoadProducts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to add product: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}", "Error Adding Product", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    private void UpdateButton_Click(object? sender, EventArgs e)
    {
        if (dataGridView.SelectedRows.Count > 0)
        {
            var selectedRow = dataGridView.SelectedRows[0];
            if (selectedRow.Cells["Id"].Value == DBNull.Value || selectedRow.Cells["Id"].Value == null)
            {
                MessageBox.Show("Selected row does not have a valid ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int productId = Convert.ToInt32(selectedRow.Cells["Id"].Value);

            using (var form = new ProductForm("Edit Product"))
            {
                form.nameTextBox.Text = Convert.ToString(selectedRow.Cells["Name"].Value);
                form.quantityTextBox.Text = Convert.ToString(selectedRow.Cells["Quantity"].Value);
                form.priceTextBox.Text = Convert.ToString(selectedRow.Cells["Price"].Value);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var connection = new SqliteConnection("Data Source=inventory.db"))
                        {
                            connection.Open();
                            var command = connection.CreateCommand();
                            command.CommandText = "UPDATE Products SET Name = $name, Quantity = $quantity, Price = $price WHERE Id = $id";
                            command.Parameters.AddWithValue("$name", form.nameTextBox.Text);
                            command.Parameters.AddWithValue("$quantity", int.Parse(form.quantityTextBox.Text));
                            command.Parameters.AddWithValue("$price", decimal.Parse(form.priceTextBox.Text));
                            command.Parameters.AddWithValue("$id", productId);
                            command.ExecuteNonQuery();
                        }
                        LoadProducts();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to update product: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}", "Error Updating Product", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        else
        {
            MessageBox.Show("Please select a product to update.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void DeleteButton_Click(object? sender, EventArgs e)
    {
        if (dataGridView.SelectedRows.Count > 0)
        {
            if (MessageBox.Show("Are you sure you want to delete this product?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    if (dataGridView.SelectedRows[0].Cells["Id"].Value == DBNull.Value || dataGridView.SelectedRows[0].Cells["Id"].Value == null)
                    {
                        MessageBox.Show("Selected row does not have a valid ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    int productId = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["Id"].Value);
                    using (var connection = new SqliteConnection("Data Source=inventory.db"))
                    {
                        connection.Open();
                        var command = connection.CreateCommand();
                        command.CommandText = "DELETE FROM Products WHERE Id = $id";
                        command.Parameters.AddWithValue("$id", productId);
                        command.ExecuteNonQuery();
                    }
                    LoadProducts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to delete product: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}", "Error Deleting Product", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        else
        {
            MessageBox.Show("Please select a product to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}