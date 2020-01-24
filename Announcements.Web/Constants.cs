using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Announcements.Web
{
    public static class Constants
    {
        public static class User
        {
            public static string CompanyRole = "CompanyUser";
            public static string AdminRole = "admin";
        }
        public static class AdType
        {
            public static string Jobs = "Jobs";
            public static string Projects = "Projects";
            public static string Announcements = "Announcements";
            public static IEnumerable<string> AdTypes()
            {
                yield return Jobs;
                yield return Projects;
                yield return Announcements;
            }
        }
    }
}
