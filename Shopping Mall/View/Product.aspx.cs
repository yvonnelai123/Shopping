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
        public Discount dis = new Discount();
        protected void Page_Load(object sender, EventArgs e)
        {         
            //0708每次load都先判斷是否有回傳值，第一次開網頁並沒有回傳
            delete();
            PutIntoCart();
            setLeftBar();
            pageShow(5);
            //paging(page);
        }

        private void setLeftBar()
        {
            String[][] arrType = db.searchGroupBy("type");
            for (int i = 0; i < arrType.Length; i++)
            {
                leftbarStr += "<div class='leftbar-type'>" + arrType[i][0] + "</div><ul>";
                String[][] productArr = db.searchByRow("type", arrType[i][0]);
                for (int j = 0; j < productArr.Length; j++)
                {
                    leftbarStr += "<a href='ProductInformation.aspx?p=" + productArr[j][0] + "'><li>" + productArr[j][1] + "</li></a>";
                }
                leftbarStr += "</ul>";
            }
        }
        //刪除
        private void delete()
        {
            String del = Request.QueryString["d"];
            if (del != null)
            {
                db.delete("ID", del);
            }
        }

        //click商品數量加入購物車
        private void PutIntoCart()
        {
            String num = Request.QueryString["num"];
            String ID = Request.QueryString["ID"];
            if (num != null && num!="")
            {
                if ((String)Session["account"]!=null)
                {

                }
                else{
                    Response.Write("<Script language='JavaScript'>alert('請登入');</Script>");
                }
                
            }
        }
        /*
        private void pageShow()
        {
            String[][] array;
            if (Request.QueryString["t"] == null)
            {
                array = db.searchByColumnOrder("ID,name,type,price,num,picture,introduction,discountID");
            }
            else
            {
                array = db.searchByRowOrder("type", Request.QueryString["t"]);
            }
            //0707 新增
            //0708 修改可以回傳ID、購買數量給自己這頁
            for (int a = 0; a <= (array.Length / 2); a++)
            {
                if (2 * a == array.Length)
                    break;
                else
                    rightStr += "<div class ='product'>";
                for (int b = 0; b < 2; b++)
                {
                    if (2 * a + b < array.Length)
                    {
                        String[] discount = null;
                        rightStr += "<div class ='product-inside'>"
                            + "<div class='ImgDel'>";
                        if (array[2 * a + b][7] != null && array[2 * a + b][7] != "0")
                        {
                            discount = dis.findingType(Convert.ToInt32(array[2 * a + b][7]), 1, Convert.ToInt32(array[2 * a + b][3]));
                            rightStr += "<a href='ProductInformation.aspx?p=" + array[2 * a + b][0] + "'><div class='image' style='background:url(../UploadPic/" + array[2 * a + b][5] + ") no-repeat; background-size:300px 200px;'><t class = 'dis-text'>" + discount[0] + "</t></div></a>";
                        }
                        else rightStr += "<div class='image'><a href='ProductInformation.aspx?p=" + array[2 * a + b][0] + "'><img src=../UploadPic/" + array[2 * a + b][5] + "></a></div>";
                        rightStr += "<div class='delete'>";

                        //刪除按鈕visible的判斷
                        if ((String)Session["account"] == "admin")
                        {
                            rightStr += "<a href='Product.aspx?d=" + array[2 * a + b][0] + "'><img src=../Picture/delete.png style='width:48px;'></a>";
                            rightStr += "</div><div class='delete'>";
                            rightStr += "<a href='ProductEditor.aspx?u=" + array[2 * a + b][0] + "'><img src=../Picture/edit.png style='width:45px;'></a></div>";
                        }
                        else
                        {
                            rightStr += "</div>";
                        }
                        rightStr += "</div>"
                        + "<div class='name'><a href='ProductInformation.aspx?p=" + array[2 * a + b][0] + "'>" + array[2 * a + b][1] + "</a></div>";
                        if (array[2 * a + b][7] != null && array[2 * a + b][7] != "0")
                        {
                            //String[][] discount = db.innerJoin("product.ID,discount.discountID", "discount", "product.discountID", "discount.discountID", "product.ID", array[2 * a + b][0]);
                            //策略顯示
                            //String[] discount = dis.findingType(Convert.ToInt32(array[2 * a + b][7]), 1, Convert.ToInt32(array[2 * a + b][3]));
                            //rightStr += "<div class='information'><b style='font-size=0.5cm'>價格：</b><del class='discount'>" + array[2 * a + b][3] + "元</del><t class = 'dis'>" + discount[0] + "</t><b style='font-size=0.5cm;padding-left:35px;'>數量：</b>" + array[2 * a + b][4] + "</div>";
                            rightStr += "<div class='information'><b style='font-size=0.5cm'>價格：</b><del class='discount'>" + array[2 * a + b][3] + "元</del><t class = 'dis'>" + discount[1] + "元</t><b style='font-size=0.5cm;padding-left:30px;'>數量：</b>" + array[2 * a + b][4] + "</div>";
                        }
                        else
                        {
                            rightStr += "<div class='information'><b style='font-size=0.5cm'>價格：</b>" + array[2 * a + b][3] + "元<b style='font-size=0.5cm;padding-left:35px;'>數量：</b>" + array[2 * a + b][4] + "</div>";

                        }
                        //欄位ID,name,type,price,num,picture,discountID
                        rightStr += "<div class='information'>"
                                + "<form action='Product.aspx' method='get' onsubmit='return validate_form(this)'>購買數量："
                                + "<input type='number' name='num' min='1' max='" + array[2 * a + b][4] + "' style=width:50px runat'server'>"
                                + "<input type='hidden' name='ID' value='" + array[2 * a + b][0] + "' runat'server'><br>"
                                + "<input class='button-style' type='submit' value='加入購物車'>"
                                + "</form>"
                                + "</div>"
                                + "</div>";

                    }
                }

                rightStr += "</div>";
            }
            rightStr += "<div class='page'>";
            for (int page = 1; page < (array.Length / 3) + 1; page++)
            {
                rightStr += "<a href='Product.aspx?pp=" + page + "'>" + page + "";
            }
            rightStr += "</div>";
        }*/

        //num is the amount of items at each page
        //p為分頁碼
        private void pageShow(int num)
        {
            String pp = Request.QueryString["pp"];
            Boolean finish =false;
            int index = Convert.ToInt32(pp);
            if (pp == null)
            {
                index = 1;
            }
            
                //首頁click more進來的畫面
                String[][] array;
                if (Request.QueryString["t"] == null)
                {
                    array = db.searchByColumnOrder("ID,name,type,price,num,picture,introduction,discountID");
                }
                else
                {
                    array = db.searchByRowOrder("type", Request.QueryString["t"]);
                }
                //0707 新增
                //0708 修改可以回傳ID、購買數量給自己這頁
                for (int a = 0; a <= (num / 2); a++)
                {
                    if (finish == true)
                        break;
                    if (2 * a == num)
                        break;
                    else
                        rightStr += "<div class ='product'>";
                    for (int b = 0; b < 2; b++)
                    {
                        if (finish == true)
                            break;
                        if (2 * a + b < num)
                        {
                            int i = 2 * a + b + (index-1) * num;
                            if(i == array.Length-1){
                                finish=true;
                            }
                            rightStr += "<div class ='product-inside'>"
                                + "<div class='ImgDel'>"
                                + "<div class='image'><a href='ProductInformation.aspx?p=" + array[i][0] + "'><img src=../UploadPic/" + array[i][5] + "></a></div>"
                                + "<div class='delete'>";
                            //刪除按鈕visible的判斷
                            if ((String)Session["account"] == "admin")
                            {
                                rightStr += "<a href='Product.aspx?d=" + array[i][0] + "'><img src=../Picture/delete.png style='width:50px;'></a>";
                                rightStr += "<a href='ProductEditor.aspx?u=" + array[i][0] + "'>update</a></div>";
                            }
                            else
                            {
                                rightStr += "</div>";
                            }
                            rightStr += "</div>"
                            + "<div class='name'><a href='ProductInformation.aspx?p=" + array[i][0] + "'>" + array[i][1] + "</a></div>";
                            if (array[i][7] != null && array[i][7] != "0")
                            {

                                //String[][] discount = db.innerJoin("product.ID,discount.discountID", "discount", "product.discountID", "discount.discountID", "product.ID", array[i][0]);
                                //策略顯示
                                String[] discount = dis.findingType(Convert.ToInt32(array[i][7]), 1, Convert.ToInt32(array[i][3]));
                                rightStr += "<div class='information'><b style='font-size=0.5cm'>價格：</b><del class='discount'>" + array[i][3] + "元</del><t class = 'dis'>" + discount[0] + "</t><b style='font-size=0.5cm;padding-left:35px;'>數量：</b>" + array[i][4] + "</div>";
                            }
                            else
                            {
                                rightStr += "<div class='information'><b style='font-size=0.5cm'>價格：</b>" + array[i][3] + "元<b style='font-size=0.5cm;padding-left:35px;'>數量：</b>" + array[i][4] + "</div>";

                            }
                            //欄位ID,name,type,price,num,picture,discountID
                            rightStr += "<div class='information'>"
                                    + "<form action='Product.aspx' method='get' onsubmit='return validate_form(this)'>購買數量："
                                    + "<input type='number' name='num' min='1' max='" + array[i][4] + "' style=width:50px runat'server'>"
                                    + "<input type='hidden' name='ID' value='" + array[i][0] + "' runat'server'><br>"
                                    + "<input class='button-style' type='submit' value='加入購物車'>"
                                    + "</form>"
                                    + "</div>"
                                    + "</div>";

                        }
                    }

                    rightStr += "</div>";
                }
                rightStr += "<div class='page'>";
                for (int page = 1; page <= (array.Length / num) + 1; page++)
                {
                    if (num * page == array.Length)
                    {
                    }
                    else
                    {
                        rightStr += "<a href='Product.aspx?pp=" + page + "'>" + page + "";
                    }
                }
                rightStr += "</div>";
                        
        }
    }
}