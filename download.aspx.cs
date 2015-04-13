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
        if (!common.isValidSession(Page.Session))
        {
            Response.Redirect("login.aspx");
        }

        if (!IsPostBack)
        {
            common.FillInfo(Page.Session, lblLoggedInAs);

            if (Session[common.strUserID].ToString() != common.strAdminName)
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