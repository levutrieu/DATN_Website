﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web;

namespace DATN.WEBSITE
{
    public partial class PopUp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            }
        
        
        protected void ButtonSend_OnClicknd_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);}

        protected void btnDangNhap_OnClickOnClick(object sender, EventArgs e)
        {
            }
    }
}