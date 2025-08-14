using System.Windows.Forms;

public class CustomerForm : Form
{
    public TextBox nameTextBox, emailTextBox, phoneTextBox;
    private Button saveButton, cancelButton;

    public CustomerForm(string title = "Add Customer")
    {
        Text = title;
        Size = new System.Drawing.Size(350, 300);
        StartPosition = FormStartPosition.CenterScreen;
        MaximizeBox = false;

        this.BackColor = InventoryManagementSystem.ThemeColors.Background;
        this.ForeColor = InventoryManagementSystem.ThemeColors.Foreground;

        this.FormBorderStyle = FormBorderStyle.None;
        this.Padding = new Padding(1);

        Panel container = new Panel { Dock = DockStyle.Fill };
        Controls.Add(container);

        Label nameLabel = new Label { Text = "Name:", Width = 250, Dock = DockStyle.Top, TextAlign = System.Drawing.ContentAlignment.MiddleLeft };
        nameTextBox = new TextBox { Width = 250, Dock = DockStyle.Top };
        Label emailLabel = new Label { Text = "Email:", Width = 250, Dock = DockStyle.Top, TextAlign = System.Drawing.ContentAlignment.MiddleLeft };
        emailTextBox = new TextBox { Width = 250, Dock = DockStyle.Top };
        Label phoneLabel = new Label { Text = "Phone:", Width = 250, Dock = DockStyle.Top, TextAlign = System.Drawing.ContentAlignment.MiddleLeft };
        phoneTextBox = new TextBox { Width = 250, Dock = DockStyle.Top };
        saveButton = new Button { Text = "Save", Width = 90, DialogResult = DialogResult.OK };
        cancelButton = new Button { Text = "Cancel", Width = 90, DialogResult = DialogResult.Cancel };

        Panel buttonPanel = new Panel { Height = 30, Dock = DockStyle.None, Location = new System.Drawing.Point(40, 185) };
        buttonPanel.Controls.Add(saveButton);
        buttonPanel.Controls.Add(cancelButton);

        saveButton.Location = new System.Drawing.Point(15, 0);
        cancelButton.Location = new System.Drawing.Point(115, 0);

        Panel inputPanel = new Panel { AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Location = new System.Drawing.Point(10, 15) };
        inputPanel.Controls.Add(nameLabel);
        inputPanel.Controls.Add(nameTextBox);
        inputPanel.Controls.Add(emailLabel);
        inputPanel.Controls.Add(emailTextBox);
        inputPanel.Controls.Add(phoneLabel);
        inputPanel.Controls.Add(phoneTextBox);

        container.Controls.Add(inputPanel);
        container.Controls.Add(buttonPanel);

        container.Padding = new Padding(40);
        nameTextBox.Margin = new Padding(0, 5, 0, 15);
        emailTextBox.Margin = new Padding(0, 5, 0, 15);
        phoneTextBox.Margin = new Padding(0, 5, 0, 25);

        this.Paint += (s, e) =>
        {
            e.Graphics.DrawRectangle(new System.Drawing.Pen(InventoryManagementSystem.ThemeColors.Accent, 1), this.ClientRectangle);
        };

        nameTextBox.BackColor = InventoryManagementSystem.ThemeColors.ControlBackground;
        nameTextBox.ForeColor = InventoryManagementSystem.ThemeColors.ControlForeground;
        emailTextBox.BackColor = InventoryManagementSystem.ThemeColors.ControlBackground;
        emailTextBox.ForeColor = InventoryManagementSystem.ThemeColors.ControlForeground;
        phoneTextBox.BackColor = InventoryManagementSystem.ThemeColors.ControlBackground;
        phoneTextBox.ForeColor = InventoryManagementSystem.ThemeColors.ControlForeground;

        saveButton.BackColor = InventoryManagementSystem.ThemeColors.Accent;
        saveButton.ForeColor = InventoryManagementSystem.ThemeColors.ControlForeground;
        saveButton.FlatStyle = FlatStyle.Flat;
        saveButton.FlatAppearance.BorderColor = InventoryManagementSystem.ThemeColors.Accent;

        cancelButton.BackColor = InventoryManagementSystem.ThemeColors.Accent;
        cancelButton.ForeColor = InventoryManagementSystem.ThemeColors.ControlForeground;
        cancelButton.FlatStyle = FlatStyle.Flat;
        cancelButton.FlatAppearance.BorderColor = InventoryManagementSystem.ThemeColors.Accent;

        this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

        saveButton.MouseEnter += (s, e) => saveButton.BackColor = InventoryManagementSystem.ThemeColors.ButtonHover;
        saveButton.MouseLeave += (s, e) => saveButton.BackColor = InventoryManagementSystem.ThemeColors.Accent;

        cancelButton.MouseEnter += (s, e) => cancelButton.BackColor = InventoryManagementSystem.ThemeColors.ButtonHover;
        cancelButton.MouseLeave += (s, e) => cancelButton.BackColor = InventoryManagementSystem.ThemeColors.Accent;
    }
}