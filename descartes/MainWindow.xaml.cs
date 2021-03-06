﻿using System;
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
        public System.Threading.Thread th;
        public BitmapImage unavailableImage, prevImage, nextImage, currentImage;
        public String app_path;
        public MainWindow()
        {
            InitializeComponent();
            app_path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            unavailableImage = new BitmapImage(new Uri(System.IO.Path.GetDirectoryName(app_path) + @"\Images\no.gif"));
        }

        private void buttonBrowse_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            if (folderBrowser.ShowDialog().Equals(System.Windows.Forms.DialogResult.OK))
            {
                textBoxInputFolder.Text = folderBrowser.SelectedPath;
                textBoxOutputSelectedFolder.Text = textBoxInputFolder.Text + @"\selected\";
                textBoxOutputDiscardedFolder.Text = textBoxInputFolder.Text + @"\discarded\";

                this.buttonReloadFilesFound_Click(sender, e);

            }
        }

        private void textBoxInputFolder_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Directory.Exists(textBoxInputFolder.Text))
            {
                textBoxInputFolder.Background = Brushes.LightGreen;
                if (buttonStartProcess != null) this.buttonStartProcess.IsEnabled = true;
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
            labelCurrentImageFilename.Content = System.IO.Path.GetFileName(pathCurr);
            setCurrentImageStatusLabel();

            String pathNext = dh.getImagePathForItem(dh.inputList.Current + 1, ".JPG");
            BitmapImage bmp = System.IO.File.Exists(pathNext) ? new BitmapImage(new Uri(pathNext)) : unavailableImage;
            imageNext.Source = bmp;
            labelNextImageFilename.Content = System.IO.Path.GetFileName(pathNext);
            labelCurrentImagePositionInList.Content = getCurrentImagePositionCaption();
            tabItemProcess.IsEnabled = true;
            tabItemOutput.IsEnabled = true;
            checkInputListBounds();
            tabItemProcess.Focus();
        }



        private void buttonPrevImage_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Input.Cursor oldCursor = this.Cursor;
            this.Cursor = System.Windows.Input.Cursors.AppStarting;
            if ((Int32)dh.inputList.Current < dh.inputList.count()
                && (Int32)dh.inputList.Current > 0)
            {
                dh.inputList.Current--;

                String pathPrev = dh.getImagePathForItem(dh.inputList.Current - 1, ".JPG");
                prevImage = null;
                prevImage = (System.IO.File.Exists(pathPrev)) ? new BitmapImage(new Uri(pathPrev)) : unavailableImage;
                imagePrev.Source = prevImage;
                prevImage = null;
                labelPrevImageFilename.Content = System.IO.Path.GetFileName(pathPrev);

                String pathCurr = dh.getImagePathForItem(dh.inputList.Current, ".JPG");
                currentImage = null;
                currentImage = (System.IO.File.Exists(pathCurr)) ? new BitmapImage(new Uri(pathCurr)) : unavailableImage;
                imageCurrent.Source = currentImage;
                currentImage = null;
                labelCurrentImageFilename.Content = System.IO.Path.GetFileName(pathCurr);
                setCurrentImageStatusLabel();

                String pathNext = dh.getImagePathForItem(dh.inputList.Current + 1, ".JPG");
                nextImage = null;
                nextImage = (System.IO.File.Exists(pathNext)) ? new BitmapImage(new Uri(pathNext)) : unavailableImage;
                imageNext.Source = nextImage;
                prevImage = null;
                labelNextImageFilename.Content = System.IO.Path.GetFileName(pathNext);
            }
            else
            {
                imagePrev.Source = unavailableImage;
            }
            labelCurrentImagePositionInList.Content = getCurrentImagePositionCaption();
            checkInputListBounds();
            this.Cursor = oldCursor;
        }

        private void buttonNextImage_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Input.Cursor oldCursor = this.Cursor;
            this.Cursor = System.Windows.Input.Cursors.AppStarting;

            if ((Int32)dh.inputList.Current < dh.inputList.count()
                && (Int32)dh.inputList.Current >= 0)
            {
                dh.inputList.Current++;

                String pathPrev = dh.getImagePathForItem(dh.inputList.Current - 1, ".JPG");
                prevImage = null;
                prevImage = (System.IO.File.Exists(pathPrev)) ? new BitmapImage(new Uri(pathPrev)) : unavailableImage;
                imagePrev.Source = prevImage;
                prevImage = null;
                labelPrevImageFilename.Content = System.IO.Path.GetFileName(pathPrev);

                String pathCurr = dh.getImagePathForItem(dh.inputList.Current, ".JPG");
                currentImage = null;
                currentImage = (System.IO.File.Exists(pathCurr)) ? new BitmapImage(new Uri(pathCurr)) : unavailableImage;
                imageCurrent.Source = currentImage;
                currentImage = null;
                labelCurrentImageFilename.Content = System.IO.Path.GetFileName(pathCurr);
                setCurrentImageStatusLabel();

                String pathNext = dh.getImagePathForItem(dh.inputList.Current + 1, ".JPG");
                nextImage = null;
                nextImage = (System.IO.File.Exists(pathNext)) ? new BitmapImage(new Uri(pathNext)) : unavailableImage;
                imageNext.Source = nextImage;
                prevImage = null;
                labelNextImageFilename.Content = System.IO.Path.GetFileName(pathNext);
            }
            else
            {
                imagePrev.Source = unavailableImage;
            }
            labelCurrentImagePositionInList.Content = getCurrentImagePositionCaption();
            checkInputListBounds();
            this.Cursor = oldCursor;
        }

        private void checkInputListBounds()
        {
            //check prev button
            if (dh.inputList.count() > 0 && (Int32)dh.inputList.Current != 0)
            {
                buttonPrevImage.IsEnabled = true;
            }
            else
            {
                buttonPrevImage.IsEnabled = false;
            }

            //check next button
            if (dh.inputList.count() > 0
                    && (Int32)dh.inputList.Current < dh.inputList.count() - 1
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
            dh.removeFromDiscardedList(dh.inputList.getList().ElementAt(dh.inputList.Current));
            dh.removeFromSelectedList(dh.inputList.getList().ElementAt(dh.inputList.Current));
            dh.addToSelectedList(dh.inputList.getList().ElementAt(dh.inputList.Current));
            setCurrentImageStatusLabel();
        }

        private void buttonDiscard_Click(object sender, RoutedEventArgs e)
        {
            dh.inputList.discardCurrent();
            dh.removeFromSelectedList(dh.inputList.getList().ElementAt(dh.inputList.Current));
            dh.removeFromDiscardedList(dh.inputList.getList().ElementAt(dh.inputList.Current));
            dh.addToDiscardedList(dh.inputList.getList().ElementAt(dh.inputList.Current));
            setCurrentImageStatusLabel();
        }


        private String getCurrentImagePositionCaption()
        {
            return "(" + (dh.inputList.Current + 1).ToString() + " of " + dh.inputList.count().ToString() + ")";
        }

        private void setCurrentImageStatusLabel()
        {
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
            else
            {
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

        private void buttonEndProcess_Click(object sender, RoutedEventArgs e)
        {

            progressBarOutputProcess.Minimum = 0;
            progressBarOutputProcess.Maximum = dh.discardedList.count() + dh.selectedList.count();

            dh.GenerateListFileForSelectedFiles = (comboBoxSelectedImagesOutputFormat.SelectedIndex == 1 || comboBoxSelectedImagesOutputFormat.SelectedIndex == 2);
            dh.GenerateListFileForDiscardedFiles = (comboBoxDiscardedImagesOutputFormat.SelectedIndex == 1 || comboBoxDiscardedImagesOutputFormat.SelectedIndex == 2);
            dh.GenerateFileStructureForSelectedFiles = (comboBoxSelectedImagesOutputFormat.SelectedIndex == 0 || comboBoxSelectedImagesOutputFormat.SelectedIndex == 2);
            dh.GenerateFileStructureForDiscardedFiles = (comboBoxDiscardedImagesOutputFormat.SelectedIndex == 0 || comboBoxDiscardedImagesOutputFormat.SelectedIndex == 2);
            dh.DiscardedFilesListFileFullName = dh.OutputDiscardedPath + @"\..\descartes.discarded.lst";
            dh.SelectedFilesListFileFullName = dh.OutputSelectedPath + @"\..\descartes.selected.lst";
            dh.KeepCopyOfDiscardedFiles = (comboBoxDiscardedImagesMoveCopy.SelectedIndex == 2);
            dh.KeepCopyOfSelectedFiles = (comboBoxSelectedImagesMoveCopy.SelectedIndex == 2);


            
            dh.Progress += new DirectoryHandler.ProgressHandler(onSeparateFilesProgress);
            dh.Finish += new DirectoryHandler.ProgressHandler(onSeparateFilesFinish);
            dh.separateFiles();
            
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

        private void buttonReloadFilesFound_Click(object sender, RoutedEventArgs e)
        {
            listViewFilesFound.Items.Clear();
            progressBarLoading.Visibility = System.Windows.Visibility.Visible;
            labelNumFiles.Content = "Loading";
            dh = new DirectoryHandler(textBoxInputFolder.Text);
            labelNumFiles.Content = dh.fillInputList();
            foreach (Image item in dh.inputList.getList())
            {

                String extensions = "";
                foreach (descartes.File file in item.getFiles())
                {
                    extensions += "(" + file.Ext + ")";
                }

                listViewFilesFound.Items.Add(
                        item.getFileTitle() + " " + extensions
                    );
            }

            progressBarLoading.Visibility = System.Windows.Visibility.Hidden;
        }

        private void buttonExitNow_Click(object sender, RoutedEventArgs e)
        {
            descartes.App.Current.Shutdown();
        }

        private void buttonStartAgainWithNew_Click(object sender, RoutedEventArgs e)
        {
            cleanUpControls();
        }

        private void tabControlMain_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void labelSummaryOpenInputFolder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            dh.openExplorerWindow(dh.Path);
        }

        private void labelSummaryOpenSelectedFolder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            dh.openExplorerWindow(dh.OutputSelectedPath);
        }

        private void labelSummaryOpenDiscardedFolder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            dh.openExplorerWindow(dh.OutputDiscardedPath);
        }

        private Boolean cleanUpControls()
        {
            Boolean ret = true;
            ret = dh.cleanupProcessVars();
            progressBarOutputProcess.Value = 0;
            progressBarOutputProcess.Maximum = 0;
            listViewDiscardedImages.Items.Clear();
            listViewSelectedImages.Items.Clear();
            imageCurrent.Source = null;
            imagePrev.Source = null;
            imageNext.Source = null;
            buttonPrevImage.IsEnabled = false;
            buttonNextImage.IsEnabled = false;
            listViewFilesFound.Items.Clear();
            labelNumFiles.Content = "0";
            textBoxOutputDiscardedFolder.Text = @"c:\";
            textBoxOutputSelectedFolder.Text = @"c:\";
            textBoxInputFolder.Text = @"c:\";
            buttonStartProcess.IsEnabled = false;
            tabItemInput.IsEnabled = true;
            tabItemProcess.IsEnabled = false;
            tabItemOutput.IsEnabled = false;
            tabItemEnd.IsEnabled = false;
            tabItemInput.Focus();
            return ret;
        }

        private void loadPreset(string initialDir) {
            textBoxInputFolder.Text = initialDir;
            textBoxOutputSelectedFolder.Text = textBoxInputFolder.Text + @"selected\";
            textBoxOutputDiscardedFolder.Text = textBoxInputFolder.Text + @"discarded\";
            this.buttonReloadFilesFound_Click(null, null);
            tabItemInput.Focus();
        }
        private void buttonStartAgainWithSelected_Click(object sender, RoutedEventArgs e)
        {
            string dir = dh.OutputSelectedPath;
            cleanUpControls();
            this.loadPreset(dir);
        }

        private void buttonStartAgainWithDiscarded_Click(object sender, RoutedEventArgs e)
        {
            string dir = dh.OutputDiscardedPath;
            cleanUpControls();
            this.loadPreset(dir);
        }

        delegate void SetTextCallback(string text);
        public void onSeparateFilesProgress(DirectoryHandler dh, ProgressEventArgs e)
        {
            progressBarOutputProcess.Value++;
            progressBarOutputProcess.UpdateLayout();
            System.Windows.Forms.Application.DoEvents();
        }

        public void onSeparateFilesFinish(DirectoryHandler dh, ProgressEventArgs e)
        {
            progressBarOutputProcess.Value++;
            progressBarOutputProcess.UpdateLayout();

            System.Windows.Forms.MessageBox.Show("Process finished!", "Descartes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            System.Collections.Hashtable stats = dh.getProcessStats();
            labelSummaryTotalFiles.Content = stats["input"];
            labelSummarySelectedFiles.Content = stats["selected"];
            labelSummaryDiscardedFiles.Content = stats["discarded"];
            labelSummaryIgnoredFiles.Content = stats["ignored"];
            tabItemInput.IsEnabled = false;
            tabItemProcess.IsEnabled = false;
            tabItemOutput.IsEnabled = false;
            tabItemEnd.IsEnabled = true;
            tabItemEnd.Focus();
        }
        
        
        private void dyn_resize()
        {
            if (this.WindowState == System.Windows.WindowState.Maximized)
            {
                tabControlMain.Width = Screen.PrimaryScreen.WorkingArea.Width - 20;
                tabControlMain.Height = Screen.PrimaryScreen.WorkingArea.Height - 30;
            }
            else {
                tabControlMain.Width = this.RestoreBounds.Width - 20;
                tabControlMain.Height = this.RestoreBounds.Height - 30;
            }
            
            
            List<Grid> grids = new List<Grid>();
            grids.Add(gridTabInput);
            grids.Add(gridTabProcess);
            grids.Add(gridTabOutput);
            grids.Add(gridTabEnd);

            grids.ForEach(x =>
            {
                x.Width = tabControlMain.ActualWidth - 5;
                x.Height = tabControlMain.ActualHeight - tabItemInput.ActualHeight - 5;
            });

            this.UpdateLayout();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.dyn_resize();
        }
        
        private void Window_StateChanged(object sender, EventArgs e)
        {
            this.dyn_resize();            
        }



    }
}
