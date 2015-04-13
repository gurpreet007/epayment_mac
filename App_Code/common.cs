using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI;
using System.Web.UI.WebControls;

public class common
{
    public const string strAdminName = "ADMIN";
    public const string strUserID = "userID";
    public const string strEmpID = "empID";
    public const string strLocation = "location";
    public const string strName = "name";
    public const string strSAP = "sap";
    public const string strNonSAP = "nonsap";
    public static bool DownloadXLS(string sql, string filename, Page pg)
    {
        System.Data.DataSet ds = OraDBConnection.GetData(sql);

        if (ds.Tables[0].Rows.Count == 0)
        {
            return false;
        }
        DataGrid dg = new DataGrid();
        dg.DataSource = ds;
        dg.DataBind();
        pg.Response.AddHeader("content-disposition", "attachment;filename=" + filename);
        pg.Response.Charset = "";
        pg.Response.ContentType = "application/vnd.xls";
        System.IO.StringWriter stringwrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlwrite = new System.Web.UI.HtmlTextWriter(stringwrite);
        //htmlwrite.WriteLine("TITLE");
        dg.RenderControl(htmlwrite);
        pg.Response.Write(stringwrite.ToString());
        pg.Response.End();
        dg.Dispose();
        return true;
    }
    public static void FillInfo(System.Web.SessionState.HttpSessionState mySession, Label lblLoggedIn)
    {
        lblLoggedIn.Text = string.Format("Loc: {0} ({1}),  User: {2} ({3})", 
            mySession[strLocation], mySession[strUserID], mySession[strName], mySession[strEmpID]);
    }
    public static bool isValidSession(System.Web.SessionState.HttpSessionState mySession)
    {
        if (mySession[strUserID] == null ||
           mySession[strEmpID] == null ||
           mySession[strUserID].ToString() == string.Empty ||
           mySession[strEmpID].ToString() == string.Empty ||
           mySession[strEmpID].ToString().Length != 6)
        {
            return false;
        }
        return true;
    }
}