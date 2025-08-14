using System;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using System.Security.Cryptography;
using System.Text;

public class RegistrationForm : Form
{
    private TextBox usernameTextBox;
    private TextBox passwordTextBox;
    private Button registerButton;

    public RegistrationForm()
    {
        Text = "Register";
        Size = new System.Drawing.Size(350, 250);
        StartPosition = FormStartPosition.CenterScreen;
        MaximizeBox = false;

        this.BackColor = InventoryManagementSystem.ThemeColors.Background;
        this.ForeColor = InventoryManagementSystem.ThemeColors.Foreground;

        this.FormBorderStyle = FormBorderStyle.None;
        this.Padding = new Padding(1);

        Panel container = new Panel { Dock = DockStyle.Fill };
        Controls.Add(container);

        Label usernameLabel = new Label { Text = "Username:", Width = 250, Dock = DockStyle.Top, TextAlign = System.Drawing.ContentAlignment.MiddleLeft };
        usernameTextBox = new TextBox { Width = 250, Dock = DockStyle.Top };
        Label passwordLabel = new Label { Text = "Password:", Width = 250, Dock = DockStyle.Top, TextAlign = System.Drawing.ContentAlignment.MiddleLeft };
        passwordTextBox = new TextBox { Width = 250, Dock = DockStyle.Top, PasswordChar = '*' };
        registerButton = new Button { Text = "Register", Width = 100, DialogResult = DialogResult.OK };

        Panel buttonPanel = new Panel { Height = 30, Dock = DockStyle.None, Location = new System.Drawing.Point(60, 130) };
        buttonPanel.Controls.Add(registerButton);

        registerButton.Location = new System.Drawing.Point(60, 0);

        Panel inputPanel = new Panel { AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Location = new System.Drawing.Point(10, 20) };
        inputPanel.Controls.Add(usernameLabel);
        inputPanel.Controls.Add(usernameTextBox);
        inputPanel.Controls.Add(passwordLabel);
        inputPanel.Controls.Add(passwordTextBox);

        container.Controls.Add(inputPanel);
        container.Controls.Add(buttonPanel);

        container.Padding = new Padding(40);
        usernameTextBox.Margin = new Padding(0, 5, 0, 15);
        passwordTextBox.Margin = new Padding(0, 5, 0, 25);
        

        this.Paint += (s, e) =>
        {
            e.Graphics.DrawRectangle(new System.Drawing.Pen(InventoryManagementSystem.ThemeColors.Accent, 1), this.ClientRectangle);
        };

        usernameTextBox.BackColor = InventoryManagementSystem.ThemeColors.ControlBackground;
        usernameTextBox.ForeColor = InventoryManagementSystem.ThemeColors.ControlForeground;
        passwordTextBox.BackColor = InventoryManagementSystem.ThemeColors.ControlBackground;
        passwordTextBox.ForeColor = InventoryManagementSystem.ThemeColors.ControlForeground;

        registerButton.BackColor = InventoryManagementSystem.ThemeColors.Accent;
        registerButton.ForeColor = InventoryManagementSystem.ThemeColors.ControlForeground;
        registerButton.FlatStyle = FlatStyle.Flat;
        registerButton.FlatAppearance.BorderColor = InventoryManagementSystem.ThemeColors.Accent;

        this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

        registerButton.MouseEnter += (s, e) => registerButton.BackColor = InventoryManagementSystem.ThemeColors.ButtonHover;
        registerButton.MouseLeave += (s, e) => registerButton.BackColor = InventoryManagementSystem.ThemeColors.Accent;

        registerButton.Click += RegisterButton_Click;
    }

    private string HashPassword(string password)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

    private void RegisterButton_Click(object? sender, EventArgs e)
    {
        string username = usernameTextBox.Text;
        string password = passwordTextBox.Text;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            MessageBox.Show("Username and password cannot be empty.", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        try
        {
            string hashedPassword = HashPassword(password);

            using (var connection = new SqliteConnection("Data Source=inventory.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Users (Username, PasswordHash) VALUES ($username, $passwordHash)";
                command.Parameters.AddWithValue("$username", username);
                command.Parameters.AddWithValue("$passwordHash", hashedPassword);

                command.ExecuteNonQuery();

                MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Registration failed: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}