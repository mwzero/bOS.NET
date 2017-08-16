using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Commons.CDN.Utils
{
    public class Audit
    {
        private DateTime dtAudit;
        public DateTime DateTimeAudit
        {
            get { return this.dtAudit; }
            set { this.dtAudit = value; }
        }
        private String description;
        public String Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

        private String link;
        public String Link
        {
            get { return this.link; }
            set { this.link = value; }
        }

        public Audit(String description)
        {
            DateTimeAudit = DateTime.Now;
            Description = description;
        }

        public Audit (String description, String link)
        {
            DateTimeAudit = DateTime.Now;
            Description = description;
            Link = link;
        }


    }
}