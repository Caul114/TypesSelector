﻿using Autodesk.Revit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Excel = Microsoft.Office.Interop.Excel;

namespace TypesSelector
{
    /// <summary>
    /// La classe della nostra finestra di dialogo non modale.
    /// </summary>
    /// <remarks>
    /// Oltre ad altri metodi, ha un metodo per ogni pulsante di comando. 
    /// In ognuno di questi metodi non viene fatto nient'altro che il sollevamento
    /// di un evento esterno con una richiesta specifica impostata nel gestore delle richieste.
    /// </remarks>
    /// 
    public partial class TypesSelectorForm : Form
    {
        // In questo esempio, la finestra di dialogo possiede il gestore e gli oggetti evento, 
        // ma non è un requisito. Potrebbero anche essere proprietà statiche dell'applicazione.

        #region Private data members
        
        private RequestHandler m_Handler;
        private ExternalEvent m_ExEvent;

        // Creo un'istanza di questa form
        public static TypesSelectorForm thisApp;

        // La lista del singolo elemento selezionato nei DataGridView
        private List<string> _elementList = new List<string>();

        // La lista degli elementi selezionati nei DataGRidView
        private List<string[]> _elementsList = new List<string[]>();

        // La lista degli Unit Type Identifier
        private List<string[]> _listUTI = new List<string[]>();

        // La lista dei Panel Type Identifier
        private List<string[]> _listPTI = new List<string[]>();

        #endregion

        #region Class public property
        /// <summary>
        /// Proprietà pubblica per accedere al valore della richiesta corrente
        /// </summary>
        public List<string> ElementList
        {
            get { return _elementList; }
        }

        /// <summary>
        /// Proprietà pubblica per accedere al valore della richiesta corrente
        /// </summary>
        public List<string[]> ElementsList
        {
            get { return _elementsList; }
        }
        #endregion

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

        /// <summary>
        ///   Costruttore della finestra di dialogo
        /// </summary>
        /// 
        public TypesSelectorForm(ExternalEvent exEvent, RequestHandler handler)
        {
            InitializeComponent();
            m_Handler = handler;
            m_ExEvent = exEvent;

            thisApp = this;
        }

        /// <summary>
        /// Modulo gestore eventi chiuso
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // possediamo sia l'evento che il gestore
            // dovremmo eliminarlo prima di chiudere la finestra

            m_ExEvent.Dispose();
            m_ExEvent = null;
            m_Handler = null;

            // non dimenticare di chiamare la classe base
            base.OnFormClosed(e);
        }

