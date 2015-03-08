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
	private common()
	{
	}
    public static string AdminName = "ADMIN";
    public static void DownloadXLS(string sql, string filename, Page pg)
    {
        System.Data.DataSet ds = OraDBConnection.GetData(sql);

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
    }
    public static void FillInfo(string userID, string empID, string location, string name, Label lblLoggedIn)
    {
        lblLoggedIn.Text = string.Format("Loc: {0} ({1}),  User: {2} ({3})", location, userID, name, empID);
    }   
}