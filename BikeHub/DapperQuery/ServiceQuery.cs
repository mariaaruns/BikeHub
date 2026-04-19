using System.Net.NetworkInformation;

namespace BikeHub.DapperQuery
{
    public class ServiceQuery
    {
        public const string GetMechanicLiveStats = @"
                                        select count(1) [TotalMechanics] from auth.Users  where  RoleId=2;
                                        
                                        select count(1) [JobsToday] from service.service_jobs where cast(CreatedDate as date)= cast(GETDATE() as date);
                                        
                                        select  count(distinct t2.MechanicId) BusyMechanics from service.service_jobs t1
                                        inner join Service.service_job_assignments t2
                                        on t1.ServiceJobId=t2.ServiceJobId
                                        where t1.ServiceStatus not in (1007,1008);
                                        
                                        SELECT COUNT(1) AvailableMechanics
                                        FROM auth.Users u
                                        WHERE u.RoleId = 2
                                        AND NOT EXISTS
                                        (
                                            SELECT 1
                                            FROM Service.service_job_assignments a
                                            JOIN Service.service_jobs j 
                                                ON a.ServiceJobId = j.ServiceJobId
                                            WHERE a.MechanicId = u.UserId
                                            AND j.ServiceStatus NOT IN (1005,1006)
                                        );
                                        
                                        
                                        SELECT COUNT(1) AS OverloadMechanics
                                        FROM (
                                            SELECT t2.MechanicId
                                            FROM service.service_job_assignments t2
                                            JOIN Service.service_jobs t1 
                                                ON t1.ServiceJobId = t2.ServiceJobId
                                            WHERE t1.ServiceStatus NOT IN (1007,1008)
                                            GROUP BY t2.MechanicId
                                            HAVING COUNT(*) > 5
                                        ) tab;";

        public const string GetMechanicCurrentStatus = @"Select tab.UserId  [MechanicId],
                                                                tab.Mechanic,
                                                                tab.Pending,
                                                                tab.Active,
                                                                tab.Done,
                                                            CASE 
                                                                WHEN (Pending + Active) * 20 > 100 THEN 100
                                                                ELSE (Pending + Active) * 20
                                                            END AS  'WorkLoad',
                                                            
                                                            CASE 
                                                                WHEN Active >= 1 
                                                                THEN CONCAT(cj.customerName,'~',cj.JobCardNumber,'~',cj.BikeModel)
                                                                ELSE NULL
                                                            END AS [CurrentJob]
                                                            
                                                            from (SELECT 
                                                                t1.UserId,
                                                                CONCAT(t1.FirstName,' ',t1.LastName) AS Mechanic,
                                                            
                                                                SUM(CASE WHEN t3.ServiceStatus = 1005 THEN 1 ELSE 0 END) AS Pending,
                                                                SUM(CASE WHEN t3.ServiceStatus = 1006 THEN 1 ELSE 0 END) AS Active,
                                                            
                                                                SUM(CASE 
                                                                    WHEN t3.ServiceStatus = 1006
                                                                    AND t3.CompletedDate >= CAST(GETDATE() AS DATE)
                                                                    AND t3.CompletedDate < DATEADD(DAY,1,CAST(GETDATE() AS DATE))
                                                                    THEN 1 ELSE 0 END) AS Done
                                                            
                                                                    
                                                            FROM auth.Users t1
                                                            LEFT JOIN service.service_job_assignments t2 
                                                                ON t1.UserId = t2.MechanicId
                                                            LEFT JOIN Service.service_jobs t3 
                                                                ON t2.ServiceJobId = t3.ServiceJobId
                                                            
                                                            WHERE t1.RoleId = 2
                                                            
                                                            GROUP BY t1.UserId, t1.FirstName, t1.LastName)
                                                            tab 
                                                            OUTER APPLY
                                                            (
                                                                SELECT top 1  CONCAT(c.first_name,' ',c.last_name) customerName,j.BikeModel,j.JobCardNumber
                                                                FROM service.service_job_assignments a
                                                                JOIN service.service_jobs j 
                                                                    ON j.ServiceJobId = a.ServiceJobId
                                                                    Join sales.customers c on c.customer_id=j.CustomerId
                                                                WHERE a.MechanicId = tab.UserId
                                                                AND j.ServiceStatus = 1006
                                                            ) cj";

