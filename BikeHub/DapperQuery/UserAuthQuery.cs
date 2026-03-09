using Microsoft.AspNetCore.Http;

namespace BikeHub.DapperQuery
{
    public class UserAuthQuery
    {
        public const string GetUserByUserName = @"SELECT * FROM auth.Users WHERE Username = @Username";


        public const string CreateUser = @"INSERT INTO auth.Users (Username, Password,RoleId, FirstName,LastName,PhoneNumber,Image) 
                                            VALUES (@Username, @PasswordHash, @Role,@FirstName,@LastName,@PhoneNumber,@Image)";

        

        public const string GetUserRole = @"SELECT r.RoleName FROM auth.Users u
                                                JOIN auth.Roles r ON u.RoleId = r.RoleId
                                                WHERE u.Username = @Username";


        public const string GetUserById = @"SELECT * FROM auth.Users WHERE UserId = @UserId";


        public const string UpdateUser = @"UPDATE auth.Users SET                 RoleId =  CASE WHEN @RoleId IS NOT NULL THEN @RoleId ELSE RoleId END, 
                                                                                 FirstName = CASE WHEN @FirstName IS NOT NULL THEN @FirstName ELSE FirstName END, 
                                                                                 LastName =CASE WHEN @LastName IS NOT NULL THEN @LastName ELSE LastName END, 
                                                                                 PhoneNumber = CASE WHEN  @PhoneNumber IS NOT NULL THEN @PhoneNumber ELSE PhoneNumber END   
                                                                                 WHERE UserId = @UserId";


        public const string DeleteUser = @"DELETE FROM auth.Users WHERE UserId = @UserId";

        //public const string GetAllUsers = @"SELECT u.UserId, u.Username, r.RoleName, u.FirstName, u.LastName, u.PhoneNumber 
        //                                    FROM auth.Users u
        //                                    JOIN auth.Roles r ON u.RoleId = r.RoleId";

        public const string GetAllRoles = @"SELECT RoleId [Value],RoleName [Text] FROM auth.Roles";

        public const string GetUserMenuByUserId = @"
                                                            SELECT m.*
                                                            FROM  auth.Menus m
                                                            JOIN auth.MenuPolicies mp ON m.MenuId = mp.MenuId
                                                            JOIN auth.UserPolicies up ON mp.PolicyId = up.PolicyId
                                                            WHERE up.UserId = @UserId ORDER BY SortOrder";



        public const string GetAllPolicies = @"SELECT * FROM auth.Policies";


        public const string GetPoliciesByUserId = @"  SELECT p.*, case When up.UserId is not null Then 1 else 0 end as HasPermission 
                                                FROM auth.Policies p
                                                Left join (select * from auth.UserPolicies where Isactive = 1 and UserId=@UserId) up ON p.PolicyId = up.PolicyId
                                                Order By PolicyId";


        public const string AddOrRemovePolicyForUser = @"IF EXISTS (SELECT 1 FROM auth.UserPolicies WHERE UserId = @UserId AND PolicyId = @PolicyId)
                                                        UPDATE auth.UserPolicies SET isActive = @IsActive
                                                        WHERE UserId = @UserId AND PolicyId = @PolicyId
                                                    ELSE
                                                        INSERT INTO auth.UserPolicies (UserId, PolicyId,isActive) VALUES (@UserId, @PolicyId , @IsActive)";

        public const string GettingUserInfo = @"select t1.UserId
                                                ,t1.FirstName
                                                ,t1.LastName
                                                ,t1.UserName
                                                ,t1.Password
                                                ,t1.Image
                                                ,t2.RoleName [RoleName]
                                                from [auth].Users t1
                                                Inner join auth.Roles t2 on
                                                t1.RoleId=t2.RoleId
                                                where UserName =@Email 
                                                and t1.isActive=1";

        public const string AddNewPolicy = @"INSERT INTO auth.Policies (Code, Description) VALUES (@Code, @Description)";

        public const string UpdatePolicy = @"UPDATE auth.Policies SET 
                        Code = case when @Code is null Then Code Else @Code End,
                        Description = case when @Description is null Then Description Else @Description End 
                        WHERE PolicyId = @PolicyId";


        public const string DeletePolicy = @"DELETE FROM auth.Policies WHERE PolicyId = @PolicyId";

        public const string TotalUsersCount = @"SELECT COUNT(1) FROM auth.Users u  inner join auth.Roles r  
                                                on u.RoleId=r.RoleId  where IsActive=1
                                                and   (@Search is null or @Search='' or FirstName like @Search)
                                                and (@Role  is null or @Role ='' or u.RoleId = @Role)";


        public const string GetAllUsers = @"select UserId,FirstName,LastName,UserName,PhoneNumber,Image,RoleName
                                            from auth.Users u
                                            inner join auth.Roles r  
                                            on u.RoleId=r.RoleId where isActive=1 and
                                            (@Search is null or @Search='' or FirstName like @Search)
                                            and (@Role  is null or @Role ='' or u.RoleId = @Role)
                                            order by userId desc
                                            OFFSET  @Offset rows fetch Next @PageSize rows only";
    }
}
