using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;
namespace descartes
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DirectoryHandler dh;
        public BitmapImage unavailableImage;
        public String app_path;
        public MainWindow()
        {
            InitializeComponent();
            app_path = System.Reflection.Assembly.GetExecutingAssembly().Location;

            unavailableImage = new BitmapImage(new Uri(System.IO.Path.GetDirectoryName(app_path) + @"\Images\no.gif"));
            //'C:\Users\tupolev\Documents\Visual Studio 2010\Projects\descartes\descartes\bin\Debug\Images\no.gif'
        }

        private void buttonBrowse_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            if (folderBrowser.ShowDialog().Equals(System.Windows.Forms.DialogResult.OK))
            {
                textBoxInputFolder.Text = folderBrowser.SelectedPath;
                
                progressBarLoading.Visibility = System.Windows.Visibility.Visible;
                labelNumFiles.Content = "Loading";
                dh = new DirectoryHandler(textBoxInputFolder.Text);
                labelNumFiles.Content = dh.fillInputList();
                foreach (Image item in dh.inputList.getList()) {

                    String extensions = "";
                    foreach (descartes.File file in item.getFiles()) {
                        extensions += "(" + file.Ext + ")";
                    }
                    
                    listViewFilesFound.Items.Add(
                            item.getFileTitle() + " " + extensions
                        );
                }

                progressBarLoading.Visibility = System.Windows.Visibility.Hidden;
               
            }
        }

        private void textBoxInputFolder_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Directory.Exists(textBoxInputFolder.Text))
            {
                textBoxInputFolder.Background = Brushes.LightGreen;
                if (buttonStartProcess != null)  this.buttonStartProcess.IsEnabled = true;
            }
            else
            {
                textBoxInputFolder.Background = Brushes.LightCoral;
                if (buttonStartProcess != null) this.buttonStartProcess.IsEnabled = false;
            }
        }

        private void buttonStartProcess_Click(object sender, RoutedEventArgs e)
        {
            tabItemProcess.Focus();
            dh.inputList.Current = 0;
            imagePrev.Source = unavailableImage;

            String pathCurr = getImagePathForItem(dh.inputList.Current, ".JPG");
            imageCurrent.Source = System.IO.File.Exists(pathCurr) ? new BitmapImage(new Uri(pathCurr)) : unavailableImage;

            String pathNext = getImagePathForItem(dh.inputList.Current + 1, ".JPG");
            BitmapImage bmp = System.IO.File.Exists(pathNext) ? new BitmapImage(new Uri(pathNext)) : unavailableImage;
            imageNext.Source = bmp;

        }

        private String getImagePathForItem(int item, String type = ".JPG") {
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

        private void buttonPrevImage_Click(object sender, RoutedEventArgs e)
        {
            if ((Int32)dh.inputList.Current < dh.inputList.count()
                && (Int32)dh.inputList.Current > 0)
            {
                dh.inputList.Current--;

                String pathPrev = getImagePathForItem(dh.inputList.Current - 1, ".JPG");
                imagePrev.Source = System.IO.File.Exists(pathPrev) ? new BitmapImage(new Uri(pathPrev)) : unavailableImage;

                String pathCurr = getImagePathForItem(dh.inputList.Current, ".JPG");
                imageCurrent.Source = System.IO.File.Exists(pathCurr) ? new BitmapImage(new Uri(pathCurr)) : unavailableImage;

                String pathNext = getImagePathForItem(dh.inputList.Current + 1, ".JPG");
                imageNext.Source = System.IO.File.Exists(pathNext) ? new BitmapImage(new Uri(pathNext)) : unavailableImage;
            }
            else
            {
                imagePrev.Source = unavailableImage;
            }
        }

        private void buttonNextImage_Click(object sender, RoutedEventArgs e)
        {
            if ((Int32)dh.inputList.Current < dh.inputList.count()
                && (Int32)dh.inputList.Current >= 0)
            {
                dh.inputList.Current++;


                String pathPrev = getImagePathForItem(dh.inputList.Current - 1, ".JPG");
                imagePrev.Source = System.IO.File.Exists(pathPrev) ? new BitmapImage(new Uri(pathPrev)) : unavailableImage;

                String pathCurr = getImagePathForItem(dh.inputList.Current, ".JPG");
                imageCurrent.Source = System.IO.File.Exists(pathCurr) ? new BitmapImage(new Uri(pathCurr)) : unavailableImage;

                String pathNext = getImagePathForItem(dh.inputList.Current + 1, ".JPG");
                imageNext.Source = System.IO.File.Exists(pathNext) ? new BitmapImage(new Uri(pathNext)) : unavailableImage;
            }
            else {
                imagePrev.Source = unavailableImage;
            }
        }



        private void buttonSelect_Click(object sender, RoutedEventArgs e)
        {
            dh.inputList.selectCurrent();
        }

        private void buttonDiscard_Click(object sender, RoutedEventArgs e)
        {
            dh.inputList.discardCurrent();
        }
    }
}
