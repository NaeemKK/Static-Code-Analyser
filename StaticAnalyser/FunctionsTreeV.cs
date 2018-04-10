using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace StaticAnalyser
{
    public partial class FunctionsTreeView : Form
    {
        private StaticAnalyser ObjOfParentForm;
        List<string> CallerFuncationNames;
        List<string> CalledFuncationNames;
        string FileName;
        int CounterForSourceFilesList =0;
        

        public FunctionsTreeView(StaticAnalyser ParentForm)
        {
            InitializeComponent();
            ObjOfParentForm =ParentForm;
            CalledFuncationNames = new List<string>();
            CallerFuncationNames = new List<string>();
        }

        private void FunctionsTreeView_Load(object sender, EventArgs e)
        {
            string PathToSourceCodeFiles,LineofFile,PathOfCalledFunctionsFile, StartStatementFilePath,EndStatementFilePath;
            List<string> FileContentOfFloatingPointingOperations = new List<string>();
            List<string> FileContentsOfLoops = new List<string>();
            List<string> ListStartStatement = new List<string>(),ListEndStatement= new List<string>();
            int CountOfCalledFunction=0;
            int IndexForNode = -1;// initialize it with -1
            try
            {
                TreeViewTool.Nodes.Clear();
                #region Set Index For SubNodes Of Analysis Options
                IndexOfAnalysisOptionsInTreeView.FunctionListIndex = 0;
                /** These are variables on basis of which Nodes shall be created **/
                if (ObjOfParentForm.CalledFunctions == 1)
                {
                    IndexForNode++;
                    IndexOfAnalysisOptionsInTreeView.CalledFunctionsIndex = IndexForNode;
                }
                if (ObjOfParentForm.NoOfStatementsInFunction == 1)
                {
                    IndexForNode++;
                    IndexOfAnalysisOptionsInTreeView.NoOfLinesIndex = IndexForNode;
                }
                if (ObjOfParentForm.FloatingPointOperations == 1)
                {
                    IndexForNode++;
                    IndexOfAnalysisOptionsInTreeView.FloatingPointOperationsIndex = IndexForNode;
                }
                if (ObjOfParentForm.SingleNestedLoop == 1)
                {
                    IndexForNode++;
                    IndexOfAnalysisOptionsInTreeView.SingleNestedLoops = IndexForNode;
                }
                #endregion

                #region Fill Info Of Each File
                foreach (var FileNamePath in ObjOfParentForm.ListOfPathToMainFunctionsReports)
                {
                    PathToSourceCodeFiles = ObjOfParentForm.ListOfFilesNamesAlongwithPath.ElementAt<string>(CounterForSourceFilesList); //Get Full Path
                    FileName = PathToSourceCodeFiles.Substring(PathToSourceCodeFiles.LastIndexOf(@"\") + 1);// Get the file name only
                    /*Create Node Of File and Add Functions To that Node Of A file */
                    TreeViewTool.Nodes.Add(new TreeNode(FileName));// Create Node with File Name
                    CallerFuncationNames = File.ReadLines(FileNamePath).ToList();
                    /** Add Fields Of Static Analysis Collection **/
                    TreeViewTool.Nodes[ObjOfParentForm.ListOfPathToMainFunctionsReports.IndexOf(FileNamePath)].Nodes.Add(
                    new TreeNode("Functions List")); // Add childs to Parent Node i.e File Name
                    
                    #region Make List Of Functions Start Line and End Line
                    if (ObjOfParentForm.NoOfStatementsInFunction == 1) // For Later Use
                    {
                        StartStatementFilePath = FileNamePath.Replace("_Caller", "_Statements_Start"); // Get the Start Statment File Path
                        EndStatementFilePath = FileNamePath.Replace("_Caller", "_Statements_End"); // Get the End Statment File Path
                        /** If any of these two files is not present then dont show any no of stsments for that file **/
                        if (File.Exists(StartStatementFilePath)) 
                        {
                            ListStartStatement = File.ReadLines(StartStatementFilePath).ToList();
                            if (File.Exists(EndStatementFilePath))
                                ListEndStatement = File.ReadLines(EndStatementFilePath).ToList();
                            else
                            {
                                ListStartStatement.Clear();
                                ListEndStatement.Clear();
                            }
                        }
                        else
                        {
                            ListStartStatement.Clear();
                            ListEndStatement.Clear();
                        }
                    }
                    if (ObjOfParentForm.FloatingPointOperations == 1)
                    {
                        if (ObjOfParentForm.ListOfFileNamesContainigFloatingPointOperations.IndexOf(FileName) != -1)
                        {
                            FileContentOfFloatingPointingOperations = File.ReadAllLines(FileNamePath.Replace("_Caller.txt", "_F_P_O.txt")).ToList<string>();
                        }
                    }
                    if (ObjOfParentForm.SingleNestedLoop == 1)
                    {
                        FileContentsOfLoops = File.ReadAllLines(FileNamePath.Replace("_Caller.txt", "_LoopData.txt")).ToList<string>();
                    }
                    #endregion

                    #region Fill Subfields Of Tree
                    /** Add Functions To File Name **/
                    #region Formatig Of Data According to Tree

                    int CallerFunctionCounter = 0;
                    foreach (var FunctionName in CallerFuncationNames)
                    {
                        /** Extract Called Functions in That Function**/
                        PathOfCalledFunctionsFile = FileNamePath.Replace("Caller", "Called");
                        CalledFuncationNames.Clear();
                        if (ObjOfParentForm.DetailedCalledFunctionsTreeView == 1) // Using cscope Command
                        {
                            using (StreamReader file = new StreamReader(PathOfCalledFunctionsFile))
                            {
                                while ((LineofFile = file.ReadLine()) != null)
                                {
                                    if (LineofFile.Substring(0, LineofFile.IndexOf("#")).Contains(FunctionName)) // If line conatains name at start
                                    {
                                        CalledFuncationNames.Add(LineofFile.Substring(LineofFile.IndexOf("#") + 1)); //Get string after "#".Upto # is the name of Caller Function
                                    }
                                }
                            }
                        }
                        else //using Cflow
                        {
                            using (StreamReader file = new StreamReader(PathOfCalledFunctionsFile))
                            {
                                int CallerFunctionFound;
                                CallerFunctionFound = 0;
                                while ((LineofFile = file.ReadLine()) != null) // read if it is not Null
                                {
                                    if (LineofFile.Substring(0, LineofFile.IndexOf(" ")).Contains(FunctionName)) // If line conatains Function Name and then space after function name
                                    {
                                        CallerFunctionFound = 1; // It means FUnction Found
                                        break;
                                    }
                                }
                                if (CallerFunctionFound == 1)
                                {
                                    while (true)
                                    {
                                        LineofFile = file.ReadLine();
                                        if (LineofFile != null) // It shall ensure LineofFile[0] has any valid character and no exception occur in next if statment
                                        {
                                            if (LineofFile[0] == ' ') // Now all next lines are of Called FUnction until null or some character is found at start location.File 
                                            {  // is saved in this format
                                                CalledFuncationNames.Add(LineofFile.Substring(LineofFile.IndexOf(" ")).Replace(@"TmpFolderForSourceFiles\","")); //Get string after " ".
                                            }
                                            else
                                                break;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    #endregion

                        TreeViewTool.Nodes[ObjOfParentForm.ListOfPathToMainFunctionsReports.IndexOf(FileNamePath)].
                        Nodes[IndexOfAnalysisOptionsInTreeView.FunctionListIndex].Nodes.Add(new TreeNode(FunctionName)); // Add Caller Function Name Node
                        if (ObjOfParentForm.CalledFunctions == 1)
                        {
                            
                            TreeViewTool.Nodes[ObjOfParentForm.ListOfPathToMainFunctionsReports.IndexOf(FileNamePath)].
                                Nodes[IndexOfAnalysisOptionsInTreeView.FunctionListIndex].Nodes[CallerFuncationNames.IndexOf(FunctionName)].Nodes.Add(
                                new TreeNode("Called Functions")); // Add Called Function Node to Caller Function Node

                            CountOfCalledFunction = 0;
                            foreach (var CalledFunction in CalledFuncationNames)
                            {
                                TreeViewTool.Nodes[ObjOfParentForm.ListOfPathToMainFunctionsReports.IndexOf(FileNamePath)].
                                Nodes[IndexOfAnalysisOptionsInTreeView.FunctionListIndex].Nodes[CallerFuncationNames.IndexOf(FunctionName)].Nodes[IndexOfAnalysisOptionsInTreeView.CalledFunctionsIndex].Nodes.Add(
                                new TreeNode(CalledFunction)); // Add Subchild
                                if (ObjOfParentForm.HighlightNestedFunctionCalls == 1)
                                {
                                    if (Regex.Match(CalledFunction, "\b" + FunctionName + "\b").Success) // Highligt Nested Function Call
                                    {
                                        TreeViewTool.Nodes[ObjOfParentForm.ListOfPathToMainFunctionsReports.IndexOf(FileNamePath)].
                                         Nodes[IndexOfAnalysisOptionsInTreeView.FunctionListIndex].Nodes[CallerFuncationNames.IndexOf(FunctionName)].Nodes[IndexOfAnalysisOptionsInTreeView.CalledFunctionsIndex].
                                         Nodes[CountOfCalledFunction].BackColor = Color.Yellow;
                                    }
                                }
                                CountOfCalledFunction++;
                            }
                        }
                        if (ObjOfParentForm.NoOfStatementsInFunction == 1)
                        {
                            TreeViewTool.Nodes[ObjOfParentForm.ListOfPathToMainFunctionsReports.IndexOf(FileNamePath)].
                            Nodes[IndexOfAnalysisOptionsInTreeView.FunctionListIndex].Nodes[CallerFuncationNames.IndexOf(FunctionName)].Nodes.Add(
                            new TreeNode("Total No. Of Lines")); // Add no of Lines Node to Caller Function Node

                            /** Now there are two lists.One of Start Index of Function and another one Last Index of a function
                             * and they are sorted in same manner as CallerFunctions are sorted.Now it means that each "very next row value" 
                             * to the selected function start index should be greater than the End index of this function stored in End list
                             * becuase that "very next row value is the start index of next function **/
                            if (ListEndStatement.Count == ListStartStatement.Count)
                            {
                                if ((CallerFunctionCounter + 1) < CallerFuncationNames.Count)
                                {
                                    int NoOfStatmentsCount;
                                    if (Int32.Parse(ListEndStatement[CallerFunctionCounter]) < Int32.Parse(ListStartStatement[CallerFunctionCounter + 1]))
                                    {
                                        NoOfStatmentsCount = Int32.Parse(ListEndStatement[CallerFunctionCounter]) - Int32.Parse(ListStartStatement[CallerFunctionCounter]) - 3; // name + { + } = 3
                                    }
                                    else // If somehow it is greater then minus 4 from next Function start Index.Expecting no space from another function
                                    {
                                        NoOfStatmentsCount = Int32.Parse(ListStartStatement[CallerFunctionCounter + 1]) - Int32.Parse(ListStartStatement[CallerFunctionCounter]) - 4;// name + { + } + NameofNext = 4
                                    }
                                    TreeViewTool.Nodes[ObjOfParentForm.ListOfPathToMainFunctionsReports.IndexOf(FileNamePath)].
                                     Nodes[IndexOfAnalysisOptionsInTreeView.FunctionListIndex].Nodes[CallerFuncationNames.IndexOf(FunctionName)].Nodes[IndexOfAnalysisOptionsInTreeView.NoOfLinesIndex].Nodes.
                                     Add(new TreeNode(Convert.ToString(NoOfStatmentsCount)));

                                }
                                else // Last Function
                                {
                                    int NoOfStatmentsCount;
                                    if (Int32.Parse(ListEndStatement[CallerFunctionCounter]) > Int32.Parse(ListStartStatement[CallerFunctionCounter]))
                                    {
                                        NoOfStatmentsCount = Int32.Parse(ListEndStatement[CallerFunctionCounter]) - Int32.Parse(ListStartStatement[CallerFunctionCounter]) - 3;
                                    }
                                    else
                                    {
                                        NoOfStatmentsCount = Int32.Parse(ListEndStatement[CallerFunctionCounter]) - File.ReadAllLines(PathToSourceCodeFiles).Count() - 3; // Its Last function in  a file.Expecting no extra lines at the end of file
                                    }
                                    TreeViewTool.Nodes[ObjOfParentForm.ListOfPathToMainFunctionsReports.IndexOf(FileNamePath)].
                                     Nodes[IndexOfAnalysisOptionsInTreeView.FunctionListIndex].Nodes[CallerFuncationNames.IndexOf(FunctionName)].Nodes[IndexOfAnalysisOptionsInTreeView.NoOfLinesIndex].Nodes.
                                     Add(new TreeNode(Convert.ToString(NoOfStatmentsCount)));
                                }
                            }
                            else
                            {
                                MessageBox.Show("Problem Occured in Processing Function " + FunctionName);
                            }
 
                        }
                        if (ObjOfParentForm.FloatingPointOperations == 1)
                        {
                            TreeViewTool.Nodes[ObjOfParentForm.ListOfPathToMainFunctionsReports.IndexOf(FileNamePath)].
                            Nodes[IndexOfAnalysisOptionsInTreeView.FunctionListIndex].Nodes[CallerFuncationNames.IndexOf(FunctionName)].Nodes.Add(
                            new TreeNode("Floating Point Operations")); // Add Floating Point Operations Node to Caller Function Node
                            if (ObjOfParentForm.ListOfFileNamesContainigFloatingPointOperations.Contains(FileName))
                            {
                                bool FunctionWithFloatingPointOperationsFound = false;
                                foreach (var Line in FileContentOfFloatingPointingOperations)
                                {
                                    if (Int32.Parse(Line.Substring(0, Line.IndexOf(" "))) == CallerFunctionCounter)
                                    {
                                        TreeViewTool.Nodes[ObjOfParentForm.ListOfPathToMainFunctionsReports.IndexOf(FileNamePath)].
                                            Nodes[IndexOfAnalysisOptionsInTreeView.FunctionListIndex].Nodes[CallerFuncationNames.IndexOf(FunctionName)].Nodes[IndexOfAnalysisOptionsInTreeView.FloatingPointOperationsIndex].Nodes.
                                            Add(new TreeNode(Line.Substring(Line.IndexOf(" ") + 1)));
                                        FunctionWithFloatingPointOperationsFound = true;
                                    }
                                }
                                if (FunctionWithFloatingPointOperationsFound == false)
                                {
                                    TreeViewTool.Nodes[ObjOfParentForm.ListOfPathToMainFunctionsReports.IndexOf(FileNamePath)].
                                    Nodes[IndexOfAnalysisOptionsInTreeView.FunctionListIndex].Nodes[CallerFuncationNames.IndexOf(FunctionName)].Nodes[IndexOfAnalysisOptionsInTreeView.FloatingPointOperationsIndex].Nodes.
                                    Add(new TreeNode("None"));
                                }
                            }
                            else
                            {
                                TreeViewTool.Nodes[ObjOfParentForm.ListOfPathToMainFunctionsReports.IndexOf(FileNamePath)].
                                Nodes[IndexOfAnalysisOptionsInTreeView.FunctionListIndex].Nodes[CallerFuncationNames.IndexOf(FunctionName)].Nodes[IndexOfAnalysisOptionsInTreeView.FloatingPointOperationsIndex].Nodes.
                                Add(new TreeNode("None"));
                            }
                        }
                        if (ObjOfParentForm.SingleNestedLoop == 1)
                        {
                            TreeViewTool.Nodes[ObjOfParentForm.ListOfPathToMainFunctionsReports.IndexOf(FileNamePath)].
                            Nodes[IndexOfAnalysisOptionsInTreeView.FunctionListIndex].Nodes[CallerFuncationNames.IndexOf(FunctionName)].Nodes.Add(
                            new TreeNode("Single/Nested Loops")); // Add Floating Point Operations Node to Caller Function Node
                            foreach (var Line in FileContentsOfLoops)
                            {
                                if (Line.Substring(0, Line.IndexOf(" ")).Contains(FunctionName))
                                {
                                    TreeViewTool.Nodes[ObjOfParentForm.ListOfPathToMainFunctionsReports.IndexOf(FileNamePath)].
                                            Nodes[IndexOfAnalysisOptionsInTreeView.FunctionListIndex].Nodes[CallerFuncationNames.IndexOf(FunctionName)].Nodes[IndexOfAnalysisOptionsInTreeView.SingleNestedLoops].Nodes.
                                            Add(new TreeNode(Line.Substring(Line.IndexOf(" "))));
                                }
                                else
                                {
                                    TreeViewTool.Nodes[ObjOfParentForm.ListOfPathToMainFunctionsReports.IndexOf(FileNamePath)].
                                            Nodes[IndexOfAnalysisOptionsInTreeView.FunctionListIndex].Nodes[CallerFuncationNames.IndexOf(FunctionName)].Nodes[IndexOfAnalysisOptionsInTreeView.SingleNestedLoops].Nodes.
                                            Add(new TreeNode("None"));
                                    break;
                                }
                            }
                        }
                        CallerFunctionCounter++;
                    }
                    #endregion
                    CounterForSourceFilesList++;
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                CalledFuncationNames.Clear();
                CallerFuncationNames.Clear();
            }
        }
    }
    
    public class PosOfOptionsInList
    {
        public const int PosFunctionTreeView = 0, PosNoOfStatmentsInAFunction = 1, PosHighlightNestedFunctionCalls = 2,
               PosFloatingPointOperations = 3, PosIncludeHeaderFiles = 4,PosDetailedViewOfCalledFunctions = 5,PosSingleNestedLoops = 6; // CheckListBox options
    }
    public class IndexOfAnalysisOptionsInTreeView
    {
        public static int FunctionListIndex,CalledFunctionsIndex,NoOfLinesIndex,FloatingPointOperationsIndex,SingleNestedLoops;
    }
}
