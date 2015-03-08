using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class download : System.Web.UI.Page
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

            if (Session["userID"].ToString() != common.AdminName)
            {
                btnEmpty.Visible = false;
            }
        }
    }
    [System.Web.Services.WebMethod]
    public static string GetSomething(string drpVal)
    {
        string sql = "select count(*) from onlinebill."+drpVal;
        return OraDBConnection.GetScalar(sql);
    }

    [System.Web.Services.WebMethod]
    public static string EmptyTables()
    {
        OraDBConnection.ExecProc("onlinebill.deleteall");
        return "Tables Emptied";
    }
}