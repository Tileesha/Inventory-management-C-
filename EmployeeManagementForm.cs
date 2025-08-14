using System;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using System.Data;

public class EmployeeManagementForm : Form
{
    private DataGridView dataGridView;
    private Button addButton, updateButton, deleteButton;

    public EmployeeManagementForm()
    {
        Text = "Employee Management";
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

        LoadEmployees();
    }

    private void LoadEmployees()
    {
        try
        {
            using (var connection = new SqliteConnection("Data Source=inventory.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Name, Position, Salary FROM Employees";
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
            MessageBox.Show($"Failed to load employees: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}", "Error Loading Employees", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void AddButton_Click(object? sender, EventArgs e)
    {
        using (var form = new EmployeeForm("Add Employee"))
        {
            if (form.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var connection = new SqliteConnection("Data Source=inventory.db"))
                    {
                        connection.Open();
                        var command = connection.CreateCommand();
                        command.CommandText = "INSERT INTO Employees (Name, Position, Salary) VALUES ($name, $position, $salary)";
                        command.Parameters.AddWithValue("$name", form.nameTextBox.Text);
                        command.Parameters.AddWithValue("$position", form.positionTextBox.Text);
                        command.Parameters.AddWithValue("$salary", decimal.Parse(form.salaryTextBox.Text));
                        command.ExecuteNonQuery();
                    }
                    LoadEmployees();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to add employee: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}", "Error Adding Employee", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            int employeeId = Convert.ToInt32(selectedRow.Cells["Id"].Value);

            using (var form = new EmployeeForm("Edit Employee"))
            {
                form.nameTextBox.Text = Convert.ToString(selectedRow.Cells["Name"].Value);
                form.positionTextBox.Text = Convert.ToString(selectedRow.Cells["Position"].Value);
                form.salaryTextBox.Text = Convert.ToString(selectedRow.Cells["Salary"].Value);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var connection = new SqliteConnection("Data Source=inventory.db"))
                        {
                            connection.Open();
                            var command = connection.CreateCommand();
                            command.CommandText = "UPDATE Employees SET Name = $name, Position = $position, Salary = $salary WHERE Id = $id";
                            command.Parameters.AddWithValue("$name", form.nameTextBox.Text);
                            command.Parameters.AddWithValue("$position", form.positionTextBox.Text);
                            command.Parameters.AddWithValue("$salary", decimal.Parse(form.salaryTextBox.Text));
                            command.Parameters.AddWithValue("$id", employeeId);
                            command.ExecuteNonQuery();
                        }
                        LoadEmployees();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to update employee: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}", "Error Updating Employee", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        else
        {
            MessageBox.Show("Please select an employee to update.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void DeleteButton_Click(object? sender, EventArgs e)
    {
        if (dataGridView.SelectedRows.Count > 0)
        {
            if (MessageBox.Show("Are you sure you want to delete this employee?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    if (dataGridView.SelectedRows[0].Cells["Id"].Value == DBNull.Value || dataGridView.SelectedRows[0].Cells["Id"].Value == null)
                    {
                        MessageBox.Show("Selected row does not have a valid ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    int employeeId = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["Id"].Value);
                    using (var connection = new SqliteConnection("Data Source=inventory.db"))
                    {
                        connection.Open();
                        var command = connection.CreateCommand();
                        command.CommandText = "DELETE FROM Employees WHERE Id = $id";
                        command.Parameters.AddWithValue("$id", employeeId);
                        command.ExecuteNonQuery();
                    }
                    LoadEmployees();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to delete employee: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}", "Error Deleting Employee", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        else
        {
            MessageBox.Show("Please select an employee to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}