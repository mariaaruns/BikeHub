namespace BikeHub.DapperQuery
{
    public class CustomerQuery
    {
        public const string TotalCustomerCount =@"select
                                                  count(1)
                                                  from sales.customers 
                                                  where isActive=1 
                                                  and (@customerName IS NULL OR @customerName = '' OR first_name like @customerName)";
       
        public const string GetAllCustomer = @"select
                                               customer_id [CustomerId],
                                               first_name+' '+last_name [CustomerName],
                                               first_name [FirstName],
                                               last_name [LastName],
                                               phone [Phone],
                                               email [Email],
                                               street [Street],
                                               city [City],
                                               state [State],
                                               zip_code [ZipCode],
                                            (case when  image is null or image ='' then 'man.png' 
                                             else image end ) as [Image]
                                               from sales.customers 
                                               where isActive=1 and
                                               (@customerName IS NULL OR @customerName = '' OR first_name like @customerName)
                                               order by first_name asc
                                               offset @offset rows fetch next @pageSize rows only;";

        public const string GetCustomerById = @"select top 1    customer_id [CustomerId],
                                               first_name+' '+last_name [CustomerName],
                                               first_name [FirstName],
                                               last_name [LastName],
                                               phone [Phone],
                                               email [Email],
                                               street [Street],
                                               city [City],
                                               state [State],
                                               zip_code [ZipCode],
                                               (case when  image is null or image ='' then 'man.png' 
                                             else image end )as [Image] from sales.customers 
                                               where customer_id=@Id;";

        public const string AddCustomer = @"insert into sales.customers 
                                     (first_name,last_name,phone,email,street,city,state,zip_code,image,isActive)  
                                     values (@firstName,@LastName,@phone,@email,@street,@city,@state,@zipCode,@Image,1);";

        public const string UpdateCustomer = @"update sales.customers 
                                               set first_name=@FirstName,
                                               last_name =@LastName,
                                               phone=@PhoneNo,
                                               email=@MailId,
                                               street=@Street,
                                               city=@City,
                                               state=@State,
                                               zip_code=@ZipCode,
                                               image=@image 
                                               where customer_id = @CustomerId;";

        public const string DesActiveCustomer = @"update sales.customers 
                                                  set IsActive = 0 
                                                  where customer_id=@Id;";
 
        public const string GetCustomerDropdown = @"select 
                                                    customer_id as Value,
                                                    first_name+' '+last_name as Text
                                                    from sales.customers 
                                                    where isActive=1 and (@search is null or first_name  like @search)
                                                    order by first_name asc;";

    }
}
