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

            String pathCurr = dh.getImagePathForItem(dh.inputList.Current, ".JPG");
            imageCurrent.Source = System.IO.File.Exists(pathCurr) ? new BitmapImage(new Uri(pathCurr)) : unavailableImage;
            labelCurrentImageFilename.Content = pathCurr;
            setCurrentImageStatusLabel();

            String pathNext = dh.getImagePathForItem(dh.inputList.Current + 1, ".JPG");
            BitmapImage bmp = System.IO.File.Exists(pathNext) ? new BitmapImage(new Uri(pathNext)) : unavailableImage;
            imageNext.Source = bmp;
            labelNextImageFilename.Content = pathNext;
            labelCurrentImagePositionInList.Content = getCurrentImagePositionCaption();
            tabItemProcess.IsEnabled = true;
            tabItemOutput.IsEnabled = true;
        }

        

        private void buttonPrevImage_Click(object sender, RoutedEventArgs e)
        {
            if ((Int32)dh.inputList.Current < dh.inputList.count()
                && (Int32)dh.inputList.Current > 0)
            {
                dh.inputList.Current--;

                String pathPrev = dh.getImagePathForItem(dh.inputList.Current - 1, ".JPG");
                imagePrev.Source = System.IO.File.Exists(pathPrev) ? new BitmapImage(new Uri(pathPrev)) : unavailableImage;
                labelPrevImageFilename.Content = pathPrev;

                String pathCurr = dh.getImagePathForItem(dh.inputList.Current, ".JPG");
                imageCurrent.Source = System.IO.File.Exists(pathCurr) ? new BitmapImage(new Uri(pathCurr)) : unavailableImage;
                labelCurrentImageFilename.Content = pathCurr;
                setCurrentImageStatusLabel();
                
                String pathNext = dh.getImagePathForItem(dh.inputList.Current + 1, ".JPG");
                imageNext.Source = System.IO.File.Exists(pathNext) ? new BitmapImage(new Uri(pathNext)) : unavailableImage;
                labelNextImageFilename.Content = pathNext;
            }
            else
            {
                imagePrev.Source = unavailableImage;
            }
            labelCurrentImagePositionInList.Content = getCurrentImagePositionCaption();
        }

        private void buttonNextImage_Click(object sender, RoutedEventArgs e)
        {
            if ((Int32)dh.inputList.Current < dh.inputList.count()
                && (Int32)dh.inputList.Current >= 0)
            {
                dh.inputList.Current++;


                String pathPrev = dh.getImagePathForItem(dh.inputList.Current - 1, ".JPG");
                imagePrev.Source = System.IO.File.Exists(pathPrev) ? new BitmapImage(new Uri(pathPrev)) : unavailableImage;
                labelPrevImageFilename.Content = pathPrev;

                String pathCurr = dh.getImagePathForItem(dh.inputList.Current, ".JPG");
                imageCurrent.Source = System.IO.File.Exists(pathCurr) ? new BitmapImage(new Uri(pathCurr)) : unavailableImage;
                labelCurrentImageFilename.Content = pathCurr;
                setCurrentImageStatusLabel();
                
                String pathNext = dh.getImagePathForItem(dh.inputList.Current + 1, ".JPG");
                imageNext.Source = System.IO.File.Exists(pathNext) ? new BitmapImage(new Uri(pathNext)) : unavailableImage;
                labelNextImageFilename.Content = pathNext;
            }
            else {
                imagePrev.Source = unavailableImage;
            }
            labelCurrentImagePositionInList.Content = getCurrentImagePositionCaption();
        }



        private void buttonSelect_Click(object sender, RoutedEventArgs e)
        {
            dh.inputList.selectCurrent();
            dh.removeFromSelectedList(dh.inputList.getList().ElementAt(dh.inputList.Current));
            dh.addToSelectedList(dh.inputList.getList().ElementAt(dh.inputList.Current));
            setCurrentImageStatusLabel();
        }

        private void buttonDiscard_Click(object sender, RoutedEventArgs e)
        {
            dh.inputList.discardCurrent();
            dh.removeFromDiscardedList(dh.inputList.getList().ElementAt(dh.inputList.Current));
            dh.addToDiscardedList(dh.inputList.getList().ElementAt(dh.inputList.Current));
            setCurrentImageStatusLabel();
        }


        private String getCurrentImagePositionCaption() {
            return "(" + (dh.inputList.Current + 1).ToString() + " of " + dh.inputList.count().ToString() + ")";
        }

        private void setCurrentImageStatusLabel() {
            if (dh.inputList.getList().ElementAt(dh.inputList.Current).Status.Equals("selected"))
            {

                labelCurrentImageStatus.Foreground = Brushes.Green;
                labelCurrentImageStatus.Content = "SELECTED";
            }
            else if (dh.inputList.getList().ElementAt(dh.inputList.Current).Status.Equals("discarded"))
            {
                labelCurrentImageStatus.Foreground = Brushes.Red;
                labelCurrentImageStatus.Content = "DISCARDED";
            }
            else {
                labelCurrentImageStatus.Foreground = Brushes.DarkGray;
                labelCurrentImageStatus.Content = "UNRATED";            
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void tabItemOutput_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            
        }

        private void tabItemProcess_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void tabControlMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabControlMain.SelectedIndex == 2) {
                foreach (Image item in dh.selectedList.getList())
                {
                    String extensions = "";
                    foreach (descartes.File file in item.getFiles())
                    {
                        extensions += "(" + file.Ext + ")";
                    }

                    listViewSelectedImages.Items.Add(
                            item.getFileTitle() + " " + extensions
                        );
                }
                labelNumSelectedImages.Content = "(" + dh.selectedList.count().ToString() + ")";
                foreach (Image item in dh.discardedList.getList())
                {
                    String extensions = "";
                    foreach (descartes.File file in item.getFiles())
                    {
                        extensions += "(" + file.Ext + ")";
                    }

                    listViewDiscardedImages.Items.Add(
                            item.getFileTitle() + " " + extensions
                        );
                }
                labelNumSelectedImages.Content = "(" + dh.discardedList.count().ToString() + ")";
            }
        }
    }
}
