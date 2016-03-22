using System;
using System.Collections.Generic;

namespace SocialApp.Utils
{
    public static class HelperMethods
    {
        public static Boolean List_Contains_Caseinsensitve(List<String> list, String ob)
        {
            foreach (String entry in list)
                if (string.Equals(entry, ob, StringComparison.OrdinalIgnoreCase))
                    return true;

            return false;
        }
    }
}