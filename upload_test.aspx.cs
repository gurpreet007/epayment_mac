using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class upload : System.Web.UI.Page
{
    const string dtFmtDotNet = "dd-MMM-yyyy HH:mm:ss";
    const string dtFmtOracle = "DD-MON-YYYY HH24:MI:SS";
    struct Categs
    {
        public string categName;
        public int posAccountNo;
        public int posBillCycle;
        public int posBillYear;
        public string tableName;
        public char delimiter;
        public int numFields;

        public Categs(string categName, int posAccountNo, int posBillCycle,
            int posBillYear, string tableName, char delimiter, int numFields)
        {
            this.categName = categName;
            this.posAccountNo = posAccountNo;
            this.posBillCycle = posBillCycle;
            this.posBillYear = posBillYear;
            this.tableName = tableName;
            this.delimiter = delimiter;
            this.numFields = numFields;
        }
    };

    void Upload(Categs categ)
    {
        string path = string.Empty;
        string[] lines;
        int oracle_ins = 0;
        int oracle_dup = 0;
        int oracle_err = 0;
        int start = 0;
        string userID = Session["userID"].ToString();
        string empID = Session["empID"].ToString();
        string dtUpload = DateTime.Now.ToString(dtFmtDotNet);
        string strExtension = string.Empty;

        StringBuilder sbsql = new StringBuilder(2000);

        //capture sessionid
        hidSID.Value = System.Guid.NewGuid().ToString();

        if (FileUpload1.HasFile)
        {
            strExtension = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName).ToLower();
            if (strExtension == ".txt" || strExtension == ".web")
            {
                path = Server.MapPath(string.Format("./bills/BILL_{0}.txt", hidSID.Value));
                FileUpload1.SaveAs(path);
            }
            else
            {
                lblMessage.Text = "Invalid File";
                return;
            }
        }

        //open file
        try
        {
            lines = System.IO.File.ReadAllLines(path);
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error Reading File: " + ex.Message;
            return;
        }

        //delete uploaded file
        System.IO.File.Delete(path);

        //if "AccountNo" word at line 0 then start from line 1 else start from line 0
        start = (lines[0].Split(categ.delimiter)[0] == "AccountNo") ? 1 : 0;

        for (int line = start; line < lines.Length; line++)
        {
            //sanitize line
            lines[line] = lines[line].Replace("'", "").Replace("--", "");

            //get fields
            string[] fields = lines[line].Split(categ.delimiter);

            //check the field length (for first record in the file)
            if (fields.Length != categ.numFields && line == start)
            {
                lblMessage.Text = "Error. Invalid File.";
                return;
            }

            //set/reset sbsql
            sbsql.Clear();

            sbsql.Append(string.Format("INSERT INTO {0} VALUES (", categ.tableName));

            //make the insert query
            for (int field = 0; field < fields.Length; field++)
            {
                sbsql.Append("'")
                    .Append(fields[field])
                    .Append("', ");
            }

            //add userID, empID and date_upload at last, we already have comma and space at the end of string
            sbsql.Append(string.Format("'{0}', '{1}', to_date('{2}','{3}'))", userID, empID, dtUpload, dtFmtOracle));

            //insert into oracle 
            //Composite Primary Key: ACCOUNTNO, BILLCYCLE, BILLYEAR
            //to handle dups we let the exception come and then continue
            //in case of error, we record the info about record containing error and continue
            
            try
            {
                OraDBConnection.ExecQry(sbsql.ToString());
                oracle_ins++;
            }
            catch (Exception ex)
            {
                sbsql.Clear();
                if (ex.Message.Contains("ORA-00001:"))
                {
                    oracle_dup++;
                    sbsql.Append(string.Format("INSERT INTO ONLINEBILL.DUPBILL"+
                        "(LINENO, ACCOUNTNO, BILLCYCLE, BILLYEAR, SESSIONID, DATED, TYPE) "+
                        "VALUES({0},'{1}','{2}','{3}','{4}',to_date('{5}','{6}'),'{7}')",
                        line + 1, fields[categ.posAccountNo], fields[categ.posBillCycle], 
                        fields[categ.posBillYear], hidSID.Value, dtUpload, dtFmtOracle, "D"));
                }
                else{
                    oracle_err++;
                    sbsql.Append(string.Format("INSERT INTO ONLINEBILL.DUPBILL"+
                        "(LINENO, ACCOUNTNO, BILLCYCLE, BILLYEAR, SESSIONID, DATED, TYPE) "+
                        "VALUES({0},'{1}','{2}','{3}','{4}',to_date('{5}','{6}'),'{7}')",
                        line + 1, "&lt;ERR&gt;", "&lt;ERR&gt;", "&lt;ERR&gt;", hidSID.Value, dtUpload, dtFmtOracle, "E"));
                    //sbsql.Append(string.Format("INSERT INTO ONLINEBILL.DUPBILL(LINENO, SESSIONID, DATED, TYPE) "+
                    //        "VALUES({0},'{1}',to_date('{2}','{3}'),'{4}')", line + 1, hidSID.Value, dtUpload, dtFmtOracle,"E"));
                }
                OraDBConnection.ExecQry(sbsql.ToString());
            }
        }

        //enable Export Button if dup rows or error rows
        btnExport.Visible = (oracle_dup > 0 || oracle_err > 0);

        //show success message
        lblMessage.Text = String.Format("Done. Total Rows {0}. Inserted {1}. Duplicate {2}. Error {3}", 
            lines.Length, oracle_ins, oracle_dup, oracle_err);

        //insert summary in userrec
        InsertUserRec(oracle_ins, oracle_dup, oracle_err, dtUpload, categ.categName);
    }
    void InsertUserRec(int numInsRec, int numDupRec, int numErrRec, string dated, string categName)
    {
        string userID = Session["userID"].ToString();
        string empID = Session["empID"].ToString();
        string sql = string.Empty;

        sql = string.Format("insert into onlinebill.userrec (userid, empid, insrec, duprec, errrec, dated, categ) " +
                "values('{0}', '{1}', '{2}', '{3}', '{4}',to_date('{5}', '{6}'), '{7}')",
                userID, empID, numInsRec, numDupRec, numErrRec, dated, dtFmtOracle, categName);
        try
        {
            OraDBConnection.ExecQry(sql);
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error in saving user record: " + ex.Message;
            return;
        }
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
        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        switch (drpBillType.SelectedValue)
        {
            case "LS":
                Upload(new Categs("LS", 0, 1, 2, "ONLINEBILL.LS", '"', 111));
                break;
            case "SP":
                Upload(new Categs("SP", 0, 18, 68, "ONLINEBILL.SP", '"', 69));
                break;
            case "MS":
                Upload(new Categs("MS", 0, 28, 81, "ONLINEBILL.MS", '"', 92));
                break;
            case "DSBELOW10KW":
                Upload(new Categs("DSBELOW10KW", 0, 18, 55, "ONLINEBILL.DSBELOW10KW", '"', 72));
                break;
            case "DSABOVE10KW":
                Upload(new Categs("DSABOVE10KW", 0, 18, 55, "ONLINEBILL.DSABOVE10KW", '"', 76));
                break;
            default:
                lblMessage.Text = "Select a valid Bill Type";
                return;
        }
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        string strFileName;
        string sql = string.Format("select LINENO, ACCOUNTNO, BILLCYCLE, BILLYEAR, DATED, "+
            "decode(TYPE,'E','ERROR','DUPLICATE') as PROBLEM from " +
            "ONLINEBILL.dupbill where SESSIONID = '{0}' ORDER BY LINENO", hidSID.Value);
        strFileName = "not_entered_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

        common.DownloadXLS(sql, strFileName, this);
    }
    [System.Web.Services.WebMethod]
    public static string GetSomething(string drpVal)
    {
        string sql = "select count(*) from onlinebill." + drpVal;
        return "aaa";
    }
}