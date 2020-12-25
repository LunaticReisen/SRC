using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace MinecraftMETACreater {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            VerComboBox();
            SkinComboBox();
        }

        //版本下拉框选项
        public void VerComboBox() {
            BindingList<BoxInfo> infoList = new BindingList<BoxInfo>();
            VerBox.ItemsSource = infoList;
            VerBox.DisplayMemberPath = "Title";
            VerBox.SelectedValuePath = "Id";
            infoList.Add(new BoxInfo() { Id = 1, Title = "1.6.1-1.8.9" });
            infoList.Add(new BoxInfo() { Id = 2, Title = "1.9-1.10.2" });
            infoList.Add(new BoxInfo() { Id = 3, Title = "1.11-1.12" });
            infoList.Add(new BoxInfo() { Id = 4, Title = "1.13-1.14.4" });
            infoList.Add(new BoxInfo() { Id = 5, Title = "1.15-1.16.1" });
            infoList.Add(new BoxInfo() { Id = 6, Title = "1.16.2-1.16.4" });
            infoList.Add(new BoxInfo() { Id = 7, Title = "1.17+" });
            VerBox.SelectedValue = 1;
        }

        //皮肤下拉框选项
        public void SkinComboBox() {
            BindingList<BoxInfo> infoList = new BindingList<BoxInfo>();
            SkinSizeBox.ItemsSource = infoList;
            SkinSizeBox.DisplayMemberPath = "Title";
            SkinSizeBox.SelectedValuePath = "Id";
            infoList.Add(new BoxInfo() { Id = 1, Title = "Alex" });
            infoList.Add(new BoxInfo() { Id = 2, Title = "Steve" });
            SkinSizeBox.SelectedValue = 2;
        }

        private void VerBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            this.NumBox.Text = VerBox.SelectedValue.ToString();
        }

        private void SkinSizeBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            this.IDBox.Text = SkinSizeBox.SelectedValue.ToString();
        }

        //默认保存目录在桌面
        private void FolderBox_Initialized(object sender, EventArgs e) {
            FolderBox.Text = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        }

        //保存目录框过长自动拉到最前面
        private void FolderBox_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e) {
            FolderBox.ScrollToHome();
        }

        //保存目录选择框
        private void FolderButton_Click(object sender, RoutedEventArgs e) {
            CommonOpenFileDialog c_dialog = new CommonOpenFileDialog {
                Title = "请选择保存文件夹",
                IsFolderPicker = true//设置为选择文件夹
            };
            CommonFileDialogResult result = c_dialog.ShowDialog();
            if (result == CommonFileDialogResult.Cancel) {
                return;
            }
            string c_Dir = c_dialog.FileName;
            this.FolderBox.Text = c_Dir;
        }

        //皮肤选择框
        private void SkinFolderButton_Click(object sender, RoutedEventArgs e) {
            CommonOpenFileDialog c_dialog = new CommonOpenFileDialog {
                Title = "请选择png文件"
            };
            c_dialog.Filters.Add(new CommonFileDialogFilter("PNG/JPG Files", "*.png;*.jpg"));
            CommonFileDialogResult result = c_dialog.ShowDialog();
            if (result == CommonFileDialogResult.Cancel) {
                return;
            }
            string c_Dir = c_dialog.FileName;
            this.SkinFolderBox.Text = c_Dir;
        }


        private void SkinFolderBox_Initialized(object sender, EventArgs e) {
            SkinFolderBox.Text = "*.png/*.jpg";
        }

        //生成
        private void BuildButton_Click(object sender, RoutedEventArgs e) {
            const string message1 = "如有同名文件将会替换!";
            const string messageFinish = "完成";
            const string messageError = "皮肤路径出错!";
            string[] lines = {
                "{",
                "    " + "\"pack\":{,"
            };
            string[] lines2 = {
                 "        " + "\"description\":skin resource pack,",
                 "    " + "}",
                 "}"
            };

            //生成弹窗
            var result = MessageBox.Show(
                message1,
                "!",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );
            if (result == MessageBoxResult.Yes) {
                string packPath = this.FolderBox.Text + "\\SkinTexturePack";
                string skinPath = packPath + "\\aessts\\minecraft\\textures\\entity";

                //创建所需文件夹
                Directory.CreateDirectory(skinPath);

                //创建pack.mcmeta文件
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(packPath, "pack.mcmeta"))) {
                    foreach (string line in lines) {
                        outputFile.WriteLine(line);
                    }
                    outputFile.Write("        " + "\"pack_format\"" + ":");
                    outputFile.Write(this.NumBox.Text + ",");
                    outputFile.WriteLine();
                    foreach (string line in lines2) {
                        outputFile.WriteLine(line);
                    }
                }
                try {
                    //创建皮肤文件
                    FileInfo skIN = new FileInfo(this.SkinFolderBox.Text);
                    if (this.IDBox.Text == "1") {
                        string player = "Alex.png";
                        skIN.CopyTo(skinPath + "\\" + player);
                    } else {
                        string player = "Steve.png";
                        skIN.CopyTo(skinPath + "\\" + player);
                    }
                } catch (Exception) {
                    //如果皮肤文件所在目录错误则弹出窗口
                    var errorResult = MessageBox.Show(
                        messageError,
                        "!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                    if (errorResult == MessageBoxResult.OK) {
                        return;
                    } else {
                        return;
                    }
                }

            } else {
                return;
            }

            //完成弹窗
            var finishResult = MessageBox.Show(
                messageFinish,
                "!",
                MessageBoxButton.OK,
                MessageBoxImage.Question);
            if (finishResult == MessageBoxResult.OK) {
                return;
            } else {
                return;
            }
        }
    }
}