using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace descartes {
    public class DirectoryHandler {
        private String path = "";
        public String Path
        {
            get { return path; }
            set { path = value; }
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
            return System.IO.File.Exists(this.Path + @"\" + this.Name + "." + this.Ext);
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

}
