using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CurrencyApp
{
    public class CurrencyService
    {
        public async Task<IEnumerable<Currency>> GetAllAsync()
        {
            string strDoc = await GetFormWebAsync();
            if (strDoc == null)
                return null;
            List<Currency> list = new List<Currency>();
            var doc = XDocument.Parse(strDoc);

            foreach (XElement item in doc.Root.Elements())
            {
                Currency currency = new Currency
                {
                    Name = item.Element("CurrencyName").Value,
                    Code = item.Attribute("CurrencyCode").Value,
                    Buying = item.Element("ForexBuying").Value,
                    Selling = item.Element("ForexSelling").Value,
                };
                currency.FlagURL = "https://www.tcmb.gov.tr/kurlar/kurlar_tr_dosyalar/images/" + currency.Code + ".gif";
                list.Add(currency);
            }
            return list;
        }
        public async Task<string> GetFormWebAsync()
        {
            const string url = "https://www.tcmb.gov.tr/kurlar/today.xml";
            HttpClient client = new HttpClient();
            var responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var stringDoc = await responseMessage.Content.ReadAsStringAsync();
                return stringDoc;
            }
            return null;
        }
    }
}