        public const string GetTodayJobFeed = @"
                                                select 
                                                 t1.ServiceJobId,
                                                 t1.JobCardNumber [JobId], 
                                                 concat(t4.FirstName,' ',t4.LastName) [Mechanic], 
                                                 t1.ProblemDescription [Service],
                                                 concat(t3.first_name,' ',t3.last_name) [CustomerName],
                                                 t000.Value [Status],
                                                 cast(t1.CreatedDate  as date)[CreatedAt]
                                                from Service.service_jobs t1
                                                inner join Service.service_job_assignments t2
                                                on t1.ServiceJobId=t2.ServiceJobId
                                                inner join sales.customers t3 on t3.customer_id=t1.CustomerId
                                                inner join auth.Users t4 on t4.UserId=t2.MechanicId
                                                inner join t000_lookUp t000 on t000.Id =t1.ServiceStatus
                                                where ( cast(CreatedDate as date) = cast(GETDATE() as date)  or ServiceStatus in (1005,1006)
                                                or cast(t1.CompletedDate as date)  = cast(getDate() as date)) and 
                                                (@serviceStatus is null or @serviceStatus <> '' or ServiceStatus =@serviceStatus) ";

        public const string GetJobDetails = @" select 
                                             t1.ServiceJobId,
                                             t1.JobCardNumber [JobId], 
                                             concat(t4.FirstName,' ',t4.LastName) [Mechanic], 
                                             t1.ProblemDescription [Service],
                                             concat(t3.first_name,' ',t3.last_name) [CustomerName],
                                             t000.Value [Status],
                                             cast(t1.CreatedDate  as date)[CreatedAt],
                                             t1.BikeModel,
                                             t1.BikeNumber,
                                             t1.CompletedDate,
                                             t1.EstimatedCost,
                                             t1.FinalCost,
                                             t2.StartTime,
                                             t2.EstimatedDuration,
                                             t2.ActualDuration,
                                             (select top 1  CONCAT(FirstName,' ',LastName) from auth.users where userId= t1.CreatedBy) CreatedBy,
                                             t2.AssignedDate,
                                             t3.email [Email],
                                             Concat(t3.street,',',t3.city ,',',t3.state,',',t3.zip_code)[Address]
                                            from Service.service_jobs t1
                                            inner join Service.service_job_assignments t2
                                            on t1.ServiceJobId=t2.ServiceJobId
                                            inner join sales.customers t3 on t3.customer_id=t1.CustomerId
                                            inner join auth.Users t4 on t4.UserId=t2.MechanicId
                                            inner join t000_lookUp t000 on t000.Id =t1.ServiceStatus
                                            where t1.ServiceJobId=@serviceJobId";

        public const string GetServiceItemsByServiceJobId = @"select Description,Cost,Quantity,Total from Service.service_items
                                                             where ServiceJobId=@serviceJobId";

        public const string UpdatePendingToInProgressStatus = @" 
                                                    update Service.service_jobs
                                                    set ServiceStatus = 1006
                                                    where ServiceJobId = @serviceJobId;
                                                    --Start time update-- 
                                                    update service.Service_job_Assignments
                                                    set StartTime=GETDATE()
                                                    where ServiceJobId=@serviceJobId   ";

        public const string UpdateInprogressToCompletedStatus = @" 
                                                            update Service.service_jobs
                                                            set ServiceStatus = 1007, CompletedDate=getDate()
                                                            where ServiceJobId = @serviceJobId
                                                            
                                                            UPDATE t SET  ActualDuration = DATEDIFF(MINUTE, StartTime, GETDATE())
                                                            FROM Service.service_job_assignments t
                                                            WHERE ServiceJobId = @serviceJobId;";

        public const string UpdateServiceStatus = @"update Service.service_jobs
                                                    set ServiceStatus = @serviceStatus
                                                    where ServiceJobId = @serviceJobId";
        
