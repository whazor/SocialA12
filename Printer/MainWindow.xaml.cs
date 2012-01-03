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
using Dymo;

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
    bool start = true;

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
      var client = new WebClient();
      client.DownloadDataCompleted += new DownloadDataCompletedEventHandler(client_DownloadDataCompleted);
      client.DownloadDataAsync(new Uri("http://" + tbSite.Text + "/Enrollment/Json/" + lastId));
      timer.Stop();
    }


    void client_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
    {
      if (e.Error != null) { timer.Start(); return; };
      string data = Encoding.Default.GetString(e.Result);
      //list.Clear();
      var enrollments = JsonConvert.DeserializeObject<List<Enrollment>>(data);
      foreach (var item in enrollments)
      {
        list.Insert(0, item);
        lastId = item.EnrollmentID.ToString();
        if (!start)
        {
          print(item);
        }
      }
      lbLabels.Items.Refresh();
      timer.Start();
      start = false;
    }

    private void lbLabels_KeyUp(object sender, KeyEventArgs e)
    {
      if (lbLabels.SelectedItem == null) return;
      if (e.Key != Key.Enter) return;
      var enrollment = (Enrollment)lbLabels.SelectedItem;
      print(enrollment);
    }

    private void print(Enrollment enrollment)
    {
      var oldtitle = window.Title;
      window.Title = oldtitle + " (printing...)";
      var printjob = new DymoAddIn();
      var label = new DymoLabels();
      if (printjob.Open(@"Person.LWL"))
      {
        label.SetField("Name", enrollment.FullName);
        label.SetField("School", enrollment.School);
        printjob.StartPrintJob();
        printjob.Print(1, false);
        printjob.EndPrintJob();
        window.Title = oldtitle;
      }
    }

    private void tbSite_TextChanged(object sender, TextChangedEventArgs e)
    {

    }
  }
}
