using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace PrepersSupplies.Models
{
    public class Product
    {
    }

    public class ProductResponse
    {
        [JsonPropertyName("product")]
        public ProductInfo? Product { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }
    }

    public class ProductInfo
    {
        [JsonPropertyName("product_name")]
        public string? ProductName { get; set; }
    }
}
