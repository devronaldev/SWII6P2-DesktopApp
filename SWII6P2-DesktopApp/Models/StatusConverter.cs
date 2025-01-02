using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWII6P2_DesktopApp.Models
{
    public static class StatusConverter
    {

        public static string Convert(bool value)
        {
            return (bool)value ? "Ativo" : "Inativo";
        }

        public static string Convert(int value)
        {
            return value == 1 ? "Ativo" : "Inativo";
        }

        public static bool ConvertBack(int value)
        {
            return value == 1 ? true : false;
        }

        public static List<object> Convert(List<User> users)
        {
            List<object> result = new List<object>();
            foreach (User user in users)
            {
                result.Add(Convert(user.Status));
            }
            return result;
        }
    }
}
