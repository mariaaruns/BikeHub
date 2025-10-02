using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Response;
using Microsoft.Data.SqlClient;

namespace BikeHub.DapperQuery
{
    public class ProductQuery
    {
        public const string TotalProductsCount= @"select count(1)
                                                      from production.products t1
                                                      inner join production.categories t2 on t1.category_id = t2.category_id
                                                      inner join production.stocks t3 on t1.product_id = t3.product_id
                                                      where (@Search IS NULL OR @Search = '' OR t1.product_name like @Search)
                                                      and t3.store_id=1;"
                                                      
                                                      ;
        public const string  GetProducts= @"select t1.product_id [ProductId],
                                                      t1.product_name [ProductName],
                                                      t3.quantity [Stock],
                                                      t2.category_name [CategoryName],
                                                      t1.product_image [ProductImage],
                                                      t1.list_price [Price]
                                                      from production.products t1
                                                      inner join production.categories t2 on t1.category_id = t2.category_id
                                                      inner join production.stocks t3 on t1.product_id = t3.product_id
                                                      where (@Search IS NULL OR @Search = '' OR t1.product_name like @Search)
                                                      and t3.store_id=1
                                                      order by t1.CreatedAt desc
                                                      OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";
    }
}





