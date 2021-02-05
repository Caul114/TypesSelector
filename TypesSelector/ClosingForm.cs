using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TypesSelector
{
    public partial class ClosingForm : Form
    {
        // Inizializzo la Form base
        private TypesSelectorForm _typesSelectorForm;
        public ClosingForm()
        {
            InitializeComponent();
        }

        /// <summary>
        ///   Snippet per nascondere l'X button
        /// </summary>
        /// 
        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            // Chiudo TypesSelector senza chiudere la selezione
            TypesSelectorForm.thisApp.FormClose();
            this.Close();
        }

        private void noButton_Click(object sender, EventArgs e)
        {
            // Eseguo la cancellazione della selezione
            TypesSelectorForm.thisApp.MakeRequest(RequestId.Esc);
            this.Close();
        }
    }
}
