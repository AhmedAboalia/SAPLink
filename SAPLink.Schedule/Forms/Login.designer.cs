using System.ComponentModel;
using Guna.UI2.WinForms;

namespace SAPLink.Schedule.Forms
{
    partial class Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new Container();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges17 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(Login));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            guna2Panel1 = new Guna2Panel();
            label1 = new Label();
            guna2PictureBox2 = new Guna2PictureBox();
            label3 = new Label();
            guna2CirclePictureBox1 = new Guna2CirclePictureBox();
            comboBoxClients = new Guna2ComboBox();
            textBoxUserName = new Guna2TextBox();
            guna2ControlBox2 = new Guna2ControlBox();
            textBoxPassword = new Guna2TextBox();
            guna2ControlBox1 = new Guna2ControlBox();
            buttonLogin = new Guna2GradientButton();
            guna2DragControl1 = new Guna2DragControl(components);
            guna2ShadowForm1 = new Guna2ShadowForm(components);
            guna2Elipse1 = new Guna2Elipse(components);
            guna2Panel1.SuspendLayout();
            ((ISupportInitialize)guna2PictureBox2).BeginInit();
            ((ISupportInitialize)guna2CirclePictureBox1).BeginInit();
            SuspendLayout();
            // 
            // guna2Panel1
            // 
            guna2Panel1.BackColor = Color.WhiteSmoke;
            guna2Panel1.BorderColor = Color.White;
            guna2Panel1.BorderRadius = 25;
            guna2Panel1.Controls.Add(label1);
            guna2Panel1.Controls.Add(guna2PictureBox2);
            guna2Panel1.Controls.Add(label3);
            guna2Panel1.Controls.Add(guna2CirclePictureBox1);
            guna2Panel1.Controls.Add(comboBoxClients);
            guna2Panel1.Controls.Add(textBoxUserName);
            guna2Panel1.Controls.Add(guna2ControlBox2);
            guna2Panel1.Controls.Add(textBoxPassword);
            guna2Panel1.Controls.Add(guna2ControlBox1);
            guna2Panel1.Controls.Add(buttonLogin);
            guna2Panel1.CustomBorderColor = Color.Transparent;
            guna2Panel1.CustomizableEdges = customizableEdges16;
            guna2Panel1.Dock = DockStyle.Fill;
            guna2Panel1.Location = new Point(0, 0);
            guna2Panel1.Margin = new Padding(4);
            guna2Panel1.Name = "guna2Panel1";
            guna2Panel1.ShadowDecoration.CustomizableEdges = customizableEdges17;
            guna2Panel1.Size = new Size(836, 420);
            guna2Panel1.TabIndex = 0;
            guna2Panel1.Click += guna2Panel1_Click;
            guna2Panel1.Paint += guna2Panel1_Paint;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.WhiteSmoke;
            label1.Font = new Font("Agency FB", 16.25F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
            label1.Location = new Point(732, 110);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(47, 26);
            label1.TabIndex = 1;
            label1.Text = "Login";
            // 
            // guna2PictureBox2
            // 
            guna2PictureBox2.BackColor = Color.WhiteSmoke;
            guna2PictureBox2.CustomizableEdges = customizableEdges1;
            guna2PictureBox2.Image = (Image)resources.GetObject("guna2PictureBox2.Image");
            guna2PictureBox2.ImageRotate = 0F;
            guna2PictureBox2.InitialImage = null;
            guna2PictureBox2.Location = new Point(383, 69);
            guna2PictureBox2.Name = "guna2PictureBox2";
            guna2PictureBox2.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2PictureBox2.Size = new Size(39, 41);
            guna2PictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            guna2PictureBox2.TabIndex = 16;
            guna2PictureBox2.TabStop = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.WhiteSmoke;
            label3.Font = new Font("Agency FB", 30F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
            label3.Location = new Point(422, 69);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(361, 50);
            label3.TabIndex = 1;
            label3.Text = "SAP Link Synchronization";
            // 
            // guna2CirclePictureBox1
            // 
            guna2CirclePictureBox1.Image = (Image)resources.GetObject("guna2CirclePictureBox1.Image");
            guna2CirclePictureBox1.ImageRotate = 0F;
            guna2CirclePictureBox1.Location = new Point(27, 47);
            guna2CirclePictureBox1.Name = "guna2CirclePictureBox1";
            guna2CirclePictureBox1.ShadowDecoration.CustomizableEdges = customizableEdges3;
            guna2CirclePictureBox1.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            guna2CirclePictureBox1.Size = new Size(288, 306);
            guna2CirclePictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            guna2CirclePictureBox1.TabIndex = 13;
            guna2CirclePictureBox1.TabStop = false;
            // 
            // comboBoxClients
            // 
            comboBoxClients.BackColor = Color.Transparent;
            comboBoxClients.BorderColor = Color.Goldenrod;
            comboBoxClients.BorderRadius = 12;
            comboBoxClients.BorderThickness = 2;
            comboBoxClients.CustomizableEdges = customizableEdges4;
            comboBoxClients.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxClients.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxClients.FocusedColor = Color.FromArgb(94, 148, 255);
            comboBoxClients.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            comboBoxClients.Font = new Font("Bahnschrift SemiLight SemiConde", 9F, FontStyle.Regular, GraphicsUnit.Point);
            comboBoxClients.ForeColor = Color.Black;
            comboBoxClients.ItemHeight = 30;
            comboBoxClients.Location = new Point(370, 153);
            comboBoxClients.Margin = new Padding(3, 2, 3, 2);
            comboBoxClients.Name = "comboBoxClients";
            comboBoxClients.ShadowDecoration.CustomizableEdges = customizableEdges5;
            comboBoxClients.Size = new Size(402, 36);
            comboBoxClients.TabIndex = 4;
            comboBoxClients.SelectedIndexChanged += comboBoxClients_SelectedIndexChanged;
            // 
            // textBoxUserName
            // 
            textBoxUserName.Animated = true;
            textBoxUserName.AutoRoundedCorners = true;
            textBoxUserName.BackColor = SystemColors.Control;
            textBoxUserName.BorderColor = Color.Goldenrod;
            textBoxUserName.BorderRadius = 15;
            textBoxUserName.BorderThickness = 3;
            textBoxUserName.Cursor = Cursors.IBeam;
            textBoxUserName.CustomizableEdges = customizableEdges6;
            textBoxUserName.DefaultText = "User Name";
            textBoxUserName.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            textBoxUserName.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            textBoxUserName.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            textBoxUserName.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            textBoxUserName.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            textBoxUserName.Font = new Font("Bahnschrift SemiLight SemiConde", 9F, FontStyle.Regular, GraphicsUnit.Point);
            textBoxUserName.ForeColor = Color.Black;
            textBoxUserName.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            textBoxUserName.IconLeft = (Image)resources.GetObject("textBoxUserName.IconLeft");
            textBoxUserName.IconLeftSize = new Size(28, 28);
            textBoxUserName.IconRightOffset = new Point(5, 0);
            textBoxUserName.IconRightSize = new Size(28, 28);
            textBoxUserName.Location = new Point(370, 200);
            textBoxUserName.Margin = new Padding(2, 7, 2, 7);
            textBoxUserName.Name = "textBoxUserName";
            textBoxUserName.PasswordChar = '\0';
            textBoxUserName.PlaceholderForeColor = Color.Black;
            textBoxUserName.PlaceholderText = "";
            textBoxUserName.SelectedText = "";
            textBoxUserName.ShadowDecoration.CustomizableEdges = customizableEdges7;
            textBoxUserName.Size = new Size(402, 33);
            textBoxUserName.Style = Guna.UI2.WinForms.Enums.TextBoxStyle.Material;
            textBoxUserName.TabIndex = 2;
            textBoxUserName.TextOffset = new Point(7, 0);
            textBoxUserName.Enter += guna2TextBox1_Enter;
            textBoxUserName.KeyDown += textBoxUserName_KeyDown;
            textBoxUserName.Leave += guna2TextBox1_Leave;
            // 
            // guna2ControlBox2
            // 
            guna2ControlBox2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            guna2ControlBox2.BorderRadius = 8;
            guna2ControlBox2.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MinimizeBox;
            guna2ControlBox2.CustomizableEdges = customizableEdges8;
            guna2ControlBox2.FillColor = Color.Transparent;
            guna2ControlBox2.IconColor = Color.Black;
            guna2ControlBox2.Location = new Point(739, 14);
            guna2ControlBox2.Margin = new Padding(4);
            guna2ControlBox2.Name = "guna2ControlBox2";
            guna2ControlBox2.ShadowDecoration.CustomizableEdges = customizableEdges9;
            guna2ControlBox2.Size = new Size(32, 28);
            guna2ControlBox2.TabIndex = 5;
            // 
            // textBoxPassword
            // 
            textBoxPassword.Animated = true;
            textBoxPassword.AutoRoundedCorners = true;
            textBoxPassword.BackColor = SystemColors.Control;
            textBoxPassword.BorderColor = Color.Goldenrod;
            textBoxPassword.BorderRadius = 15;
            textBoxPassword.BorderThickness = 3;
            textBoxPassword.Cursor = Cursors.IBeam;
            textBoxPassword.CustomizableEdges = customizableEdges10;
            textBoxPassword.DefaultText = "Password";
            textBoxPassword.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            textBoxPassword.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            textBoxPassword.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            textBoxPassword.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            textBoxPassword.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            textBoxPassword.Font = new Font("Bahnschrift SemiLight SemiConde", 9F, FontStyle.Regular, GraphicsUnit.Point);
            textBoxPassword.ForeColor = Color.Black;
            textBoxPassword.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            textBoxPassword.IconLeft = (Image)resources.GetObject("textBoxPassword.IconLeft");
            textBoxPassword.IconLeftSize = new Size(28, 28);
            textBoxPassword.IconRightOffset = new Point(5, 0);
            textBoxPassword.IconRightSize = new Size(28, 28);
            textBoxPassword.Location = new Point(370, 246);
            textBoxPassword.Margin = new Padding(2, 7, 2, 7);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.PasswordChar = '*';
            textBoxPassword.PlaceholderForeColor = Color.Black;
            textBoxPassword.PlaceholderText = "";
            textBoxPassword.SelectedText = "";
            textBoxPassword.ShadowDecoration.CustomizableEdges = customizableEdges11;
            textBoxPassword.Size = new Size(402, 33);
            textBoxPassword.Style = Guna.UI2.WinForms.Enums.TextBoxStyle.Material;
            textBoxPassword.TabIndex = 3;
            textBoxPassword.TextOffset = new Point(7, 0);
            textBoxPassword.Enter += guna2TextBox2_Enter;
            textBoxPassword.KeyDown += textBoxPassword_KeyDown;
            textBoxPassword.Leave += guna2TextBox2_Leave;
            // 
            // guna2ControlBox1
            // 
            guna2ControlBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            guna2ControlBox1.BorderRadius = 8;
            guna2ControlBox1.CustomizableEdges = customizableEdges12;
            guna2ControlBox1.FillColor = Color.Transparent;
            guna2ControlBox1.IconColor = Color.Black;
            guna2ControlBox1.Location = new Point(777, 14);
            guna2ControlBox1.Margin = new Padding(4);
            guna2ControlBox1.Name = "guna2ControlBox1";
            guna2ControlBox1.ShadowDecoration.CustomizableEdges = customizableEdges13;
            guna2ControlBox1.Size = new Size(32, 28);
            guna2ControlBox1.TabIndex = 6;
            guna2ControlBox1.Click += guna2ControlBox1_Click;
            // 
            // buttonLogin
            // 
            buttonLogin.AutoRoundedCorners = true;
            buttonLogin.BorderRadius = 25;
            buttonLogin.CustomizableEdges = customizableEdges14;
            buttonLogin.FillColor = Color.Goldenrod;
            buttonLogin.FillColor2 = Color.DarkOrange;
            buttonLogin.Font = new Font("Segoe Print", 12F, FontStyle.Bold, GraphicsUnit.Point);
            buttonLogin.ForeColor = Color.White;
            buttonLogin.HoverState.FillColor = Color.OliveDrab;
            buttonLogin.HoverState.FillColor2 = Color.YellowGreen;
            buttonLogin.Image = (Image)resources.GetObject("buttonLogin.Image");
            buttonLogin.ImageAlign = HorizontalAlignment.Left;
            buttonLogin.ImageSize = new Size(40, 40);
            buttonLogin.Location = new Point(370, 301);
            buttonLogin.Margin = new Padding(4);
            buttonLogin.Name = "buttonLogin";
            buttonLogin.ShadowDecoration.CustomizableEdges = customizableEdges15;
            buttonLogin.Size = new Size(402, 52);
            buttonLogin.TabIndex = 1;
            buttonLogin.Text = "Login";
            buttonLogin.Click += guna2GradientButton1_Click;
            buttonLogin.KeyDown += buttonLogin_KeyDown;
            // 
            // guna2DragControl1
            // 
            guna2DragControl1.ContainerControl = this;
            guna2DragControl1.DockIndicatorTransparencyValue = 0.6D;
            guna2DragControl1.TargetControl = guna2Panel1;
            guna2DragControl1.UseTransparentDrag = true;
            // 
            // guna2Elipse1
            // 
            guna2Elipse1.BorderRadius = 10;
            guna2Elipse1.TargetControl = this;
            // 
            // Login
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(836, 420);
            Controls.Add(guna2Panel1);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4);
            Name = "Login";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login Screen";
            Load += LoginScreen_Load;
            guna2Panel1.ResumeLayout(false);
            guna2Panel1.PerformLayout();
            ((ISupportInitialize)guna2PictureBox2).EndInit();
            ((ISupportInitialize)guna2CirclePictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna2Panel guna2Panel1;
        private Guna2DragControl guna2DragControl1;
        private Guna2ShadowForm guna2ShadowForm1;
        private Guna2Elipse guna2Elipse1;
        private Guna2TextBox textBoxPassword;
        private Guna2GradientButton buttonLogin;
        private Guna2ControlBox guna2ControlBox2;
        private Guna2ControlBox guna2ControlBox1;
        private Label label3;
        private Guna2TextBox textBoxUserName;
        private Guna2ComboBox comboBoxClients;
        private Guna2CirclePictureBox guna2CirclePictureBox1;
        private Label label1;
        private Guna2PictureBox guna2PictureBox2;
    }
}