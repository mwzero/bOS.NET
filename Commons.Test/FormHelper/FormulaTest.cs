using Microsoft.JScript;
using Microsoft.JScript.Vsa;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace bOS.Commons.FormHelper
{
    [TestClass]
    public class FormulaTest
    {
        [TestMethod]
        public void MatchTest()
        {
            string text = "=66,5+((13,75*&Peso;)+(5*&Altezza;)-(6,75*&EtàPaziente;))";
            string pat = @"(&)(\w+);";
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);

            Match match = r.Match(text);
            while (match.Success) 
            {
                Console.WriteLine(match.Value);
                match = match.NextMatch();
            }
        }

        [TestMethod]
        public void TestFormulaOneTable()
        {
            DataTable dt = new DataTable("tableName");
            dt.Columns.Add("Peso", typeof(float));
            dt.Columns.Add("Altezza", typeof(float));
            dt.Columns.Add("EtàPaziente", typeof(float));
            dt.Rows.Add(70, 1.70, 40);

            List<DataTable> ds = new List<DataTable>();
            ds.Add(dt);
           
            FormulaHelper helper = new FormulaHelper(ds);
            String formula = "=66,5+((13,75*&amp;Peso;)+(5*&amp;Altezza;)-(6,75*&amp;EtàPaziente;))";
            String result = helper.ResolveFormula(formula);
            Assert.AreEqual(result, "766", "Something wrong with formula using variables");

        }

        [TestMethod]
        public void TestFormulaMultiTables()
        {
            DataTable dt = new DataTable("tableName");
            dt.Columns.Add("Peso", typeof(float));
            dt.Columns.Add("Altezza", typeof(float));
            dt.Columns.Add("EtàPaziente", typeof(float));
            dt.Rows.Add(70, 1.70, 40);

            DataTable dt2 = new DataTable("aux_ref_visite");
            dt2.Columns.Add("NomeReferto", typeof(String));
            dt2.Rows.Add("Referto1");


            List<DataTable> ds = new List<DataTable>();
            ds.Add(dt);
            ds.Add(dt2);


            FormulaHelper helper = new FormulaHelper(ds);
            String formula = "==&amp;aux_ref_visite.NomeReferto;";
            String result = helper.ResolveFormula(formula);
            Assert.AreEqual(result, "Referto1", "Something wrong with formula using tables");

        }

    }
}
