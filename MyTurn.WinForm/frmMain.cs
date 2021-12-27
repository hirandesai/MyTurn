using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Configuration;
namespace MyTurn.WinForm
{
    public partial class frmMain : Form
    {
        static string RootUrl = ConfigurationManager.AppSettings["RootUrl"];
        static string APIUrl = RootUrl + "api/";
        private IHubProxy HubProxy { get; set; }
        private HubConnection Connection { get; set; }

        public frmMain()
        {
            InitializeComponent();
            Connection = new HubConnection(RootUrl + "signalr");
            HubProxy = Connection.CreateHubProxy("MyTurnHub");
            HubProxy.On<int, bool>("triggerToilet", (toiletId, IsInUse) =>
                this.Invoke((Action)(() =>
                    {
                        var toilet = this.Controls.Find(string.Format("btnToilet{0}", toiletId), true);
                        if (toilet.Length > 0)
                        {
                            toilet.First().BackColor = IsInUse ? System.Drawing.Color.Brown : System.Drawing.Color.OliveDrab;
                        }
                    }
                ))
            );
            try
            {
                Connection.Start();
            }
            catch (Exception)
            {

            }
            GenerateControlers();
        }

        private void pcBox_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void pcMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void GenerateControlers()
        {
            Task<List<Toilet>> toilets = GetToilets();
            if (toilets.Result != null)
            {
                foreach (var group in (from toilet in toilets.Result
                                       group toilet by toilet.Group.Name into g
                                       orderby g.Key
                                       select g))
                {
                    var grpBoxToiletContainer = new GroupBox();
                    grpBoxToiletContainer.SuspendLayout();
                    //grpBoxToiletContainer.Controls.Add(this.btnToilet21);
                    grpBoxToiletContainer.Name = string.Format("grpBox{0}", group.Key.Replace(" ", ""));
                    grpBoxToiletContainer.Dock = DockStyle.Top;
                    grpBoxToiletContainer.Width = pnlToiletContainer.Width - 7;
                    grpBoxToiletContainer.Height = 60;
                    grpBoxToiletContainer.TabStop = true;
                    grpBoxToiletContainer.Text = group.Key;
                    grpBoxToiletContainer.ResumeLayout(false);
                    //grpBoxToiletContainer.AutoSize = true;


                    var flPanelToiletControlContainer = new FlowLayoutPanel();
                    flPanelToiletControlContainer.Name = string.Format("flPanelToiletControlContainer{0}", group.Key.Replace(" ", ""));
                    flPanelToiletControlContainer.Dock = DockStyle.Fill;
                    flPanelToiletControlContainer.Padding = new Padding(5);


                    foreach (var toilet in group)
                    {
                        var btnToilet = new System.Windows.Forms.Label();
                        btnToilet.BackColor = toilet.IsInUse ? System.Drawing.Color.Brown : System.Drawing.Color.OliveDrab;
                        btnToilet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        btnToilet.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
                        //btnToilet.Location = new System.Drawing.Point(71, 24);
                        btnToilet.Name = string.Format("btnToilet{0}", toilet.Id);
                        btnToilet.Padding = new Padding(10, 3, 10, 3);
                        btnToilet.TabStop = true;
                        btnToilet.Text = toilet.Identifier;
                        btnToilet.AutoSize = true;                        
                        flPanelToiletControlContainer.Controls.Add(btnToilet);
                    }
                    grpBoxToiletContainer.Controls.Add(flPanelToiletControlContainer);
                    pnlToiletContainer.Controls.Add(grpBoxToiletContainer);

                }
            }
        }

        private async Task<List<Toilet>> GetToilets()
        {
            try
            {
                WebRequest request = WebRequest.Create(APIUrl + "toilets");
                using (WebResponse response = request.GetResponse())
                using (Stream dataStream = response.GetResponseStream())
                using (StreamReader streamReader = new StreamReader(dataStream))
                {
                    string contents = streamReader.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<Toilet>>(contents);
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

    }

    public partial class Toilet
    {
        public int Id { get; set; }
        public string Identifier { get; set; }
        public int GroupId { get; set; }
        public bool IsInUse { get; set; }
        public bool IsActive { get; set; }
        public virtual Group Group { get; set; }
    }
    public partial class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
