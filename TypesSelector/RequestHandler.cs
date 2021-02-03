//
// (C) Copyright 2003-2017 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TypesSelector
{
    /// <summary>
    ///   Una classe con metodi per eseguire le richieste effettuate dall'utente della finestra di dialogo.
    /// </summary>
    /// 
    public class RequestHandler : IExternalEventHandler  // Un'istanza di una classe che implementa questa interfaccia verrà registrata prima con Revit e ogni volta che viene generato l'evento esterno corrispondente, verrà richiamato il metodo Execute di questa interfaccia.
    {
        #region Private data members
        // Il valore dell'ultima richiesta effettuata dal modulo non modale
        private Request m_request = new Request();

        // Un instanza della finestra di dialogo
        private TypesSelectorForm _typesSelectorForm;

        // Dichiarazione della lista degli elementi scelti
        private List<Element> _elements;

        // Dichiarazione della lista degli Unit Type Identifier
        private List<string[]> _listUTI;

        // Dichiarazione della lista dei Panel Type Identifier
        private List<string[]> _listPTI;

        // Dichiarazione della lista degli Unit Type Identifier selezionati
        private List<string> _elementListUTI;

        // Dichiarazione della lista dei Panel Type Identifier selezionati
        private List<string> _elementListPTI;

        // Dichiarazazione della lista per il cambio colore
        private List<string[]> _changeColor;
        #endregion

        #region Class public property
        /// <summary>
        /// Proprietà pubblica per accedere al valore della richiesta corrente
        /// </summary>
        public Request Request
        {
            get { return m_request; }
        }

        /// <summary>
        /// Proprietà pubblica per accedere al valore della richiesta corrente
        /// </summary>
        public List<Element> GetElements
        {
            get { return _elements; }
        }

        /// <summary>
        /// Proprietà pubblica per accedere al valore della richiesta corrente
        /// </summary>
        public List<string[]> ListUTI
        {
            get { return _listUTI; }
        }

        /// <summary>
        /// Proprietà pubblica per accedere al valore della richiesta corrente
        /// </summary>
        public List<string[]> ListPTI
        {
            get { return _listPTI; }
        }
        #endregion

        #region Class public method
        /// <summary>
        /// Costruttore di default di RequestHandler
        /// </summary>
        public RequestHandler()
        {
            // Costruisce i membri dei dati per le proprietà
            _elements = new List<Element>();
            _elementListUTI = new List<string>();
            _elementListPTI = new List<string>();
            _changeColor = new List<string[]>();
        }
        #endregion

        /// <summary>
        ///   Un metodo per identificare questo gestore di eventi esterno
        /// </summary>
        public String GetName()
        {
            return "R2014 External Event Sample";
        }

        /// <summary>
        ///   Il metodo principale del gestore di eventi.
        /// </summary>
        /// <remarks>
        ///   Viene chiamato da Revit dopo che è stato generato l'evento esterno corrispondente 
        ///   (dal modulo non modale) e Revit ha raggiunto il momento in cui potrebbe 
        ///   chiamare il gestore dell'evento (cioè questo oggetto)
        /// </remarks>
        /// 
        public void Execute(UIApplication uiapp)
        {
            try
            {
                switch (Request.Take())
                {
                    case RequestId.None:
                        {
                            return;  // no request at this time -> we can leave immediately
                        }
                    case RequestId.Initial:
                        {

                            // Chiama il metodo di riempimento delle liste degli UTI e dei PTI
                            _elements = GetElementsfromDb(uiapp);
                            // Chiamo i metodi per il riempimento delle liste del UTI e del PTI
                            GetListUTI(uiapp, _elements);
                            GetListPTI(uiapp, _elements);
                            // Chiama i metodi di riempimento dei DataGridView
                            _typesSelectorForm = App.thisApp.RetriveForm();
                            _typesSelectorForm.SetDataGridViewUTI();
                            _typesSelectorForm.SetDataGridViewPTI();
                            //// Cambia il Detail level in Hidden Line
                            //ChangeDetailLevel(uiapp);
                            // Chiama il metodo che imposta il View Template
                            ApplyNewViewtemplate(uiapp);
                            break;
                        }
                    case RequestId.UTI:
                        {
                            // Chiama la lista degli elementi selezionati nel DataGridView1
                            _typesSelectorForm = App.thisApp.RetriveForm();
                            _elementListUTI = _typesSelectorForm.ElementList;
                            if(_elementListUTI.Count == 6)
                            {
                                ChoiceOfParameterAndChangeColor(uiapp, _elementListUTI);
                            }
                            _elementListUTI.Clear();
                            break;
                        }
                    case RequestId.PTI:
                        {
                            // Chiama la lista degli elementi selezionati nel DataGridView2                            
                            _typesSelectorForm = App.thisApp.RetriveForm();
                            _elementListPTI = _typesSelectorForm.ElementList;
                            if (_elementListPTI.Count == 5)
                            {
                                ChoiceOfParameterAndChangeColor(uiapp, _elementListPTI);
                            }
                            _elementListPTI.Clear();
                            break;
                        }
                    default:
                        {
                            // Una sorta di avviso qui dovrebbe informarci di una richiesta imprevista
                            break;
                        }
                }
            }
            finally
            {
                App.thisApp.WakeFormUp();
                App.thisApp.ShowFormTop();
            }

            return;
        }

        /// <summary>
        ///   Metodo richiamato nello switch
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="uiapp">L'oggetto Applicazione di Revit</param>m>
        /// 
        private List<Element> GetElementsfromDb(UIApplication uiapp)
        {
            List<Element> elements = new List<Element>();

            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Metodo per catturare i Curtain Panels del Document
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_CurtainWallPanels);
            collector.WherePasses(categoryFilter);

            foreach (Element element in collector)
            {
                if (null != element.Category && element.Category.HasMaterialQuantities)
                {
                    elements.Add(element);
                }
            }

            return elements;
        }

        /// <summary>
        ///   Metodo riempie le liste con gli UNIT Type Identifier
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="uiapp">L'oggetto Applicazione di Revit</param>m>
        /// 
        private void GetListUTI(UIApplication uiapp,List<Element> elements)
        {
            List<string[]> stringsList = new List<string[]>();
            List<string> tempUTI = new List<string>();
            List<string> newUTI = new List<string>();
            List<string> listColors = ColorsDrawing.GetColors();

            foreach (Element el in elements)
            {
                ElementId eTypeId = el.GetTypeId();
                ElementType eType = uiapp.ActiveUIDocument.Document.GetElement(eTypeId) as ElementType;

                string uti = PickUnitTypeIdentifier(uiapp, el);

                if (!uti.Contains("xx") && eType != null)
                {
                    tempUTI.Add(uti);
                }
            }

            // Rimuove i duplicati
            newUTI = RemoveDuplicates(tempUTI);
            // Li ordina in modo ascendente
            newUTI.OrderBy(x => x);
            
            for (int i = 0; i < newUTI.Count; i++)
            {
                for (int j = 0; j < listColors.Count; j++)
                {
                    if (i == j)
                    {
                        stringsList.Add(new[] { newUTI[i], listColors[j] });
                    }
                }
            }                
            
            _listUTI = stringsList;
        }

        /// <summary>
        ///   Metodo che riempie le liste con i PANEL Type Identifier
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="uiapp">L'oggetto Applicazione di Revit</param>m>
        /// 
        private void GetListPTI(UIApplication uiapp, List<Element> elements)
        {
            List<string[]> stringsList = new List<string[]>();
            List<string> tempPTI = new List<string>();
            List<string> newPTI = new List<string>();
            List<string> listColors = ColorsDrawing.GetColors();


            foreach (Element el in elements)
            {
                ElementId eTypeId = el.GetTypeId();
                ElementType eType = uiapp.ActiveUIDocument.Document.GetElement(eTypeId) as ElementType;

                string pti = PickPanelTypeIdentifier(uiapp, el);
                
                if (!pti.Contains("xx") && eType != null)
                {
                    tempPTI.Add(pti);
                }
            }

            // Rimuove i duplicati
            newPTI = RemoveDuplicates(tempPTI);
            // Li ordina in modo ascendente
            newPTI.OrderBy(x => x);

            for (int i = 0; i < newPTI.Count; i++)
            {
                for (int j = 100; j < listColors.Count; j++)
                {
                    if (i == (j - 100))
                    {
                        stringsList.Add(new[] { newPTI[i], listColors[j] });
                    }
                }
            }

            _listPTI = stringsList;
        }

        /// <summary>
        ///   Metodo che rimuove i duplicati
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="myList">Lista da ripulire dei duplicati</param>m>
        /// 
        public List<string> RemoveDuplicates(List<string> myList)
        {
            List<string> newList = new List<string>();
            foreach (string str in myList)
            {
                if (!newList.Contains(str))
                    newList.Add(str);
            }
            return newList;
        }

        // <summary>
        ///   La subroutine di selezione di un elemento che torna il valore stringa dell'Unit Identifier
        /// </summary>
        /// <remarks>
        /// Il valore dell'UnitIdentifier e' composto dai Parametri dell'elemento UI-ItemCategory, 
        /// UI-ProjectAbbreviation, UI-Quadrant, UI-FloorNumber e UI-UnitNumber
        /// </remarks>
        /// <param name="uiapp">L'oggetto Applicazione di Revit</param>m>
        /// 
        private string PickUnitTypeIdentifier(UIApplication uiapp, Element ele)
        {
            // Chiamo la vista attiva e seleziono gli elementi che mi servono
            UIDocument uidoc = uiapp.ActiveUIDocument;
            ElementType eleType = uidoc.Document.GetElement(ele.GetTypeId()) as ElementType;

            // Restituisce il valore del parametro UI-ItemCategory  
            string strUIItemCategory = "";
            if (eleType != null && eleType.LookupParameter("UI-ItemCategory") != null)
            {
                Parameter par = eleType.LookupParameter("UI-ItemCategory");
                strUIItemCategory = par.AsString();
            }
            else { strUIItemCategory = "xxx"; }

            // Restituisce il valore del parametro UI-ProjectAbbreviation  
            string strUIProjectAbbreviation = "";
            if (eleType != null && eleType.LookupParameter("UI-ProjectAbbreviation") != null)
            {
                Parameter par = eleType.LookupParameter("UI-ProjectAbbreviation");
                strUIProjectAbbreviation = par.AsString();
            }
            else { strUIProjectAbbreviation = "xxx"; }

            // Restituisce il valore del parametro UI-Quadrant
            string strUIQuadrant = "";
            if (ele.LookupParameter("UI-Quadrant") != null)
            {
                Parameter par = ele.LookupParameter("UI-Quadrant");
                strUIQuadrant = par.AsString();
            }
            else { strUIQuadrant = "xx"; }

            // Restituisce il valore del parametro UI-FloorNumber
            string strUIFloorNumber = "";
            if (ele.LookupParameter("UI-FloorNumber") != null)
            {
                Parameter par = ele.LookupParameter("UI-FloorNumber");
                strUIFloorNumber = par.AsString();
            }
            else { strUIFloorNumber = "xx"; }

            // Restituisce il valore del parametro UI-UnitNumber
            string strUIUnitNumber = "";
            if (ele.LookupParameter("UI-UnitNumber") != null)
            {
                Parameter par = ele.LookupParameter("UI-UnitNumber");
                strUIUnitNumber = par.AsString();
            }
            else { strUIUnitNumber = "xxx"; }

            // Imposta la stringa finale
            string _unitIdentifier =
                strUIItemCategory + "-" +
                strUIProjectAbbreviation + "-" +
                strUIQuadrant + "-" +
                strUIFloorNumber + "-" +
                strUIUnitNumber;

            return _unitIdentifier;
        }

        /// <summary>
        ///   La subroutine di selezione di un elemento che torna il valore stringa del Panel Type Identifier
        /// </summary>
        /// <remarks>
        /// Il valore del Panel Type Identifier e' composto dai Parametri dell'elemento PNT-ItemCategory, 
        /// PNT-ProjectAbbreviation, PNT-WallType e PNT-PanelType        
        /// </remarks>
        /// <param name="uiapp">L'oggetto Applicazione di Revit</param>m>
        /// 
        private string PickPanelTypeIdentifier(UIApplication uiapp, Element ele)
        {
            // Dall'elemento ottengo l'elementType
            UIDocument uidoc = uiapp.ActiveUIDocument;
            ElementType eleType = uidoc.Document.GetElement(ele.GetTypeId()) as ElementType;

            // Restituisce il valore del parametro PNT-ItemCategory
            string strPNTItemCategory = "";
            if (eleType != null && eleType.LookupParameter("PNT-ItemCategory") != null)
            {
                Parameter par = eleType.LookupParameter("PNT-ItemCategory");
                strPNTItemCategory = par.AsString();
            }
            else { strPNTItemCategory = "xxx"; }

            // Restituisce il valore del parametro PNT-ProjectAbbreviation
            string strPNTProjectAbbreviation = "";
            if (eleType != null && eleType.LookupParameter("PNT-ProjectAbbreviation") != null)
            {
                Parameter par = eleType.LookupParameter("PNT-ProjectAbbreviation");
                strPNTProjectAbbreviation = par.AsString();
            }
            else { strPNTProjectAbbreviation = "xxx"; }

            // Restituisce il valore del parametro PNT-WallType
            string strPNTWallType = "";
            if (eleType != null && eleType.LookupParameter("PNT-WallType") != null)
            {
                Parameter par = eleType.LookupParameter("PNT-WallType");
                strPNTWallType = par.AsString();
            }
            else { strPNTWallType = "xxxx"; }

            // Restituisce il valore del parametro PNT-PanelType
            string strPNTPanelType = "";
            if (eleType != null && ele.LookupParameter("PNT-PanelType") != null)
            {
                Parameter par = ele.LookupParameter("PNT-PanelType");
                strPNTPanelType = par.AsString();
            }
            else { strPNTPanelType = "xx"; }

            string _panelTypeIdentifier =
                strPNTItemCategory + "-" +
                strPNTProjectAbbreviation + "-" +
                strPNTWallType + "-" +
                strPNTPanelType;

            return _panelTypeIdentifier;
        }

        /// <summary>
        ///   Metodo che cambia il View Template
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="uiapp">L'oggetto Applicazione di Revit</param>m>
        /// 
        public void ApplyNewViewtemplate(UIApplication uiapp)
        {
            Autodesk.Revit.DB.View viewTemplate = new FilteredElementCollector(uiapp.ActiveUIDocument.Document)
                .OfClass(typeof(Autodesk.Revit.DB.View))
                .Cast<Autodesk.Revit.DB.View>()
                .Where(x => x.Name.Equals("3D - Curtain Wall") && x.IsTemplate == true)
                .FirstOrDefault();
            ElementId templateId = viewTemplate.Id;

            using (Transaction trans = new Transaction(uiapp.ActiveUIDocument.Document))
            {
                trans.Start("Change View Template");
                uiapp.ActiveUIDocument.Document.ActiveView.ViewTemplateId = templateId;
                trans.Commit();
            }

            uiapp.ActiveUIDocument.RefreshActiveView();
        }

        /// <summary>
        ///   Metodo che cambia il colore dei Pannelli
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="uiapp">L'oggetto Applicazione di Revit</param>m>
        /// 
        private void ChoiceOfParameterAndChangeColor(UIApplication uiapp, List<string> valueElements)
        {
            // Lista degli elementi che hanno l'identificatore selezionato
            List<Element> chosenElements = new List<Element>();

            foreach (Element ele in _elements)
            {
                ElementType eleType = uiapp.ActiveUIDocument.Document.GetElement(ele.GetTypeId()) as ElementType;

                if (eleType != null &&
                    eleType.LookupParameter("UI-ItemCategory") != null &&
                    eleType.LookupParameter("UI-ProjectAbbreviation") != null &&
                    eleType.LookupParameter("UI-ItemCategory").AsString() == valueElements[0])
                {
                    if (eleType.LookupParameter("UI-ProjectAbbreviation").AsString() == valueElements[1] &&
                        ele.LookupParameter("UI-Quadrant").AsString() == valueElements[2] &&
                        ele.LookupParameter("UI-FloorNumber").AsString() == valueElements[3] &&
                        ele.LookupParameter("UI-UnitNumber").AsString() == valueElements[04])
                    {
                        chosenElements.Add(ele);
                    }
                }
                else if (eleType != null &&
                    eleType.LookupParameter("PNT-ItemCategory") != null &&
                    eleType.LookupParameter("PNT-ProjectAbbreviation") != null &&
                    eleType.LookupParameter("PNT-WallType") != null &&
                    ele.LookupParameter("PNT-PanelType") != null &&
                    eleType.LookupParameter("PNT-ItemCategory").AsString() == valueElements[0])
                {
                    if (eleType.LookupParameter("PNT-ProjectAbbreviation").AsString() == valueElements[1] &&
                        eleType.LookupParameter("PNT-WallType").AsString() == valueElements[2] &&
                        ele.LookupParameter("PNT-PanelType").AsString() == valueElements[3])
                    {
                        chosenElements.Add(ele);
                    }
                }            
            }

            using (Transaction trans = new Transaction(uiapp.ActiveUIDocument.Document))
            {
                trans.Start("Change Color");

                // Metodo per la trasformazione del colore da System.Drawing.Color a Autodesk.Revit.DB.Color
                System.Drawing.Color colorToConvert = new System.Drawing.Color();
                if (valueElements.Count() == 6)
                {
                    colorToConvert = System.Drawing.Color.FromName(valueElements[5]);
                }
                else if(valueElements.Count() == 5)
                {
                    colorToConvert = System.Drawing.Color.FromName(valueElements[4]);
                }                
                Autodesk.Revit.DB.Color newColor = new Autodesk.Revit.DB.Color(colorToConvert.R, colorToConvert.B, colorToConvert.G);

                // Assegna il nuovo coloro all'elemento di override delle impostazioni grafiche
                OverrideGraphicSettings ogs = new OverrideGraphicSettings();
                ogs.SetSurfaceForegroundPatternColor(newColor);

                // Estrae il valore Id per settare il Pattern come Solid FIll(Riempimento)
                FilteredElementCollector elements = new FilteredElementCollector(uiapp.ActiveUIDocument.Document);
                FillPatternElement solidFillPattern = elements.OfClass(typeof(FillPatternElement))
                    .Cast<FillPatternElement>()
                    .First(a => a.GetFillPattern().IsSolidFill);
                ElementId solidFillPatternId = null;

                if (solidFillPattern.GetFillPattern().IsSolidFill)
                {
                    solidFillPatternId = solidFillPattern.Id;
                }

                // Imposta l'elemento come Solid Fill
                ogs.SetSurfaceForegroundPatternId(solidFillPatternId);

                foreach (Element el in chosenElements)
                {
                    // Fa l'override delle impostazioni grafiche dell'elemento
                    uiapp.ActiveUIDocument.Document.ActiveView.SetElementOverrides(el.Id, ogs);
                }

                trans.Commit();
            }
            //List<Element> control = chosenElements;
        }


        /// <summary>
        /// Metodo per la modifica della Livello di Dettaglio della View
        /// </summary>
        private void ChangeDetailLevel(UIApplication uiapp)
        {
            Autodesk.Revit.DB.View viewActive = uiapp.ActiveUIDocument.Document.ActiveView;
            Document doc = viewActive.Document;

            // Cambia il Livello di Dettaglio della View attiva
            using (Transaction tsx = new Transaction(doc))
            {
                tsx.Start("Change the View Detail Level");

                doc.ActiveView.get_Parameter(
                      BuiltInParameter.VIEW_DETAIL_LEVEL)
                        .Set(1);

                tsx.Commit();
            }

            //// metodo che salva il file in un percorso 
            //SaveChanges(uiapp);
        }

    }  // class

}  // namespace

