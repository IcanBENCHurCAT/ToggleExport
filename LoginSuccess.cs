using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using TogglData;
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;

namespace TogglExport
{
    public partial class LoginSuccess : Form
    {
        private static Uri url_detailed = new Uri(TogglCommonConnections.url_prefix_report, "details?");
        private BackgroundWorker reportWorker;
        private Timer reportTimer;
        private LoginData loginData;
        private int pageNo;
        private int maxPages;
        private bool morePages;
        private int activeWorkspace;
        private List<TogglReportData> finishedData;
        //private Excel.Application excelApp;
        //private Excel.Workbooks excelWorkBooks;
        //private Excel.Workbook excelWorkBook;

        public LoginSuccess(LoginData login_Data)
        {
            InitializeComponent();
            InitializeReportWorker();
            InitializeTimer();
            lbl_Loading.Text = "";
            loginData = login_Data;
            dt_End.Format = DateTimePickerFormat.Custom;
            dt_Begin.Format = DateTimePickerFormat.Custom;

            dt_End.CustomFormat = "yyyy-MM-dd";
            dt_Begin.CustomFormat = "yyyy-MM-dd";
            pageNo = 1;
            morePages = false;
            activeWorkspace = loginData.workspaces[0].id;
            btn_Save.Enabled = false;
        }

        private void btn_Generate_Click(object sender, EventArgs e)
        {
            if (!reportTimer.Enabled)
            {
                btn_Save.Enabled = false;
                lbl_Loading.Text = "Loading";
                reportTimer.Start();
                reportWorker.RunWorkerAsync();
            }
        }

        private void InitializeReportWorker()
        {
            reportWorker = new BackgroundWorker();
            reportWorker.DoWork += new DoWorkEventHandler(reportWorker_DoWork);
            reportWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(reportWorker_RunWorkerCompleted);
        }

        private void reportWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<TogglReportData> reportList = new List<TogglReportData>();
            do
            {
                var report = RequestReport();
                if (report != null)
                    reportList.Add(report);
                pageNo++;
            }
            while (morePages);

            if (reportList.Count > 0)
                e.Result = reportList;
            else
                throw new InvalidOperationException("Zero Reports were given.");
        }

