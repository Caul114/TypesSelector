using Autodesk.Revit.UI;
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

        // La lista del singolo elemento selezionato nei DataGridView
        private List<string> _elementList = new List<string>();

        // La lista degli elementi selezionati nei DataGRidView
        private List<string[]> _elementsList = new List<string[]>();
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
        ///   Costruttore della finestra di dialogo
        /// </summary>
        /// 
        public TypesSelectorForm(ExternalEvent exEvent, RequestHandler handler)
        {
            InitializeComponent();
            m_Handler = handler;
            m_ExEvent = exEvent;
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
        private void MakeRequest(RequestId request)
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
            List<string[]> lista = m_Handler.ListUTI;

            // La trasforma in una lista anonima
            var stringslist = lista
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
            for (int i = 0; i < (dataGridView1.RowCount - 1); i++)
            {
                dataGridView1.Rows[i].Cells[1].Style.BackColor =
                    Color.FromName(dataGridView1.Rows[i].Cells[1].Value.ToString());
            }
        }

        /// <summary>
        ///   Metodo per il riempimento del DataGridView2 per i PANEL Type Ideintifier
        /// </summary>
        /// 
        public void SetDataGridViewPTI()
        {
            // Ottiene la lista dei Panel Type identifier
            List<string[]> lista = m_Handler.ListPTI;

            // La trasforma in una lista anonima
            var stringslist = lista
                .Select(arr => new {
                    PanelTypeIdentifier = arr[0],
                    Colors = arr[1]
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
                ColumnName = "Colors",
                DataType = typeof(String)
            });

            List<DataRow> list = new List<DataRow>();
            foreach (var x in stringslist)
            {
                var row = dataTable2.NewRow();
                row.SetField<string>("PanelTypeIdentifier", x.PanelTypeIdentifier);
                row.SetField<string>("Colors", x.Colors);
                list.Add(row);
            }
            dataTable2 = list.CopyToDataTable();

            // Riempie il DataGRidView
            dataGridView2.DataSource = dataTable2;

            // Colora il background della colonna Colors con il colore corrispondente
            for (int i = 0; i < (dataGridView2.RowCount - 1); i++)
            {
                dataGridView2.Rows[i].Cells[1].Style.BackColor =
                    Color.FromName(dataGridView2.Rows[i].Cells[1].Value.ToString());
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
            for (int i = 0; i < (dataGridView1.RowCount - 1); i++)
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
            for (int i = 0; i < (dataGridView2.RowCount - 1); i++)
            {
                dataGridView2.Rows[i].Cells[1].Style.BackColor =
                    Color.FromName(dataGridView2.Rows[i].Cells[1].Value.ToString());
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
            // Riempie la Lista con le proprieta' dell'elemento o degli elementi selezionati
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
            else if(selectedRowsCount == 1)
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

        /// <summary>
        ///   Metodo di selezione delle righe del DataGridView2 e di raccolta dei dati selezionati al suo interno
        /// </summary>
        /// 
        private void dataGridView2_selectedRowsButton_Click(object sender, EventArgs e)
        {
            // Riempie la Lista con le proprieta' dell'elemento o degli elementi selezionati

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
                    singleElement[4] = dataGridView2.SelectedRows[i].Cells[1].Value.ToString();
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
                _elementList.Add(dataGridView2.SelectedRows[0].Cells[1].Value.ToString());
            }

            // Chiama il metodo nella classe RequestHandler
            MakeRequest(RequestId.PTI);
        }

        /// <summary>
        ///   Exit - chiude la finestra di dialogo
        /// </summary>
        /// 
        private void exitButton_Click_1(object sender, EventArgs e)
        {
            Close();
        }
    }  // class
}
