using COI.DAL;
using System;
namespace COI.Util
{
    public class StarlightEngine
    {
        public string GetLinks(int investigationID)
        {
            try
            {
                var manager = new XMLManager();
                return manager.GetLinks(investigationID).Replace("<individual_company_link",
                                                                 "\n<individual_company_link");
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
