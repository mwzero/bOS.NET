using log4net;
using Microsoft.JScript;
using Microsoft.JScript.Vsa;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace bOS.Commons.FormHelper
{
    public class FormulaHelper
    {
        protected static readonly ILog logger = LogManager.GetLogger(typeof(FormulaHelper));
        protected static Dictionary<String, String> mappingVariables = new Dictionary<string,string>()
        { 
            {"nomepaziente", "nome"}, 
            {"cognomepaziente","cognome"}
        };

        private List<System.Data.DataTable> ds;

        public FormulaHelper(List<System.Data.DataTable> ds)
        {
            this.ds = ds;
        }
        
        public String ResolveFormula(String formula)
        {
            if (String.IsNullOrEmpty(formula))
                return String.Empty;

            if ("now".CompareTo(formula.ToLower()) == 0)
                return DateHelper.GetCurrentDateTimeToString("dd/MM/yyyy");

            Boolean isFormulaNotEvaluable = false;
            logger.Debug("Resolving formula: " + formula);
            formula = formula.Replace("=", "").Replace("&amp;", "&").Replace(",", ".");

            string pat = @"(&)(\w+)(.)(\w+);";
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);

            Match match = r.Match(formula);
            while (match.Success)
            {
                String variable = match.Value.Replace("&", "").Replace(";", "").ToLower();
                String table2find = String.Empty;
                if (variable.IndexOf(".") > 0)
                {
                    table2find = variable.Substring(0, variable.IndexOf("."));
                    variable = variable.Substring(variable.IndexOf(".") + 1);

                    //try to map variable name
                    if (mappingVariables.ContainsKey(variable))
	                {
	                    variable = mappingVariables[variable];
	                }

                    if ( variable.IndexOf(table2find) > 0 )
                    {
                        variable = variable.Replace(table2find, "");
                    }
                }
                


                Object value = null;
                if (String.IsNullOrEmpty(table2find))
                {
                    if ((ds != null) && (ds.Count > 0) && (ds[0] != null) && (ds[0].Rows[0] != null))
                    {
                        try
                        {
                            value = ds[0].Rows[0][variable];
                        }
                        catch (Exception err)
                        {
                            logger.Warn("Something wrong with variable: " + variable, err);
                        }
                    }
                }
                else
                {
                    if (ds != null)
                    {
                        DataTable dtFind = ds.FirstOrDefault(t => t.TableName.Equals(table2find, StringComparison.InvariantCultureIgnoreCase));
                        if (dtFind != null)
                        {
                            try
                            {
                                value = dtFind.Rows[0][variable];
                            }
                            catch ( Exception err)
                            {
                                logger.Warn("Variable not found: " + variable, err);
                            }
                        }
                    }
                }
                if (value == null)
                    isFormulaNotEvaluable = true;
                else
                {
                    if (value.GetType() == typeof(String))
                        value = "'" + value + "'";
                    else if ( value.GetType() == typeof(DateTime))
                        value = "'" + value + "'";

                    formula = formula.Replace(match.Value, value.ToString());
                }

                match = match.NextMatch();
            }

            logger.Debug("Formula to evaluate: " + formula);

            if (isFormulaNotEvaluable)
            {
                logger.Warn("Something wrong with variables evaluating. Formula:" + formula);
                return String.Empty;
            }
            else
            {
                try
                {
                    VsaEngine _engine = VsaEngine.CreateEngine();
                    String result = Eval.JScriptEvaluate(formula, _engine).ToString();

                    return result;
                }
                catch (Exception err)
                {
                    logger.Error("Something wrong evaluating formula:" + formula, err);
                    return String.Empty;
                }
            }
        }
    }
}
