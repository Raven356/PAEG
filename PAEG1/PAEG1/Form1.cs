using PAEG1.Models;

namespace PAEG1
{
    public partial class Form1 : Form
    {
        private Paeg1Context _context;
        public Form1(Paeg1Context context)
        {
            InitializeComponent();
            _context = context;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (loginInput.Text.Equals("admin") && passwordInput.Text.Equals("admin"))
            {
                this.Visible = false;
                AdminForm adminForm = new AdminForm(_context);
                adminForm.Show();
            }
            else
            {
                var user = _context.Users.Where(user => user.Login == loginInput.Text && user.Password == passwordInput.Text).FirstOrDefault();
                if (user is not null)
                {
                    errorProvider1.Clear();
                    if (_context.Electorates.Where(el => el.Id == user.ElectorateId).First().HasVoted == 0)
                    {
                        this.Visible = false;
                        ElectionForm electionForm = new ElectionForm(_context, user);
                        electionForm.Show();
                    }
                    else
                    {
                        DialogResult resultDial = MessageBox.Show("Ваш голос вже зараховано, неможливо повторно проголосувати!", "Message Box", MessageBoxButtons.OK);
                        loginInput.Text = "";
                        passwordInput.Text = "";
                    }
                }
                else
                {
                    errorProvider1.SetError(loginInput, "Invalid data!");
                    errorProvider1.SetError(passwordInput, "Invalid data!");
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loginInput.Text = "";
            passwordInput.Text = "";
        }

        private void Form1_VisibleChanged(object sender, EventArgs e)
        {
            loginInput.Text = "";
            passwordInput.Text = "";
        }
    }
}