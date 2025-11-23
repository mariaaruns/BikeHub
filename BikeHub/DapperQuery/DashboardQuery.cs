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
    }
}
