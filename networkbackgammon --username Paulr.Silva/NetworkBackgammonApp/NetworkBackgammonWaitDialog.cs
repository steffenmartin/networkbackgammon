using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetworkBackgammon
{
    public partial class NetworkBackgammonWaitDialog : Form
    {
        // Action button delegate definition
        public delegate void WaitButtonActionDelegate();
        // Action button delegate 
        WaitButtonActionDelegate buttonActionDelegate = null;
        
        public NetworkBackgammonWaitDialog(WaitButtonActionDelegate btnActionDelegate)
        {
            InitializeComponent();

            // Create messaging callback objects
            buttonActionDelegate = btnActionDelegate;
        }

        public Label WaitDialogLabel
        {
            get
            {
                return m_waitlabel;
            }
            set
            {
                m_waitlabel = value;
            }
        }

        private void m_cancelButton_Click(object sender, EventArgs e)
        {
            if (buttonActionDelegate != null)
            {
                try
                {
                    buttonActionDelegate();
                }
                catch
                {
                }
            }

            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
