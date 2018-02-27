using System.Collections.Generic;
using System.Runtime.Serialization;

namespace K2DemoBot
{
    /// <summary>
    /// {
    ///  "odata.metadata":"https://denallixtecheu.onk2.com/api/odata/v3/$metadata#com_K2_App_DiscountApprovalApp_SmartObject_ProductName",
    ///  "odata.count":"1",
    ///  "value":[
    ///    {
    ///      "ProductNameID":"0faef59d-70e6-4cad-9483-1119ebb94da6",
    ///      "Title":"K2 Cloud",
    ///      "Description":"Our PaaS offering, hassle-free, enterprise cloud automation",
    ///      "IsDeleted":false,
    ///      "SortBy":"1",
    ///      "List_Price":"15000.00"
    ///    }
    ///  ]
    /// }
    /// 
    /// </summary>
    /// 

    [DataContract]
    class ODataRespose
    {
        [DataMember(Name = "odata.metadata")]
        public string metadata { get; set; }

        [DataMember(Name = "odata.count")]
        public int count { get; set; }

        [DataMember(Name = "value")]
        public List<com_K2_App_DiscountApprovalApp_SmartObject_ProductName> Products { get; set; }

    }

    [DataContract]
    class com_K2_App_DiscountApprovalApp_SmartObject_ProductName
    {
        [DataMember(Name ="ProductNameID")]
        public string ProductNameID { get; set; }

        [DataMember(Name = "Title")]
        public string Title { get; set; }

        [DataMember(Name = "Description")]
        public string Description { get; set; }

        [DataMember(Name = "IsDeleted")]
        public bool IsDeleted { get; set; }

        [DataMember(Name = "SortBy")]
        public int SortBy { get; set; }

        [DataMember(Name = "List_Price")]
        public double List_Price { get; set; }
    }
}
