using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace bOS.Commons.Web.PageWizard
{
    public class PageHelper
    {
        List<PageFlow> pageFlows = new List<PageFlow>();
        String currentFlowName = String.Empty;

        public PageHelper()
        { }

        public String GetCurrentFlowName() { return this.currentFlowName; }

        public void AddPageFlow(PageFlow pageFlow)
        {
            pageFlows.Add(pageFlow);
        }

        public PageFlow AddPageFlow(String pageFlowName, String[] controls)
        {
            PageFlow pageFlow = new PageFlow(pageFlowName);
            pageFlow.AddControls(controls);
            pageFlows.Add(pageFlow);

            return pageFlow;

        }

        public PageFlow AddPageFlow(string pageFlowName)
        {
            PageFlow pageFlow = new PageFlow(pageFlowName);
            pageFlows.Add(pageFlow);
            return pageFlow;
        }

        public Boolean AlmosteOneControlEnabled(Control control)
        {
            foreach (var flowStep in pageFlows)
            {

                if (flowStep.GetName().Equals(currentFlowName))
                {
                    return flowStep.AlmosteOneControlEnabled(control);
                }
            }

            return false;
        }

        public Boolean AlmosteOneControlEnabled()
        {
            foreach (var flowStep in pageFlows)
            {

                if (flowStep.GetName().Equals(currentFlowName))
                {
                    return flowStep.AlmosteOneControlEnabled();
                }
            }

            return false;
        }

        public void Register(Control control)
        {
            Boolean bRegister = false;
            foreach (var flowStep in pageFlows)
            {
                if (currentFlowName.Equals(flowStep.GetName()))
                    bRegister = true;

                if (bRegister)
                    flowStep.Register(control);
            }
        }

        public Boolean Next(Control control)
        {
            bool bFind = false;
            bool bLast = false;


            foreach (var flowStep in pageFlows)
            {
                if (bFind)
                {
                    SetCurrentFlowName(control, flowStep.GetName(), true);
                    bLast = false;
                    break;
                }
                else
                {
                    if (flowStep.GetName().Equals(currentFlowName))
                    {
                        bFind = true;
                        bLast = true;
                    }
                }

            }

            return bLast;
        }

        public Boolean Previous(Control control)
        {
            bool bFind = false;
            bool bLast = false;

            foreach (var flowStep in pageFlows.Reverse<PageFlow>())
            {
                if (bFind)
                {
                    SetCurrentFlowName(control, flowStep.GetName(), true);
                    bLast = false;
                    break;
                }
                else
                {

                    if (flowStep.GetName().Equals(currentFlowName))
                    {
                        bFind = true;
                        bLast = true;
                    }
                }

            }

            return bLast;
        }

        public void SetCurrentFlowName(Control control, String currentFlowName, Boolean basedOnRegister)
        {
            this.currentFlowName = currentFlowName;

            foreach (var pageFlow in pageFlows)
            {
                if (currentFlowName.Equals(pageFlow.GetName()))
                {
                    if (basedOnRegister)
                        pageFlow.Refresh(control);
                    else
                        pageFlow.OnOff(control, true);
                }
                else
                {
                    pageFlow.OnOff(control, false);
                }
            }
        }

        public bool IsFirstStep()
        {
            int i = 0;
            foreach (var pageFlow in pageFlows)
            {
                if (currentFlowName.Equals(pageFlow.GetName()))
                {
                    break;
                }

                i++;
            }

            return (i == 0);
        }


    }
}