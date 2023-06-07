using App.Web.Models.DTS;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Data
{
    public static class Inbound
    {
        public static List<InboundModel> GetLis()
        {
            List<InboundModel> list = new List<InboundModel>();

            list.Add(new InboundModel
            {
                ID = 1,
                AJUNo = "1799",
                MSONo = "7FJZM",
                PONo = "4800009329",
                LoadingPort = "India",
                DischargePort = "Jakarta",
                Model = "120K",
                ModelDescription = "120K/ROPS CAN/V-SCAR/PL631/TL508 14-MP",
                Status = "PLB IN",
                SerialNumber = "SZN10234",
                ETAPort = "15-Feb-2018",
                ETACakung = "20-Feb-2018",
                ATAPort = "15-Feb-2018",
                ATACakung = "20-Feb-2018",
                Notes = "From CCOS sudah di proposed ke CKB 16 Feb 2018",
                Detail = new InboundDetailModel
                {
                    ID = 1,
                    InboundID = 1,
                    RTS_PLAN = "10-Jan-2018",
                    RTS_ACTUAL = "11-Jan-2018",
                    ONBOARDVESSEL_PLAN = "12-Jan-2018",
                    ONBOARDVESSEL_ACTUAL = "13-Jan-2018",
                    PORTIN_PLAN = "14-Jan-2018",
                    PORTIN_ACTUAL = "15-Jan-2018",
                    PORTOUT_PLAN = "16-Jan-2018",
                    PORTOUT_ACTUAL = "17-Jan-2018",
                    PLBIN_PLAN = "18-Jan-2018",
                    PLBIN_ACTUAL = "19-Jan-2018",
                    PLBOUT_PLAN = "20-Jan-2018",
                    PLBOUT_ACTUAL = "21-Jan-2018",
                    YARDIN_PLAN = "22-Jan-2018",
                    YARDIN_ACTUAL = "23-Jan-2018",
                    YARDOUT_PLAN = "24-Jan-2018",
                    YARDOUT_ACTUAL = "25-Jan-2018"
                }
            });
            list.Add(new InboundModel
            {
                ID = 2
                ,
                AJUNo = "1799",
                MSONo = "7FJZM",
                PONo = "4800009329",
                LoadingPort = "India",
                DischargePort = "Jakarta",
                Model = "120K",
                ModelDescription = "120K/ROPS CAN/V-SCAR/PL631/TL508 14-MP",
                Status = "PLB IN",
                SerialNumber = "SZN10234",
                ETAPort = "15-Feb-2018",
                ETACakung = "20-Feb-2018",
                ATAPort = "15-Feb-2018",
                ATACakung = "20-Feb-2018",
                Notes = "Pergerkan order ini lambat",
                Detail = new InboundDetailModel
                {
                    ID = 1,
                    InboundID = 1,
                    RTS_PLAN = "10-Jan-2018",
                    RTS_ACTUAL = "11-Jan-2018",
                    ONBOARDVESSEL_PLAN = "12-Jan-2018",
                    ONBOARDVESSEL_ACTUAL = "13-Jan-2018",
                    PORTIN_PLAN = "14-Jan-2018",
                    PORTIN_ACTUAL = "15-Jan-2018",
                    PORTOUT_PLAN = "16-Jan-2018",
                    PORTOUT_ACTUAL = "17-Jan-2018",
                    PLBIN_PLAN = "18-Jan-2018",
                    PLBIN_ACTUAL = "19-Jan-2018",
                    PLBOUT_PLAN = "20-Jan-2018",
                    PLBOUT_ACTUAL = "21-Jan-2018",
                    YARDIN_PLAN = "22-Jan-2018",
                    YARDIN_ACTUAL = "23-Jan-2018",
                    YARDOUT_PLAN = "24-Jan-2018",
                    YARDOUT_ACTUAL = "25-Jan-2018"
                }
            });

            return list;
        }

        public static List<InboundDetailModel> GetDetailList(long InboundID)
        {
            List<InboundDetailModel> list = new List<InboundDetailModel>();

            list.Add(new InboundDetailModel
            {
                ID = 1,
                InboundID = 1,
                RTS_PLAN = "10-Jan-2018",
                RTS_ACTUAL = "11-Jan-2018",
                ONBOARDVESSEL_PLAN = "12-Jan-2018",
                ONBOARDVESSEL_ACTUAL = "13-Jan-2018",
                PORTIN_PLAN = "14-Jan-2018",
                PORTIN_ACTUAL = "15-Jan-2018",
                PORTOUT_PLAN = "16-Jan-2018",
                PORTOUT_ACTUAL = "17-Jan-2018",
                PLBIN_PLAN = "18-Jan-2018",
                PLBIN_ACTUAL = "19-Jan-2018",
                PLBOUT_PLAN = "20-Jan-2018",
                PLBOUT_ACTUAL = "21-Jan-2018",
                YARDIN_PLAN = "22-Jan-2018",
                YARDIN_ACTUAL = "23-Jan-2018",
                YARDOUT_PLAN = "24-Jan-2018",
                YARDOUT_ACTUAL = "25-Jan-2018"
            });
            list.Add(new InboundDetailModel
            {
                ID = 2,
                InboundID = 2,
                RTS_PLAN = "10-Jan-2018",
                RTS_ACTUAL = "11-Jan-2018",
                ONBOARDVESSEL_PLAN = "12-Jan-2018",
                ONBOARDVESSEL_ACTUAL = "13-Jan-2018",
                PORTIN_PLAN = "14-Jan-2018",
                PORTIN_ACTUAL = "15-Jan-2018",
                PORTOUT_PLAN = "16-Jan-2018",
                PORTOUT_ACTUAL = "17-Jan-2018",
                PLBIN_PLAN = "18-Jan-2018",
                PLBIN_ACTUAL = "19-Jan-2018",
                PLBOUT_PLAN = "20-Jan-2018",
                PLBOUT_ACTUAL = "21-Jan-2018",
                YARDIN_PLAN = "22-Jan-2018",
                YARDIN_ACTUAL = "23-Jan-2018",
                YARDOUT_PLAN = "24-Jan-2018",
                YARDOUT_ACTUAL = "25-Jan-2018"
            });

            return list.Where(w => w.InboundID == InboundID).ToList();
        }
    }
}