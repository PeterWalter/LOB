using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CETAP_LOB.Model.easypay
{
    public class StringDate
    {
        private string mydate;

        public string TrueDate
        {
            get
            {
                return mydate;
            }
            set
            {
                mydate = value;
            }
        }

        public StringDate(string aDate)
        {
            mydate = ChangeFormat(aDate);
        }

        public string ChangeFormat(string aarpDate)
        {
            char[] array = aarpDate.ToCharArray();
            string text = "";
            for (int i = 0; i < 4; i++)
            {
                text += array[i];
            }

            string text2 = "";
            for (int i = 4; i < 6; i++)
            {
                text2 += array[i];
            }

            string text3 = "";
            for (int i = 6; i < 8; i++)
            {
                text3 += array[i];
            }

            return text + "/" + text2 + "/" + text3;
        }
    }
}
