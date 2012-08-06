using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace descartes {
    public class DirectoryHandler {

        public event ProgressHandler Progress;
        public event ProgressHandler Finish;

        public delegate void ProgressHandler(DirectoryHandler m, ProgressEventArgs e);
        //public delegate void FinishHandler(DirectoryHandler m, ProgressEventArgs e);
        
        
        
        
        private String path = "";
        public String Path
        {
            get { return path; }
            set { path = value; }
        }

        private String outputDiscardedPath = "";
        public String OutputDiscardedPath
        {
            get { return outputDiscardedPath; }
            set { outputDiscardedPath = value; }
        }

        private String outputSelectedPath = "";
        public String OutputSelectedPath
        {
            get { return outputSelectedPath; }
            set { outputSelectedPath = value; }
        }

        private Boolean generateListFileForSelectedFiles = false;
        public Boolean GenerateListFileForSelectedFiles
        {
            get { return generateListFileForSelectedFiles; }
            set { generateListFileForSelectedFiles = value; }
        }

        private Boolean generateListFileForDiscardedFiles = false;
        public Boolean GenerateListFileForDiscardedFiles
        {
            get { return generateListFileForDiscardedFiles; }
            set { generateListFileForDiscardedFiles = value; }
        }

        private Boolean generateFileStructureForSelectedFiles = false;
        public Boolean GenerateFileStructureForSelectedFiles
        {
            get { return generateFileStructureForSelectedFiles; }
            set { generateFileStructureForSelectedFiles = value; }
        }

        private Boolean generateFileStructureForDiscardedFiles = false;
        public Boolean GenerateFileStructureForDiscardedFiles
        {
            get { return generateFileStructureForDiscardedFiles; }
            set { generateFileStructureForDiscardedFiles = value; }
        }

        private string discardedFilesListFileFullName = "";
        public string DiscardedFilesListFileFullName
        {
            get { return discardedFilesListFileFullName; }
            set { discardedFilesListFileFullName = value; }
        }

        private string selectedFilesListFileFullName = "";
        public string SelectedFilesListFileFullName
        {
            get { return selectedFilesListFileFullName; }
            set { selectedFilesListFileFullName = value; }
        }

        private Boolean keepCopyOfDiscardedFiles = false;
        public Boolean KeepCopyOfDiscardedFiles
        {
            get { return keepCopyOfDiscardedFiles; }
            set { keepCopyOfDiscardedFiles = value; }
        }

        private Boolean keepCopyOfSelectedFiles = false;
        public Boolean KeepCopyOfSelectedFiles
        {
            get { return keepCopyOfSelectedFiles; }
            set { keepCopyOfSelectedFiles = value; }
        }
        

        public ImageList inputList, selectedList, discardedList;
        private DirectoryInfo dirInfo;
        public DirectoryHandler(String path) {
            this.inputList = new ImageList();
            this.selectedList = new ImageList();
            this.discardedList = new ImageList();
            this.Path = path;
        }

        public Int32 fillInputList() {
            this.dirInfo = new DirectoryInfo(this.Path);
            IEnumerable<FileInfo> fileList = this.dirInfo.EnumerateFiles("*");
            List<String> files = new List<String>();
            foreach(FileInfo item in fileList.ToArray()) {
                
            }
            
            for (Int32 i = 0; i < fileList.Count(); i++) {
                if (files.IndexOf(System.IO.Path.GetFileNameWithoutExtension(fileList.ElementAt(i).Name)) < 0)
                {
                    files.Add(System.IO.Path.GetFileNameWithoutExtension(fileList.ElementAt(i).Name));
                }
                
            }
            files.Sort();


            foreach (String item in files) {
                fileList = this.dirInfo.EnumerateFiles(item + ".*");
                List<descartes.File> fil = new List<descartes.File>();
                for (Int32 i = 0; i < fileList.Count(); i++) {
                    fil.Add(new descartes.File(
                                            this.Path,
                                            fileList.ElementAt(i).Name,
                                            fileList.ElementAt(i).Extension
                                            
                        ));
                }
                this.inputList.add(new Image(fil));
            }
            return this.inputList.count();
        }

        public String getImagePathForItem(int item, String type = ".JPG")
        {
            String path = "";
            if (item >= 0 && item < this.inputList.count())
            {
                List<descartes.File> fl = this.inputList.getList().ElementAt(item).getFiles();
                Boolean found = false;

                for (int i = 0; (i < fl.Count && !found); i++)
                {
                    if (fl.ElementAt(i).Ext == type)
                    {
                        path = fl.ElementAt(i).Path + @"\" + fl.ElementAt(i).Name;
                        found = true;
                    }
                }

            }
            return path;
        }

        public static String getImagePathForItem(DirectoryHandler dh, int item, String type = ".JPG")
        {
            String path = "";
            if (item >= 0 && item < dh.inputList.count())
            {
                List<descartes.File> fl = dh.inputList.getList().ElementAt(item).getFiles();
                Boolean found = false;

                for (int i = 0; (i < fl.Count && !found); i++)
                {
                    if (fl.ElementAt(i).Ext == type)
                    {
                        path = fl.ElementAt(i).Path + @"\" + fl.ElementAt(i).Name;
                        found = true;
                    }
                }

            }
            return path;
        }

        public bool addToSelectedList(Image img) {
            return this.selectedList.add(img);
        }

        public bool addToDiscardedList(Image img) {
            return this.discardedList.add(img);
        }

        public bool removeFromSelectedList(Image img) {
            return this.selectedList.getList().Remove(img);
        }

        public bool removeFromDiscardedList(Image img)
        {
            return this.discardedList.getList().Remove(img);
        }

        public void checkAndCreateOutputDirs() {
            try
            {
                if (!Directory.Exists(this.OutputDiscardedPath))
                    Directory.CreateDirectory(this.OutputDiscardedPath);
                if (!Directory.Exists(this.OutputSelectedPath))
                    Directory.CreateDirectory(this.OutputSelectedPath);
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
            }
        }

        public Hashtable getProcessStats() { 
            Hashtable values = new Hashtable();
            Int32 input = 0;
            foreach (Image img in this.inputList.getList())
                foreach (File file in img.getFiles())
                    input++;

            Int32 selected = 0;
            foreach (Image img in this.selectedList.getList())
                foreach (File file in img.getFiles())
                    selected++;

            Int32 discarded = 0;
            foreach (Image img in this.discardedList.getList())
                foreach (File file in img.getFiles())
                    discarded++;

            values.Add("input", input);
            values.Add("selected", selected);
            values.Add("discarded", discarded);
            values.Add("ignored", (input - (selected + discarded)));

            return values; 
        }

        public void openExplorerWindow(string directory = @"c:\") {
            var runExplorer = new System.Diagnostics.ProcessStartInfo();
            runExplorer.FileName = "explorer.exe";
            runExplorer.Arguments = directory;
            System.Diagnostics.Process.Start(runExplorer); 
        }

        public Boolean cleanupProcessVars() {
            Boolean ret = true;
            try
            {
                this.inputList.getList().Clear();
                this.selectedList.getList().Clear();
                this.discardedList.getList().Clear();
                this.OutputDiscardedPath = "";
                this.OutputSelectedPath = "";
                this.Path = "";
            }
            catch (Exception ex) {
                ret = false;
            }
            return ret;
        }

        public void separateFiles()
        {
            Boolean ret = true;
            Int32 totalFiles = 0;
            try
            {
                if (this.GenerateFileStructureForDiscardedFiles || this.GenerateFileStructureForSelectedFiles)
                    this.checkAndCreateOutputDirs();

                //start collect process with discarded files
                StreamWriter fileWriter = new StreamWriter(this.DiscardedFilesListFileFullName);
                foreach (Image item in this.discardedList.getList())
                {
                    foreach (File file in item.getFiles())
                    {
                        //write discarded files list file
                        if (this.GenerateListFileForDiscardedFiles)
                            fileWriter.WriteLine(file.Name);

                        //write discarded files structure
                        if (this.GenerateFileStructureForDiscardedFiles)
                        {
                            file.move(this.OutputDiscardedPath + @"\" + file.Name, this.KeepCopyOfDiscardedFiles);
                        }
                        totalFiles++;

                        if (Progress != null)
                        {
                            ProgressEventArgs progress = new ProgressEventArgs();
                            progress.Progress = totalFiles;
                            Progress(this, progress);
                        }

                        //progressBar.Value++;
                        //progressBar.Update();
                    }
                }
                fileWriter.Flush();
                fileWriter.Close();
                fileWriter.Dispose();

                //next collect process with selected files
                fileWriter = new StreamWriter(this.SelectedFilesListFileFullName);
                foreach (Image item in this.selectedList.getList())
                {
                    foreach (File file in item.getFiles())
                    {
                        //selectedFilesPlainList.Add(file.ToString());
                        if (this.GenerateListFileForSelectedFiles)
                            fileWriter.WriteLine(file.Name);

                        if (this.GenerateFileStructureForSelectedFiles)
                        {
                            //write selected files structure
                            file.move(this.OutputSelectedPath + @"\" + file.Name, this.KeepCopyOfSelectedFiles);
                        }
                        totalFiles++;

                        if (Progress != null)
                        {
                            ProgressEventArgs progress = new ProgressEventArgs();
                            progress.Progress = totalFiles;
                            Progress(this, progress);
                        }
                        //progressBar.Value++;
                        //progressBar.Update();
                    }
                }
                fileWriter.Flush();
                fileWriter.Close();
                fileWriter.Dispose();
                
                if (Finish != null)
                {
                    ProgressEventArgs progress = new ProgressEventArgs();
                    progress.Progress = totalFiles;
                    progress.Data.Add("totalFiles",totalFiles);
                    Finish(this, progress);
                }
                
            }
            catch (Exception ex)
            {
                
                ret = false;
            }
            //return ret;
        }
    }

    public class File {
        private String path = "";
        public String Path {
            get { return path; }
            set { path = value; }
        }
        
        private String name = "";
        public String Name {
            get { return name; }
            set { name = value; }
        }
        
        private String ext = "";
        public String Ext {
            get { return ext; }
            set { ext = value; }
        }

        public File(String path, String name, String ext){
            this.Path = path;
            this.Name = name;
            this.Ext = ext;
          
        }

        public bool verify() {
            return System.IO.File.Exists(this.Path + @"\" + this.Name);
        }

        public bool move(string destFileFullPath, bool keepCopy = false) {
            bool ret = true;
            try
            {
                if (keepCopy)
                    System.IO.File.Copy(this.GetFullName(), destFileFullPath);
                else
                    System.IO.File.Move(this.GetFullName(), destFileFullPath);
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                ret = false;
            } 
            return ret;    
        }

        public String GetFullName()
        {
            return this.Path + @"\" + this.Name;
        }

    }

    public class Image {
        private String status = "unrated";
        public String Status
        {
            get { return status; }
            set { status = value; }
        }
        
        private List<File> fileList;

        public String getFileTitle() {
            return Path.GetFileNameWithoutExtension(this.fileList.First().Name);
        }

        public List<File> getFiles()
        {
            return this.fileList;
        }
            
        public Image(List<File> fileList) {
            this.fileList = fileList;
        }
      
        public Boolean isImage() {
            return true;
        }
    }

    public class ImageList {
        private Int32 current = 0;
        public Int32 Current
        {
            get { return current; }
            set { current = value; }
        }

        private List<Image> imageList;

        public List<Image> getList() {
            return this.imageList;
        }

        public ImageList() {
            this.imageList = new List<Image>();
        }

        public Boolean add(Image image) {
            this.imageList.Add(image);
            return (this.imageList.LastIndexOf(image) >= 0);
        }

        public Int32 count() {
            return this.imageList.Count;
        }

        public String selectCurrent() {
            this.imageList.ElementAt(this.Current).Status = "selected";
            return this.imageList.ElementAt(this.Current).Status;
        }

        public String discardCurrent()
        {
            this.imageList.ElementAt(this.Current).Status = "discarded";
            return this.imageList.ElementAt(this.Current).Status;
        }
    }


    public class ProgressEventArgs : EventArgs {
        private Int32 progress = 0;
        public Int32 Progress
        {
            get { return progress; }
            set { progress = value; }
        }

        private Hashtable data;
        public Hashtable Data
        {
            get { return data; }
            set { data = value; }
        }

        public ProgressEventArgs()
        {
            // TODO: Complete member initialization
            this.Data = new Hashtable();
        }
    }
}
