using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using Newtonsoft.Json;
using WpfApp1.Models;

namespace WpfApp1.Services
{
    public class JSONDataService
    {
        private static readonly string filePath = "C:/Users/PC/Documents/GitHub/PriceCheckerApp--WPF/WpfApp1/Data/Product.json";
        public static List<ProductInDB> JSONokuma()
        {
            if (!File.Exists(filePath))
            {
                return new List<ProductInDB>();
            }
            string json = File.ReadAllText(filePath);
            List<ProductInDB> products = JsonConvert.DeserializeObject<List<ProductInDB>>(json);
            return products;

        }
        public static void JSONguncelleme(List<ProductInDB> products)
        {
            
            List<ProductInDB> TempProducts =  JSONokuma();

            TempProducts.AddRange(products);
            var updatedJson = JsonConvert.SerializeObject(TempProducts,Formatting.Indented);
            
            File.WriteAllText(filePath, updatedJson);
        }
        public static void JSONVeriSilme(ProductInDB product)
        {
            List<ProductInDB> tempProducts = JSONokuma();

            var itemToRemove = tempProducts.FirstOrDefault(p => p.Link == product.Link);

            if (itemToRemove != null)
            {
                tempProducts.Remove(itemToRemove);

                var updatedJson = JsonConvert.SerializeObject(tempProducts, Formatting.Indented);
                File.WriteAllText(filePath, updatedJson);
            }
        }
    }
}
