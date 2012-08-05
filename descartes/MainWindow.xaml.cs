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
        public BitmapImage unavailableImage, prevImage, nextImage, currentImage;
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
                listViewFilesFound.Items.Clear();
                textBoxInputFolder.Text = folderBrowser.SelectedPath;
                textBoxOutputSelectedFolder.Text = textBoxInputFolder.Text + @"\selected\";
                textBoxOutputDiscardedFolder.Text = textBoxInputFolder.Text + @"\discarded\";
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
            
            dh.inputList.Current = 0;
            imagePrev.Source = unavailableImage;

            dh.OutputSelectedPath = textBoxOutputSelectedFolder.Text;
            dh.OutputDiscardedPath = textBoxOutputDiscardedFolder.Text;

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
            tabItemProcess.Focus();
        }

        

        private void buttonPrevImage_Click(object sender, RoutedEventArgs e)
        {
            if ((Int32)dh.inputList.Current < dh.inputList.count()
                && (Int32)dh.inputList.Current > 0)
            {
                dh.inputList.Current--;

                String pathPrev = dh.getImagePathForItem(dh.inputList.Current - 1, ".JPG");
                prevImage = null;
                prevImage = (System.IO.File.Exists(pathPrev)) ? new BitmapImage(new Uri(pathPrev)) : unavailableImage;
                imagePrev.Source = prevImage;
                prevImage = null;
                labelPrevImageFilename.Content = pathPrev;

                String pathCurr = dh.getImagePathForItem(dh.inputList.Current, ".JPG");
                currentImage = null;
                currentImage = (System.IO.File.Exists(pathCurr)) ? new BitmapImage(new Uri(pathCurr)) : unavailableImage;
                imageCurrent.Source = currentImage;
                currentImage = null; 
                labelCurrentImageFilename.Content = pathCurr;
                setCurrentImageStatusLabel();
                
                String pathNext = dh.getImagePathForItem(dh.inputList.Current + 1, ".JPG");
                nextImage = null;
                nextImage = (System.IO.File.Exists(pathNext)) ? new BitmapImage(new Uri(pathNext)) : unavailableImage;
                imageNext.Source = nextImage;
                prevImage = null; 
                labelNextImageFilename.Content = pathNext;
            }
            else
            {
                imagePrev.Source = unavailableImage;
            }
            labelCurrentImagePositionInList.Content = getCurrentImagePositionCaption();
            checkInputListBounds();
        }

        private void buttonNextImage_Click(object sender, RoutedEventArgs e)
        {
            if ((Int32)dh.inputList.Current < dh.inputList.count()
                && (Int32)dh.inputList.Current >= 0)
            {
                dh.inputList.Current++;

                String pathPrev = dh.getImagePathForItem(dh.inputList.Current - 1, ".JPG");
                prevImage = null;
                prevImage = (System.IO.File.Exists(pathPrev)) ? new BitmapImage(new Uri(pathPrev)) : unavailableImage;
                imagePrev.Source = prevImage;
                prevImage = null;
                labelPrevImageFilename.Content = pathPrev;

                String pathCurr = dh.getImagePathForItem(dh.inputList.Current, ".JPG");
                currentImage = null;
                currentImage = (System.IO.File.Exists(pathCurr)) ? new BitmapImage(new Uri(pathCurr)) : unavailableImage;
                imageCurrent.Source = currentImage;
                currentImage = null;
                labelCurrentImageFilename.Content = pathCurr;
                setCurrentImageStatusLabel();

                String pathNext = dh.getImagePathForItem(dh.inputList.Current + 1, ".JPG");
                nextImage = null;
                nextImage = (System.IO.File.Exists(pathNext)) ? new BitmapImage(new Uri(pathNext)) : unavailableImage;
                imageNext.Source = nextImage;
                prevImage = null;
                labelNextImageFilename.Content = pathNext;
            }
            else {
                imagePrev.Source = unavailableImage;
            }
            labelCurrentImagePositionInList.Content = getCurrentImagePositionCaption();
            checkInputListBounds();
        }

        private void checkInputListBounds() { 
            //check prev button
            if (dh.inputList.count() > 0 && (Int32)dh.inputList.Current != 0)
            {
                buttonPrevImage.IsEnabled = true;
            }
            else {
                buttonPrevImage.IsEnabled = false;    
            }

            //check next button
            if (dh.inputList.count() > 0
                    && (Int32)dh.inputList.Current < dh.inputList.count() -1
                )
            {
                buttonNextImage.IsEnabled = true;
            }
            else
            {
                buttonNextImage.IsEnabled = false;
            }

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

        private void buttonRestart_Click(object sender, RoutedEventArgs e)
        {
            buttonStartProcess_Click(sender, e);
        }

        private void buttonNextStep_Click(object sender, RoutedEventArgs e)
        {
            listViewSelectedImages.Items.Clear();
            listViewDiscardedImages.Items.Clear();
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
            labelNumDiscardedImages.Content = "(" + dh.discardedList.count().ToString() + ")";
            tabItemOutput.Focus();
        }

        private void buttonEndProcess_Click(object sender, RoutedEventArgs e) {
            progressBarOutputProcess.Minimum = 0;
            progressBarOutputProcess.Maximum = dh.discardedList.count() + dh.selectedList.count();

            dh.GenerateListFileForSelectedFiles = (comboBoxSelectedImagesOutputFormat.SelectedIndex == 1 || comboBoxSelectedImagesOutputFormat.SelectedIndex == 2);
            dh.GenerateListFileForDiscardedFiles = (comboBoxDiscardedImagesOutputFormat.SelectedIndex == 1 || comboBoxDiscardedImagesOutputFormat.SelectedIndex == 2);
            dh.GenerateFileStructureForSelectedFiles = (comboBoxSelectedImagesOutputFormat.SelectedIndex == 0|| comboBoxSelectedImagesOutputFormat.SelectedIndex == 2);
            dh.GenerateFileStructureForDiscardedFiles = (comboBoxDiscardedImagesOutputFormat.SelectedIndex == 0 || comboBoxDiscardedImagesOutputFormat.SelectedIndex == 2);
            dh.DiscardedFilesListFileFullName = dh.OutputDiscardedPath + @"\..\descartes.discarded.lst";
            dh.SelectedFilesListFileFullName = dh.OutputSelectedPath + @"\..\descartes.selected.lst";
            dh.KeepCopyOfDiscardedFiles = (comboBoxDiscardedImagesMoveCopy.SelectedIndex == 2);
            dh.KeepCopyOfSelectedFiles = (comboBoxSelectedImagesMoveCopy.SelectedIndex == 2);

            if (dh.GenerateFileStructureForDiscardedFiles || dh.GenerateFileStructureForSelectedFiles)
                dh.checkAndCreateOutputDirs();

            //start collect process with discarded files
            StreamWriter fileWriter = new StreamWriter(dh.DiscardedFilesListFileFullName);
            foreach (Image item in dh.discardedList.getList()) {
                foreach (File file in item.getFiles()) {
                    //write discarded files list file
                    if (dh.GenerateListFileForDiscardedFiles)
                        fileWriter.WriteLine(file.Name);
                    
                    //write discarded files structure
                    if (dh.GenerateFileStructureForDiscardedFiles) {
                        file.move(dh.OutputDiscardedPath + @"\" + file.Name, dh.KeepCopyOfDiscardedFiles);     
                    }
                    progressBarOutputProcess.Value++;
                    progressBarOutputProcess.UpdateLayout();
                }
            }
            fileWriter.Flush();
            fileWriter.Close();
            fileWriter.Dispose();

            //next collect process with selected files
            fileWriter = new StreamWriter(dh.SelectedFilesListFileFullName);
            foreach (Image item in dh.selectedList.getList()) {
                foreach (File file in item.getFiles()) {
                    //selectedFilesPlainList.Add(file.ToString());
                    if (dh.GenerateListFileForSelectedFiles)
                        fileWriter.WriteLine(file.Name);

                    if (dh.GenerateFileStructureForSelectedFiles) {
                        //write selected files structure
                        file.move(dh.OutputSelectedPath + @"\" + file.Name, dh.KeepCopyOfSelectedFiles);  
                    }

                    progressBarOutputProcess.Value++;
                    progressBarOutputProcess.UpdateLayout();
                }
            }
            fileWriter.Flush();
            fileWriter.Close();
            fileWriter.Dispose();

            System.Windows.Forms.MessageBox.Show("Process finished!");
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            tabControlMain.Width = this.Width;
            tabControlMain.Height = this.Height;
        }

        private void buttonBrowseSelectedFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            if (folderBrowser.ShowDialog().Equals(System.Windows.Forms.DialogResult.OK))
            {
                textBoxOutputSelectedFolder.Text = folderBrowser.SelectedPath;
            }
        }

        private void buttonBrowseDiscardedFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            if (folderBrowser.ShowDialog().Equals(System.Windows.Forms.DialogResult.OK))
            {
                textBoxOutputDiscardedFolder.Text = folderBrowser.SelectedPath;
            }
        }

        private void textBoxOutputSelectedFolder_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!textBoxOutputSelectedFolder.Text.Equals(""))
            {
                textBoxOutputSelectedFolder.Background = Brushes.LightGreen;
                if (buttonStartProcess != null) this.buttonStartProcess.IsEnabled = true;
            }
            else
            {
                textBoxOutputSelectedFolder.Background = Brushes.LightCoral;
                if (buttonStartProcess != null) this.buttonStartProcess.IsEnabled = false;
            }
        }

        private void textBoxOutputDiscardedFolder_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!textBoxOutputDiscardedFolder.Text.Equals(""))
            {
                textBoxOutputDiscardedFolder.Background = Brushes.LightGreen;
                if (buttonStartProcess != null) this.buttonStartProcess.IsEnabled = true;
            }
            else
            {
                textBoxOutputDiscardedFolder.Background = Brushes.LightCoral;
                if (buttonStartProcess != null) this.buttonStartProcess.IsEnabled = false;
            }
        }
        
    }
}
