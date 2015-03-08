using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class reports : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userID"] == null ||
           Session["empID"] == null ||
           Session["userID"].ToString() == string.Empty ||
           Session["empID"].ToString() == string.Empty ||
           Session["empID"].ToString().Length != 6)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {
            common.FillInfo(Session["userID"].ToString(), Session["empID"].ToString(),
                Session["location"].ToString(), Session["name"].ToString(), lblLoggedInAs);
        }
    }
    protected void lnkUserActivity_Click(object sender, EventArgs e)
    {
        string sql = string.Format("select userid, categ as category, empid, " +
                "insrec as Rec_Inserted, duprec as Rec_Duplicate, " +
                "to_char(dated,'DD-MON-YYYY HH24:MI:SS') as dated from onlinebill.userrec " +
                "{0} order by dated desc", 
                (Session["userID"].ToString() != common.AdminName) ? 
                    string.Format(" where userid = '{0}'", Session["userID"]) : 
                    "");
        common.DownloadXLS(sql, "activity.xls", this);
    }
}