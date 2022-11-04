using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExpenseTracker
{
    public partial class Form2 : Form
    {
        ExpenseTracker.ServiceReference1.ExpenseTrackerServiceOf_String_Int32Client service;
        ExpenseTracker.ServiceReference1.ExpenseDefinitionOfstringint expense;

        public delegate void Alert_Generator(string message);

        public Form2()
        {
            InitializeComponent();
            Refresh("Welcome To Expense Tracker");
        }

        public void Refresh(string message)
        {
            service = new ExpenseTracker.ServiceReference1.ExpenseTrackerServiceOf_String_Int32Client();

            expense = new ExpenseTracker.ServiceReference1.ExpenseDefinitionOfstringint();

            ExpenseDataView.DataSource = service.Get_Expense();

            IncomeDataView.DataSource = service.Get_Income();

            totalExpense.Text = service.Get_Expense().Sum(x => x.Amount).ToString();

            totalIncome.Text = service.Get_Income().Sum(x => x.Amount).ToString();

            int balance = Convert.ToInt32(service.Get_Income().Sum(x => x.Amount)).getBalance(Convert.ToInt32(service.Get_Expense().Sum(x => x.Amount)));

            label17.Text = "Balance " + balance.ToString() +" Rs";

            if (balance > 0) { label17.ForeColor = Color.Green; } else { label17.ForeColor = Color.Red; }

            Alert_Generator alert = delegate (string information)
            {
                MessageBox.Show(information);
            };

            alert(message);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            expense = new ExpenseTracker.ServiceReference1.ExpenseDefinitionOfstringint
            {
                Name = add_name.Text,
                Date = add_date.Text,
                Amount = Convert.ToInt32(add_amount.Text)
            };

            if (add_type.SelectedItem.Equals("Income"))
            {
                service.Add_Income(expense);
                Refresh("Income Added");
            }
            else
            {
                service.Add_Expense(expense);
                Refresh("Expense Added");
            }
        }

        private void update_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (update_type.SelectedItem.Equals("Income"))
            {
                update_id.Items.Clear();
                foreach (ExpenseTracker.ServiceReference1.Income x in service.Get_Income())
                {
                    update_id.Items.Add(x.Id);
                }
            }
            else
            {
                update_id.Items.Clear();
                foreach (ExpenseTracker.ServiceReference1.Expense x in service.Get_Expense())
                {
                    update_id.Items.Add(x.Id);
                }
            }
        }

        private void delete_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (delete_type.SelectedItem.Equals("Income"))
            {
                delete_id.Items.Clear();
                foreach (ExpenseTracker.ServiceReference1.Income x in service.Get_Income())
                {
                    delete_id.Items.Add(x.Id);
                }
            }
            else
            {
                delete_id.Items.Clear();
                foreach (ExpenseTracker.ServiceReference1.Expense x in service.Get_Expense())
                {
                    delete_id.Items.Add(x.Id);
                }
            }
        }

        private void update_id_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (update_type.SelectedItem.Equals("Income"))
            {
                var data = service.Get_Income_By_Id(Convert.ToInt32(update_id.SelectedItem));

                update_name.Text = data.Name;
                update_date.Text = data.Date;
                update_amount.Text = data.Amount.ToString();
            }
            else
            {
                var data = service.Get_Expense_By_Id(Convert.ToInt32(update_id.SelectedItem));

                update_name.Text = data.Name;
                update_date.Text = data.Date;
                update_amount.Text = data.Amount.ToString();
            }

        }

        private void delete_id_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (delete_type.SelectedItem.Equals("Income"))
            {
                var data = service.Get_Income_By_Id(Convert.ToInt32(delete_id.SelectedItem));

                delete_name.Text = data.Name;
                delete_date.Text = data.Date;
                delete_amount.Text = data.Amount.ToString();
            }
            else
            {
                var data = service.Get_Expense_By_Id(Convert.ToInt32(delete_id.SelectedItem));

                delete_name.Text = data.Name;
                delete_date.Text = data.Date;
                delete_amount.Text = data.Amount.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            expense = new ExpenseTracker.ServiceReference1.ExpenseDefinitionOfstringint
            {
                Id = Convert.ToInt32(update_id.SelectedItem),
                Name = update_name.Text,
                Date = update_date.Text,
                Amount = Convert.ToInt32(update_amount.Text)
            };

            if (update_type.SelectedItem.Equals("Income"))
            {
                service.Update_Income(expense);
                Refresh("Income Updated");
            }
            else
            {
                service.Update_Expense(expense);
                Refresh("Expense Updated");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (delete_type.SelectedItem.Equals("Income"))
            {
                service.Delete_Income(Convert.ToInt32(delete_id.SelectedItem));
                Refresh("Income Deleted");
            }
            else
            {
                service.Delete_Expense(Convert.ToInt32(delete_id.SelectedItem));
                Refresh("Expense Deleted");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
