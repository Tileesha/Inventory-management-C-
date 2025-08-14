using System;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using System.Security.Cryptography;
using System.Text;

namespace InventoryManagementSystem;

public partial class LoginForm : Form
{
    private TextBox usernameTextBox;
    private TextBox passwordTextBox;
    private Button loginButton;
    private Button registerButton;

    public LoginForm()
    {
        Text = "Login";
        Size = new System.Drawing.Size(350, 250);
        StartPosition = FormStartPosition.CenterScreen;
        MaximizeBox = false;

        this.BackColor = ThemeColors.Background;
        this.ForeColor = ThemeColors.Foreground;

        this.FormBorderStyle = FormBorderStyle.None;
        this.Padding = new Padding(1);

        Panel container = new Panel { Dock = DockStyle.Fill };
        Controls.Add(container);

        Label usernameLabel = new Label { Text = "Username:", Width = 250, Dock = DockStyle.Top, TextAlign = System.Drawing.ContentAlignment.MiddleLeft };
        usernameTextBox = new TextBox { Width = 250, Dock = DockStyle.Top };
        Label passwordLabel = new Label { Text = "Password:", Width = 250, Dock = DockStyle.Top, TextAlign = System.Drawing.ContentAlignment.MiddleLeft };
        passwordTextBox = new TextBox { Width = 250, Dock = DockStyle.Top, PasswordChar = '*' };
        loginButton = new Button { Text = "Login", Width = 90, DialogResult = DialogResult.OK };
        registerButton = new Button { Text = "Register", Width = 90, DialogResult = DialogResult.Cancel };

        Panel buttonPanel = new Panel { Height = 30, Dock = DockStyle.None, Location = new System.Drawing.Point(40, 130) };
        buttonPanel.Controls.Add(loginButton);
        buttonPanel.Controls.Add(registerButton);

        loginButton.Location = new System.Drawing.Point(265, 0);
        registerButton.Location = new System.Drawing.Point(365, 0);

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
            e.Graphics.DrawRectangle(new System.Drawing.Pen(ThemeColors.Accent, 1), this.ClientRectangle);
        };

        usernameTextBox.BackColor = ThemeColors.ControlBackground;
        usernameTextBox.ForeColor = ThemeColors.ControlForeground;
        passwordTextBox.BackColor = ThemeColors.ControlBackground;
        passwordTextBox.ForeColor = ThemeColors.ControlForeground;

        loginButton.BackColor = ThemeColors.Accent;
        loginButton.ForeColor = ThemeColors.ControlForeground;
        loginButton.FlatStyle = FlatStyle.Flat;
        loginButton.FlatAppearance.BorderColor = ThemeColors.Accent;

        registerButton.BackColor = ThemeColors.Accent;
        registerButton.ForeColor = ThemeColors.ControlForeground;
        registerButton.FlatStyle = FlatStyle.Flat;
        registerButton.FlatAppearance.BorderColor = ThemeColors.Accent;

        this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

        loginButton.MouseEnter += (s, e) => loginButton.BackColor = ThemeColors.ButtonHover;
        loginButton.MouseLeave += (s, e) => loginButton.BackColor = ThemeColors.Accent;

        registerButton.MouseEnter += (s, e) => registerButton.BackColor = ThemeColors.ButtonHover;
        registerButton.MouseLeave += (s, e) => registerButton.BackColor = ThemeColors.Accent;

        loginButton.Click += LoginButton_Click;
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

    private void LoginButton_Click(object? sender, EventArgs e)
    {
        string username = usernameTextBox.Text;
        string password = passwordTextBox.Text;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            MessageBox.Show("Username and password cannot be empty.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        try
        {
            string hashedPassword = HashPassword(password);

            using (var connection = new SqliteConnection("Data Source=inventory.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(1) FROM Users WHERE Username = $username AND PasswordHash = $passwordHash";
                command.Parameters.AddWithValue("$username", username);
                command.Parameters.AddWithValue("$passwordHash", hashedPassword);

                var result = command.ExecuteScalar();

                if (result != null && (long)result == 1)
                {
                    MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    var mainForm = new MainForm();
                    mainForm.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid username or password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Login failed: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void RegisterButton_Click(object? sender, EventArgs e)
    {
        var registrationForm = new RegistrationForm();
        registrationForm.ShowDialog();
    }
}