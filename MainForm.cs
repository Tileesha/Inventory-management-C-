using System;
using System.Windows.Forms;

namespace InventoryManagementSystem;

public class MainForm : Form
{
    private Button inventoryButton;
    private Button customerButton;
    private Button employeeButton;

    public MainForm()
    {
        Text = "Main Menu";
        Size = new System.Drawing.Size(400, 300);
        StartPosition = FormStartPosition.CenterScreen;

        this.BackColor = ThemeColors.Background;
        this.ForeColor = ThemeColors.Foreground;

        this.FormBorderStyle = FormBorderStyle.FixedDialog;

        inventoryButton = new Button { Text = "Manage Inventory", Location = new System.Drawing.Point(50, 50), Width = 300, Height = 50 };
        customerButton = new Button { Text = "Manage Customers", Location = new System.Drawing.Point(50, 110), Width = 300, Height = 50 };
        employeeButton = new Button { Text = "Manage Employees", Location = new System.Drawing.Point(50, 170), Width = 300, Height = 50 };

        inventoryButton.Click += (s, e) => new InventoryManagementForm().ShowDialog();
        customerButton.Click += (s, e) => new CustomerManagementForm().ShowDialog();
        employeeButton.Click += (s, e) => new EmployeeManagementForm().ShowDialog();

        inventoryButton.BackColor = ThemeColors.Accent;
        inventoryButton.ForeColor = ThemeColors.ControlForeground;
        inventoryButton.FlatStyle = FlatStyle.Flat;
        inventoryButton.FlatAppearance.BorderColor = ThemeColors.Accent;

        customerButton.BackColor = ThemeColors.Accent;
        customerButton.ForeColor = ThemeColors.ControlForeground;
        customerButton.FlatStyle = FlatStyle.Flat;
        customerButton.FlatAppearance.BorderColor = ThemeColors.Accent;

        employeeButton.BackColor = ThemeColors.Accent;
        employeeButton.ForeColor = ThemeColors.ControlForeground;
        employeeButton.FlatStyle = FlatStyle.Flat;
        employeeButton.FlatAppearance.BorderColor = ThemeColors.Accent;

        this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

        inventoryButton.MouseEnter += (s, e) => inventoryButton.BackColor = ThemeColors.ButtonHover;
        inventoryButton.MouseLeave += (s, e) => inventoryButton.BackColor = ThemeColors.Accent;

        customerButton.MouseEnter += (s, e) => customerButton.BackColor = ThemeColors.ButtonHover;
        customerButton.MouseLeave += (s, e) => customerButton.BackColor = ThemeColors.Accent;

        employeeButton.MouseEnter += (s, e) => employeeButton.BackColor = ThemeColors.ButtonHover;
        employeeButton.MouseLeave += (s, e) => employeeButton.BackColor = ThemeColors.Accent;

        Controls.Add(inventoryButton);
        Controls.Add(customerButton);
        Controls.Add(employeeButton);
    }
}