using System;
using System.Collections.Generic;
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
using System.Net;

using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace Printer
{

  // TODO: Move
  public class Enrollment
  {
    public int EnrollmentID { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string FullName
    {
      get {
        var middlename = MiddleName;
        if (middlename != null) middlename += ' ';
        return(FirstName + ' ' + middlename + LastName);
      }
    }

    public string Email { get; set; }

    public string School { get; set; }

    // Too big int
    public string FacebookID { get; set; }

  }


  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public Collection<Enrollment> list = new Collection<Enrollment>();
    string lastId = "0";
    DispatcherTimer timer = new DispatcherTimer();

    public MainWindow()
    {
      InitializeComponent();
      //lbLabels.DataContext = list;
      lbLabels.ItemsSource = list;

      timer.Interval = TimeSpan.FromSeconds(1);
      timer.Tick += new EventHandler(timer_Tick);
      timer.Start();
    }

    void timer_Tick(object sender, EventArgs e)
    {
      refresh();
    }
    private void btnRefresh_Click(object sender, RoutedEventArgs e)
    {
      refresh();
    }

    void refresh()
    {
      var client = new WebClient();
      client.DownloadDataCompleted += new DownloadDataCompletedEventHandler(client_DownloadDataCompleted);
      client.DownloadDataAsync(new Uri("http://" + tbSite.Text + "/Enrollment/Json/" + lastId));
      btnRefresh.IsEnabled = false;
      timer.Stop();
    }


    void client_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
    {
      string data = Encoding.Default.GetString(e.Result);
      //list.Clear();
      var enrollments = JsonConvert.DeserializeObject<List<Enrollment>>(data);
      foreach (var item in enrollments)
      {
        list.Add(item);
        lastId = item.EnrollmentID.ToString();
      }
      lbLabels.Items.Refresh();
      btnRefresh.IsEnabled = true;
      timer.Start();
    }

    private void btnPrint_Click(object sender, RoutedEventArgs e)
    {

    }



  }
}
