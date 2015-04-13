using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class reports : System.Web.UI.Page
{
    private void fillUsers(DropDownList drpUsers, bool fillOnlyActive = true, bool addAdmin = false)
    {
        DataSet ds;
        bool isAdmin = Session[common.strUserID].ToString().Equals(common.strAdminName);
        string sql = string.Empty;
        string strAddAdmin = addAdmin ? string.Empty : string.Format("and upper(userid) <> '{0}'", common.strAdminName);
        string strFillOnlyActive = fillOnlyActive ? "and active=1" : string.Empty;
        string strUser = isAdmin ? string.Empty : string.Format("and upper(userid) = '{0}'", Session[common.strUserID].ToString());
        string strOnlyExisting = "and userid in (select distinct userid from onlinebill.userrec)";

        sql = string.Format("select userid||' (' || offcname || ')' as usern," +
                " userid from onlinebill.users where 1=1 {0} {1} {2} {3} order by userid", 
                strAddAdmin, strFillOnlyActive, strUser, strOnlyExisting);
        ds = OraDBConnection.GetData(sql);

        drpUsers.DataSource = ds;
        drpUsers.DataTextField = "usern";
        drpUsers.DataValueField = "userid";
        drpUsers.DataBind();

        if (isAdmin)
        {
            drpUsers.Items.Insert(0, new ListItem("All Users", "ALL"));
        }
        ds.Clear();
        ds.Dispose();
    }
    private bool getDates(out string startDate, out string endDate)
    {
        string duration = panActivity_drpDuration.SelectedValue;
        string dtFormat = string.Empty;
        startDate = DateTime.Now.ToString("dd-MMM-yyyy");
        endDate = DateTime.Now.ToString("dd-MMM-yyyy");
        try
        {
            switch (duration)
            {
                case "dates":
                    startDate = sDate.Value;
                    endDate = eDate.Value;
                    dtFormat = startDate.Contains("-") ? "yyyy-MM-dd" : "dd/MM/yyyy";
                    startDate = DateTime.ParseExact(startDate, dtFormat, null).ToString("dd-MMM-yyyy");
                    endDate = DateTime.ParseExact(endDate, dtFormat, null).ToString("dd-MMM-yyyy");
                    break;
                case "day1":
                    startDate = endDate;
                    break;
                case "day2":
                    startDate = DateTime.ParseExact(endDate, "dd-MMM-yyyy", null).AddDays(-1).ToString("dd-MMM-yyyy");
                    break;
                case "day3":
                    startDate = DateTime.ParseExact(endDate, "dd-MMM-yyyy", null).AddDays(-2).ToString("dd-MMM-yyyy");
                    break;
                case "week":
                    startDate = DateTime.ParseExact(endDate, "dd-MMM-yyyy", null).AddDays(-7).ToString("dd-MMM-yyyy");
                    break;
                case "month":
                    startDate = DateTime.ParseExact(endDate, "dd-MMM-yyyy", null).AddMonths(-1).ToString("dd-MMM-yyyy");
                    break;
                case "year":
                    startDate = DateTime.ParseExact(endDate, "dd-MMM-yyyy", null).AddYears(-1).ToString("dd-MMM-yyyy");
                    break;
            }
        }
        catch(System.FormatException)
        {
            panActivity_lblMsg.Text = "Invalid Date. Use format dd/mm/yyyy.";
            return false;
        }
        return true;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!common.isValidSession(Page.Session))
        {
            Response.Redirect("login.aspx");
        }

        if (!IsPostBack)
        {
            common.FillInfo(Page.Session, lblLoggedInAs);
            fillUsers(panActivity_drpUsers, false, true);
            sDate.Attributes["type"] = "date";
            eDate.Attributes["type"] = "date";
        }
    }
    protected void btnUserActivity_Click(object sender, EventArgs e)
    {
        string userID = panActivity_drpUsers.SelectedValue;
        string billType = panActivity_drpBillType.SelectedValue;
        string sql = string.Empty;
        string sqlUser = string.Empty;
        string sqlBillType = string.Empty;
        string sDate = string.Empty;
        string eDate = string.Empty;
        bool xlReturn = false;
        bool dtReturn = false;

        dtReturn = getDates(out sDate, out eDate);
        if(dtReturn ==  false)
        {
            //date error, msg already shown, return
            return;
        }
        if(userID != "ALL") {
            sqlUser = string.Format("and userid = '{0}'",userID);
        }
        if(billType != "ALL") {
            sqlBillType = string.Format("and categ = '{0}'",billType);
        }
        //sql = string.Format("select userid, categ as category, empid, " +
        //         "insrec as Rec_Inserted, du5prec as Rec_Duplicate, " +
        //         "to_char(dated,'DD-MON-YYYY HH24:MI:SS') as dated from onlinebill.userrec " +
        //         "where 1=1 {0} {1} and dated between '{2}' and '{3}' order by dated desc", 
        //         sqlUser, sqlBillType, sDate, eDate);
        sql = string.Format("select userid, categ as category, " +
                 "insrec as Rec_Inserted, duprec as Rec_Duplicate, " +
                 "to_char(dated,'DD-MON-YYYY HH24:MI:SS') as dated from onlinebill.userrec " +
                 "where 1=1 {0} {1} and trunc(dated) between '{2}' and '{3}' order by dated desc",
                 sqlUser, sqlBillType, sDate, eDate);
        xlReturn = common.DownloadXLS(sql, "activity.xls", this);

        
        if(xlReturn == false)
        {
            panActivity_lblMsg.Text = "No Record Exist";
        }
    }
}