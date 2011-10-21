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

        public MainWindow()
        {
            InitializeComponent();
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
            }
            else
            {
                textBoxInputFolder.Background = Brushes.LightCoral;
            }
        }

        private void buttonStartProcess_Click(object sender, RoutedEventArgs e)
        {
            tabItemProcess.Focus();
        }

        
    }
}
