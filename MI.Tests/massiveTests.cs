using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Massive;
using System.Dynamic;
using MI.Tests;


namespace Massive
{

    [TestFixture]
    public class dbTests : TestBase
    {
        //use inine or create a model
        // dynamic tbl = new DynamicModel("mi", tableName: "MEDIA_CAT_DIGITAL_PLCMNT", primaryKeyField: "OID_MEDIA_CAT_DIGITAL_PLCMNT");

        dynamic tbl = new Placements();

        [Test]
        public void test_all()
        {  
            var x = tbl.All();
            int cnt = tbl.Count();
            Assert.GreaterOrEqual(cnt, 1);
        }

        [Test]
        public void test_single()
        {
            string name = "ABC.com";
            string crit = "PUBLISHER_NAME ='" + name + "'";
            var result = tbl.Single(where: crit);
            string placement_name = result.PLACEMENT_NAME;
            Assert.IsNotEmpty(placement_name);
        }


        [Test]
        public void test_multiple()
        {

            IEnumerable<dynamic> results = tbl.All();
            dynamic d = new ExpandoObject();
            d = results.FirstOrDefault();
            string Pubname = d.PUBLISHER_NAME;
            Assert.IsNotEmpty(Pubname);

        }


        [Test]
        public void test_find()
        {
            IEnumerable<dynamic> results = tbl.Find(PLACEMENT_CODE: "PAID_SEARCH_200106_636119");
            dynamic d = new ExpandoObject();
            d = results.FirstOrDefault();
            string Pubname = d.PUBLISHER_NAME;
            Assert.IsNotEmpty(Pubname);
           
        }

        [Test]
        public void test_paged()
        {

            IEnumerable<dynamic> results = tbl.Paged(where: "OID_MEDIA_CAT_DIGITAL_PLCMNT > 1", currentPage: 1, pageSize: 20).Items;

            string desc = "";
            foreach (dynamic result in results)
            {
                desc = result.PUBLISHER_NAME;
            }
            Assert.IsNotEmpty(desc);
        }

        //helper fn shows how to get schema info
        public string GetDataType(string field)
        {
            DynamicModel mytbl = tbl;

            var schema = mytbl.Schema;
            string ret = "";

            IEnumerable<dynamic> results = mytbl.Schema
                .Where(x => x.COLUMN_NAME.ToLower() == field.ToString().ToLower());
            dynamic d = new ExpandoObject();
            d = results.FirstOrDefault();

            ret = d.DATA_TYPE;

            return ret;

        }


        [Test]
        public void test_join()
        {
            string s = @"
                       SELECT     MEDIA_CAT_DIGITAL_PLCMNT.OID_MEDIA_CAT_DIGITAL_PLCMNT, MEDIA_CAT_DIGITAL_PLCMNT.MEDIA_GENRE, 
                      MEDIA_CAT_DIGITAL_PLCMNT.MEDIA_PLAN_NAME, MEDIA_CAT_DIGITAL_PLCMNT.PLACEMENT_NAME, MEDIA_CAT_DIGITAL_PLCMNT.PLACEMENT_CODE, 
                      MEDIA_CAT_DIGITAL_PLCMNT.COST, MEDIA_CAT_DIGITAL_PLCMNT.BEGIN_DATE, MEDIA_CAT_DIGITAL_PLCMNT.END_DATE, 
                      MEDIA_CAT_DIGITAL_PLCMNT.PLACEMENT_MEDIA_CHANNEL, PUBLICATION_NETWORKS.PUBLICATION_NETWORK
                      FROM         MEDIA_CAT_DIGITAL_PLCMNT INNER JOIN
                      PUBLICATION_NETWORKS ON MEDIA_CAT_DIGITAL_PLCMNT.OID_PUBLISHER_ID = PUBLICATION_NETWORKS.OID_PUBLICATION_NETWORK_LU       

            ";


            IEnumerable<dynamic> results = tbl.Query(s);

            int cnt = results.Count();

            dynamic d = new ExpandoObject();
            d = results.FirstOrDefault();
            string Pubname = d.PUBLICATION_NETWORK;
            Assert.IsNotEmpty(Pubname);

        }

        [Test]
        public void test_placements_stats_join()
        {
            string s = @"
                 SELECT     MEDIA_STATISTICS.*
                FROM         MEDIA_CAT_DIGITAL_PLCMNT INNER JOIN
                 MEDIA_STATISTICS ON MEDIA_CAT_DIGITAL_PLCMNT.PLACEMENT_CODE 
                 = MEDIA_STATISTICS.PLACEMENT_CODE
                and OID_MEDIA_CAT_DIGITAL_PLCMNT = 349

            ";


            IEnumerable<dynamic> results = tbl.Query(s);

            int cnt = results.Count();

            dynamic d = new ExpandoObject();
            d = results.FirstOrDefault();

            ShapeRow(d);
            //string Pubname = d.PLACEMENT_CODE;

            //string x = getFieldValue(d, "PLACEMENT_CODE");
            
            //Assert.IsNotEmpty(Pubname);

        }

        //---helper to build grid row ---------

        public void ShapeRow(dynamic d)
        {
            
            List<string> fields_to_display = new List<string>() { "OID_MEDIA_CAT_DIGITAL_PLCMNT", "MEDIA_GENRE","MEDIA_PLAN_NAME", "OID_PUBLISHER_ID","PUBLICATION_NETWORK","PLACEMENT_CODE","BEGIN_DATE","END_DATE","COST" };
            List<string> field_values = new List<string>();
           
            foreach (string s in fields_to_display)
            {
                string myvalue = getFieldValue(d,s);
                field_values.Add(myvalue);
            }
            
        }
        public string getFieldValue(dynamic myexpando, string fieldname )
        {
            string ret = "";

            foreach (KeyValuePair<string, object> pair in myexpando)
            {
                string val = "";
                if (pair.Value != null)
                {
                    val = pair.Value.ToString();
                    if (pair.Key == fieldname)
                    {
                        ret = val;
                    }
                }

            }
           
            return ret;

        }
    }//
}//
