using System;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using System.Data;

public class CustomerManagementForm : Form
{
    private DataGridView dataGridView;
    private Button addButton, updateButton, deleteButton;

    public CustomerManagementForm()
    {
        Text = "Customer Management";
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

        LoadCustomers();
    }

    private void LoadCustomers()
    {
        try
        {
            using (var connection = new SqliteConnection("Data Source=inventory.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Name, Email, Phone FROM Customers";
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
            MessageBox.Show($"Failed to load customers: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}", "Error Loading Customers", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void AddButton_Click(object? sender, EventArgs e)
    {
        using (var form = new CustomerForm("Add Customer"))
        {
            if (form.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var connection = new SqliteConnection("Data Source=inventory.db"))
                    {
                        connection.Open();
                        var command = connection.CreateCommand();
                        command.CommandText = "INSERT INTO Customers (Name, Email, Phone) VALUES ($name, $email, $phone)";
                        command.Parameters.AddWithValue("$name", form.nameTextBox.Text);
                        command.Parameters.AddWithValue("$email", form.emailTextBox.Text);
                        command.Parameters.AddWithValue("$phone", form.phoneTextBox.Text);
                        command.ExecuteNonQuery();
                    }
                    LoadCustomers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to add customer: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}", "Error Adding Customer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    private void UpdateButton_Click(object? sender, EventArgs e)
    {
        if (dataGridView.SelectedRows.Count > 0)
        {
            var selectedRow = dataGridView.SelectedRows[0];
            // Ensure the 'Id' column exists and contains valid integer data
            if (selectedRow.Cells["Id"].Value == DBNull.Value || selectedRow.Cells["Id"].Value == null)
            {
                MessageBox.Show("Selected row does not have a valid ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int customerId = Convert.ToInt32(selectedRow.Cells["Id"].Value);

            using (var form = new CustomerForm("Edit Customer"))
            {
                form.nameTextBox.Text = Convert.ToString(selectedRow.Cells["Name"].Value);
                form.emailTextBox.Text = Convert.ToString(selectedRow.Cells["Email"].Value);
                form.phoneTextBox.Text = Convert.ToString(selectedRow.Cells["Phone"].Value);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var connection = new SqliteConnection("Data Source=inventory.db"))
                        {
                            connection.Open();
                            var command = connection.CreateCommand();
                            command.CommandText = "UPDATE Customers SET Name = $name, Email = $email, Phone = $phone WHERE Id = $id";
                            command.Parameters.AddWithValue("$name", form.nameTextBox.Text);
                            command.Parameters.AddWithValue("$email", form.emailTextBox.Text);
                            command.Parameters.AddWithValue("$phone", form.phoneTextBox.Text);
                            command.Parameters.AddWithValue("$id", customerId);
                            command.ExecuteNonQuery();
                        }
                        LoadCustomers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to update customer: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}", "Error Updating Customer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        else
        {
            MessageBox.Show("Please select a customer to update.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void DeleteButton_Click(object? sender, EventArgs e)
    {
        if (dataGridView.SelectedRows.Count > 0)
        {
            if (MessageBox.Show("Are you sure you want to delete this customer?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    // Ensure the 'Id' column exists and contains valid integer data
                    if (dataGridView.SelectedRows[0].Cells["Id"].Value == DBNull.Value || dataGridView.SelectedRows[0].Cells["Id"].Value == null)
                    {
                        MessageBox.Show("Selected row does not have a valid ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    int customerId = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["Id"].Value);
                    using (var connection = new SqliteConnection("Data Source=inventory.db"))
                    {
                        connection.Open();
                        var command = connection.CreateCommand();
                        command.CommandText = "DELETE FROM Customers WHERE Id = $id";
                        command.Parameters.AddWithValue("$id", customerId);
                        command.ExecuteNonQuery();
                    }
                    LoadCustomers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to delete customer: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}", "Error Deleting Customer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        else
        {
            MessageBox.Show("Please select a customer to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}