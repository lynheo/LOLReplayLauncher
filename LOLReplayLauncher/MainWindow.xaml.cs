using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LOLReplayLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            dlgExe.FileName = @"League of Legends.exe";
            dlgExe.Filter = "LOL실행파일 (*.exe)|League of Legends.exe";
            dlgReplay.Filter = "LOL리플레이 (*.rofl)|*.rofl";

            dlgExe.InitialDirectory = @"C:\Riot Games\League of Legends\Game";
            dlgReplay.InitialDirectory =
                System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                @"League of Legends\Replays");
        }

        Microsoft.Win32.OpenFileDialog dlgExe = new Microsoft.Win32.OpenFileDialog();
        Microsoft.Win32.OpenFileDialog dlgReplay = new Microsoft.Win32.OpenFileDialog();
        private void BtnExe_Click(object sender, RoutedEventArgs e)
        {
            var result = dlgExe.ShowDialog();

            if (result == true)
            {
                txtExe.Text = dlgExe.FileName;
            }
        }

        private void BtnReplay_Click(object sender, RoutedEventArgs e)
        {
            var result = dlgReplay.ShowDialog();

            if (result == true)
            {
                txtReplay.Text = dlgReplay.FileName;
            }
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if ((string.IsNullOrWhiteSpace(txtExe.Text) == true) || (string.IsNullOrWhiteSpace(txtReplay.Text) == true))
            {
                MessageBox.Show("실행파일과 리플레이 선택 필수");
                return;
            }

            var exeText = txtExe.Text;
            var exePath = System.IO.Path.GetDirectoryName(txtExe.Text);
            var runningPath = System.IO.Directory.GetParent(exePath).FullName;
            var startArg = new StringBuilder();
            //startArg.Append("\"").Append(txtExe.Text).Append("\"").Append(' ');

            startArg.Append("\"").Append(txtReplay.Text).Append("\"").Append(' ');

            startArg.Append("\"")
                .Append("-GameBaseDir=")
                .Append(runningPath)
                .Append("\"")
                .Append(' ');

            startArg.Append("\"").Append("-SkipRads").Append("\"").Append(' ');
            startArg.Append("\"").Append("-SkipBuild").Append("\"").Append(' ');
            startArg.Append("\"").Append("-EnableLNP").Append("\"").Append(' ');
            startArg.Append("\"").Append("-UseNewX3D=1").Append("\"").Append(' ');
            startArg.Append("\"").Append("-UseNewX3DFramebuffers=1").Append("\"");

            Task.Run(() =>
            {
                var process = new Process();
                process.StartInfo = new ProcessStartInfo()
                {
                    UseShellExecute = false,
                    Arguments = startArg.ToString(),
                    FileName = exeText,
                    WorkingDirectory = exePath,
                };

                process.Start();
                process.WaitForExit();
            });
        }
    }
}
