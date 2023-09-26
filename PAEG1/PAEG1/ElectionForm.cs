using PAEG1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace PAEG1
{
    public partial class ElectionForm : Form
    {
        private Paeg1Context _context;
        private int yPos = 20;
        private int xOffset = 20;
        private System.Windows.Forms.RadioButton _currentChoice;
        private User _user;

        public ElectionForm(Paeg1Context context, User user)
        {
            InitializeComponent();
            _context = context;
            _user = user;
        }

        private void ElectionForm_Load(object sender, EventArgs e)
        {
            foreach (var candidate in _context.Candidates)
            {
                System.Windows.Forms.RadioButton radioButton = new System.Windows.Forms.RadioButton();
                radioButton.Name = $"radioButton_{candidate.Id}";
                radioButton.Text = $"{candidate.Surname} {candidate.Name} {candidate.Patronymic} - \"{candidate.Description}\"";
                radioButton.AutoSize = true;
                radioButton.Location = new Point(xOffset, yPos);
                radioButton.CheckedChanged += RadioButton_CheckedChanged;

                electionBox.Controls.Add(radioButton);

                yPos += 30;
            }
            xOffset = electionBox.Width / 2 - 50;
            Button button = new Button();
            button.Text = "Проголосувати";
            button.AutoSize = true;
            button.BackColor = SystemColors.Control;
            button.Location = new Point(xOffset, yPos);
            button.Click += Button_Click;

            electionBox.Controls.Add(button);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (_currentChoice is null)
            {
                errorProvider1.SetError(electionBox, "Нічого не обрано!");
                return;
            }

            string message = _currentChoice.Text;
            EncodeMessage encodeMessage = new EncodeMessage(int.Parse(_currentChoice.Name.Split("_")[1]) + " " + message);
            var encoded = encodeMessage.GenerateECP();
            if (!CompareEcp.Compare(encoded))
            {
                DialogResult resultDial = MessageBox.Show("Wrong ecp!", "Message Box", MessageBoxButtons.OK);

                if (resultDial == DialogResult.OK)
                {
                    var forms = Application.OpenForms;
                    for (int i = 0; i < forms.Count; i++)
                    {
                        if (forms[i] is Form1)
                            forms[i].Visible = true;
                    }
                    this.Close();
                }
            }
            var result = CompareEcp.DecryptMessage(encoded);

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var electorate = _context.Electorates
                        .Where(el => el.Id == _user.ElectorateId)
                        .First();

                    electorate.HasVoted = 1;

                    _context.SaveChanges();

                    int candidateId = int.Parse(result.Split(" ")[0]);
                    var candidate = _context.Candidates.Find(candidateId);

                    if (candidate != null)
                    {
                        if (candidate.AmountOfVotes is null)
                        {
                            candidate.AmountOfVotes = 1;
                        }
                        else
                            candidate.AmountOfVotes++;

                        _context.SaveChanges();
                    }
                    else
                    {
                        throw new KeyNotFoundException(result);
                    }

                    transaction.Commit();

                    DialogResult resultDial = MessageBox.Show("Ваш голос успішно враховано!", "Message Box", MessageBoxButtons.OK);

                    if (resultDial == DialogResult.OK)
                    {
                        var forms = Application.OpenForms;
                        for (int i = 0; i < forms.Count; i++)
                        {
                            if (forms[i] is Form1)
                                forms[i].Visible = true;
                        }
                        this.Close();
                    }

                }
                catch (Exception)
                {
                    transaction.Rollback();
                    errorProvider1.SetError(electionBox, "Error occured when trying to vote");
                }
            }
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            _currentChoice = (System.Windows.Forms.RadioButton)sender;
        }

        private void ElectionForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            var forms = Application.OpenForms;
            for (int i = 0; i < forms.Count; i++)
            {
                if (forms[i] != this && !forms[i].Visible)
                    forms[i].Close();
            }
        }
    }
}
