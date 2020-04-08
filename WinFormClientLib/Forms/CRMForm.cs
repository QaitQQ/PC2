using Client.Class;
using Client.Class.Net;
using CRMLibs;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Client.Forms
{
    public partial class CRMForm : Form
    {
        private List<Partner> list;
        private Partner ActivePartner;
        public CRMForm ( ) => InitializeComponent();

        private void FillTable ( )
        {

            list = new CrmNetClass().GetPartnerList();

            foreach (Partner item in list)
            { ListPartner.Rows.Add(item.Name); }
        }
        private void CRMForm_Activated ( object sender, EventArgs e ) => FillTable();
        private void ShowEvents ( )
        {
            List<Event> Events = new CrmNetClass().ShowEvents(ActivePartner.Id);
            EventBox.Rows.Clear();
            int i = 0;
            foreach (Event item in Events)
            {
                EventBox.Rows.Add();
                EventBox[0, i].Value = item.DateОccurred;
                EventBox[1, i].Value = item.Сontent;
                i++;
            }
        }
        private void CreateNewEvent ( )
        {
            using (CreateEvents NewEwent = new CreateEvents())
            {
                NewEwent.ShowDialog();
                if (NewEwent.DialogResult.ToString() == "OK")
                {
                    new TCP_Client_GetObj(3).Get(4, new Event() { DateОccurred = NewEwent.Date1(), DatePlanned = NewEwent.Date2(), TypeID = NewEwent.TypeContact(), Сontent = NewEwent.Content(), PartnerID = ActivePartner.Id });
                    ShowEvents();
                }
            }

        }
        private void ShowContact ( )
        {
            textBox1.Text = ActivePartner.Phone;
            textBox2.Text = ActivePartner.Аddress;
            textBox3.Text = ActivePartner.Email;
            //    textBox4.Text = ActivePartner.СontacPersons[0].Name;

        }
        private void ListPartner_SelectionChanged ( object sender, EventArgs e )
        {

            if (ListPartner.SelectedCells[0].Value != null)
            {
                ActivePartner = StaticForm.SearchPartner(list, ListPartner.SelectedCells[0].Value.ToString());
                ShowEvents();
                ShowContact();
            }

        }

        private void AddEventButton_Click ( object sender, EventArgs e ) => CreateNewEvent();
    }
}
