using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;
using System;
using UnityEngine;

public static class CountryCode
{
    private static string url = "http://ip-api.com/json";



    /// <summary>
    /// return current county where you are stay now
    /// </summary>
    /// <param name="onSuccessCallBack"></param>
    /// <param name="onErrorCallBack"></param>
    /// <returns></returns>
    public static IEnumerator GetCurentountryJson(RestApiHandeler.OnSuccessCallBack onSuccessCallBack, RestApiHandeler.OnErrorCallBack onErrorCallBack)
    {
        yield return RestApiHandeler.GetData(url,null, onSuccessCallBack, onErrorCallBack);
    }

    /// <summary>
    /// return country data from json to type of "ISO3166Country"
    /// </summary>
    /// <param name="json"> country json data </param>
    /// <returns></returns>
    public static ISO3166Country JsonToObject(string json)
    {
        CountryData countryinfo = JsonUtility.FromJson<CountryData>(json);
        string region = countryinfo.countryCode;
        return GeneratorFromAlpha2(region);
    }


    /// <summary>
    /// Obtain ISO3166-1 Country based on its alpha3 code.
    /// </summary>
    /// <param name="alpha3"></param>
    /// <returns></returns>
    public static ISO3166Country GeneratorFromAlpha3(string alpha3)
    {
        return GetCollection().FirstOrDefault(p => p.Alpha3 == alpha3);
    }

    /// <summary>
    /// return country data from country code to type of "ISO3166Country"
    /// </summary>
    /// <param name="alpha2">Country code like BD, US,UK </param>
    /// <returns></returns>
    public static ISO3166Country GeneratorFromAlpha2(string alpha2)
    {
        return GetCollection().FirstOrDefault(p => p.Alpha2 == alpha2);
    }

    /// <summary>
    /// return country data from phone number to type of "ISO3166Country"
    /// </summary>
    /// <param name="number"> phone number like +880, +993, +355 etc</param>
    /// <returns></returns>
    public static ISO3166Country GeneratorFromPhoneNumber(string number)
    {
        return GetCollection().FirstOrDefault(p => number.Contains(p.DialCodes[0]));
    }

    /// <summary>
    /// return all of the countrys data
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<ISO3166Country> GetALLCountry()
    {
        return GetCollection();
    }