        private void reportWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lbl_Loading.Text = "";
            reportTimer.Stop();
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                MessageBox.Show("Report Generation Interrupted");
            }
            else
            {
                finishedData = e.Result as List<TogglReportData>;
                btn_Save.Enabled = true;
                lbl_Loading.Text = "Report Loaded";
            }
        }

        private void InitializeTimer()
        {
            reportTimer = new Timer();
            reportTimer.Tick += new EventHandler(reportTimer_Tick);
            reportTimer.Interval = 500;
        }

        private void reportTimer_Tick(Object sender, EventArgs e)
        {
            if (lbl_Loading.Text.Length > "Loading...".Length)
                lbl_Loading.Text = "Loading";
            else
                lbl_Loading.Text += ".";
        }

        private TogglReportData RequestReport()
        {
            TogglReportData report = new TogglReportData();

            NameValueCollection reportHeader = new NameValueCollection();
            reportHeader.Add(
                    HttpRequestHeader.Authorization.ToString(),
                    "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(loginData.api_token +
                    ":api_token"))
                );

            Uri url_full_request = new Uri(url_detailed + "user_agent=ToggleExport" + "&workspace_id=" + activeWorkspace + "&since=" + dt_Begin.Text + "&until=" + dt_End.Text + "&page=" + pageNo);
            var reportRequest = (HttpWebRequest)HttpWebRequest.Create(url_full_request);
            reportRequest.Headers.Add(reportHeader);

            HttpWebResponse reportResponse;
            try
            {
                reportResponse = (HttpWebResponse)reportRequest.GetResponse();
            }
            catch (System.Net.WebException netException)
            {
                MessageBox.Show(netException.Message);
                return null;
            }

            using (var reader = new StreamReader(reportResponse.GetResponseStream(), Encoding.UTF8))
            {
                JsonSerializer serializer = new JsonSerializer();

                //string content = reader.ReadToEnd();
                try
                {
                    report = (TogglReportData)serializer.Deserialize(reader, typeof(TogglReportData));
                }
                catch (Exception jsonException)
                {
                    MessageBox.Show(jsonException.Message);
                    return null;
                }

            }

            if (pageNo == 1)
            {
                maxPages = 0;
                int total = report.total_count;
                int page = report.per_page;
                do
                {
                    maxPages++;
                    total -= page;
                } while (total > 0);

                //= report.total_count / report.per_page;
            }

            if (maxPages > pageNo)
                morePages = true;
            else
                morePages = false;

            return report;
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            int selection = 0;
            OutputSelection form = new OutputSelection();
            var res = form.ShowDialog();
            if (res == DialogResult.OK)
            {
                selection = form.selection;
            }
            else
                return;
            var outputMap = new Dictionary<string, Tuple<double, string, string, string>>();
            foreach (var page in finishedData)
            {
                foreach (var entry in page.data)
                {
                    DateTime d = new DateTime();
                    

                    //DateTimeConverter d = new DateTimeConverter();
                    //d.ConvertFromString(entry.start);
                    string dBegin = "";
                    string dEnd = "";//d.ConvertTo(d, typeof string);
                    DateTime.TryParse(entry.start, out d);
                    dBegin = d.ToString("MM/dd/yyyy HH:mm:ss");
                    string key = d.ToShortDateString() + entry.description;
                    DateTime.TryParse(entry.end, out d);
                    dEnd = d.ToString("MM/dd/yyyy HH:mm:ss");
                    
                    //excelWorksheet.Cells[activerow, 2] = dConverted;
                    TimeSpan t = new TimeSpan(0, 0, entry.dur / 1000);
                    double hours = Math.Round(t.TotalHours);
                    //excelWorksheet.Cells[activerow, 3] = hours;
                    
                    if (outputMap.ContainsKey(key))
                    {
                        hours += outputMap[key].Item1;
                    }

                    outputMap[key] = new Tuple<double, string, string, string>(hours, dBegin, dEnd, entry.description);
                }
            }

            if (selection == 0)
            {
                //Description	Notes	Begin Date	End Date	Hours:Minutes
                var lines = new List<string>();
                foreach (var tuple in outputMap)
                {
                    StringBuilder line = new StringBuilder();
                    line.Append(tuple.Value.Item4);
                    line.Append("\t");
                    line.Append("Generated by TogglExport");
                    line.Append("\t");
                    line.Append(tuple.Value.Item2);
                    line.Append("\t");
                    line.Append(tuple.Value.Item3);
                    line.Append("\t");
                    line.Append(tuple.Value.Item1);
                    line.Append(":");
                    line.Append("00");
                    lines.Add(line.ToString());
                }
                SaveFileDialog dia = new SaveFileDialog();
                dia.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                dia.FileName = "timeTracker.txt";
                dia.ShowDialog();
                if (dia.FileName.Length > 0)
                {
                    System.IO.File.WriteAllLines(dia.FileName, lines);
                }
            }
            else if (selection == 1)
            {
                var excelApp = new Excel.Application();
                try
                {                    
                    Excel.Workbook excelWorkBook = excelApp.Workbooks.Add(1);
                    Excel.Worksheet excelWorksheet = excelWorkBook.Worksheets[1];
                    List<string> header = new List<string> { "Description", "Date", "Time" };
                    int activerow = 1;
                    for (int i = 1; i <= header.Count; i++)
                    {
                        excelWorksheet.Cells[activerow, i] = header[i - 1];
                    }
                    activerow++;

                    foreach (var tuple in outputMap)
                    {
                        excelWorksheet.Cells[activerow, 1] = tuple.Value.Item4;
                        excelWorksheet.Cells[activerow, 2] = tuple.Value.Item2;
                        excelWorksheet.Cells[activerow, 3] = tuple.Value.Item1;
                        activerow++;
                    }
                    SaveFileDialog dia = new SaveFileDialog();
                    dia.Filter = "Excel Files (*.xlsx)|*.xlsx";
                    dia.FilterIndex = 1;
                    dia.FileName = "toggl.xlsx";
                    dia.ShowDialog();
                    if (dia.FileName.Length > 0)
                        excelWorkBook.SaveAs(dia.FileName);
                }
                catch (Exception unknownException)
                {
                    MessageBox.Show(unknownException.Message);
                }

                excelApp.Workbooks.Close();
                excelApp.Quit();
            }
        }

    }
}