        /// <summary>
        ///   Attivatore / disattivatore del controllo
        /// </summary>
        ///
        private void EnableCommands(bool status)
        {
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Enabled = status;
            }
            if (!status)
            {
                this.exitButton.Enabled = true;
            }
        }

        /// <summary>
        ///   Un metodo di supporto privato per effettuare una richiesta 
        ///   e allo stesso tempo mettere la finestra di dialogo in sospensione.
        /// </summary>
        /// <remarks>
        ///   Ci si aspetta che il processo che esegue la richiesta 
        ///   (l'helper Idling in questo caso particolare) 
        ///   riattivi anche la finestra di dialogo dopo aver terminato l'esecuzione.
        /// </remarks>
        ///
        public void MakeRequest(RequestId request)
        {
            App.thisApp.DontShowFormTop();
            m_Handler.Request.Make(request);
            m_ExEvent.Raise();
            DozeOff();
        }


        /// <summary>
        ///   DozeOff -> disabilita tutti i controlli (tranne il pulsante Esci)
        /// </summary>
        /// 
        private void DozeOff()
        {
            EnableCommands(false);
        }

        /// <summary>
        ///   WakeUp -> abilita tutti i controlli
        /// </summary>
        /// 
        public void WakeUp()
        {
            EnableCommands(true);
        }

        /// <summary>
        ///   Metodo di interazione con la finestra di dialogo che viene attivato all'apertura della Form
        /// </summary>
        /// 
        private void TypesSelectorForm_Load(object sender, EventArgs e)
        {
            MakeRequest(RequestId.Initial);
        }

        /// <summary>
        ///   Metodo per il riempimento del DataGridView1 per gli UNIT Type Ideintifier
        /// </summary>
        /// 
        public void SetDataGridViewUTI()
        {
            // Ottiene la lista degli Unit Type identifier
            _listUTI = m_Handler.ListUTI;

            // La trasforma in una lista anonima
            var stringslist = _listUTI
                .Select(arr => new {
                    UnitTypeIdentifier = arr[0],
                    Colors = arr[1]
                }).ToArray();

            // Crea un DataTable (utile per fare poi l'ordinamento per colonne)
            DataTable dataTable1 = new DataTable();
            dataTable1.Columns.Add(new DataColumn
            {
                ColumnName = "UnitTypeIdentifier",
                DataType = typeof(String)
            });
            dataTable1.Columns.Add(new DataColumn
            {
                ColumnName = "Colors",
                DataType = typeof(String)
            });           

            List<DataRow> list = new List<DataRow>();
            foreach (var item in stringslist)
            {
                var row = dataTable1.NewRow();
                row.SetField<string>("UnitTypeIdentifier", item.UnitTypeIdentifier);
                row.SetField<string>("Colors", item.Colors);
                list.Add(row);
            }
            dataTable1 = list.CopyToDataTable();

            // Riempie il DataGridView
            dataGridView1.DataSource = dataTable1;

            // Colora il background della colonna Colors con il colore corrispondente
            for (int i = 0; i < (dataGridView1.RowCount); i++)
            {
                dataGridView1.Rows[i].Cells[1].Style.BackColor =
                    Color.FromName(dataGridView1.Rows[i].Cells[1].Value.ToString());
            }
        }

        /// <summary>
        ///   Metodo per il riempimento del DataGridView2 per i PANEL Type Identifier
        /// </summary>
        /// 
        public void SetDataGridViewPTI()
        {
            // Ottiene la lista dei Panel Type identifier
            _listPTI = m_Handler.ListPTI;

            // La trasforma in una lista anonima
            var stringslist = _listPTI
                .Select(arr => new {
                    PanelTypeIdentifier = arr[0],
                    Qty = arr[1],
                    Colors = arr[2]
                }).ToArray();

            // Crea un DataTable (utile per fare poi l'ordinamento per colonne)
            DataTable dataTable2 = new DataTable();
            dataTable2.Columns.Add(new DataColumn
            {
                ColumnName = "PanelTypeIdentifier",
                DataType = typeof(String)
            });
            dataTable2.Columns.Add(new DataColumn
            {
                ColumnName = "Qty",
                DataType = typeof(String)
            });
            dataTable2.Columns.Add(new DataColumn
            {
                ColumnName = "Colors",
                DataType = typeof(String)
            });

            List<DataRow> list = new List<DataRow>();
            foreach (var x in stringslist)
            {
                var row = dataTable2.NewRow();
                row.SetField<string>("PanelTypeIdentifier", x.PanelTypeIdentifier);
                row.SetField<string>("Qty", x.Qty);
                row.SetField<string>("Colors", x.Colors);
                list.Add(row);
            }
            dataTable2 = list.CopyToDataTable();

            // Riempie il DataGridView
            dataGridView2.DataSource = dataTable2;

            // Colora il background della colonna Colors con il colore corrispondente
            for (int i = 0; i < (dataGridView2.RowCount); i++)
            {
                dataGridView2.Rows[i].Cells[2].Style.BackColor =
                    Color.FromName(dataGridView2.Rows[i].Cells[2].Value.ToString());
            }
        }

        /// <summary>
        ///   Metodo di ordinamento delle colonne nel DataGrid1
        /// </summary>
        /// 
        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn newColumn = dataGridView1.Columns[e.ColumnIndex];
            DataGridViewColumn oldColumn = dataGridView1.SortedColumn;
            ListSortDirection direction;

            // Se OldColumn e' nullo, allora il DataGridView non viene ordinato
            if (oldColumn != null)
            {
                // Ordina la stessa colonna, invertendo il SortOrder.
                if (oldColumn == newColumn &&
                    dataGridView1.SortOrder == SortOrder.Ascending)
                {
                    direction = ListSortDirection.Descending;
                }
                else
                {
                    // Ordina la nuova colonna e rimuove il vecchio SortGlyph.
                    direction = ListSortDirection.Ascending;
                    oldColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                }
            }
            else
            {
                direction = ListSortDirection.Ascending;
            }

            // Ordina la colonna selezionata
            dataGridView1.Sort(newColumn, direction);
            newColumn.HeaderCell.SortGlyphDirection =
                direction == ListSortDirection.Ascending ?
                SortOrder.Ascending : SortOrder.Descending;

            // Colora il background della colonna Colors con il colore corrispondente
            for (int i = 0; i < (dataGridView1.RowCount); i++)
            {
                dataGridView1.Rows[i].Cells[1].Style.BackColor =
                    Color.FromName(dataGridView1.Rows[i].Cells[1].Value.ToString());
            }
        }

        /// <summary>
        ///   Metodo di cambio del metodo di ordinamento del DataGrid1
        /// </summary>
        /// 
        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // Imposta ciascuna delle colonne nel sort mode programmatico.
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Programmatic;
            }
        }

        /// <summary>
        ///   Metodo di ordinamento delle colonne nel DataGrid2
        /// </summary>
        /// 
        private void dataGridView2_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn newColumn = dataGridView2.Columns[e.ColumnIndex];
            DataGridViewColumn oldColumn = dataGridView2.SortedColumn;
            ListSortDirection direction;

            // Se OldColumn e' nullo, allora il DataGridView non viene ordinato
            if (oldColumn != null)
            {
                // Ordina la stessa colonna, invertendo il SortOrder.
                if (oldColumn == newColumn &&
                    dataGridView2.SortOrder == SortOrder.Ascending)
                {
                    direction = ListSortDirection.Descending;
                }
                else
                {
                    // Ordina la nuova colonna e rimuove il vecchio SortGlyph.
                    direction = ListSortDirection.Ascending;
                    oldColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                }
            }
            else
            {
                direction = ListSortDirection.Ascending;
            }

            // Ordina la colonna selezionata
            dataGridView2.Sort(newColumn, direction);
            newColumn.HeaderCell.SortGlyphDirection =
                direction == ListSortDirection.Ascending ?
                SortOrder.Ascending : SortOrder.Descending;

            // Colora il background della colonna Colors con il colore corrispondente
            for (int i = 0; i < (dataGridView2.RowCount); i++)
            {
                dataGridView2.Rows[i].Cells[2].Style.BackColor =
                    Color.FromName(dataGridView2.Rows[i].Cells[2].Value.ToString());
            }
        }

        /// <summary>
        ///   Metodo di cambio del metodo di ordinamento del DataGrid2
        /// </summary>
        /// 
        private void dataGridView2_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // Imposta ciascuna delle colonne nel sort mode programmatico.
            foreach (DataGridViewColumn column in dataGridView2.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Programmatic;
            }
        }

        /// <summary>
        ///   Metodo di selezione delle righe del DataGridView1 e di raccolta dei dati selezionati al suo interno
        /// </summary>
        /// 
        private void dataGridView1_selectedRowsButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Index != -1)
            {
                // Riempie la Lista con le proprietà dell'elemento o degli elementi selezionati
                _elementsList.Clear();

                int selectedRowsCount = dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected);

                if (selectedRowsCount > 1)
                {
                    for (int i = 0; i < selectedRowsCount; i++)
                    {
                        string[] singleElement = new string[6];
                        string[] temp = dataGridView1.SelectedRows[i].Cells[0].Value.ToString().Split('-');
                        for (int j = 0; j < temp.Length; j++)
                        {
                            singleElement[j] = temp[j];
                        }
                        singleElement[5] = dataGridView1.SelectedRows[i].Cells[1].Value.ToString();
                        _elementsList.Add(singleElement);
                    }
                }
                else if (selectedRowsCount == 1)
                {
                    string[] temp = dataGridView1.SelectedRows[0].Cells[0].Value.ToString().Split('-');
                    foreach (var item in temp)
                    {
                        _elementList.Add(item);
                    }
                    _elementList.Add(dataGridView1.SelectedRows[0].Cells[1].Value.ToString());
                }

                // Chiama il metodo nella classe RequestHandler
                MakeRequest(RequestId.UTI);
            }
        }

        /// <summary>
        ///   Metodo di selezione delle righe del DataGridView2 e di raccolta dei dati selezionati al suo interno
        /// </summary>
        /// 
        private void dataGridView2_selectedRowsButton_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentRow.Index != -1)
            {
                // Riempie la Lista con le proprietà dell'elemento o degli elementi selezionati
                _elementsList.Clear();

                int selectedRowsCount = dataGridView2.Rows.GetRowCount(DataGridViewElementStates.Selected);

                if (selectedRowsCount > 1)
                {
                    for (int i = 0; i < selectedRowsCount; i++)
                    {
                        string[] singleElement = new string[5];
                        string[] temp = dataGridView2.SelectedRows[i].Cells[0].Value.ToString().Split('-');
                        for (int j = 0; j < temp.Length; j++)
                        {
                            singleElement[j] = temp[j];
                        }
                        singleElement[4] = dataGridView2.SelectedRows[i].Cells[2].Value.ToString();
                        _elementsList.Add(singleElement);
                    }
                }
                else if (selectedRowsCount == 1)
                {
                    string[] temp = dataGridView2.SelectedRows[0].Cells[0].Value.ToString().Split('-');
                    foreach (var item in temp)
                    {
                        _elementList.Add(item);
                    }
                    _elementList.Add(dataGridView2.SelectedRows[0].Cells[2].Value.ToString());
                }

                // Chiama il metodo nella classe RequestHandler
                MakeRequest(RequestId.PTI);
            }
        }

        /// <summary>
        ///   Metodo che permette la selezione di più Unit Type Identifier con l'uso del pulsante Key
        /// </summary>
        /// 
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            // Riempie la Lista con le proprietà dell'elemento o degli elementi selezionati
            _elementsList.Clear();

            int selectedRowsCount = dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected);

            if (e.KeyCode == Keys.Control && selectedRowsCount > 1)
            {  
                for (int i = 0; i < selectedRowsCount; i++)
                {
                    string[] singleElement = new string[6];
                    string[] temp = dataGridView1.SelectedRows[i].Cells[0].Value.ToString().Split('-');
                    for (int j = 0; j < temp.Length; j++)
                    {
                        singleElement[j] = temp[j];
                    }
                    singleElement[5] = dataGridView1.SelectedRows[i].Cells[1].Value.ToString();
                    _elementsList.Add(singleElement);
                }

                // Chiama il metodo nella classe RequestHandler
                MakeRequest(RequestId.UTI);
            }
        }

        /// <summary>
        ///   Metodo che permette la selezione di più Panel Type Identifier con l'uso del pulsante Key
        /// </summary>
        /// 
        private void dataGridView2_KeyUp(object sender, KeyEventArgs e)
        {
            // Riempie la Lista con le proprietà dell'elemento o degli elementi selezionati
            _elementsList.Clear();

            int selectedRowsCount = dataGridView2.Rows.GetRowCount(DataGridViewElementStates.Selected);

            if (e.KeyCode == Keys.Control && selectedRowsCount > 1)
            {
                for (int i = 0; i < selectedRowsCount; i++)
                {
                    string[] singleElement = new string[6];
                    string[] temp = dataGridView2.SelectedRows[i].Cells[0].Value.ToString().Split('-');
                    for (int j = 0; j < temp.Length; j++)
                    {
                        singleElement[j] = temp[j];
                    }
                    singleElement[5] = dataGridView2.SelectedRows[i].Cells[2].Value.ToString();
                    _elementsList.Add(singleElement);
                }

                // Chiama il metodo nella classe RequestHandler
                MakeRequest(RequestId.PTI);
            }
        }

        /// <summary>
        ///   Metodo che cambia il colore di tutti gli Unit Type Identifier
        /// </summary>
        /// 
        private void allUTIButton_Click(object sender, EventArgs e)
        {
            // Ripulisce la selezione precedente
            _elementsList.Clear();

            // Riempie la lista di tutti gli Unit Type Identifier
            for (int i = 0; i < _listUTI.Count(); i++)
            {
                string[] singleElement = new string[6];
                string[] temp = dataGridView1.Rows[i].Cells["UnitTypeIdentifier"].Value.ToString().Split('-');
                for (int j = 0; j < temp.Length; j++)
                {
                    singleElement[j] = temp[j];
                }
                singleElement[5] = dataGridView1.Rows[i].Cells["Colors"].Value.ToString();
                _elementsList.Add(singleElement);
                temp = null;
            }

            // Chiama il metodo in Request Handler che si occupa del cambio colore
            MakeRequest(RequestId.AllUTI);
        }

        /// <summary>
        ///   Metodo che cambia il colore di tutti i Panel Type Identifier
        /// </summary>
        /// 
        private void allPTIButton_Click(object sender, EventArgs e)
        {
            // Ripulisce la selezione precedente
            _elementsList.Clear();

            // Riempie la lista di tutti gli Unit Type Identifier
            for (int i = 0; i < _listPTI.Count(); i++)
            {
                string[] singleElement = new string[5];
                string[] temp = dataGridView2.Rows[i].Cells["PanelTypeIdentifier"].Value.ToString().Split('-');
                for (int j = 0; j < singleElement.Length - 1; j++)
                {
                    if(temp[j] != "")
                    {
                        singleElement[j] = temp[j];
                    } else
                    {
                        singleElement[j] = "xxx";
                    }
                    
                }
                singleElement[4] = dataGridView2.Rows[i].Cells["Colors"].Value.ToString();
                _elementsList.Add(singleElement);
            }

            // Chiama il metodo in Request Handler che si occupa del cambio colore
            MakeRequest(RequestId.AllPTI);
        }

        /// <summary>
        ///   Metodo che cancella la selezione in dataGridView1
        /// </summary>
        /// 
        public void clearSelection_dataGridView1()
        {
            dataGridView1.ClearSelection();
        }

        /// <summary>
        ///   Metodo che cancella la selezione in dataGridView2
        /// </summary>
        /// 
        public void clearSelection_dataGridView2()
        {
            dataGridView2.ClearSelection();
        }

        /// <summary>
        ///   Metodo che ripristina il colore di default del View Template
        /// </summary>
        /// 
        private void cancelButton_Click(object sender, EventArgs e)
        {
            MakeRequest(RequestId.Cancel);
        }

        /// <summary>
        ///   Exit - imposta tutte le operazioni di chiusura della Form
        /// </summary>
        /// 
        private void exitButton_Click(object sender, EventArgs e)
        {
            ClosingForm closingForm = new ClosingForm();
            closingForm.Show();
            this.DozeOff();
            closingForm.TopMost = true;
        }

        /// <summary>
        ///   Exit - Evento che si genera quando si chiude la Form
        /// </summary>
        /// 

        private void TypesSelector_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        /// <summary>
        ///   Chiude la finestra di dialogo
        /// </summary>
        /// 
        public void FormClose()
        {            
            Close();
        }

    }  // class
}
