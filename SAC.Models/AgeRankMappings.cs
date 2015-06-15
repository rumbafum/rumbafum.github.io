using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAC.Models
{
    public static class AgeRankMappings
    {
        static Dictionary<string, string> _ageRankMappings = new Dictionary<string, string>();

        public static void Init()
        {
            _ageRankMappings.Add("Benjamins B (M)", "BENJAMINS MASCULINOS");
            _ageRankMappings.Add("Benjamins B (F)", "BENJAMINS FEMININOS");
            _ageRankMappings.Add("Infantis (M)", "INFANTIS MASCULINOS");
            _ageRankMappings.Add("Infantis (F)", "INFANTIS FEMININOS");
            _ageRankMappings.Add("Iniciados (M)", "INICIADOS MASCULINOS");
            _ageRankMappings.Add("Iniciados (F)", "INICIADOS FEMININOS");
            _ageRankMappings.Add("Juvenis (M)", "JUVENIS MASCULINOS");
            _ageRankMappings.Add("Juvenis (F)", "JUVENIS FEMININOS");
            _ageRankMappings.Add("Juniores (M)", "JUNIORES MASCULINOS");
            _ageRankMappings.Add("Juniores (F)", "JUNIORES FEMININOS");
            _ageRankMappings.Add("Seniores (M)", "SENIORES MASCULINOS");
            _ageRankMappings.Add("Seniores (F)", "SENIORES FEMININOS");
            _ageRankMappings.Add("M 35", "VETERANOS M35");
            _ageRankMappings.Add("F 35", "VETERANAS F35");
            _ageRankMappings.Add("M 40", "VETERANOS M40");
            _ageRankMappings.Add("F 40", "VETERANAS F40");
            _ageRankMappings.Add("M 45", "VETERANOS M45");
            _ageRankMappings.Add("F 45", "VETERANAS F45");
            _ageRankMappings.Add("M 50", "VETERANOS M50");
            _ageRankMappings.Add("F 50", "VETERANAS F50");
            _ageRankMappings.Add("M 55", "VETERANOS M55");
            _ageRankMappings.Add("F 55", "VETERANAS F55");
            _ageRankMappings.Add("M 60", "VETERANOS M60");
            _ageRankMappings.Add("F 60", "VETERANAS F60");
            _ageRankMappings.Add("M 65", "VETERANOS M65");
            _ageRankMappings.Add("F 65", "VETERANAS F65");
            _ageRankMappings.Add("M 70", "VETERANOS M70");
            _ageRankMappings.Add("F 70", "VETERANAS F70");
        }

        public static string GetAgeRankNameByCode(string code)
        {
            if (_ageRankMappings.Count == 0)
                Init();
            return _ageRankMappings.ContainsKey(code) ? _ageRankMappings[code] : code.ToUpper();
        }
    }
}
