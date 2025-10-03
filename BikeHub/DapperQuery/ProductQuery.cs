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
                                                      and t3.store_id=1 and isactive=1
                                                      order by t1.CreatedAt desc
                                                      OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";


        public const string CreateProduct = @"insert into production.products
                                               values(@productName,@brandId,@categoryId,@modelyear,@listprice,@productImage,getdate());select SCOPE_IDENTITY();";

        public const string AddProductStock = @"insert into production.stocks values(1,@productId,@stockQty)";

        public const string GetProduct = @"select t1.product_id[ProductId],
                                            t1.brand_id[BrandId],
                                            t1.category_id[categoryId],
                                            t1.product_name[ProductName],
                                            t1.model_year[ModelYear],
                                            t1.list_price[Price],
                                            t1.product_image[ProductImage],
                                            t2.quantity[Stock]
                                            from production.products t1
                                            inner join production.stocks t2 on t1.product_id=t2.product_id
                                            where t2.store_id=1 and t1.product_id=@id";

        public const string updateProduct = @"update production.products set product_name=@productName,
                                                            brand_id=@brandId,
                                                            category_id=@categoryId,
                                                            model_year=@modelyear,
                                                            list_price=@listprice,
                                                            product_image=@productImage
                                                            where product_id=@productId";

        public const string updateProductStock = @"update production.stocks set quantity =@stockqty
                                                   where product_id=@productId and store_id=1";

        public const string deactivateProduct = @"update production.products set isactive=0 where product_id=@id";


    }

}