    #region Country Data Collection
    private static IEnumerable<ISO3166Country> GetCollection()
    {
        // This collection built from Wikipedia entry on ISO3166-1 on 9th Feb 2016
        return new[] {
                new ISO3166Country("Afghanistan", "AF", "AFG", 9, new[] { "+993" }),
                new ISO3166Country("Åland Islands", "AX", "ALA", 10, new[] { "+358" }),
                new ISO3166Country("Albania", "AL", "ALB", 9, new[] { "+355" }),
                new ISO3166Country("Algeria", "DZ", "DZA", 9, new[] { "+213" }),
                new ISO3166Country("American Samoa", "AS", "ASM", 7, new[] { "+1 684" }),
                new ISO3166Country("Andorra", "AD", "AND", -1, new[] { "+376" }),
                new ISO3166Country("Angola", "AO", "AGO", -1, new[] { "+244" }),
                new ISO3166Country("Anguilla", "AI", "AIA", 7, new[] { "+1 264" }),
                new ISO3166Country("Antarctica", "AQ", "ATA", 9, new[] { "+672" }),
                new ISO3166Country("Antigua and Barbuda", "AG", "ATG", 7, new[] { "+1 268" }),
                new ISO3166Country("Argentina", "AR", "ARG", -1, new[] { "+54" }),
                new ISO3166Country("Armenia", "AM", "ARM", 6, new[] { "+374" }),
                new ISO3166Country("Aruba", "AW", "ABW", 7, new[] { "+297" }),
                new ISO3166Country("Australia", "AU", "AUS", 9, new[] { "+61" }),
                new ISO3166Country("Austria", "AT", "AUT", -1, new[] { "+43" }),
                new ISO3166Country("Azerbaijan", "AZ", "AZE", 9, new[] { "+994" }),
                new ISO3166Country("Bahamas", "BS", "BHS", 7, new[] { "+1 242" }),
                new ISO3166Country("Bahrain", "BH", "BHR", 8, new[] { "+973" }),
                new ISO3166Country("Bangladesh", "BD", "BGD", 10, new[] { "+880" }),
                new ISO3166Country("Barbados", "BB", "BRB", 7, new[] { "+1 246" }),
                new ISO3166Country("Belarus", "BY", "BLR", 9, new[] { "+375" }),
                new ISO3166Country("Belgium", "BE", "BEL", 9, new[] { "+32" }),
                new ISO3166Country("Belize", "BZ", "BLZ", 7, new[] { "+501" }),
                new ISO3166Country("Benin", "BJ", "BEN", -1, new[] { "+229" }),
                new ISO3166Country("Bermuda", "BM", "BMU", 7, new[] { "+1 441" }),
                new ISO3166Country("Bhutan", "BT", "BTN", -1, new[] { "+975" }),
                new ISO3166Country("Bolivia", "BO", "BOL", -1, new[] { "+591" }),
                new ISO3166Country("Bonaire", "BQ", "BES", -1, new[] { "+599" }),
                new ISO3166Country("Bosnia and Herzegovina", "BA", "BIH", 8, new[] { "+387" }),
                new ISO3166Country("Botswana", "BW", "BWA", -1, new[] { "+267" }),
                new ISO3166Country("Jan Mayen", "BV", "BVT", -1,new[] { "+47" }),
                new ISO3166Country("Brazil", "BR", "BRA", 11, new[] { "+55" }),
                new ISO3166Country("British Indian Ocean Territory", "IO", "IOT", 7, new[] { "+246" }),
                new ISO3166Country("Brunei", "BN", "BRN", -1, new[] { "+673" }),
                new ISO3166Country("Bulgaria", "BG", "BGR", 9, new[] { "+359" }),
                new ISO3166Country("Burkina Faso", "BF", "BFA", 8, new[] { "+226" }),
                new ISO3166Country("Burundi", "BI", "BDI", -1, new[] { "+257" }),
                new ISO3166Country("Cape Verde", "CV", "CPV", -1, new[] { "+238" }),
                new ISO3166Country("Cambodia", "KH", "KHM", 9, new[] { "+855" }),
                new ISO3166Country("Cameroon", "CM", "CMR", -1, new[] { "+237" }),
                new ISO3166Country("Canada", "CA", "CAN", 10, new[] { "+1" }),
                new ISO3166Country("Cayman Islands", "KY", "CYM", 7, new[] { "+1 345" }),
                new ISO3166Country("Central African Republic", "CF", "CAF", -1, new[] { "+236" }),
                new ISO3166Country("Chad", "TD", "TCD", 8, new[] { "+235" }),
                new ISO3166Country("Chile", "CL", "CHL", 9, new[] { "+56" }),
                new ISO3166Country("China", "CN", "CHN", -1, new[] { "+86" }),
                new ISO3166Country("Christmas Island", "CX", "CXR", -1, new[] { "+61" }),
                new ISO3166Country("Cocos (Keeling) Islands", "CC", "CCK", -1, new[] { "+61" }),
                new ISO3166Country("Colombia", "CO", "COL", 10, new[] { "+57" }),
                new ISO3166Country("Comoros", "KM", "COM", -1, new[] { "+269" }),
                new ISO3166Country("Congo", "CG", "COG", -1, new[] { "+242" }),
                new ISO3166Country("Congo Democratic Republic of the", "CD", "COD", -1, new[] { "+243" }),
                new ISO3166Country("Cook Islands", "CK", "COK", 5, new[] { "+682" }),
                new ISO3166Country("Costa Rica", "CR", "CRI", 8, new[] { "+506" }),
                new ISO3166Country("Côte d'Ivoire", "CI", "CIV", -1, new[] { "+225" }),
                new ISO3166Country("Croatia", "HR", "HRV", 9, new[] { "+385" }),
                new ISO3166Country("Cuba", "CU", "CUB", -1, new[] { "+53" }),
                new ISO3166Country("Curaçao", "CW", "CUW", -1, new[] { "+599" }),
                new ISO3166Country("Cyprus", "CY", "CYP", 8, new[] { "+357" }),
                new ISO3166Country("Czech Republic", "CZ", "CZE", 9, new[] { "+420" }),
                new ISO3166Country("Denmark", "DK", "DNK", 8, new[] { "+45" }),
                new ISO3166Country("Djibouti", "DJ", "DJI", 8, new[] { "+253" }),
                new ISO3166Country("Dominica", "DM", "DMA", 7, new[] { "+1 767" }),
                new ISO3166Country("Dominican Republic", "DO", "DOM", 10, new[] { "+1"}),
                new ISO3166Country("Ecuador", "EC", "ECU", 9, new[] { "+593" }),
                new ISO3166Country("Egypt", "EG", "EGY", 10, new[] { "+20" }),
                new ISO3166Country("El Salvador", "SV", "SLV", 8, new[] { "+503" }),
                new ISO3166Country("Equatorial Guinea", "GQ", "GNQ", -1, new[] { "+240" }),
                new ISO3166Country("Eritrea", "ER", "ERI", -1, new[] { "+291" }),
                new ISO3166Country("Estonia", "EE", "EST", -1, new[] { "+372" }),
                new ISO3166Country("Ethiopia", "ET", "ETH", -1, new[] { "+251" }),
                new ISO3166Country("Falkland Islands", "FK", "FLK", 5, new[] { "+500" }),
                new ISO3166Country("Faroe Islands", "FO", "FRO", 5, new[] { "+298" }),
                new ISO3166Country("Fiji", "FJ", "FJI", -1, new[] { "+679" }),
                new ISO3166Country("Finland", "FI", "FIN", 10, new[] { "+358" }),
                new ISO3166Country("France", "FR", "FRA", 9, new[] { "+33" }),
                new ISO3166Country("French Guiana", "GF", "GUF", -1, new[] { "+594" }),
                new ISO3166Country("French Polynesia", "PF", "PYF", 6, new[] { "+689" }),
                new ISO3166Country("French Southern Territories", "TF", "ATF", -1, new[] { "+262" }),
                new ISO3166Country("Gabon", "GA", "GAB", 7, new[] { "+241" }),
                new ISO3166Country("Gambia", "GM", "GMB", -1, new[] { "+220" }),
                new ISO3166Country("Georgia", "GE", "GEO", 9, new[] { "+995" }),
                new ISO3166Country("Germany", "DE", "DEU", -1, new[] { "+49" }),
                new ISO3166Country("Ghana", "GH", "GHA", 9, new[] { "+233" }),
                new ISO3166Country("Gibraltar", "GI", "GIB", -1, new[] { "+350" }),
                new ISO3166Country("Greece", "GR", "GRC", 10, new[] { "+30" }),
                new ISO3166Country("Greenland", "GL", "GRL", 6, new[] { "+299" }),
                new ISO3166Country("Grenada", "GD", "GRD", 10, new[] { "+1" }),
                new ISO3166Country("Guadeloupe", "GP", "GLP", 9, new[] { "+590" }),
                new ISO3166Country("Guam", "GU", "GUM", 7, new[] { "+1 671" }),
                new ISO3166Country("Guatemala", "GT", "GTM", 8, new[] { "+502" }),
                new ISO3166Country("Guernsey", "GG", "GGY", 10, new[] { "+44" }),
                new ISO3166Country("Guinea", "GN", "GIN", -1, new[] { "+224" }),
                new ISO3166Country("Guinea-Bissau", "GW", "GNB", -1, new[] { "+245" }),
                new ISO3166Country("Guyana", "GY", "GUY", -1, new[] { "+592" }),
                new ISO3166Country("Haiti", "HT", "HTI", -1, new[] { "+509" }),
                new ISO3166Country("Norfolk Islands", "HM", "HMD", 6, new[] { "+672" }),
                new ISO3166Country("Honduras", "HN", "HND", 8, new[] { "+504" }),
                new ISO3166Country("Hong Kong", "HK", "HKG", 8, new[] { "+852" }),
                new ISO3166Country("Hungary", "HU", "HUN", 9, new[] { "+36" }),
                new ISO3166Country("Iceland", "IS", "ISL", -1, new[] { "+354" }),
                new ISO3166Country("India", "IN", "IND", 10, new[] { "+91" }),
                new ISO3166Country("Indonesia", "ID", "IDN", -1, new[] { "+62" }),
                new ISO3166Country("Iran", "IR", "IRN", -1, new[] { "+98" }),
                new ISO3166Country("Iraq", "IQ", "IRQ", -1, new[] { "+964" }),
                new ISO3166Country("Ireland", "IE", "IRL", 9, new[] { "+353" }),
                new ISO3166Country("Isle of Man", "IM", "IMN", 10, new[] { "+44" }),
                new ISO3166Country("Israel", "IL", "ISR", 9, new[] { "+972" }),
                new ISO3166Country("Italy", "IT", "ITA", -1, new[] { "+39" }),
                new ISO3166Country("Jamaica", "JM", "JAM", 7, new[] { "+1 876" }),
                new ISO3166Country("Japan", "JP", "JPN", 11, new[] { "+81" }),
                new ISO3166Country("Jersey", "JE", "JEY", 6, new[] { "+44 1534" }),
                new ISO3166Country("Jordan", "JO", "JOR", -1, new[] { "+962" }),
                new ISO3166Country("Kazakhstan", "KZ", "KAZ", 10, new[] { "+7" }),
                new ISO3166Country("Kenya", "KE", "KEN", 10, new[] { "+254" }),
                new ISO3166Country("Kiribati", "KI", "KIR", 8, new[] { "+686" }),
                new ISO3166Country("North Korea", "KP", "PRK", -1, new[] { "+850" }),
                new ISO3166Country("South Korea", "KR", "KOR", -1, new[] { "+82" }),
                new ISO3166Country("Kuwait", "KW", "KWT", 8, new[] { "+965" }),
                new ISO3166Country("Kyrgyzstan", "KG", "KGZ", -1, new[] { "+996" }),
                new ISO3166Country("Laos", "LA", "LAO", -1, new[] { "+856" }),
                new ISO3166Country("Latvia", "LV", "LVA", 8, new[] { "+371" }),
                new ISO3166Country("Lebanon", "LB", "LBN", -1, new[] { "+961" }),
                new ISO3166Country("Lesotho", "LS", "LSO", -1, new[] { "+266" }),
                new ISO3166Country("Liberia", "LR", "LBR", -1, new[] { "+231" }),
                new ISO3166Country("Libya", "LY", "LBY", 10, new[] { "+218" }),
                new ISO3166Country("Liechtenstein", "LI", "LIE", -1, new[] { "+423" }),
                new ISO3166Country("Lithuania", "LT", "LTU", 8, new[] { "+370" }),
                new ISO3166Country("Luxembourg", "LU", "LUX", 9, new[] { "+352" }),
                new ISO3166Country("Macao", "MO", "MAC", -1, new[] { "+853" }),
                new ISO3166Country("North Macedonia", "MK", "MKD", 8, new[] { "+389" }),
                new ISO3166Country("Madagascar", "MG", "MDG", -1, new[] { "+261" }),
                new ISO3166Country("Malawi", "MW", "MWI", -1, new[] { "+265" }),
                new ISO3166Country("Malaysia", "MY", "MYS", 7, new[] { "+60" }),
                new ISO3166Country("Maldives", "MV", "MDV", 7, new[] { "+960" }),
                new ISO3166Country("Mali", "ML", "MLI", -1, new[] { "+223" }),
                new ISO3166Country("Malta", "MT", "MLT", -1, new[] { "+356" }),
                new ISO3166Country("Marshall Islands", "MH", "MHL", 7, new[] { "+692" }),
                new ISO3166Country("Martinique", "MQ", "MTQ", -1, new[] { "+596" }),
                new ISO3166Country("Mauritania", "MR", "MRT", -1, new[] { "+222" }),
                new ISO3166Country("Mauritius", "MU", "MUS", 8, new[] { "+230" }),
                new ISO3166Country("Mayotte", "YT", "MYT", -1, new[] { "+262" }),
                new ISO3166Country("Mexico", "MX", "MEX", 10, new[] { "+52" }),
                new ISO3166Country("Micronesia Federated States of", "FM", "FSM", 7, new[] { "+691" }),
                new ISO3166Country("Moldova", "MD", "MDA", 8, new[] { "+373" }),
                new ISO3166Country("Monaco", "MC", "MCO", -1, new[] { "+377" }),
                new ISO3166Country("Mongolia", "MN", "MNG", 8, new[] { "+976" }),
                new ISO3166Country("Montenegro", "ME", "MNE", 8, new[] { "+382" }),
                new ISO3166Country("Montserrat", "MS", "MSR", 7, new[] { "+1 664" }),
                new ISO3166Country("Morocco", "MA", "MAR", -1, new[] { "+212" }),
                new ISO3166Country("Mozambique", "MZ", "MOZ", -1, new[] { "+258" }),
                new ISO3166Country("Myanmar", "MM", "MMR", -1, new[] { "+95" }),
                new ISO3166Country("Namibia", "NA", "NAM", -1, new[] { "+264" }),
                new ISO3166Country("Nauru", "NR", "NRU", -1, new[] { "+674" }),
                new ISO3166Country("Nepal", "NP", "NPL", 10, new[] { "+977" }),
                new ISO3166Country("Netherlands", "NL", "NLD", 9, new[] { "+31" }),
                new ISO3166Country("New Caledonia", "NC", "NCL", 6, new[] { "+687" }),
                new ISO3166Country("New Zealand", "NZ", "NZL", -1, new[] { "+64" }),
                new ISO3166Country("Nicaragua", "NI", "NIC", 8, new[] { "+505" }),
                new ISO3166Country("Niger", "NE", "NER", 8, new[] { "+227" }),
                new ISO3166Country("Nigeria", "NG", "NGA", 8, new[] { "+234" }),
                new ISO3166Country("Niue", "NU", "NIU", 4, new[] { "+683" }),
                new ISO3166Country("Norfolk Island", "NF", "NFK", 6, new[] { "+672" }),
                new ISO3166Country("Northern Mariana Islands", "MP", "MNP", 10, new[] { "+1 670" }),
                new ISO3166Country("Norway", "NO", "NOR", 8, new[] { "+47" }),
                new ISO3166Country("Oman", "OM", "OMN", -1, new[] { "+968" }),
                new ISO3166Country("Pakistan", "PK", "PAK", 10, new[] { "+92" }),
                new ISO3166Country("Palau", "PW", "PLW", 7, new[] { "+680" }),
                new ISO3166Country("Palestine, State of", "PS", "PSE", 9, new[] { "+970" }),
                new ISO3166Country("Panama", "PA", "PAN", 8, new[] { "+507" }),
                new ISO3166Country("Papua New Guinea", "PG", "PNG", -1, new[] { "+675" }),
                new ISO3166Country("Paraguay", "PY", "PRY", 9, new[] { "+595" }),
                new ISO3166Country("Peru", "PE", "PER", 9, new[] { "+51" }),
                new ISO3166Country("Philippines", "PH", "PHL", 10, new[] { "+63" }),
                new ISO3166Country("Pitcairn", "PN", "PCN", -1, new[] { "+64" }),
                new ISO3166Country("Poland", "PL", "POL", 9, new[] { "+48" }),
                new ISO3166Country("Portugal", "PT", "PRT", 9, new[] { "+351" }),
                new ISO3166Country("Puerto Rico", "PR", "PRI", 10, new[] { "+1"}),
                new ISO3166Country("Qatar", "QA", "QAT", 8, new[] { "+974" }),
                new ISO3166Country("Réunion", "RE", "REU", -1, new[] { "+262" }),
                new ISO3166Country("Romania", "RO", "ROU", 10, new[] { "+40" }),
                new ISO3166Country("Russian Federation", "RU", "RUS", 10, new[] { "+7" }),
                new ISO3166Country("Rwanda", "RW", "RWA", -1, new[] { "+250" }),
                new ISO3166Country("Saint Barthélemy", "BL", "BLM", -1, new[] { "+590" }),
                new ISO3166Country("Saint Helena, Ascension and Tristan da Cunha", "SH", "SHN", 4, new[] { "+290" }),
                new ISO3166Country("Saint Kitts and Nevis", "KN", "KNA", 7, new[] { "+1 869" }),
                new ISO3166Country("Saint Lucia", "LC", "LCA", 7, new[] { "+1 758" }),
                new ISO3166Country("Saint Martin (French part)", "MF", "MAF", -1, new[] { "+590" }),
                new ISO3166Country("Saint Pierre and Miquelon", "PM", "SPM", -1, new[] { "+508" }),
                new ISO3166Country("Saint Vincent and the Grenadines", "VC", "VCT", 7, new[] { "+1 784" }),
                new ISO3166Country("Samoa", "WS", "WSM", 5, new[] { "+685" }),
                new ISO3166Country("San Marino", "SP", "SMR", -1, new[] { "+378" }),
                new ISO3166Country("Sao Tome and Principe", "ST", "STP", -1, new[] { "+239" }),
                new ISO3166Country("Saudi Arabia", "SA", "SAU", 9, new[] { "+966" }),
                new ISO3166Country("Senegal", "SN", "SEN", -1, new[] { "+221" }),
                new ISO3166Country("Serbia", "RS", "SRB", 9, new[] { "+381" }),
                new ISO3166Country("Seychelles", "SC", "SYC", -1, new[] { "+248" }),
                new ISO3166Country("Sierra Leone", "SL", "SLE", -1, new[] { "+232" }),
                new ISO3166Country("Singapore", "SG", "SGP", 8, new[] { "+65" }),
                new ISO3166Country("Sint Maarten", "SX", "SXM", 7, new[] { "+1 721" }),
                new ISO3166Country("Slovakia", "SK", "SVK", 9, new[] { "+421" }),
                new ISO3166Country("Slovenia", "SI", "SVN", -1, new[] { "+386" }),
                new ISO3166Country("Solomon Islands", "SB", "SLB", 7, new[] { "+677" }),
                new ISO3166Country("Somalia", "SO", "SOM", -1, new[] { "+252" }),
                new ISO3166Country("South Africa", "ZA", "ZAF", 9, new[] { "+27" }),
                new ISO3166Country("South Georgia and the South Sandwich Islands", "GS", "SGS", -1, new[] { "+500" }),
                new ISO3166Country("South Sudan", "SS", "SSD", -1, new[] { "+211" }),
                new ISO3166Country("Spain", "ES", "ESP", 9, new[] { "+34" }),
                new ISO3166Country("Sri Lanka", "LK", "LKA", 7, new[] { "+94" }),
                new ISO3166Country("Sudan", "SD", "SDN", -1, new[] { "+249" }),
                new ISO3166Country("Suriname", "SR", "SUR", -1, new[] { "+597" }),
                new ISO3166Country("Svalbard", "SJ", "SJM", -1, new[] { "+47" }),
                new ISO3166Country("Swaziland", "SZ", "SWZ", 8, new[] { "+268" }),
                new ISO3166Country("Sweden", "SE", "SWE", -1, new[] { "+46" }),
                new ISO3166Country("Switzerland", "CH", "CHE", 9, new[] { "+41" }),
                new ISO3166Country("Syria", "SY", "SYR", 9, new[] { "+963" }),
                new ISO3166Country("Taiwan, Province of China[a]", "TW", "TWN", 9, new[] { "+886" }),
                new ISO3166Country("Tajikistan", "TJ", "TJK", -1, new[] { "+992" }),
                new ISO3166Country("Tanzania, United Republic of", "TZ", "TZA", -1, new[] { "+255" }),
                new ISO3166Country("Thailand", "TH", "THA", 9, new[] { "+66" }),
                new ISO3166Country("Timor-Leste", "TL", "TLS", 8, new[] { "+670" }),
                new ISO3166Country("Togo", "TG", "TGO", 8, new[] { "+228" }),
                new ISO3166Country("Tokelau", "TK", "TKL", -1, new[] { "+690" }),
                new ISO3166Country("Tonga", "TO", "TON", -1, new[] { "+676" }),
                new ISO3166Country("Trinidad and Tobago", "TT", "TTO", 7, new[] { "+1 868" }),
                new ISO3166Country("Tunisia", "TN", "TUN", 8, new[] { "+216" }),
                new ISO3166Country("Turkey", "TR", "TUR", 11, new[] { "+90" }),
                new ISO3166Country("Turkmenistan", "TM", "TKM", -1, new[] { "+993" }),
                new ISO3166Country("Turks and Caicos Islands", "TC", "TCA", 7, new[] { "+1 649" }),
                new ISO3166Country("Tuvalu", "TV", "TUV", -1, new[] { "+688" }),
                new ISO3166Country("Uganda", "UG", "UGA", -1, new[] { "+256" }),
                new ISO3166Country("Ukraine", "UA", "UKR", 9, new[] { "+380" }),
                new ISO3166Country("United Arab Emirates", "AE", "ARE", 9, new[] { "+971" }),
                new ISO3166Country("United Kingdom", "GB", "GBR", 10, new[] { "+44" }),
                new ISO3166Country("United States of America", "US", "USA", 10, new[] { "+1" }),
                new ISO3166Country("United States Minor Outlying Islands", "UM", "UMI", 10, new[] { "+1" }),
                new ISO3166Country("Uruguay", "UY", "URY", -1, new[] { "+598" }),
                new ISO3166Country("Uzbekistan", "UZ", "UZB", -1, new[] { "+998" }),
                new ISO3166Country("Vanuatu", "VU", "VUT", -1, new[] { "+678" }),
                new ISO3166Country("Venezuela", "VE", "VEN", 7, new[] { "+58" }),
                new ISO3166Country("Vietnam", "VN", "VNM", 9, new[] { "+84" }),
                new ISO3166Country("Virgin Islands (British)", "VG", "VGB", 7, new[] { "+1 284" }),
                new ISO3166Country("Virgin Islands (U.S.)", "VI", "VIR", 7, new[] { "+1 340" }),
                new ISO3166Country("Wallis and Futuna", "WF", "WLF", -1, new[] { "+681" }),
                new ISO3166Country("Western Sahara", "EH", "ESH", -1, new[] { "+212" }),
                new ISO3166Country("Yemen", "YE", "YEM", 9, new[] { "+967" }),
                new ISO3166Country("Zambia", "ZM", "ZMB", -1, new[] { "+260" }),
                new ISO3166Country("Zimbabwe", "ZW", "ZWE", -1, new[] { "+263" })
             };
    }
    #endregion Country Data Collection
}


#region Location For Country Code
/// <summary>
/// Representation of an ISO3166-1 Country
/// </summary>
[System.Serializable]
public struct CountryData
{
    public string status;
    public string country;
    public string countryCode;
    public string region;
    public string regionName;
    public string city;
    public string zip;
    public double lat;
    public double lon;
    public string timezone;
    public string isp;
    public string org;
    public string _as;
    public string query;
}
[System.Serializable]
public struct ISO3166Country
{
    public ISO3166Country(string name, string alpha2, string alpha3, int numericCode, string[] dialCodes = null)
    {
        Name = name;
        Alpha2 = alpha2;
        Alpha3 = alpha3;
        limit = numericCode;
        DialCodes = dialCodes;
    }

    public string Name;
    public string Alpha2;
    public string Alpha3;
    public int limit;
    public string[] DialCodes;
}


#endregion
