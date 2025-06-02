using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.Services
{
    public class PriceCheckerService
    {
        private static readonly HttpClient client = new HttpClient();
        static PriceCheckerService()
        {
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
        }

        static async Task<string> SiteyeIstekdeBulun(string url)
        {
            try
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                return "Hata";
            }

        }
        static async Task<string> HtmlBilgileriniParcalayarakFiyatCek(string url)
        {
            HtmlNode fiyatElementi;
            string fiyat = "-1";

            string urlTemp = url;
            urlTemp = urlTemp.Replace("https://", "").TrimStart().Split('/')[0]; 

            string response = await SiteyeIstekdeBulun(url);

            if (response == null || response == "Hata")
            {
                return fiyat;
            }
            else
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(response);

                switch (urlTemp)
                {
                    case "www.fashfed.com":
                        fiyatElementi = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'price')]/span[contains(@class, 'price__new')]");
                        if (fiyatElementi != null)
                        {
                            fiyat = fiyatElementi.InnerText.Trim();
                            fiyat = fiyat.Replace("₺ ", "").Replace(".", "").Replace(",00", "");
                        }
                        break;
                    case "www.nike.com":
                        fiyatElementi = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'mb4-sm mb8-lg')]/div/span[contains(@class, 'nds-text mr2-sm')]");
                        if (fiyatElementi != null)
                        {
                            fiyat = fiyatElementi.InnerText.Trim();
                            fiyat = fiyat.Replace("₺", "").Replace(".", "").Split(',')[0];
                        }
                        break;
                    case "www.superstep.com.tr":
                        fiyatElementi = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'flex')]/div/span[contains(@class, 'text-lg leading-6 md:leading-7 font-bold h-full inline-block')]");
                        if (fiyatElementi != null)
                        {
                            fiyat = fiyatElementi.InnerText.Trim();
                            fiyat = fiyat.Replace(".", "").Replace(" TL", "");
                        }
                        break;
                    case "kaptanspor.com.tr":
                        fiyatElementi = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'tf-product-info-price')]//div[contains(@class, 'price-on-sale')]");
                        if (fiyatElementi != null)
                        {
                            fiyat = fiyatElementi.InnerText.Trim();
                            fiyat = fiyat.Replace(",", "").Split('.')[0];
                        }
                        break;

                }
                return fiyat;
            }
        }
        static async Task<ProductInDB> GuncelFiyatBilgisiniCek(ProductInDB product)
        {
            product.GuncelFiyat = Int32.Parse(await HtmlBilgileriniParcalayarakFiyatCek(product.Link));

            if (product.GuncelFiyat == -1)
            {
                product.Durum = "Fiyat bilgisi bulunamadı";
            }
            else
            {
                if (product.GuncelFiyat < product.EklenirkenkiFiyat)
                {
                    product.Durum = "İNDİRİMDE";
                }
                else if (product.GuncelFiyat == product.EklenirkenkiFiyat)
                {
                    product.Durum = "--------";
                }
                else
                {
                    product.Durum = "Fiyatı artmış";
                }
            }
            return product;
        }
        public static async Task<List<ProductInDB>> TumFiyatlariKontrolEtAsync(List<ProductInDB> urunler)
        {
            var tasks = urunler.Select(urun => GuncelFiyatBilgisiniCek(urun));
            var guncellenmisUrunler = await Task.WhenAll(tasks);
            return guncellenmisUrunler.ToList();
        }

    }
}
