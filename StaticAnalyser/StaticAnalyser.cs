/*** 
 *  Hierarchy Of Code
 *  1. Copy All Files to Tmp Directory
 *  2. Remove Comments within code for Better and accurate Results and save in _tmp.c
 *  3. Check For User Preprocessor Directive like #ifdef etc and save results back in .c
 *  4. If user does not select Preprocessor Directive option then manually remove .c and rename _tmp.c to .c File
 *  5. Now generate Function Tree by storing Caller and Called functions Reults
 *  6.If user has selected DetailedCalledFunctionName then use cscope else cflow.
 *  7. If user has selected number of lines in code then find starting line of function from Caller functions reults and store in _start file and for ending bracket,
 *  use cut command and take first two coloums and then find "}" bracket and store its line number in _end file
 *  Now Make Tree of these reults.
 * 
 * 
 */


#define GET_END_LINE_NUMBER_OF_EACH_FUNCTION_USING_CUT_COMMAND


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
using System.Threading;

namespace StaticAnalyser
{
    public partial class StaticAnalyser : Form
    {
        enum EnumFileOrFolder
        {
            File,
            Folder
        };
        public int FunctionsTreeView, HighlightNestedFunctionCalls, FloatingPointOperations, IncludeHeaderFiles, NoOfStatementsInFunction, DetailedCalledFunctionsTreeView, CalledFunctions,SingleNestedLoop;
        EnumFileOrFolder FileOrFolder;
        string[] CallerFunctionsNameCmdArgument, CalledCompactFunctionsNameCmdArgument, CalledDetailedFunctionsNameCmdArgument, NoOfStatmentsCmdArgument_1, UnifdefCmdArgument, RemoveCommentsCmdArgument,
            GetGlobalVarsDefLocationInEachFileCmdArgument, GetStartLineNumberOfEachFunctionCmdArgument;
#if GET_END_LINE_NUMBER_OF_EACH_FUNCTION_USING_CUT_COMMAND
        string[] NoOfStatmentsCmdArgument_2;
#else
        string[][] NoOfStatmentsCmdArgument_2;
#endif
        List<string> GetLocalFloatDoubleVarsUseInfoInEachFileCmdArgument;
        string GetGlobalFloatDoubleVarDefCmdArgument, GetLocalFloatDoubleVarDefCmdArgument, GetFunctionsdefinitionWithLocalFloatDoubleVarCmdArgument,
            GetFileNamesOfAllFloatDoubleUsersCmdArgument,ArrangeAllFloatDoubleInProperFormat;
        string Dos2UnixCommandArgument, Remove_OutDataBaseCmdArgument, SetTmpDirCmdArgumentForCscope, RemoveAnyTmpDirectoryCmdArgument, GetFileNamesForLocalVarsOfFloatDoubleCmdArgument, GetFileNamesForLocalVarsOfFloatDoubleFromFuncDefCmdArgument;
        string AnanlysisDirectory, TmpFolderForSourceFiles;
        string[] FilePathOfStatemetsStart;
        public List<string> ListOfPathToMainFunctionsReports, ListOfFilesNamesAlongwithPath, ListOfDefMacrosProvidedByUser, ListOfUnDefMacrosProvidedByUser;
        bool DebugMode = false,IsDefBoxChecked,IsUnDefBoxChecked;
        BackgroundWorker ObjBackGroundWorker;
        bool BackGroundWorkRetVal = true;
        string ErrorArray;
        List<string> ListOfGlobalFloatDoubleVar, ListOfLocalFloatDoubleVar;
        List<string> ListOfFileNamesOfFloatDoubleVars;
        List<List<string>> ListOfLocalVarsAlongWithFileNames;
        public List<string> ListOfFileNamesContainigFloatingPointOperations;
        string ProgressReport;
        List<string> ListOfFileNamesOfFloatDoubleVarsFromFuncDef;

        public StaticAnalyser()
        {
            InitializeComponent();
            ListOfFilesNamesAlongwithPath = new List<string>();
            ListOfPathToMainFunctionsReports = new List<string>();
            this.FormClosing += new FormClosingEventHandler(BtnClose_Click);
            /** BackGround Worker **/
            ObjBackGroundWorker = new BackgroundWorker();
            // Create a background worker thread that ReportsProgress &
            // Hook up the appropriate events.
            ObjBackGroundWorker.DoWork += new DoWorkEventHandler(ObjBackGroundWorker_DoWork);
            ObjBackGroundWorker.ProgressChanged += new ProgressChangedEventHandler
                    (ObjBackGroundWorker_ProgressChanged);
            ObjBackGroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler
                    (ObjBackGroundWorker_RunWorkerCompleted);
            ObjBackGroundWorker.WorkerReportsProgress = true;
            ObjBackGroundWorker.WorkerSupportsCancellation = true;
            ErrorArray = string.Empty;
            ListOfUnDefMacrosProvidedByUser = new List<string>();
            ListOfDefMacrosProvidedByUser = new List<string>();
            ListOfGlobalFloatDoubleVar = new List<string>();
            ListOfLocalFloatDoubleVar = new List<string>();
            ListOfFileNamesOfFloatDoubleVars = new List<string>();
            ListOfLocalVarsAlongWithFileNames = new List<List<string>>();
            ListOfFileNamesContainigFloatingPointOperations = new List<string>();
            ProgressReport = string.Empty;
        }

        /// On completed do the appropriate task
        void ObjBackGroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // The background process is complete. We need to inspect
            // our response to see if an error occurred or if we completed successfully.  
            if (ErrorArray != string.Empty)
            {
                MessageBox.Show("Errors :" + Environment.NewLine + ErrorArray);
            }
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else
            {
                // Everything completed normally.
                
                // Set cursor as default arrow
                if (BackGroundWorkRetVal == true)
                {
                    Cursor.Current = Cursors.Default;
                    AnalysisProgressBar.Value = 100;
                    MessageBox.Show("Analysis Report Is Ready");
                    this.Text = "Static Analyser";
                    ProgressLabel.Visible = false;
                    FunctionsTreeView CreateFunctionTree = new FunctionsTreeView(this);
                    CreateFunctionTree.ShowDialog();
                    CreateFunctionTree.Dispose();
                }
            }
            

            /** Reset All **/
            Create_RemoveDataBaseCommand();
            ExecuteCommand(Remove_OutDataBaseCmdArgument);
            Create_RemoveAnyTmpDirectoryCommand(AnanlysisDirectory);
            ExecuteCommand(RemoveAnyTmpDirectoryCmdArgument);
            Create_RemoveAnyTmpDirectoryCommand(TmpFolderForSourceFiles);
            ExecuteCommand(RemoveAnyTmpDirectoryCmdArgument);

            ResetGui();
        }

