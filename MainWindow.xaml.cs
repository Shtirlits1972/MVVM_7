using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.DependencyInjection;
using LoggerClass;

namespace MVVM_7
{
    public partial class MainWindow : Window
    {
        ApplicationContext db = new ApplicationContext();
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        static IServiceCollection services = new ServiceCollection().AddTransient<ILogger, WriteToFile>();
        static ServiceProvider serviceProvider = services.BuildServiceProvider();
        ILogger logService = serviceProvider.GetService<ILogger>();
        public MainWindow()
        {
            InitializeComponent();

            ContextMenu context = new ContextMenu();

            MenuItem cvs = new MenuItem();
            cvs.Header = "CVS";
            cvs.Click += Cvs_Click;
            context.Items.Add(cvs);

            MenuItem txt = new MenuItem();
            txt.Header = "TXT";
            context.Items.Add(txt);
            txt.Click += Txt_Click;

            mainGrid.ContextMenu = context;

            DataContext = new ApplicationViewModel();
        }

        private void Txt_Click(object sender, RoutedEventArgs e)
        {
            if (mainGrid.SelectedItems.Count > 0)
            {
                try
                {
                    List<string> listString = new List<string>();
                   //   string head = "id;NamePerson;Phone";
                 //   listString.Add(head);

                    for (int i = 0; i < mainGrid.SelectedItems.Count; i++)
                    {
                        Person p = (Person)mainGrid.SelectedItems[i];
                        listString.Add(p.ToCVS());
                    }

                    File.AppendAllLines(path + "/data.txt", listString.ToArray());

                    logService.WriteLog("Write to file txt succesfully");
                }
                catch (Exception ex)
                {
                    logService.WriteLog(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("You have to select at least one line!");
            }
        }

        private void Cvs_Click(object sender, RoutedEventArgs e)
        {
            if (mainGrid.SelectedItems.Count > 0)
            {
                try
                {
                    List<string> listString = new List<string>();

                    string head = "id;NamePerson;Phone";
                    listString.Add(head);

                    for (int i = 0; i < mainGrid.SelectedItems.Count; i++)
                    {
                        Person p = (Person)mainGrid.SelectedItems[i];
                        listString.Add(p.ToCVS());
                    }

                    File.AppendAllLines(path + "/data.csv", listString.ToArray());
                    logService.WriteLog("Write to file csv succesfully");
                }
                catch (Exception ex)
                {
                    logService.WriteLog(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("You have to select at least one line!");
            }
        }
    }
}
