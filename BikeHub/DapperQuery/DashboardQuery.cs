namespace BikeHub.DapperQuery
{
    public class DashboardQuery
    {
        public const string TotalProductsCount= @"select (select count(1) from production.products) As TotalProductsCount,
                                                  (select count(1) from sales.orders where Month(order_date) = @month and year(order_date) = @year
                                                  and  store_id = 1 )as TotalOrdersCount,
                                                  (select  14) as TotalServiceCount,
                                                  (select 7) as  PendingServiceCount,
                                                  (select 7) as  CompletedServiceCount";

        public const string DashBoardSalesChart = @"SELECT 
                                                    FORMAT(t1.order_date, 'MMM') AS Month,    
                                                    SUM(t2.list_price) AS NetAmount
                                                    FROM sales.orders t1
                                                    LEFT JOIN sales.order_items t2 
                                                    ON t1.order_id = t2.order_id
                                                    where year(order_date)=@year
                                                    and  t1.store_id=1
                                                    GROUP BY 
                                                    MONTH(t1.order_date),
                                                    FORMAT(t1.order_date, 'MMM')
                                                    ORDER BY 
                                                    MONTH(t1.order_date);";

        public const string BrandYearlySales = @"
                   select tab.brand_id,tab.brand_name[Brand],Sum(FinalPrice) [NetAmount]
                    from(
                            select   t1.brand_id,t1.brand_name ,t4.list_price - (t4.list_price * t4.discount) AS FinalPrice 
                            from production.brands t1
                            inner join production.products t2 on t1.brand_id=t2.brand_id
                            inner join sales.order_items t4 on t4.product_id =t2.product_id
                            inner join sales.orders t3 on t3.order_id =t4.order_id
                            where year(t3.order_date )=@year and t3.order_status=@orderStatus and t3.store_id=1
                        ) tab
                   group by tab.brand_id,tab.brand_name  
                   ORDER BY [NetAmount] DESC";
    }
}
