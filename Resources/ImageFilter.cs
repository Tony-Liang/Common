using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.Resources
{
    public class ImageFilter
    {
        private string[] filter = {"*.gif","*.jpg","*.jpeg","*.bmp","*.png","*.ico" };

        private static ImageFilter imagefilter;
        private ImageFilter()
        {

        }
        public static ImageFilter GetInstance()
        {
            if (imagefilter == null)
            {
                imagefilter = new ImageFilter();
            }
            return imagefilter;
        }


        public string[] Filter
        {
            get
            {
                return filter;
            }
        }

        public string GetOpenDialog()
        {
            StringBuilder str = new StringBuilder();
            str.Append(string.Join(",",Filter));
            str.Append("|");
            str.Append(string.Join(";", Filter));
            return str.ToString();
        }
    }
}
