using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace TogglData
{

    public class TogglReportData
    {
        public int total_grand;
        public int? total_billable;
        public int total_count;
        public int per_page;
        //public Map<string, double>total_currencies
        public List<TogglReportDetails> data;
    }

    public class TogglReportDetails
    {
        public int id;
        public int? pid;
        public string tid;
        public int? uid;
        public string description;
        public string start;
        public string end;
        public string updated;
        public int dur;
        public string user;
        public bool use_stop;
        public string client;
        public string project;
        public string task;
        public double? billable;
        public bool is_billable;
        public string cur;
        public List<string> tags;
    }

    public class TogglCommonConnections
    {
        public static Uri url_prefix_base = new Uri(@"https://www.toggl.com/api/v8/");
        public static Uri url_prefix_report = new Uri(@"https://www.toggl.com/reports/api/v2/");
    }

    public class TogglProject
    {
        public int id;
        public int wid;
        public string name;
        public bool billable;
        public bool active;
        public string at;
    }

    public class TogglBlogPost
    {
        public string title;
        public string url;
        public string category;
        public string pub_date;
    }

    public class LoginData
    {
        public int id;
        public string api_token;
        public int default_wid;
        public string email;
        public string fullname;
        public string jquery_timeofday_format;
        public string jquery_date_format;
        public string timeofday_format;
        public string date_format;
        public bool store_start_and_stop_time;
        public int beginning_of_week;
        public string language;
        public string image_url;
        public TogglBlogPost new_blog_post;
        //public TogglProject[] projects;
        public bool should_upgrade;
        public bool show_offer;
        public bool share_experiment;
        public bool achievements_enabled;
        public string timezone;
        public bool openid_enabled;
        public bool send_product_emails;
        public bool send_weekly_report;
        public bool send_time_notifications;
        //public invitation
        public List<TogglWorkspace> workspaces;
    }

    public class TogglWorkspace
    {
        public int id;
        public string name;
        public string at;
    }

    public class LoginResponse
    {
        public int since;
        public LoginData data;
    }
}