        /// Notification is performed here to the progress bar
        void ObjBackGroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            AnalysisProgressBar.Value = e.ProgressPercentage;
            ProgressLabel.Text = ProgressReport;
        }

        /// Time consuming operations go here
        void ObjBackGroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int Count = 0;
            bool RetVal = true;
            string FileName;
            int ProgressBarVal = 2; // start from 2
            ProgressReport = string.Empty;
            //NOTE : Never play with the UI thread here...
            if( FileOrFolder == EnumFileOrFolder.File )
                ProgressReport = "Analysing File...";
            else
                ProgressReport = "Analysing Files...";
            ObjBackGroundWorker.ReportProgress(ProgressBarVal);
            RetVal = true;
            /** Change Tmp Directory For Cscope **/
            SetTmpDirForCscope();
            ExecuteCommand(SetTmpDirCmdArgumentForCscope);
            #region Create Tmp Directory And Copy Source Files
            /** Copy All Files To the Tmp Directory within Project **/
            CreateTempFolderForSourceFilesToBeAnalysed(); // Create Temp Directory
            /** Create Analysis Report Directory**/
            CreateAnalysisDirectory();
            /** cp command requires unix style path unlike cscope or ctag ***/
            string LinuxStylePathOfSourceFiles = WindowsToUnixStyleCommand(FileFolderPath.Text);

            if (FileOrFolder == EnumFileOrFolder.Folder)
            {
                if (System.IO.Directory.Exists(TmpFolderForSourceFiles))
                {
                    /** Copy C Files **/
                    /** Add File with Path to the list for further functionality**/
                    string DestFile;
                    DirectoryInfo Folder = new DirectoryInfo(FileFolderPath.Text);
                    FileInfo[] C_Files = Folder.GetFiles("*.c"); //Getting Source files
                    foreach (FileInfo SourceFilePath in C_Files)
                    {
                        FileName = System.IO.Path.GetFileName(SourceFilePath.ToString());
                        DestFile = System.IO.Path.Combine(TmpFolderForSourceFiles, FileName);
                        ListOfFilesNamesAlongwithPath.Add(DestFile);
                    }
                    RetVal &= ExecuteCommand("cp " + LinuxStylePathOfSourceFiles + "/*.c " + TmpFolderForSourceFiles); // copy all c files
                    /** Copy All Headers File **/
                    RetVal &= ExecuteCommand("cp " + LinuxStylePathOfSourceFiles + "/*.h " + TmpFolderForSourceFiles); // copy all h files
                }
                else
                {
                    RetVal = false;
                }
                if(RetVal == false)
                    ErrorArray = string.Concat(ErrorArray,"Folder Permissions Issue.Report it to developer." + Environment.NewLine);
            }
            else // File
            {
                if (System.IO.Directory.Exists(TmpFolderForSourceFiles))
                {
                    FileName = System.IO.Path.GetFileName(FileFolderPath.Text);
                    string DestFile = System.IO.Path.Combine(TmpFolderForSourceFiles, FileName);
                    System.IO.File.Copy(FileFolderPath.Text, DestFile, true); // Copy C file to destination
                    /** Add File with Path to the list for further functionality**/
                    ListOfFilesNamesAlongwithPath.Add(DestFile);
                    /** Copy All Headers File **/
                    RetVal &= ExecuteCommand("cp " + LinuxStylePathOfSourceFiles.Remove(LinuxStylePathOfSourceFiles.LastIndexOf("/")) + "/*.h " + TmpFolderForSourceFiles); // copy all h files
                }
                else
                {
                    RetVal = false;
                }
                if (RetVal == false)
                    ErrorArray = string.Concat(ErrorArray, "Folder Permissions Issue.Report it to developer." + Environment.NewLine);
            }
            /** Update Preogressbar**/
            ObjBackGroundWorker.ReportProgress(ProgressBarVal + 8); // 10 percent
            #endregion
            if (RetVal == true)
            {
                #region Remove Comments From File
                PrepareCommands(PrepareCommandsType.RemoveComments, 0);
                Count = 0;
                foreach (var File in ListOfFilesNamesAlongwithPath)
                {
                    ExecuteCommand(RemoveCommentsCmdArgument[Count]); // Files shall be saved in _Tmp.c
                    Count++;
                    /** Update Preogressbar**/
                    ProgressBarVal = (((Count / ListOfFilesNamesAlongwithPath.Count) * 100) % 15) + 10; // (Percentage)%15.Never show progress more than 25 percent for 
                    //this operation.
                    ObjBackGroundWorker.ReportProgress(ProgressBarVal);
                }
                #endregion
                #region Check For Preprocessor Directive Provided By User And Fix Code Tmp Files
                /** Check and Execute if User has provided any Macros **/
                if (DefinedCheckBox.Checked == true || UnDefCheckBox.Checked == true)
                {
                    ProgressReport = "Processing Conditional Directives..";
                    ObjBackGroundWorker.ReportProgress(ProgressBarVal + 8); // To update Progress Label

                    PrepareCommands(PrepareCommandsType.FixPreprocessorDirectiveIssues, 0);
                    /** Execute Command for Macros **/
                    Count = 0;
                    foreach (String Str in ListOfFilesNamesAlongwithPath)
                    {
                        RetVal &= ExecuteCommand(UnifdefCmdArgument[Count]);
                        Count++;
                        /** Update Preogressbar**/
                        ProgressBarVal = (((Count / ListOfFilesNamesAlongwithPath.Count) * 100) % 25) + 25; // (Percentage)%15.Never show progress more than 40 percent for 
                        //this operation.
                        ObjBackGroundWorker.ReportProgress(ProgressBarVal);
                    }
                    if (RetVal == false)
                        ErrorArray = string.Concat(ErrorArray, "Problem occured in UnifdefCmdArgument");
                    RetVal = true;// No need to take decision for failure for now
                }
                else
                {
                    Count = 0;
                    // If Unifdef command is not executed by User then change files name from _tmp.c to .c manually
                    foreach (var File in ListOfFilesNamesAlongwithPath)
                    {
                        if (System.IO.File.Exists(File) && System.IO.File.Exists(File.Replace(".c", "_Tmp.c")))
                        {
                            System.IO.File.Delete(File); // delete .c File so that _tmp.c can be saved with same name
                            System.IO.File.Copy(File.Replace(".c", "_Tmp.c"), File); // Copy back _Tmp.c to .c File
                        }
                        /** Update Preogressbar**/
                        ProgressBarVal = (((Count / ListOfFilesNamesAlongwithPath.Count) * 100) % 15) + 25; // (Percentage)%15.Never show progress more than 40 percent for 
                        //this operation.
                        ObjBackGroundWorker.ReportProgress(ProgressBarVal);
                        Count++;
                    }
                }
                // Now delete _Tmp.c Files
                RetVal = ExecuteCommand("del " + TmpFolderForSourceFiles+ @"\*_Tmp.c");
                if (RetVal == true)
                {
                #endregion
                    #region FunctionTreeView
                    if (FunctionsTreeView == 1)
                    {
                        ProgressReport = "Getting Functions Info..";
                        ObjBackGroundWorker.ReportProgress(ProgressBarVal);
                        Count = 0;
                        /*** Build Information Necessary for all other Operations ***/
                        /** Prepare All Commands For Each Function in one go **/
                        PrepareCommands(PrepareCommandsType.ShowFunctionsTreeView, 0);
                        if (DetailedCalledFunctionsTreeView != 1)
                            PrepareCommands(PrepareCommandsType.ShowCompactCalledFunctionsNames, 0); // build using cflow command
                        foreach (String Str in ListOfFilesNamesAlongwithPath)
                        {
                            /** Execute Commands for Functions Tree and Other Functionality **/
                            RetVal = ExecuteCommand(CallerFunctionsNameCmdArgument[Count]);// this String must always have commands equal to no of files
                            if (RetVal == true)
                            {
                                if (DetailedCalledFunctionsTreeView == 1)
                                {
                                    RetVal = ExecuteCommand(CalledDetailedFunctionsNameCmdArgument[Count]);// this String must always have commands equal to no of files
                                }
                                else
                                {
                                    RetVal = ExecuteCommand(CalledCompactFunctionsNameCmdArgument[Count]);// this String must always have commands equal to no of files
                                }
                                if (RetVal == false)
                                {
                                    /** Write Error Occured in that File in case of failure of Command Execution **/
                                    List<string> TmpListForCallerFunctionName;
                                    string TmpFileName = Str.Substring(Str.LastIndexOf(@"\") + 1);
                                    TmpListForCallerFunctionName = File.ReadLines(AnanlysisDirectory + "/" + TmpFileName.Replace(".c", "_Caller.txt")).ToList();
                                    foreach (var Item in TmpListForCallerFunctionName)
                                        File.AppendAllText(AnanlysisDirectory + "/" +
                                            TmpFileName.Replace(".c", "_Called.txt"), Item + "#" + "Error Occured!!"
                                            + Environment.NewLine);
                                }
                            }
                            else
                            {
                                string TmpFileName = Str.Substring(Str.LastIndexOf(@"\") + 1);
                                File.WriteAllText(AnanlysisDirectory + @"\" + TmpFileName.Replace(".c", "_Caller.txt"), "Error Occured!!");
                            }
                            Count++;
                            /** Update Preogressbar**/
                            ProgressBarVal = (((Count / ListOfFilesNamesAlongwithPath.Count) * 100) % 20) + 40; // (Percentage)%20.Never show progress more than 60 percent for 
                            //this operation.
                            ObjBackGroundWorker.ReportProgress(ProgressBarVal);

                        }
                        RetVal = true;
                    }
                    #endregion

                    if (HighlightNestedFunctionCalls == 1)
                    {
                        /** Commands(of Functiontreeview) are already Executed and Variable is set too.All done in above segment of code **/
                    }
                    #region NoOfStatements
                    if (NoOfStatementsInFunction == 1) // it must be before floating point Operations
                    {
                        ProgressReport = "Getting No Of Lines In Functions..";
                        ObjBackGroundWorker.ReportProgress(ProgressBarVal);
                        
                        PrepareCommands(PrepareCommandsType.ShowNoOfStatementsInFunction_CreateStart, 0);// prepare Commands for Start File
#if GET_END_LINE_NUMBER_OF_EACH_FUNCTION_USING_CUT_COMMAND
                        PrepareCommands(PrepareCommandsType.ShowNoOfStatementsInFunction_CreateEnd, 0); // prepare Commands for End File
#endif
                        Count = 0;
                        foreach (String Str in ListOfFilesNamesAlongwithPath)
                        {
                            RetVal = ExecuteCommand(NoOfStatmentsCmdArgument_1[Count]);
                            /** Find Number Of functions in each file for next Step **/
                            if (RetVal == false)
                            {
                                break;
                            }
                            Count++;
                        }
                        /** Now Create File Conatianig End Line Number of Each Function **/
                        if (RetVal == true)
                        {

#if GET_END_LINE_NUMBER_OF_EACH_FUNCTION_USING_CUT_COMMAND
                            Count = 0;
                            foreach (String Str in ListOfFilesNamesAlongwithPath)
                            {
                                FileName = Str.Substring(Str.LastIndexOf(@"\") + 1);
                                String PathOfTempFile = AnanlysisDirectory + "/" +
                                FileName.Replace(".c", "_Statements_Temp.txt");
                                String PathOfEndLinenumberContainingFile = AnanlysisDirectory + "/" +
                                FileName.Replace(".c", "_Statements_End.txt");
                                RetVal = ExecuteCommand(NoOfStatmentsCmdArgument_2[Count]);
                                /** Now Create End File from Temp file contents **/
                                if (RetVal == true)
                                {
                                    using (StreamReader file = new StreamReader(PathOfTempFile))
                                    {
                                        string LineofFile;
                                        int CountForLine = 1;
                                        while ((LineofFile = file.ReadLine()) != null)
                                        {
                                            if (LineofFile.Trim().Contains("}")) // If line conatins "}" at start index
                                            {
                                                if (LineofFile[0] == '}') // check if it is at zeroth location then it is function end
                                                {// Expecting proper Intendation
                                                    if (!(Regex.IsMatch(LineofFile, ";+"))) // one or more matches
                                                    {// it shall mean that if Regex is true then it was not the ending 
                                                        //brace of FunctionsTreeView but it is the ending brace of struct or class
                                                        File.AppendAllText(PathOfEndLinenumberContainingFile, Convert.ToString(CountForLine) + Environment.NewLine);

                                                    }
                                                }
                                            }
                                            CountForLine++;
                                        }
                                    }
                                    Count++;
                                }
                                else
                                {
                                    break;
                                }
                                /** Update Preogressbar**/
                                ProgressBarVal = (((Count / ListOfFilesNamesAlongwithPath.Count) * 100) % 10) + 70; // (Percentage)%10.Never show progress more
                                //than 80 percent for this operation.that is why 10% plus 70 of previous function which shall always be 
                                //executed i.e. Function Tree View
                                ObjBackGroundWorker.ReportProgress(ProgressBarVal);
                            }
#else
                                                PrepareCommands(ClassOptionsType.ShowNoOfStatmentsInFunction_CreateEnd, 0); // prepare Commands for End File
                                                Count = 0;
                                                foreach (String Str in ListOfFilesNamesAlongwithPath)
                                                {
                                                    FileName = Str.Substring(Str.LastIndexOf(@"\") + 1);
                                                    String PathOfEndLineNumberContainingFile = AnanlysisDirectory + "/" +
                                                    FileName.Replace(".c", "_Statements_Start.txt");

                                                    using (StreamReader file = new StreamReader(PathOfEndLineNumberContainingFile))
                                                    {
                                                        string LineofFile;
                                                        int CountForLine = 0;
                                                        while ((LineofFile = file.ReadLine()) != null)
                                                        {
                                                            RetVal = ExecuteCommand(NoOfStatmentsCmdArgument_2[Count][CountForLine]);
                                                            if (RetVal == false)
                                                                break;
                                                            CountForLine++;
                                                        }
                                                    }
                                                    if (RetVal == false)
                                                        break;
                                                    Count++;
                                                    /** Update Preogressbar**/
                                                    ProgressBarVal = ( ((Count / ListOfFilesNamesAlongwithPath.Count) * 100) % 20) + 60; // Percentage%60.Never show progress more
                                                    //than 80 percent for this operation.that is why 20% plus 60 of previous function which shall always be 
                                                    //executed i.e. Function Tree View
                                                    ObjBackGroundWorker.ReportProgress(ProgressBarVal);
                                                
                                                }
#endif
                        }

                    }
                    #endregion
                    if (FloatingPointOperations == 1)
                    {
                        ProgressReport = "Searching Floating Point Operations..";
                        ObjBackGroundWorker.ReportProgress(ProgressBarVal);

                        /** 1.Create Files of Global Var,Local Var and function definition conatinig float or double variables **/
                        PrepareCommands(PrepareCommandsType.SearchFloatDoubleVarDefinitionInTheirResFilesName, 0);
                        RetVal = ExecuteCommand(GetLocalFloatDoubleVarDefCmdArgument);
                        if (RetVal == true)
                        {
                            RetVal = ExecuteCommand(GetGlobalFloatDoubleVarDefCmdArgument);
                        }
                        if (RetVal == true)
                        {
                            RetVal = ExecuteCommand(GetFunctionsdefinitionWithLocalFloatDoubleVarCmdArgument);
                        }
                        if (RetVal == true)
                        {
                            /** 2.Now list all Global variables in a list **/
                            using (StreamReader InputFile = new StreamReader(AnanlysisDirectory + "/_G_V.txt"))
                            {
                                string LineofFile;
                                while ((LineofFile = InputFile.ReadLine()) != null)
                                {
                                    ListOfGlobalFloatDoubleVar.Add(LineofFile.Substring(0, LineofFile.IndexOf(" ")));
                                }
                            }
                            /** 3.Now search those float or double global variables in All files and save with location in a file **/
                            PrepareCommands(PrepareCommandsType.SearchGlobalFloatDoubleVarsInAllFiles, 0);
                            foreach (var Command in GetGlobalVarsDefLocationInEachFileCmdArgument)
                            {
                                RetVal = ExecuteCommand(Command);
                                if (RetVal == false)
                                {
                                    break;
                                }
                            }
                            if (RetVal == true)
                            {
                                /** 4.Now Locate All Local Variables in their Corresponding File **/
                                // First Get File Names having local Var.s
                                PrepareCommands(PrepareCommandsType.GetFileNamesForLocalFloatDoubleVars, 0);
                                RetVal = ExecuteCommand(GetFileNamesForLocalVarsOfFloatDoubleCmdArgument);
                                if (RetVal == true)
                                {
                                    RetVal = ExecuteCommand(GetFileNamesForLocalVarsOfFloatDoubleFromFuncDefCmdArgument); // Get only FIle names
                                }
                                if (RetVal == true)
                                {
                                    ListOfFileNamesOfFloatDoubleVars = File.ReadAllLines(AnanlysisDirectory + "/L_V_Used_In_File_Names.txt").ToList<string>();
                                    // 5. Now save local Vars with File name on Index zero of list 
                                    Count = 0;
                                    foreach (var LocalVarFileName in ListOfFileNamesOfFloatDoubleVars)
                                    {
                                        List<string> TmpListForFileName = new List<string>();
                                        TmpListForFileName.Add(LocalVarFileName);
                                        ListOfLocalVarsAlongWithFileNames.Add(TmpListForFileName);
                                        Count++;
                                    }
                                    //6.Save variables in its corresponding FileName conataining list
                                    using (StreamReader file = new StreamReader(AnanlysisDirectory + "/_L_V.txt"))
                                    {
                                        string LineofFile;
                                        while ((LineofFile = file.ReadLine()) != null)
                                        {
                                            string FileNameInFile = LineofFile.Substring(LineofFile.IndexOf(" ") + 1).Trim();
                                            for (int ForCount = 0; ForCount < ListOfLocalVarsAlongWithFileNames.Count; ForCount++)
                                            {
                                                if ((ListOfLocalVarsAlongWithFileNames[ForCount][0].Contains(FileNameInFile)) == true)
                                                {
                                                    ListOfLocalVarsAlongWithFileNames[ForCount].Add(LineofFile.Substring(0, LineofFile.IndexOf(" ")).Trim());
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    /** Save Local var.s after extracting from Func Def **/
                                    ListOfFileNamesOfFloatDoubleVarsFromFuncDef = File.ReadAllLines(AnanlysisDirectory + "/L_V_Used_In_Func_def_File_Names.txt").ToList<string>();
                                    using (StreamReader file = new StreamReader(AnanlysisDirectory + "/_F_V.txt"))
                                    {
                                        string LineofFile;
                                        while ((LineofFile = file.ReadLine()) != null)
                                        {
                                            string FileNameInFile = LineofFile.Substring(LineofFile.IndexOf(" ") + 1).Trim();
                                            int ForCount = 0;
                                            for (ForCount = 0; ForCount < ListOfLocalVarsAlongWithFileNames.Count; ForCount++)
                                            {
                                                if ((ListOfLocalVarsAlongWithFileNames[ForCount][0].Contains(FileNameInFile)) == true)
                                                {
                                                    string StringWithinBrackets = LineofFile.Substring(LineofFile.IndexOf("(")+1).Trim();
                                                    int NoOfFloatDoubleVars = LineofFile.Length - (LineofFile.Replace("float", "").Length + LineofFile.Replace("double", "").Length); // No of floats or doubles in a string
                                                    for (int TmpCount = 0; TmpCount < NoOfFloatDoubleVars; TmpCount++)
                                                    {
                                                        if (LineofFile.IndexOf("float") != -1)
                                                            ListOfLocalVarsAlongWithFileNames[ForCount].Add(LineofFile.Substring(LineofFile.IndexOf("float")));
                                                        else if(LineofFile.IndexOf("double") != -1)
                                                            ListOfLocalVarsAlongWithFileNames[ForCount].Add(LineofFile.Substring(LineofFile.IndexOf("double")));
                                                    }
                                                    break;
                                                }
                                            }
                                            if (ForCount >= ListOfLocalVarsAlongWithFileNames.Count) // it mean file name is not found in list so create an entry 
                                            {
                                                List<string> TmpListForFileName = new List<string>();
                                                TmpListForFileName.Add(FileNameInFile);
                                                ListOfLocalVarsAlongWithFileNames.Add(TmpListForFileName);
                                                ListOfLocalVarsAlongWithFileNames[ListOfLocalVarsAlongWithFileNames.Count -1].Add(LineofFile.Substring(0, LineofFile.IndexOf(" ")).Trim()); // add it to the end of list
                                            }
                                        }
                                    }
                                    /** 7.Save each Global variable(Not definition but use) with location in a file and file name from Previous Result.
                                    */
                                    if (File.Exists(AnanlysisDirectory + "/G_V_Use_Loc_In_Files.txt"))
                                    {
                                        using (StreamReader InputFile = new StreamReader(AnanlysisDirectory + "/G_V_Use_Loc_In_Files.txt"))
                                        {
                                            using (StreamWriter OutFile = new StreamWriter(AnanlysisDirectory + "/_F_D_V_Use.txt", false))
                                            {
                                                string LineofFile;
                                                while ((LineofFile = InputFile.ReadLine()) != null)
                                                {
                                                    if (LineofFile.Trim().Contains("="))
                                                    {
                                                        OutFile.Write(LineofFile + Environment.NewLine);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    // 8. Search Local variables
                                    PrepareCommands(PrepareCommandsType.FindLocalVarsUseInFiles, 0);
                                    foreach (var Command in GetLocalFloatDoubleVarsUseInfoInEachFileCmdArgument)
                                    {
                                        RetVal = ExecuteCommand(Command);
                                        if (RetVal == false)
                                            break;
                                    }
                                    if (RetVal == true)
                                    {
                                        //9.Now arrange them all if needed
                                        if(File.Exists(AnanlysisDirectory + "/_F_D_V_Use.txt"))
                                        {
                                            PrepareCommands(PrepareCommandsType.ArrangeAllFloatDoubleVarsInProperFormat, 0);
                                            RetVal = ExecuteCommand(ArrangeAllFloatDoubleInProperFormat);
                                            if (RetVal == true)
                                            {
                                                /*10. Now Get The function name for each command using FileName and Line Number
                                                 * and Called Function Information */
                                                // First Get File Names for ease
                                                PrepareCommands(PrepareCommandsType.GetFileNamesOfAllFloatDoubleVarsUsers, 0);
                                                RetVal = ExecuteCommand(GetFileNamesOfAllFloatDoubleUsersCmdArgument);
                                                if (RetVal == true)
                                                {
                                                    /* Now Use Filename to check for coresponding _Called.txt and End.txt and set statements of Vars with Function Names */
                                                    ListOfFileNamesContainigFloatingPointOperations = File.ReadAllLines(AnanlysisDirectory + "/FileNames_Of_F_D_Vars.txt").ToList<string>();
                                                    if (NoOfStatementsInFunction != 1)
                                                    {
                                                        PrepareCommands(PrepareCommandsType.GetEndOfFunctionsInFilesOfFloatingPountOperations, 0); // prepare Commands for End File   
                                                        Count = 0;
                                                        foreach (String Str in ListOfFileNamesContainigFloatingPointOperations)
                                                        {
                                                            FileName = Str.Substring(Str.LastIndexOf(@"\") + 1);
                                                            String PathOfTempFile = AnanlysisDirectory + "/" +
                                                            FileName.Replace(".c", "_Statements_Temp.txt");
                                                            String PathOfEndLinenumberContainingFile = AnanlysisDirectory + "/" +
                                                            FileName.Replace(".c", "_Statements_End.txt");
                                                            RetVal = ExecuteCommand(NoOfStatmentsCmdArgument_2[Count]);
                                                            /** Now Create End File from Temp file contents **/
                                                            if (RetVal == true)
                                                            {
                                                                using (StreamReader file = new StreamReader(PathOfTempFile))
                                                                {
                                                                    string LineofFile;
                                                                    int CountForLine = 1;
                                                                    while ((LineofFile = file.ReadLine()) != null)
                                                                    {
                                                                        if (LineofFile.Trim().Contains("}")) // If line conatins "}" at start index
                                                                        {
                                                                            if (LineofFile[0] == '}') // check if it is at zeroth location then it is function end
                                                                            {// Expecting proper Intendation
                                                                                if (!(Regex.IsMatch(LineofFile, ";+"))) // one or more matches
                                                                                {// it shall mean that if Regex is true then it was not the ending 
                                                                                    //brace of FunctionsTreeView but it is the ending brace of struct or class
                                                                                    File.AppendAllText(PathOfEndLinenumberContainingFile, Convert.ToString(CountForLine) + Environment.NewLine);

                                                                                }
                                                                            }
                                                                        }
                                                                        CountForLine++;
                                                                    }
                                                                }
                                                                Count++;
                                                            }
                                                            else
                                                            {
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    // Now get function name by comparing each line number from end index of function
                                                    if (File.Exists(AnanlysisDirectory + "/Formatted_F_D_V_Use.txt"))
                                                    {
                                                        using (StreamReader InputFile = new StreamReader(AnanlysisDirectory + "/Formatted_F_D_V_Use.txt"))
                                                        {
                                                            string LastString = string.Empty; // it is for the case if line has been read but did not match the file name so after opening another file with corresponding fileName,Next line should be read and last read line could not be written 
                                                            foreach (var FileNameInList in ListOfFileNamesContainigFloatingPointOperations)
                                                            {
                                                                List<string> EndingIndexOfFunctions = File.ReadAllLines(AnanlysisDirectory + "/" + FileNameInList.Replace(".c", "_Statements_End.txt")).ToList<string>();

                                                                using (StreamWriter OutputFile = new StreamWriter(AnanlysisDirectory + "/" + FileNameInList.Replace(".c", "_F_P_O.txt"), false))
                                                                {
                                                                    string LineOfFile;
                                                                    if (LastString != string.Empty)
                                                                    {
                                                                        int StartIndexOfLineNumber = LastString.IndexOf(":") + 1;
                                                                        int LengthOfLineNumber = LastString.IndexOf(":", StartIndexOfLineNumber) - StartIndexOfLineNumber; // length
                                                                        int IndexOfFloatingPointOperation = Int32.Parse(LastString.Substring(StartIndexOfLineNumber, LengthOfLineNumber));
                                                                        Count = 1;
                                                                        foreach (var Value in EndingIndexOfFunctions)
                                                                        {
                                                                            if (IndexOfFloatingPointOperation < Int32.Parse(Value))
                                                                            {
                                                                                OutputFile.WriteLine(Count.ToString() + " " + LastString.Substring(LastString.IndexOf(":", StartIndexOfLineNumber) + 1).Trim()); // Count shall detrmine Function name in Function Tree View
                                                                                break;
                                                                            }
                                                                            Count++;
                                                                        }
                                                                        LastString = string.Empty;
                                                                    }
                                                                    while ((LineOfFile = InputFile.ReadLine()) != null)
                                                                    {
                                                                        if (LineOfFile.Substring(0, LineOfFile.IndexOf(":")) == FileNameInList)
                                                                        {
                                                                            int StartIndexOfLineNumber = LineOfFile.IndexOf(":") + 1;
                                                                            int LengthOfLineNumber = (LineOfFile.IndexOf(":", StartIndexOfLineNumber)) - StartIndexOfLineNumber; // length
                                                                            int IndexOfFloatingPointOperation = Int32.Parse(LineOfFile.Substring(StartIndexOfLineNumber, LengthOfLineNumber));
                                                                            Count = 0;
                                                                            foreach (var Value in EndingIndexOfFunctions)
                                                                            {
                                                                                if (IndexOfFloatingPointOperation < Int32.Parse(Value))
                                                                                {
                                                                                    OutputFile.WriteLine(Count.ToString() + " " + LineOfFile.Substring(LineOfFile.IndexOf(":", StartIndexOfLineNumber) + 1).Trim()); // Count shall detrmine Function name in Function Tree View
                                                                                    break;
                                                                                }
                                                                                Count++;
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            LastString = LineOfFile;
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ErrorArray = string.Concat(ErrorArray, "Floating Point Operations Not Found"+ Environment.NewLine);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            ErrorArray = string.Concat(ErrorArray, "Floating Point Operations Not Found"+ Environment.NewLine);
                                        }
                                    }
                                }
                            }
                        }
                        /** Update Preogressbar**/
                        ProgressBarVal = (((Count / ListOfFilesNamesAlongwithPath.Count) * 100) % 10) + 80; // (Percentage)%10.Never show progress more
                        //than 90 percent for this operation.that is why 10% plus 70 of previous function which shall always be 
                        //executed i.e. Function Tree View
                        ObjBackGroundWorker.ReportProgress(ProgressBarVal);
                    }
                    if (IncludeHeaderFiles == 1)
                    {
                        /*** That Option is already handled in FunctionTreeView ***/
                    }
                    if (SingleNestedLoop == 1)
                    {
                        /** First find all occurences of for loop along with its line number and write it in a file with C file name 
                         * for each single file.
                        **/
                        ProgressReport = "Searching Loops..";
                        ObjBackGroundWorker.ReportProgress(ProgressBarVal);
                        int LoopBracketCount = 0;
                        int OtherCStatementsBrackets = 0;
                        int LineNumber = 0;
                        int CounterForFileLineNumber = 0;
                        List<string> FuncStartLineNumber = new List<string>();
                        List<string> MainFuncNames = new List<string>();
                        Count = 0;
                        foreach (String Str in ListOfFilesNamesAlongwithPath)
                        {
                            FileName = Str.Substring(Str.LastIndexOf(@"\") + 1);
                            if(NoOfStatementsInFunction != 1)
                            {
                                PrepareCommands(PrepareCommandsType.GetStartLineNumberOfEachFunction, 0);
                                RetVal = ExecuteCommand(GetStartLineNumberOfEachFunctionCmdArgument[Count]);
                                if (RetVal == true)
                                {
                                    FuncStartLineNumber = File.ReadLines(AnanlysisDirectory + "/" + FileName.Replace(".c", "_Func_Start.txt")).ToList();
                                }
                                Count++;
                            }
                            else
                            {
                                if (File.Exists(AnanlysisDirectory + "/" + FileName.Replace(".c", "_Statements_Start.txt")))
                                {
                                    FuncStartLineNumber = File.ReadLines(AnanlysisDirectory + "/" + FileName.Replace(".c", "_Statements_Start.txt")).ToList();
                                    RetVal = true;
                                }
                                else
                                {
                                    RetVal = false;
                                }
                            }
                            if (RetVal == true)
                            {
                                if (File.Exists(AnanlysisDirectory + "/" + FileName.Replace(".c", "_Caller.txt")))
                                {
                                    MainFuncNames = File.ReadLines(AnanlysisDirectory + "/" + FileName.Replace(".c", "_Caller.txt")).ToList();
                                    RetVal = true;
                                }
                                else
                                {
                                    RetVal = false;
                                }
                            }
                            if (RetVal == true)
                            {
                                using (var Reader = new StreamReader(Str))
                                {
                                    using (StreamWriter OutFile = new StreamWriter(AnanlysisDirectory + "/" + FileName.Replace(".c", "_LoopData.txt"), false))
                                    {
                                        while (!Reader.EndOfStream)
                                        {
                                            var Line = Reader.ReadLine().Trim();
                                            LineNumber++;
                                            if (Line.StartsWith("for("))
                                            {
                                                if (LoopBracketCount > 0)
                                                {
                                                    OutFile.Write(MainFuncNames[CounterForFileLineNumber] +" " + LineNumber.ToString() + " (N) " + " " + Line + Environment.NewLine);
                                                }
                                                else
                                                {
                                                    CounterForFileLineNumber = 0;
                                                    int LoopCouner = 0;
                                                    for (LoopCouner = 1; LoopCouner < FuncStartLineNumber.Count; LoopCouner++ )
                                                    {
                                                        if (LineNumber < int.Parse(FuncStartLineNumber[CounterForFileLineNumber+1]))
                                                        {
                                                            OutFile.Write(MainFuncNames[CounterForFileLineNumber] + " " + LineNumber.ToString() + " " + Line + Environment.NewLine);
                                                            break;
                                                        }
                                                        CounterForFileLineNumber++;
                                                    }
                                                    if (LoopCouner >= FuncStartLineNumber.Count)
                                                    {
                                                        OutFile.Write(MainFuncNames[CounterForFileLineNumber] + " " + LineNumber.ToString() + " " + Line + Environment.NewLine);
                                                    }
                                                    Line = Reader.ReadLine().Trim();
                                                    LineNumber++;
                                                    if (Line.StartsWith("{"))
                                                    {
                                                        LoopBracketCount++;
                                                    }
                                                }
                                            }
                                            else if (Line.StartsWith("while("))
                                            {
                                                if (LoopBracketCount > 0)
                                                {
                                                    OutFile.Write(MainFuncNames[CounterForFileLineNumber] + " " + LineNumber.ToString() + " (N) " + " " + Line + Environment.NewLine);
                                                }
                                                else
                                                {
                                                    CounterForFileLineNumber = 0;
                                                    int LoopCouner = 0;
                                                    for (LoopCouner = 1; LoopCouner < FuncStartLineNumber.Count; LoopCouner++)
                                                    {
                                                        if (LineNumber < int.Parse(FuncStartLineNumber[CounterForFileLineNumber + 1]))
                                                        {
                                                            OutFile.Write(MainFuncNames[CounterForFileLineNumber] + " " + LineNumber.ToString() + " " + Line + Environment.NewLine);
                                                            break;
                                                        }
                                                        CounterForFileLineNumber++;
                                                    }
                                                    if (LoopCouner >= FuncStartLineNumber.Count)
                                                    {
                                                        OutFile.Write(MainFuncNames[CounterForFileLineNumber] + " " + LineNumber.ToString() + " " + Line + Environment.NewLine);
                                                    }
                                                    Line = Reader.ReadLine().Trim();
                                                    LineNumber++;
                                                    if (Line.StartsWith("{"))
                                                    {
                                                        LoopBracketCount++;
                                                    }
                                                }
                                            }
                                            else if (Line.StartsWith("do"))
                                            {
                                                if (LoopBracketCount > 0)
                                                {
                                                    OutFile.Write(MainFuncNames[CounterForFileLineNumber] + " " + LineNumber.ToString() + " (N) " + " " + Line + Environment.NewLine);
                                                }
                                                else
                                                {
                                                    CounterForFileLineNumber = 0;
                                                    int LoopCouner = 0;
                                                    for (LoopCouner = 1; LoopCouner < FuncStartLineNumber.Count; LoopCouner++)
                                                    {
                                                        if (LineNumber < int.Parse(FuncStartLineNumber[CounterForFileLineNumber + 1]))
                                                        {
                                                            OutFile.Write(MainFuncNames[CounterForFileLineNumber] + " " + LineNumber.ToString() + " " + Line + Environment.NewLine);
                                                            break;
                                                        }
                                                        CounterForFileLineNumber++;
                                                    }
                                                    if (LoopCouner >= FuncStartLineNumber.Count)
                                                    {
                                                        OutFile.Write(MainFuncNames[CounterForFileLineNumber] + " " + LineNumber.ToString() + " " + Line + Environment.NewLine);
                                                    }
                                                    Line = Reader.ReadLine().Trim();
                                                    LineNumber++;
                                                    if (Line.StartsWith("{"))
                                                    {
                                                        LoopBracketCount++;
                                                    }
                                                }
                                            }
                                            else if (Line.StartsWith("{") && LoopBracketCount > 0) // Loopcount so that brackets outside for loop are not counted in
                                            {
                                                OtherCStatementsBrackets++;
                                            }
                                            else if (Line.StartsWith("}") && OtherCStatementsBrackets <= 0)
                                            {
                                                LoopBracketCount = 0;
                                            }
                                            else if (Line.StartsWith("}") && LoopBracketCount > 0) // Loopcount so that brackets outside for loop are not counted in
                                            {
                                                OtherCStatementsBrackets--;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                ErrorArray = string.Concat(ErrorArray, "Error occured in finding Loops : " + Str.Substring(Str.LastIndexOf(@"\") + 1) + Environment.NewLine);
                            }
                        }
                    }
                }
            }
            BackGroundWorkRetVal = RetVal; // Set it so that action can be taken accordingly
            //Report 100% completion on operation completed
            if (BackGroundWorkRetVal == true) // it means all went good
            {
                ObjBackGroundWorker.ReportProgress(100);
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void BtnBrowseFolder_Click(object sender, EventArgs e)
        {
            try
            {
                FileFolderPath.Clear();
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    ListOfFilesNamesAlongwithPath.Clear();
                    ListOfPathToMainFunctionsReports.Clear();
                    this.FileFolderPath.Text = folderBrowserDialog1.SelectedPath;
                    FileOrFolder = EnumFileOrFolder.Folder;
                    /* Check .C Files Are Present */
                    DirectoryInfo Folder = new DirectoryInfo(FileFolderPath.Text);
                    FileInfo[] Files = Folder.GetFiles("*.c"); //Getting Source files
                    if (Files.Length <= 0)
                    {
                        MessageBox.Show("No source file found with .C Extension");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnBrowseFile_Click(object sender, EventArgs e)
        {
            FileFolderPath.Clear();
            try
            {
                ListOfFilesNamesAlongwithPath.Clear();
                ListOfPathToMainFunctionsReports.Clear();
                OpenFileDialog OpenDlg = new OpenFileDialog();
                OpenDlg.RestoreDirectory = true; // To remove Windows XP bug of changing Current Working Directory
                OpenDlg.Multiselect = false;
                OpenDlg.Filter = "C|*.c";
                OpenDlg.Title = @"Select File";
                if (OpenDlg.ShowDialog() == DialogResult.OK)
                {
                    FileFolderPath.Text = OpenDlg.FileName;
                    FileOrFolder = EnumFileOrFolder.File;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnStartStaticAnalysis_Click(object sender, EventArgs e)
        {
            bool RetVal = true, StartAnalysis = true;
            try
            {
                if (System.IO.Directory.Exists("StaticAnalysisReport"))
                    System.IO.Directory.Delete("StaticAnalysisReport", true);
                if (System.IO.Directory.Exists("TmpFolderForSourceFiles"))
                    System.IO.Directory.Delete("TmpFolderForSourceFiles",true);

                ErrorArray = string.Empty;
                ListOfDefMacrosProvidedByUser.Clear();
                ListOfUnDefMacrosProvidedByUser.Clear();
                IsDefBoxChecked = false;
                IsUnDefBoxChecked = false;
                if (FileFolderPath.Text.Trim() != string.Empty)
                {
                    if (DefinedCheckBox.Checked == true)
                    {
                        if (DefMacrosRichTextBox.Text.Trim() == string.Empty)
                        {
                            MessageBox.Show("No Macro is Defined");
                            RetVal = false;
                        }
                        else
                        {
                            ListOfDefMacrosProvidedByUser = DefMacrosRichTextBox.Lines.ToList<string>();
                            IsDefBoxChecked = true;
                        }
                    }
                    if(UnDefCheckBox.Checked == true)
                    {
                        if (UnDefMacroRichTextBox.Text.Trim() == string.Empty)
                        {
                            MessageBox.Show("No Macro is Defined");
                            RetVal &= false;
                        }
                        else
                        {
                            ListOfUnDefMacrosProvidedByUser = UnDefMacroRichTextBox.Lines.ToList<string>();
                            IsUnDefBoxChecked = true;
                        }
                    }

                    if (RetVal == true)
                    {
                        BtnStartStaticAnalysis.Enabled = false;
                        BtnBrowseFile.Enabled = false;
                        BtnBrowseFolder.Enabled = false;
                        DefMacrosRichTextBox.Enabled = false;
                        UnDefMacroRichTextBox.Enabled = false;
                        DefinedCheckBox.Enabled = false;
                        UnDefCheckBox.Enabled = false;
                         
                        /* Ask User For Option*/
                        AnalysisOptionsList AnalysisOptionsForm = new AnalysisOptionsList(this);
                        DialogResult FormClosingState = AnalysisOptionsForm.ShowDialog();
                        if (FormClosingState == DialogResult.OK)
                        {
                            /***/
                            /** Execute User Options Accordingly **/
                            #region Choose Variables Acording To User Selected Option(s)
                            if (RetVal == true)
                            {
                                ClearOptionsVar();
                                foreach (AnalysisOptionsList.EnumAnalysisOptionsSelected Option in AnalysisOptionsList.ListOfSelectedAnalysisOptions)
                                {
                                    switch (Option)
                                    {
                                        case AnalysisOptionsList.EnumAnalysisOptionsSelected.SelectedFunctionsTreeView:
                                            FunctionsTreeView = 1;
                                            CalledFunctions = 1;
                                            break;
                                        case AnalysisOptionsList.EnumAnalysisOptionsSelected.SelectedHighlightNestedFunctionCalls:
                                            FunctionsTreeView = 1;
                                            HighlightNestedFunctionCalls = 1;
                                            break;
                                        case AnalysisOptionsList.EnumAnalysisOptionsSelected.SelectedNoOfStatmentsInAFunction:
                                            FunctionsTreeView = 1;
                                            NoOfStatementsInFunction = 1;
                                            break;
                                        case AnalysisOptionsList.EnumAnalysisOptionsSelected.SelectedFloatingPointOperations:
                                            FunctionsTreeView = 1;
                                            FloatingPointOperations = 1;
                                            break;
                                        case AnalysisOptionsList.EnumAnalysisOptionsSelected.SelectedIncludeHeaderFiles:
                                            if (FunctionsTreeView != 1)
                                            {
                                                MessageBox.Show("Select any other option too.");
                                                StartAnalysis = false;
                                            }
                                            FunctionsTreeView = 1;
                                            IncludeHeaderFiles = 1;
                                            break;
                                        case AnalysisOptionsList.EnumAnalysisOptionsSelected.SelectedDetailedViewOfCalledFUnctions:
                                            FunctionsTreeView = 1;
                                            CalledFunctions = 1;
                                            DetailedCalledFunctionsTreeView = 1;
                                            break;
                                        case AnalysisOptionsList.EnumAnalysisOptionsSelected.SelectedSingleNestedLoops:
                                            FunctionsTreeView = 1;
                                            SingleNestedLoop = 1;
                                            break;
                                    }
                                }
                                AnalysisOptionsForm.Dispose();
                            #endregion
                                if (StartAnalysis)
                                {
                                    AnalysisProgressBar.Visible = true;
                                    ProgressLabel.Visible = true;
                                    this.Text = this.Text + "(Running)";
                                    Cursor.Current = Cursors.WaitCursor;
                                    ObjBackGroundWorker.RunWorkerAsync();
                                }
                                else
                                {
                                    ResetGui();
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Choose Any Option(s)");
                            ResetGui();
                        }
                    }
                    else
                    {
                        ResetGui();
                    }
                }
                else
                {
                    MessageBox.Show("Please select File/Folder");
                }
            }
            catch (Exception ex)
            {
                ex.Message.Replace("cscope","StaticAnalysisTool:");
                ex.Message.Replace("ctags", "StaticAnalysisTool:");
                MessageBox.Show(ex.Message);
            }
        }

        private void ResetGui()
        {
            FileFolderPath.Clear();
            this.Text = "Static Analyser";
            AnalysisProgressBar.Visible = false;
            BtnStartStaticAnalysis.Enabled = true;
            BtnBrowseFile.Enabled = true;
            BtnBrowseFolder.Enabled = true;
            DefMacrosRichTextBox.Enabled = true;
            UnDefMacroRichTextBox.Enabled = true;
            DefinedCheckBox.Enabled = true;
            UnDefCheckBox.Enabled = true;
            UnDefCheckBox.Checked = false;
            DefinedCheckBox.Checked = false;
            ProgressLabel.Visible = false;
        }

        private void CreateAnalysisDirectory()
        {
            AnanlysisDirectory = "StaticAnalysisReport";
            System.IO.Directory.CreateDirectory("StaticAnalysisReport");
        }

        private void CreateTempFolderForSourceFilesToBeAnalysed()
        {
            try
            {
                TmpFolderForSourceFiles = "TmpFolderForSourceFiles";
                if(DebugMode == true)
                    MessageBox.Show( System.IO.Directory.GetCurrentDirectory());
                System.IO.Directory.CreateDirectory(TmpFolderForSourceFiles);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private string WindowsToUnixStyleCommand(string Command)
        {
            Regex Pattern = new Regex(@"[\,\\]");
            Command = Pattern.Replace(Command, "/");
            return Command;
        }
        
        private void CreateDos2UnixCommand(string Path)
        {
            Dos2UnixCommandArgument = "dos2unix " + Path;
        }

        private void Create_RemoveDataBaseCommand()
        {
            Remove_OutDataBaseCmdArgument = "rm -f " + "cscope.out";
        }

        private void Create_RemoveAnyTmpDirectoryCommand(string DirectoryName)
        {
            RemoveAnyTmpDirectoryCmdArgument = "rm -rf " + DirectoryName;
        }

        private void SetTmpDirForCscope()
        {
            SetTmpDirCmdArgumentForCscope = "bash -c export TMPDIR=" + AnanlysisDirectory;
        }
        private void PrepareCommands( int OptionType, int DataForCommand )
        {
            string C_FileName = "", CscopeCommandStartString = "cscope -k ";// -k do not include System HeaderFiles
            int Count = 0;
            try
            {
                switch (OptionType)
                {
                    case PrepareCommandsType.RemoveComments:
                        RemoveCommentsCmdArgument = new string[ListOfFilesNamesAlongwithPath.Count];
                        break;
                    case PrepareCommandsType.FixPreprocessorDirectiveIssues:
                        UnifdefCmdArgument = new string[ListOfFilesNamesAlongwithPath.Count];
                        break;
                    case PrepareCommandsType.ShowFunctionsTreeView:
                        CallerFunctionsNameCmdArgument = new string[ListOfFilesNamesAlongwithPath.Count];
                        CalledDetailedFunctionsNameCmdArgument = new string[ListOfFilesNamesAlongwithPath.Count];
                        break;
                    case PrepareCommandsType.ShowNoOfStatementsInFunction_CreateStart:
                        NoOfStatmentsCmdArgument_1 = new string[ListOfFilesNamesAlongwithPath.Count];
                        FilePathOfStatemetsStart = new string[ListOfFilesNamesAlongwithPath.Count];
                        break;
                    case PrepareCommandsType.ShowNoOfStatementsInFunction_CreateEnd:
                        
#if GET_END_LINE_NUMBER_OF_EACH_FUNCTION_USING_CUT_COMMAND
                        NoOfStatmentsCmdArgument_2 = new string[ListOfFilesNamesAlongwithPath.Count];
#else
                        NoOfStatmentsCmdArgument_2 = new string[ListOfFilesNamesAlongwithPath.Count][];
#endif
                        break;
                    case PrepareCommandsType.ShowCompactCalledFunctionsNames:
                        CalledCompactFunctionsNameCmdArgument = new string[ListOfFilesNamesAlongwithPath.Count];
                        break;
                    case PrepareCommandsType.SearchFloatDoubleVarDefinitionInTheirResFilesName:
                        GetLocalFloatDoubleVarDefCmdArgument = @"ctags -x --c-kinds=l" + " " + TmpFolderForSourceFiles+"/" +"*.c "+
                            @" | grep -w 'float^\^|double' | gawk '{print $1,$4;}' | cut -f 2 -d ':' | uniq > " + AnanlysisDirectory + "/_L_V.txt"; // Local var float and double only in .c
                        GetGlobalFloatDoubleVarDefCmdArgument = @"ctags -x --c-kinds=v" + " " + TmpFolderForSourceFiles+"/" +"*.c "+
                            TmpFolderForSourceFiles+"/" +  @"*.h | grep -w 'float^\^|double' | gawk '{print $1,$4;}' | cut -f 2 -d ':' | uniq  > "  + 
                            AnanlysisDirectory + "/_G_V.txt"; // global var float and double only in .c and .h
                        GetFunctionsdefinitionWithLocalFloatDoubleVarCmdArgument = @"ctags -x --c-kinds=f" + " " + TmpFolderForSourceFiles+"/" +"*.c "+
                            @" | grep 'float^\^|double' | gawk '{$1=" + "\"" + "\"" + "; $2=" + "\"" + 
                            "\"" + ";sub(" + "\"" +" \"" + ", " + "\"" + "\"" + ");" + "sub(" + "\"" +
                                " \"" + ", " + "\"" + "" + "\"" + ");" + "print}' | sort -b -k 2,2 > "  + AnanlysisDirectory + "/_F_V.txt"; // Local var float and double only from Function Definition in .c and .h
                            break;
                    case PrepareCommandsType.SearchGlobalFloatDoubleVarsInAllFiles:
                        Count = 0;
                        GetGlobalVarsDefLocationInEachFileCmdArgument = new string[ListOfGlobalFloatDoubleVar.Count];
                        foreach (var Variable in ListOfGlobalFloatDoubleVar)
                        {
                            GetGlobalVarsDefLocationInEachFileCmdArgument[Count] = "grep -wn " + Variable + " " + TmpFolderForSourceFiles + "/*.c /dev/null | sort -b -k 2,2  >> " + AnanlysisDirectory + "/G_V_Use_Loc_In_Files.txt"; // use grep command with /dev/null so that prints file name
                            Count++;
                        }
                        break;
                    case PrepareCommandsType.GetFileNamesForLocalFloatDoubleVars:
                        GetFileNamesForLocalVarsOfFloatDoubleCmdArgument = "gawk '{print $2}' " + AnanlysisDirectory + "/_L_V.txt" + " | sort -b -k 1,1 | uniq > " + AnanlysisDirectory + "/L_V_Used_In_File_Names.txt"; /// sort so that uniq can find unique names because it compares one by one
                        GetFileNamesForLocalVarsOfFloatDoubleFromFuncDefCmdArgument = "gawk '{print $2}' " + AnanlysisDirectory + "/_L_V.txt" + " | sort -b -k 1,1 | uniq > " + AnanlysisDirectory + "/L_V_Used_In_Func_def_File_Names.txt"; /// sort so that uniq can find unique names because it compares one by one
                        break;
                    case PrepareCommandsType.FindLocalVarsUseInFiles:
                        GetLocalFloatDoubleVarsUseInfoInEachFileCmdArgument = new List<string>();
                        for(int ForCount = 0; ForCount < ListOfLocalVarsAlongWithFileNames.Count;ForCount++)
                        {
                            int FileLocation = 0;
                            string FileNameAtStartIndexOfList = string.Empty;
                            foreach(var VariableName in ListOfLocalVarsAlongWithFileNames[ForCount])
                            {
                                if(FileLocation == 0)
                                {
                                   FileNameAtStartIndexOfList  = VariableName;
                                   FileLocation = 1;
                                }else
                                {
                                    GetLocalFloatDoubleVarsUseInfoInEachFileCmdArgument.Add("grep -wn " + VariableName + " " + FileNameAtStartIndexOfList + " /dev/null " + @" | grep -wn '=^\^|)' >> " + AnanlysisDirectory + "/_F_D_V_Use.txt");
                                }
                            }
                        }
                        break;
                    case PrepareCommandsType.ArrangeAllFloatDoubleVarsInProperFormat:
                        ArrangeAllFloatDoubleInProperFormat = "cut -f 2- -d '/' " + AnanlysisDirectory + "/_F_D_V_Use.txt" + " | sort -b -k 1,1 | uniq > " + AnanlysisDirectory + "/Formatted_F_D_V_Use.txt";
                        break;
                    case PrepareCommandsType.GetFileNamesOfAllFloatDoubleVarsUsers:
                        GetFileNamesOfAllFloatDoubleUsersCmdArgument = "cut -f 1 -d ':' " + AnanlysisDirectory + "/Formatted_F_D_V_Use.txt | uniq > " + AnanlysisDirectory + "/FileNames_Of_F_D_Vars.txt";
                        break;
                    case PrepareCommandsType.GetEndOfFunctionsInFilesOfFloatingPountOperations:
                        NoOfStatmentsCmdArgument_2 = new string[ListOfFileNamesContainigFloatingPointOperations.Count];
                        using (StreamReader InputFile = new StreamReader(AnanlysisDirectory + "/FileNames_Of_F_D_Vars.txt") )
                        {
                            string LineofFile;
                            Count = 0;
                            while ((LineofFile = InputFile.ReadLine()) != null)
                            {
                                string FilePathOfStatemetsEnd = AnanlysisDirectory + "/" + LineofFile.Replace(".c", "_Statements_Temp.txt");
                                NoOfStatmentsCmdArgument_2[Count] = "cut -c 1-2 " + TmpFolderForSourceFiles + "/"+ LineofFile + " > " + FilePathOfStatemetsEnd; // it shall give only first character at every line
                                Count++;
                            }
                        }
                        break;
                    case PrepareCommandsType.GetStartLineNumberOfEachFunction:
                        GetStartLineNumberOfEachFunctionCmdArgument = new string[ListOfFilesNamesAlongwithPath.Count];
                        break;
                }
                Count = 0;
                foreach (String Str in ListOfFilesNamesAlongwithPath)
                {
                    C_FileName = Str;
                    /** Now Write Info in Corresponding File**/
                    if (IncludeHeaderFiles == 1)
                    {
                            CscopeCommandStartString = "cscope "; 
                    }
                   
                    string FileName = C_FileName.Substring(C_FileName.LastIndexOf(@"\") + 1);
                    switch (OptionType)
                    {
                        case PrepareCommandsType.RemoveComments:
                            RemoveCommentsCmdArgument[Count] = "gcc -fpreprocessed -dD -E -P " + C_FileName + " > " + C_FileName.Replace(".c","_Tmp.c");
                            break;
                        case PrepareCommandsType.FixPreprocessorDirectiveIssues:
                            string TmpStrForMacros="";
                            if (IsDefBoxChecked == true)
                            {
                                foreach(var Item in ListOfDefMacrosProvidedByUser)
                                {
                                    TmpStrForMacros = string.Concat(TmpStrForMacros, "-D" + Item + " "); 
                                }
                            }
                            if (IsUnDefBoxChecked == true)
                            {
                                foreach (var Item in ListOfUnDefMacrosProvidedByUser)
                                {
                                    TmpStrForMacros = string.Concat(TmpStrForMacros, "-U" + Item + " ");
                                }
                            }
                            UnifdefCmdArgument[Count] = "unifdef -k " + TmpStrForMacros + " " + C_FileName.Replace(".c", "_Tmp.c") + " > " + C_FileName; //-k for #ifdef
                            break;
                        case PrepareCommandsType.ShowFunctionsTreeView:
                            if (Count < ListOfFilesNamesAlongwithPath.Count)
                            {
                                /** Main Function Names In a File **/
                                CallerFunctionsNameCmdArgument[Count] = "ctags -x --c-kinds=f " + C_FileName +
                                    " | gawk -F ' ' '{print $3" + "\"" + " \"" + "$1 }' | sort -n | uniq | gawk -F ' ' '{print $2}' > " + AnanlysisDirectory +
                                    "/" + FileName.Replace(".c", "_Caller.txt");// 1: Function name,2: Type of Tag,3: Line Number,4: File Name,5:Function Prototype
                                /** Names of Function Called in Main Functions in a file **/
                                CalledDetailedFunctionsNameCmdArgument[Count] = CscopeCommandStartString + C_FileName + " -L -3 " +
                                @""".*""" + " | gawk '{$1=" + "\"" + "\"" + "; $3=" + "\"" + "\"" + ";sub(" + "\"" +
                                " \"" + ", " + "\"" + "\"" + ");" + "sub(" + "\"" +
                                " \"" + ", " + "\"" + "#" + "\"" + ");" + "print}' > " + AnanlysisDirectory +
                                    "/" + FileName.Replace(".c", "_Called.txt"); // 3 : line number, 1: FIle name, 
                                //2: caller Function name,4:called function name and command is 
                                // /C cscope -R Filename -L -3 ".*" |gawk '{$1=""; $3=""; sub(" ", "");sub(" ", "#") print}' > output.txt 
                                ListOfPathToMainFunctionsReports.Add(AnanlysisDirectory + "/" +
                                        FileName.Replace(".c", "_Caller.txt"));
                            }
                            else
                            {
                                ErrorArray = string.Concat(ErrorArray, "Memory Leakage Detected.Contact Developer" + Environment.NewLine);
                            }
                            break;
                        case PrepareCommandsType.ShowNoOfStatementsInFunction_CreateStart:
                            /** No Of statements in a Function **/
                            NoOfStatmentsCmdArgument_1[Count] = "ctags -x --c-kinds=f " + C_FileName +
                                " | gawk -F ' ' '{ print $3 }' | sort -n > " + AnanlysisDirectory + "/" +
                                    FileName.Replace(".c", "_Statements_Start.txt"); // It shall give sorted Starting Line Numbers of each main function
                            FilePathOfStatemetsStart[Count] = AnanlysisDirectory + "/" + FileName.Replace(".c", "_Statements_Start.txt");
                            break;
                        case PrepareCommandsType.ShowNoOfStatementsInFunction_CreateEnd:
#if GET_END_LINE_NUMBER_OF_EACH_FUNCTION_USING_CUT_COMMAND                                
                            string FilePathOfStatemetsEnd = AnanlysisDirectory + "/" + FileName.Replace(".c", "_Statements_Temp.txt");   
                            NoOfStatmentsCmdArgument_2[Count] = "cut -c 1-2 " + C_FileName + " > " + FilePathOfStatemetsEnd; // it shall give only first character at every line
#else
                            NoOfStatmentsCmdArgument_2[Count] = new string[File.ReadLines(FilePathOfStatemetsStart[Count]).Count()]; // How many lines in each file
                            using (StreamReader file = new StreamReader(FilePathOfStatemetsStart[Count]))
                            {
                                string LineofFile;
                                int CountForLine = 0;
                                while ((LineofFile = file.ReadLine()) != null)
                                {
                                    NoOfStatmentsCmdArgument_2[Count][CountForLine] = "gawk 'NR ^> first ^&^& /^^}$/ { print NR; exit }' " + "first=" +
                                        LineofFile + " " + C_FileName + " >> "
                                        + FilePathOfStatemetsStart[Count].Replace("_Statements_Start.txt", "_Statements_End.txt"); // First Element is the Number before Space
                                    CountForLine++;
                                }
                            }
#endif
                            break;
                        case PrepareCommandsType.ShowCompactCalledFunctionsNames:
                            CalledCompactFunctionsNameCmdArgument[Count] = "cflow --depth=2 " + C_FileName + " > " + AnanlysisDirectory + "/" + FileName.Replace(".c", "_Called.txt"); // First Element is the Number before Space
                            break;
                        case PrepareCommandsType.GetStartLineNumberOfEachFunction:
                            GetStartLineNumberOfEachFunctionCmdArgument[Count] = "ctags -x --c-kinds=f " + C_FileName +
                                " | gawk -F ' ' '{ print $3 }' | sort -n > " + AnanlysisDirectory + "/" +
                                    FileName.Replace(".c", "_Func_Start.txt"); // It shall give sorted Starting Line Numbers of each main function
                            break;
                    }
                    Count++;
                }
            }
            catch (Exception ex)
            {
                ErrorArray = string.Concat(ErrorArray, ex.Message + Environment.NewLine);
            }
        }

        private bool ExecuteCommand(string Command)
        {
            string ErrorReader = null;
            bool RetVal = true;
            try
            {
                System.Diagnostics.Process Process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments ="/C " + Command; // /C so that it exit immediately from CMD otherwise will wait for User
                //startInfo.ErrorDialog = false;
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;
                startInfo.RedirectStandardError = true;
                startInfo.RedirectStandardInput = true;
                startInfo.RedirectStandardOutput = true;
                //startInfo.Verb = "runas"; // For Adminstrator Rights
                Process.StartInfo = startInfo;
                if(DebugMode == true)
                    File.AppendAllText(@"D:\Log_static.txt", Command + Environment.NewLine);
                bool ProcessStarted = Process.Start();
                if (ProcessStarted)
                {
                    //Get the output
                    ErrorReader = Process.StandardError.ReadToEnd();
                    Process.WaitForExit();
                    if (ErrorReader != string.Empty)
                    {
                        if(DebugMode == true)
                            File.AppendAllText(@"G:\Log_static.txt", ErrorReader + Environment.NewLine);
                        ErrorReader = ErrorReader.Replace("cscope", "StaticAnalysisTool:");
                        ErrorReader = ErrorReader.Replace("ctags", "StaticAnalysisTool:");
                        ErrorReader = ErrorReader.Replace("cflow", "StaticAnalysisTool:");
                        ErrorReader = ErrorReader.Replace("gcc", "StaticAnalysisTool:");
                        if (ErrorReader.Contains("this is the place of previous definition"))
                        {
                            ErrorArray = string.Concat(ErrorArray, "Two Functions' Declaration found.Problem May Occur." +
                                Environment.NewLine + 
                                "Hint : If Code has Preprocessor Directives(#ifdef etc) then provide and select option accordingly before analysing the code."
                                + Environment.NewLine);
                        }
                        if(DebugMode == true)
                            ErrorArray = string.Concat(ErrorArray, "Error in Executing " + Command + Environment.NewLine);
                        RetVal = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.Replace("cscope", "StaticAnalysisTool:");
                ex.Message.Replace("ctags", "StaticAnalysisTool:");
                ex.Message.Replace("cflow", "StaticAnalysisTool:");
                ex.Message.Replace("gcc", "StaticAnalysisTool:");

                ErrorArray = string.Concat(ErrorArray, ex.Message+ Environment.NewLine);
                RetVal = false;
            }
            return RetVal;
        }

        private void ClearOptionsVar()
        { 
            FunctionsTreeView =0;
            CalledFunctions = 0;
            HighlightNestedFunctionCalls=0;
            FloatingPointOperations=0;
            IncludeHeaderFiles=0;
            NoOfStatementsInFunction=0;
            DetailedCalledFunctionsTreeView=0;
        }

        private void UnDefMacrosRichTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space) // Dont Allow Enter Key
            {
                e.SuppressKeyPress = true;
            }
        }

        private void DefMacrosRichTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space) // Dont Allow Enter Key
            {
                e.SuppressKeyPress = true;
            }
        }


    }
    public static class PrepareCommandsType
    {
        public const int ShowFunctionsTreeView = 1,
            ShowNoOfStatementsInFunction_CreateStart = 2,
            ShowNoOfStatementsInFunction_CreateEnd = 3,
            ShowNoOfStatementsInFunction = 4,
            ShowHighlightNestedFunction = 5,
            ShowFloatingPointOperations = 6,
            ShowIncludeHeaderFiles = 7,
            FixPreprocessorDirectiveIssues = 8,
            ShowCompactCalledFunctionsNames = 9,
            RemoveComments = 10,
            SearchFloatDoubleVarDefinitionInTheirResFilesName = 11,
            SearchGlobalFloatDoubleVarsInAllFiles = 12,
            GetFileNamesForLocalFloatDoubleVars=13,
            FindLocalVarsUseInFiles=14,
            ArrangeAllFloatDoubleVarsInProperFormat = 15,
            GetFileNamesOfAllFloatDoubleVarsUsers=16,
            GetEndOfFunctionsInFilesOfFloatingPountOperations = 17,
            GetStartLineNumberOfEachFunction=18;

            
    }
    public static class LoopTags
    {
        public const int ForLoop = 1, WhileLoop = 2, DoWhileLoop = 3;
    }
    
}
