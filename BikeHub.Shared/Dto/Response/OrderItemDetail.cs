namespace BikeHub.Shared.Dto.Response
{
    public class OrderItemDetail
    {
        public int ItemId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal ListPrice { get; set; }
        public decimal Discount { get; set; }
        public string ProductName { get; set; }
    }
}