        public const string GetMechanicTaskSummary = @"SELECT 
                                                         SUM(CASE WHEN t2.ServiceStatus = 1005 THEN 1 ELSE 0 END) AS Pending,
                                                         SUM(CASE WHEN t2.ServiceStatus = 1006 THEN 1 ELSE 0 END) AS InProgress,
                                                         -- Tracks items completed starting at midnight today
                                                         SUM(CASE WHEN t2.ServiceStatus = 1007 AND t2.CompletedDate >= CAST(GETDATE() AS DATE) THEN 1 ELSE 0 END) AS DoneToday,
                                                         -- Tracks items completed since the 1st of the current month
                                                         SUM(CASE WHEN t2.ServiceStatus = 1007 AND t2.CompletedDate >= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()), 0) THEN 1 ELSE 0 END) AS ThisMonth
                                                         FROM Service.service_job_assignments t1
                                                         INNER JOIN Service.service_jobs t2 ON t1.ServiceJobId = t2.ServiceJobId
                                                         WHERE t1.MechanicId = @MechanicId;";
        
        public const string GetMechanicAssignedJobsQuery = @"select 
                                                            t1.ServiceJobId,
                                                            t1.JobCardNumber
                                                            ,t000.Value [ServiceStatus]
                                                            ,t1.ProblemDescription
                                                            ,t1.BikeModel
                                                            ,concat(t3.first_name ,' ',t3.last_name) [CustomerName]
                                                            ,t1.CreatedDate
                                                            ,t1.CompletedDate
                                                            ,t2.StartTime
                                                            ,t2.EstimatedDuration
                                                            ,t2.ActualDuration
                                                            from Service.service_jobs t1 
                                                            inner join service.service_job_assignments t2 
                                                            on t1.ServiceJobId=t2.ServiceJobId
                                                            inner join sales.customers t3
                                                            on t1.customerid =t3.customer_id
                                                            inner join t000_lookUp t000 on t000.Id =t1.ServiceStatus
                                                            where t2.MechanicId=@MechanicId And
                                                            -- Condition 1: Jobs not yet completed (Pending/In Progress from any date)
                                                                (t1.CompletedDate IS NULL 
                                                                OR 
                                                                -- Condition 2: Jobs completed specifically today
                                                                CAST(t1.CompletedDate AS DATE) = CAST(GETDATE() AS DATE))
                                                        ";
                                                         
        public const string CreateAndAssignJob = @"
                                                    BEGIN TRANSACTION;
                                                    BEGIN TRY
                                                        -- 1. Create the Job
                                                        INSERT INTO service.service_jobs
                                                        (CustomerId, StoreId, BikeModel, BikeNumber, ProblemDescription, ServiceStatus, EstimatedCost, FinalCost, CreatedBy, CreatedDate)
                                                        VALUES (@CustomerId, @StoreId, @BikeModel, @BikeNumber, @ProblemDescription, @ServiceStatus, @EstimatedCost, @FinalCost, @CreatedBy, @CreatedDate);

                                                        DECLARE @NewJobId INT = SCOPE_IDENTITY();

                                                        -- 2. Generate the Formatted Job Number
                                                        UPDATE service.service_jobs 
                                                        SET JobCardNumber = 'JOB-' + CAST(YEAR(GETDATE()) AS VARCHAR(4)) + '-' + RIGHT('000000' + CAST(@NewJobId AS VARCHAR), 6)
                                                        WHERE ServiceJobId = @NewJobId;

                                                        -- 3. Assign the Mechanic
                                                        INSERT INTO Service.service_job_assignments 
                                                        (ServiceJobId, MechanicId, AssignedDate, EstimatedDuration, AssignmentStatus, AssignedBy,StartTime)
                                                        VALUES (@NewJobId, @MechanicId, @AssignedDate, @EstimatedDuration, @AssignmentStatus, @AssignedBy,@StartTime);

                                                        COMMIT TRANSACTION;
                                                    END TRY
                                                    BEGIN CATCH
                                                        ROLLBACK TRANSACTION;
                                                        THROW;
                                                    END CATCH";

        public const string  ServiceStatusDropdown = @"SELECT Id [Value], Value [Text] FROM t000_lookUp WHERE LookupName = 'ServiceStatus'";
    }
}
