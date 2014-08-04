using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Specialized;
using TogglData;

namespace TogglExport
{
    public partial class Form1 : Form
    {
        private static Uri url_me = new Uri(TogglCommonConnections.url_prefix_base, "me");
        private LoginResponse response;
        private BackgroundWorker authenticationWorker;
        private Timer authenticationTimer;

        public Form1()
        {
            InitializeComponent();
            response = new LoginResponse();
            fixInputEnabling();
            InitializeAuthenticationWorker();
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            authenticationTimer = new Timer();
            authenticationTimer.Tick += new EventHandler(authenticationTimer_Tick);
            authenticationTimer.Interval = 500;
        }

        private void InitializeAuthenticationWorker()
        {
            authenticationWorker = new BackgroundWorker();
            authenticationWorker.DoWork += new DoWorkEventHandler(authenticationWorker_DoWork);
            authenticationWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(authenticationWorker_RunWorkerCompleted);
            //authenticationWorker.ProgressChanged += new ProgressChangedEventHandler(authenticationWorker_ProgressChanged);

        }

        private void authenticationWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker; //If you need to send the 'worker' into the authenticate function, 
                                                                  //so you can update the value inside of the thread 
                                                                  //(to call the ProgressChanged event)
            e.Result = authenticateToken();
        }

        private void authenticationTimer_Tick(object sender, EventArgs e)
        {
            if (lbl_Progress.Text.Length >= "Authenticating...".Length)
                lbl_Progress.Text = "Authenticating";
            else
                lbl_Progress.Text += ".";
        }

        private void authenticationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btn_Connect.Enabled = true;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                MessageBox.Show("Authentication Interrupted");
            }
            else
            {
                var t = new System.Threading.Thread(() => SuccessProc(response.data));
                t.SetApartmentState(System.Threading.ApartmentState.STA);
                t.Start();
                this.Close();
            }
        }

        //private void authenticationWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    if(lbl_Progress.Text.Length < 15)
        //        lbl_Progress.Text += ".";
        //}


        private void fixInputEnabling()
        {
            if (Properties.Settings.Default.save_password)
            {
                txt_Email.Enabled = false;
                txt_Password.Enabled = false;
                chk_SaveData.Checked = true;
            }
            else
            {
                txt_Email.Enabled = true;
                txt_Password.Enabled = true;
            }
        }

        private void btn_Connect_Click(object sender, EventArgs e)
        {
            btn_Connect.Enabled = false;
            lbl_Progress.Text = "Authenticating";
            authenticationTimer.Start();
            authenticationWorker.RunWorkerAsync();
        }

        private LoginData authenticateToken()
        {
            NameValueCollection authHeader = new NameValueCollection();
            if (txt_Email.Text.Length > 0 && txt_Password.Text.Length > 0)
            {
                authHeader.Add(
                    HttpRequestHeader.Authorization.ToString(),
                    "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(txt_Email.Text + ':' + txt_Password.Text))
                );
            }
            else if (Properties.Settings.Default.save_password &&
                Properties.Settings.Default.api_token.Length > 0)
            {
                authHeader.Add(
                    HttpRequestHeader.Authorization.ToString(),
                    "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(Properties.Settings.Default.api_token +
                    ":api_token"))
                );
            }
            else
                return null;

            var authRequest = (HttpWebRequest)HttpWebRequest.Create(url_me);
            authRequest.Headers.Add(authHeader);

            //string content; //For debugging messages
            HttpWebResponse authResponse;
            try
            {
                authResponse = (HttpWebResponse)authRequest.GetResponse();
            }
            catch (System.Net.WebException netException)
            {

                if (netException.Message.Contains("403"))
                {
                    MessageBox.Show("Username or Password are incorrect,\r\n Please check credentials and try again.\r\n" + netException.Message);
                    return null;
                }
                else
                {
                    MessageBox.Show("Network Error" + netException.Message);
                    return null;
                }
            }

            using (var reader = new StreamReader(authResponse.GetResponseStream(), Encoding.UTF8))
            {
                JsonSerializer serializer = new JsonSerializer();

                //content = reader.ReadToEnd();
                try
                {
                    response = (LoginResponse)serializer.Deserialize(reader, typeof(LoginResponse));
                }
                catch (Exception jsonException)
                {
                    MessageBox.Show(jsonException.Message);
                    return null;
                }

            }

            if (response.data.api_token.Length > 0)
            {
                if (Properties.Settings.Default.save_password)
                    Properties.Settings.Default.api_token = response.data.api_token;
                //var t = new System.Threading.Thread(() => SuccessProc(response.data));
                //t.Start();
                //this.Close();
                return response.data;
            }
            else
            {
                MessageBox.Show("Unknown Error");
                return null;
            }
            


        }

        private static void SuccessProc(LoginData data)
        {
            Application.Run(new LoginSuccess(data));
        }

        private void chk_SaveData_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_SaveData.Checked)
                Properties.Settings.Default.save_password = true;
            else
            {
                Properties.Settings.Default.save_password = false;
                Properties.Settings.Default.api_token = "";
                txt_Password.Enabled = true;
                txt_Email.Enabled = true;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

    }
}
