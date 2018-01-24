using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeanManager
{
    public class grapevine_error
    {
        public static string Error(int Id, String str)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(new grapevine_error(Id, str));
        }
        public int id;
        public string error;

        public grapevine_error(int Id, String str)
        {
            id = Id;
            error = str;
        }

        public grapevine_error(int Id, String str, params object[] parameters)
        {
            id = Id;
            error = String.Format(str, parameters);
        }


    }
}
