namespace BikeHub.DapperQuery
{
    public class OrderQuery
    {

        public const string GetOrderList = @"select t1.order_id OrderId,
                                            t2.first_name + ' ' + t2.last_name CustomerName,
                                            (select value from t000_lookup where Id=t1.order_status) as [Status],
                                            t3.cartCount,t3.TotalPrice [TotalAmount],
                                            (case when Image is null or Image=''
                                            then 'man.png' else T2.Image end) as Image
                                            from sales.orders t1
                                            left join sales.customers t2 on
                                            t1.customer_id= t2.customer_id
                                            inner join (select oi.order_id,count(oi.order_id)[cartCount] ,sum(Convert(Decimal(18,2),oi.list_price)) [TotalPrice] from sales.order_items oi  group By oi.order_id) t3 on
                                            t3.order_id=t1.order_id
                                            WHERE order_date >= CAST(@OrderDate + '-01' AS DATE)
AND order_date < DATEADD(MONTH,1, CAST(@OrderDate + '-01' AS DATE)) and 
                                            t1.order_status=@OrderStatus and (@OrderId IS NULL OR @OrderId=0 OR t1.order_id=@OrderId)
                                            order by order_date desc , t1.order_id desc
                                            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

        public const string GetOrderListCount = @"select count(1)
                                                from sales.orders t1
                                                left join sales.customers t2 on
                                                t1.customer_id= t2.customer_id
                                                inner join (select oi.order_id,count(oi.order_id)[cartCount] 
                                                ,sum(Convert(Decimal(18,2),oi.list_price)) [TotalPrice] from sales.order_items oi  group By oi.order_id) t3 on
                                                t3.order_id=t1.order_id
                                                 WHERE order_date >= CAST(@OrderDate + '-01' AS DATE)
                                                 AND order_date < DATEADD(MONTH,1, CAST(@OrderDate + '-01' AS DATE)) and 
                                                t1.order_status=@OrderStatus and (@OrderId IS NULL OR @OrderId=0 OR t1.order_id=@OrderId)";

        public const string GetOrderDetailByOrderId = @"select t1.order_id OrderId,
                                                t1.order_status OrderStatus,
                                                t1.customer_id CustomerId,
                                                t1.order_date OrderDate,
                                                t1.required_date RequiredDate,
                                                t1.shipped_date ShippedDate,
                                                t1.store_id StoreId,
                                                t1.staff_id StaffId,
                                                t2.first_name + ' ' + t2.last_name CustomerName,
                                                t2.email Email,
                                                t2.phone Phone,
                                                t2.Image Image,
                                                t3.item_id ItemId,
                                                t3.product_id ProductId,
                                                t3.quantity Quantity,
                                                t3.list_price ListPrice,
                                                t4.product_name ProductName
                                                from sales.orders t1
                                                left join sales.customers t2 on
                                                t1.customer_id= t2.customer_id
                                                inner join sales.order_items t3 on
                                                t1.order_id=t3.order_id
                                                inner join production.products t4 on
                                                t3.product_id=t4.product_id
                                                where t1.order_id=@OrderId";
        
        public const string UpdateOrderStatus = @"update sales.orders
                                            set order_status=@OrderStatus
                                            where order_id=@OrderId";
        
        public const string GetOrderStatusLookup = @"
                            select Id [Value],Value [Text] from t000_lookup where LookupName='OrderStatus'";

        public const string AddOrder = @"INSERT INTO sales.orders
                                         (customer_id,order_status,order_date,required_date,shipped_date,store_id,staff_id)
                                         VALUES (@CustomerId,@OrderStatus,@OrderDate,@RequiredDate,@ShippedDate,1,@StaffId);
                                         SELECT CAST(SCOPE_IDENTITY() as int);";

        public const string AddOrderItems = @"INSERT INTO sales.order_items
                                                (order_id, item_id, product_id, quantity, list_price, discount)
                                                VALUES
                                                (@OrderId, @ItemId, @ProductId, @Quantity, @ListPrice, @Discount);";

    }

}
