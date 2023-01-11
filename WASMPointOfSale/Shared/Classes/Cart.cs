using WASMPointOfSale.Shared.ViewModels;

namespace WASMPointOfSale.Shared.Classes
{
    public class Cart
    {
        public List<CartLine> Lines { get; set; }

        public Cart()
        {
            Lines = new();
            DiscountAmount = 0;
        }

        public int Quantity
        {
            get
            {
                return Lines.Sum(l => l.Quantity);
            }
        }

        public decimal Tax
        {
            get
            {
                return Lines.Sum(l => l.Tax);
            }
        }

        public decimal Total
        {
            get
            {
                return Lines.Sum(l => l.Total);
            }
        }

        public decimal DiscountAmount { get; set; }

        public decimal Due
        {
            get
            {
                if (DiscountAmount == 0)
                {
                    return Total;
                }
                else
                {
                    return Total - Total * DiscountAmount / 100;
                }
            }
        }

        public void AddToCart(ProductViewModel product)
        {
            var existingLine = GetCartLine(product.Code);

            if (existingLine != null)
            {
                existingLine.Quantity++;
            }
            else
            {
                Lines.Add(new CartLine
                {
                    Product = product,
                    Quantity = 1
                });
            }
        }

        public void RemoveFromCart(ProductViewModel product)
        {
            var existingLine = GetCartLine(product.Code);

            if (existingLine!.Quantity == 1)
            {
                Lines.Remove(existingLine);
            }
            else
            {
                existingLine.Quantity--;
            }
        }

        private CartLine? GetCartLine(string code)
        {
            return Lines.FirstOrDefault(l => l.Product.Code == code);
        }
    }

    public class CartLine
    {
        public ProductViewModel Product { get; set; }

        public int Quantity { get; set; }

        public decimal Total
        {
            get
            {
                return Product.Price * Quantity;
            }
        }

        public decimal Tax
        {
            get
            {
                return Product.Tax * Quantity;
            }
        }
    }
}
