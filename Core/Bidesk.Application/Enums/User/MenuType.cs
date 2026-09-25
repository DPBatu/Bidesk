namespace Bidesk.Application.Enums.User
{
    public enum MenuType
    {
        //Common
        ApplicationServices,
        Users,
        Roles,
        File,
        Notification,

        //EMS
        EMSGeneric,
        Document,
        DocumentType,
        Process,
        ProcessType,
        TargetType,
        Target,
        ComplaintType,
        Complaint,
        ReturnSource,
        NotificationForm,
        Action,

        //SUGGESTION
        SuggestionPost,
        SuggestionGet,
        AssignmentPost,
        AssignmentGet,
        AssessmentPost,
        AssessmentGet,
        ActionPost,
        ActionGet,
        ItemsPost,
        ItemsGet,
        AttachmentPost,
        AttachmentGet,
        Payments,
        TransferTask,
        DashboardInfo,
        Reports,

        //HELPDESK
        TalepAdmin,

        //MES
        Durus,
        Hata,
        Kalip,
        Personel,
        Teyit,
        ProductionLine,
        WorkCenter,
        Skk,

        //BIDESK
        BideskTicket,
        BideskTechnician,
        BideskAdmin,

        //MBO
        CustomerGroup,
        PortalPoint,
        CustomerComplaint,
    }
}
