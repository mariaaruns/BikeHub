namespace BikeHub.DapperQuery
{
    public class EmailTemplateQuery
    {
        //Order-Ready-Delivery
        //Bike-Repair-Delivery
        public const string EmailTemplate= @"select top 1 Subject,HtmlBody from messaging.emailTemplates where Slug=@slugName";
    }
}
