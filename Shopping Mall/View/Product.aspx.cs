﻿using Shopping_Mall.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Shopping_Mall.View
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public String leftbarStr = "";
        public String rightStr = "";
        public DBFunction db = new DBFunction("product");
        protected void Page_Load(object sender, EventArgs e)
        {
            setLeftBar();
            //0707 新增
            String[] arrName = db.searchByColumn("name");
            String[] arrImg = db.searchByColumn("picture");
            String[] arrPrice = db.searchByColumn("price");
            String[] arrNum = db.searchByColumn("num");
            for (int a = 0; a <= (arrName.Length / 2); a++)
            {
                if (2 * a == arrName.Length)
                    break;
                else
                    rightStr += "<div class ='product'>";
                for (int b = 0; b < 2; b++)
                {
                    if(2*a+b<arrName.Length)
                        rightStr += "<div class ='product-inside'><div class='image'><img src=../UploadPic/" + arrImg[2 * a + b] + "></div><div class='name'><a href='#'>" + arrName[2 * a + b] + "</a></div><div class='information'><b style='font-size=0.5cm'>價格：</b>" + arrPrice[2 * a + b] + "<b style='font-size=0.5cm;padding-left:10px;'>數量：</b>" + arrNum[2 * a + b] + "</div></div>";
                }
                rightStr += "</div>";
            }
        }
        private void setLeftBar()
        {
            String[][] arrType = db.searchGroupBy("type");
            for (int i = 0; i < arrType.Length; i++)
            {
                leftbarStr += "<a href='Product.aspx'><div class='leftbar-type'>" + arrType[i][0] + "</div><div class='leftbar-baseline'></div></a><ul>";
                String[][] productArr = db.searchByRow("type", arrType[i][0]);
                for (int j = 0; j < productArr.Length; j++)
                {
                    leftbarStr += "<a href='ProductInformation.aspx?p=" + productArr[j][0] + "'><li>" + productArr[j][1] + "</li></a>";
                }
                leftbarStr += "</ul>";
            }
        }
    }
}