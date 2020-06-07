using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Enum
{
   
    public enum ActionType
    {   
        
      
        View = 1,
        Insert = 2,
        Update = 3,
        Delete = 4,
        Login = 5,
        Admin = 6,
        Department = 7,
        Report = 8,
        Password = 9,
        HeadDepartment = 10,
        SRVHeadDepartment = 11,
        HeadDept = 12,
        HeadDepartmentProject = 13,
        CommissaryManager = 14,
        ViewEvaluation = 15,
        ProjectDonator = 16,
        MoveWorkFlow = 17,
        AllBranches = 18,
        Logout = 19,
    }


    public enum AppSettingEnum
    {
        CameraQuality = 1,
        MaxUploadHeight = 2,
        MaxUploadWidth = 3,
        MaxThumbWidth = 4,
        MaxThumbHeight = 5,
        HasNotification = 6,
        MaxSearchDistance =7,
        MaxSearchDuration =8,
        SearchStep = 9,
        LastUpdatedDatetimeRange =10,
        NoOfKiloGrams=11,
        CostOfKilograms =12,
        NoOfKiloMeters=13,
        CostOfKiloMeters=14,
        IntialTripCost = 15,
        MaxDriverNotificationDurationlimit	= 16
    }


    public enum ShipmentStatusEnum
    {
        SearchingForDriver = 1,
        ApprovedByDriver = 2,
        OnTrip = 3,
        Delivered = 4,
        CancelledByMerchant = 5,
        NoDriversFound = 6,
        CancelledByDriver = 7
    }

    public enum ShipmentAttachmentTypeEnum
    {
        ShipmentImage=1
    }

    public enum NotificationTypeEnum
    {
        DriversFound=1, //sent to the merchant when drivers are found
        ShipmentRequest=2 ,//sent to the driver when drivers are found with shipment details
        ShipmentRequestApproved = 3,
        ShipmentPickedUp = 4,
        ShipmentDelivered = 5,
        ShipmentCancelled = 6,
        DriverLocationUpdate = 7,
        DriverArriving = 8,
        DriverPreArriving = 9,
        NoDriversFound=10

    }


    public enum ReceiverTypeEnum
    {
        Merchant=1,
        Driver=2
    }

   

 



  
}
