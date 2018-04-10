using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StaticAnalyser
{
    public partial class AnalysisOptionsList : Form
    {
        /** Whenever a new entry is added in analysis options' list, new entry is made in following objects
         * ListOfSelectedAnalysisOptions
         * PosOfOptionsInList
         * IndexOfAnalysisOptionsInTreeView
         * EnumAnalysisOptionsSelected
         */
        public enum EnumAnalysisOptionsSelected
        {
            SelectedFunctionsTreeView,
            SelectedNoOfStatmentsInAFunction,
            SelectedHighlightNestedFunctionCalls,
            SelectedFloatingPointOperations,
            SelectedIncludeHeaderFiles,
            SelectedDetailedViewOfCalledFUnctions,
            SelectedSingleNestedLoops,
            None
            
        };
        List<int> CheckedBoxOptionsSelectedList;
        public static List<EnumAnalysisOptionsSelected> ListOfSelectedAnalysisOptions;
        private StaticAnalyser ObjOfParentForm;
        
        public AnalysisOptionsList(StaticAnalyser ParentForm)
        {
            InitializeComponent();
            ObjOfParentForm = ParentForm;
            ListOfSelectedAnalysisOptions = new List<EnumAnalysisOptionsSelected>() // Whenever User wants to select Option(s),Reset List.
            {
               EnumAnalysisOptionsSelected.None,
               EnumAnalysisOptionsSelected.None,
               EnumAnalysisOptionsSelected.None,
               EnumAnalysisOptionsSelected.None,
               EnumAnalysisOptionsSelected.None,
               EnumAnalysisOptionsSelected.None,
               EnumAnalysisOptionsSelected.None
            };
        }

        private void BtnAnalysisOptionsSelcted_Click(object sender, EventArgs e)
        {
            try
            {
                /** Update Respective Fields of List from CheckListBox for Other Forms **/ 
                CheckedBoxOptionsSelectedList = AnalysisOptionsCheckedListBox.CheckedIndices.OfType<int>().ToList();

                for (int i = 0; i < CheckedBoxOptionsSelectedList.Count; i++)
                {
                    switch (CheckedBoxOptionsSelectedList[i])
                    {
                        case PosOfOptionsInList.PosFunctionTreeView:
                            ListOfSelectedAnalysisOptions[PosOfOptionsInList.PosFunctionTreeView] = EnumAnalysisOptionsSelected.SelectedFunctionsTreeView;
                            break;
                        case PosOfOptionsInList.PosNoOfStatmentsInAFunction:
                            ListOfSelectedAnalysisOptions[PosOfOptionsInList.PosNoOfStatmentsInAFunction] = EnumAnalysisOptionsSelected.SelectedNoOfStatmentsInAFunction;
                            break;
                        case PosOfOptionsInList.PosHighlightNestedFunctionCalls:
                            ListOfSelectedAnalysisOptions[PosOfOptionsInList.PosHighlightNestedFunctionCalls] = EnumAnalysisOptionsSelected.SelectedHighlightNestedFunctionCalls;
                            break;
                        case PosOfOptionsInList.PosFloatingPointOperations:
                            ListOfSelectedAnalysisOptions[PosOfOptionsInList.PosFloatingPointOperations] = EnumAnalysisOptionsSelected.SelectedFloatingPointOperations;
                            break;
                        case PosOfOptionsInList.PosIncludeHeaderFiles:
                            ListOfSelectedAnalysisOptions[PosOfOptionsInList.PosIncludeHeaderFiles] = EnumAnalysisOptionsSelected.SelectedIncludeHeaderFiles;
                            break;
                        case PosOfOptionsInList.PosDetailedViewOfCalledFunctions:
                            ListOfSelectedAnalysisOptions[PosOfOptionsInList.PosDetailedViewOfCalledFunctions] = EnumAnalysisOptionsSelected.SelectedDetailedViewOfCalledFUnctions;
                            break;
                        case PosOfOptionsInList.PosSingleNestedLoops:
                            ListOfSelectedAnalysisOptions[PosOfOptionsInList.PosDetailedViewOfCalledFunctions] = EnumAnalysisOptionsSelected.SelectedSingleNestedLoops;
                            break;
                    }
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
         }
    }
}

