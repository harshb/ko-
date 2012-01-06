using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Massive;
using jGrid.Helpers;
using System.Dynamic;
using System.Web.Mvc;


namespace MI.Domain.jGridHelpers
{

    public class jgridDisplayHelper : Controller
    {
        DynamicModel tbl;
        
        //each grid using this will define its own shape
        //Func does not work with dynamic hence have to use delegate
        public delegate List<string> DelegateShapeGridRow(dynamic inString);

        public DelegateShapeGridRow ConvertMethod;

        public jgridDisplayHelper(DynamicModel _tbl, DelegateShapeGridRow convertMethod)
        {
            tbl = _tbl;
            this.ConvertMethod = convertMethod;
        }

        public jgridDisplayHelper(DelegateShapeGridRow convertMethod)
        {
            
            this.ConvertMethod = convertMethod;
        }
     
        public JsonResult GetSearchResults(string sidx, string sord, int page, int rows, bool _search, string filters)
        {
            //Placements tbl = new Placements();
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int currentPage = pageIndex * pageSize == 0 ? 1 : pageIndex * pageSize;

            int totalRecords = tbl.Count();
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            jgridFilter searchRules = jgridFilter.Create(filters ?? "");
            bool IsSearch = _search;

            string where_string = "";

           

            if (IsSearch)
            {
                string groupOp = searchRules.groupOp;
                int i = 0;
                foreach (Rule myrule in searchRules.rules)
                {
             

                    string line = BuildLine(myrule.op, myrule.field, myrule.data);
                    
                    if (i == 0)
                    {
                        where_string = line;
                    }
                    else
                    {
                        where_string +=  " " + groupOp + " " + line;
                    }

                    i++;
                }

            }//


            IEnumerable<dynamic> results;
            if (IsSearch)
            {
                results = tbl.Paged(where: where_string, orderBy: sidx + " " + sord, currentPage: currentPage, pageSize: pageSize).Items;
            }
            else
            {
                results = tbl.Paged(orderBy: sidx + " " + sord,currentPage: currentPage, pageSize: pageSize).Items;
            }

            JsonResult  myJasonResults= GetJson(results,totalPages,page,totalRecords);

            return myJasonResults;
        }

        public JsonResult GetJson(IEnumerable<dynamic> mydata, int totalPages, int page, int totalRecords)
        {
            List<GridRowData> l = new List<GridRowData>();
            //DelegateShapeGridRow convertMethod = ShapeGridRow;

            foreach (dynamic result in mydata)
            {
                GridRowData cls = new GridRowData();
          
                //cls.cell = new List<string>() { result.autoid.ToString(), result.Objid.ToString(), result.WorkDate.ToString(), result.TimeCard2ServiceCode.ToString(), result.TimeCard2Client.ToString(), result.TimeCard2Employee.ToString(), result.Hours.ToString(), result.Amount.ToString(), result.Locked.ToString(), result.Description.ToString(), result.Created.ToString(), result.TimeCard2Creator.ToString(), result.TimeCard2EmpRate.ToString(), result.Code };
                cls.cell = ConvertMethod(result);

                l.Add(cls);
               
            }

            GridData mygridData = new GridData();

            mygridData.total = totalPages;
            mygridData.page = page;
            mygridData.records = totalRecords;
            mygridData.rows = l;

            return Json(mygridData);
        }

        
        //method 
        //private static List<string> ShapeGridRow(dynamic result)
        //{
        //    List<string> s = new List<string>();

        //    s = new List<string>() { result.autoid.ToString(), result.Objid.ToString(), result.WorkDate.ToString(), result.TimeCard2ServiceCode.ToString(), result.TimeCard2Client.ToString(), result.TimeCard2Employee.ToString(), result.Hours.ToString(), result.Amount.ToString(), result.Locked.ToString(), result.Description.ToString(), result.Created.ToString(), result.TimeCard2Creator.ToString(), result.TimeCard2EmpRate.ToString(), result.Code };


        //    return s;
        //}
        
       

        public string BuildLine(string op, string field, string data)
        {
            string s = "";
            string data_type = GetDataType(field).ToLower();

            switch (data_type)
            {
                case "int":
                case "tinyint":
                case "smallint":
                case "numeric":
                    s = field + GetOP(op) + data;
                    if (op == "cn")
                    {
                        s = field + " like '" + data.ToString() + "%'";
                    }
                    break;

                case "datetime":
                case "varchar":
                case "nvarchar":
                    s = field +  GetOP(op) + "'" + data + "'";
                    
                    if (op == "cn")
                    {
                        s = field +  " like '" + data + "%'";
                    }
                    break;
                default:
                    break;
            }


            return s;
        }

        public string GetOP(string op)
        {
            string s = "";

            switch (op)
            {
                case "eq":
                    s = " = ";
                    break;
                case "ne":
                    s = " != ";
                    break;
                case "cn":
                    s = " like ";
                    break;
                case "gt":
                    s = " > ";
                    break;
                case "lt":
                    s = " < ";
                    break;
                default:
                    break;
            }
            return s;

      
        }

        public string GetDataType(string field)
        {
            var schema = tbl.Schema;
            string ret="";

            IEnumerable<dynamic> results = tbl.Schema
                .Where(x => x.COLUMN_NAME.ToLower() == field.ToString().ToLower());
            dynamic d = new ExpandoObject();
            d = results.FirstOrDefault();

            ret = d.DATA_TYPE;

            return ret;

        }

      

    }//

    public class GridData
    {
        public int total { get; set; }
        public int page { get; set; }
        public int records { get; set; }
        public List<GridRowData> rows { get; set; }

    }

    public class GridRowData
    {
        public int i { get; set; }
        public List<string> cell { get; set; }
    }
}//ns
