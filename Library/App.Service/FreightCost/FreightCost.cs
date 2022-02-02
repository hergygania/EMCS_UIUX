using App.Data.Domain.Extensions;
using System.Collections.Generic;
using System.Linq;
using System;

namespace App.Service.FreightCost
{
    public class FreightCost
    {
        public static List<getModaFactor> GetModaFactor(string Origin, string Destination, string ServiceCode)
        {
            using (var db = new Data.EfDbContext())
            {
                var tb = from t in db.MasterSurcharge
                             .Where(o => o.Origin_Code == Origin)
                             .Where(w => w.Destination_Code == Destination)
                             .Where(s => s.Service_Code == ServiceCode)
                         join m in db.MasterGeneric on t.Moda_Factor_ID equals m.ID
                         select new getModaFactor
                         {
                             ID = m.ID,
                             Origin = t.Origin_Code,
                             Destination = t.Destination_Code,
                             ModaName = m.Name,
                             ValueModa = m.Value
                         };
                return tb.ToList();
            }
        }

        public static List<ModaFleet> GetFleetList(string Origin, string Destination)
        {
            using (var db = new Data.EfDbContext())
            {
                var tb = from t in db.MasterTruckingRate.Where(o => o.Origin_Code == Origin).Where(w => w.Destination_Code == Destination)
                         join m in db.ModaOfCondition on t.ConditionModa_ID equals m.ID
                         select new ModaFleet
                         {
                             ID = m.ID,
                             Origin = t.Origin_Code,
                             Destination = t.Destination_Code,
                             ModaName = m.Moda,
                             RatePerTrip = t.Rate_Per_Trip_IDR
                         };
                return tb.ToList();
            }
        }

        public static List<RateList> GetRate(string ServiceCode, string Origin, string Destination, decimal ActualWeight)
        {

            using (var db = new Data.EfDbContext())
            {
                var tb = from t in db.MasterRate.Where(s => s.Service_Code == ServiceCode).
                             Where(o => o.Origin_Code == Origin).
                             Where(w => w.Destination_Code == Destination)

                         select new RateList
                         {
                             ServiceCode = t.Service_Code,
                             Origin = t.Origin_Code,
                             Destination = t.Destination_Code,
                             Rate = ActualWeight > 0 && ActualWeight <= 1000 ? t.WeightBreak1000 :
                                    ActualWeight > 1000 && ActualWeight < 4000 ? t.WeightBreak3999 :
                                    ActualWeight >= 4000 ? t.WeightBreak99999 : "0",
                             CurrRate = t.Currency,
                             MinRate = t.MIN_Rate,
                             RA = t.Ragulated,
                             LeadTime = t.Lead_Time,
                             CostRateIDR = t.Cost
                         };
                return tb.ToList();
            }
        }

        public static List<InRateList> GetInRate(string ServiceCode, string Origin, string Destination)
        {
            using (var db = new Data.EfDbContext())
            {
                var tb = from t in db.MasterInboundRate.Where(s => s.Service_Code == ServiceCode).
                             Where(o => o.Origin_Code == Origin).
                             Where(w => w.Destination_Code == Destination)

                         select new InRateList
                         {
                             ServiceCode = t.Service_Code,
                             Origin = t.Origin_Code,
                             Destination = t.Destination_Code,
                             InRate = t.Rate_Inbound,
                             CurrInRate = t.Currency
                         };
                return tb.ToList();
            }
        }

        public static List<SurchargeList> GetSurcharge(string ServiceCode, string Origin, string Destination, string ModaFactor)
        {
            using (var db = new Data.EfDbContext())
            {
                var tb = from t in db.MasterSurcharge
                         join md in db.MasterGeneric on t.Moda_Factor_ID equals md.ID
                         where t.Service_Code == ServiceCode && t.Origin_Code == Origin &&
                         t.Destination_Code == Destination && md.Name == ModaFactor
                         select new SurchargeList
                         {
                             ServiceCode = t.Service_Code,
                             Origin = t.Origin_Code,
                             Destination = t.Destination_Code,
                             Moda = md.Name,
                             Surcharge = t.Surcharge,
                             Cost50 = t.Surcharge_50,
                             Cost100 = t.Surcharge_100,
                             Cost200 = t.Surcharge_200
                         };
                return tb.ToList();
            }
        }

        public static Calculator hitungFreightCalculator(Calculator model)
        {
            decimal _actualWeight = 0, _rate = 0, _cost200 = 0, _chargWeight = 0, _currency = 0, _minRate = 0, _minWeight = 0, _inRate = 0, _costRA = 0,
                _lenght = 0, _wide = 0, _height = 0, _modaFactor = 0, _dimWeight = 0, _costCBM = 0, _costPacking = 0, _costRateIDR = 0, _costSurcharge = 0, _costRateUSD = 0;

            try
            {
                _lenght = Convert.ToDecimal(model.Lenght);
                _wide = Convert.ToDecimal(model.Wide);
                _height = Convert.ToDecimal(model.Height);
                _modaFactor = Convert.ToDecimal(model.ModaFactor);
                _currency = Convert.ToDecimal(model.Currency);

                decimal.TryParse(model.ActualWeight, out _actualWeight);
                _lenght = decimal.Round(_lenght, 2, MidpointRounding.AwayFromZero);
                _wide = decimal.Round(_wide, 2, MidpointRounding.AwayFromZero);
                _height = decimal.Round(_height, 2, MidpointRounding.AwayFromZero);
                _modaFactor = decimal.Round(_modaFactor, 2, MidpointRounding.AwayFromZero);
                _currency = decimal.Round(_currency, 2, MidpointRounding.AwayFromZero);

                SurchargeList DataSurcharge = GetDataSurcharge(model._Service, model.Origin, model.Destination, model.ModaFactorName);
                if (DataSurcharge != null) decimal.TryParse(DataSurcharge.Cost200, out _cost200);
                _cost200 = decimal.Round(_cost200, 2, MidpointRounding.AwayFromZero);

                model.LeadTime = "0";
                RateList DataRate = GetDataRate(model._Service, model.Origin, model.Destination, _actualWeight);
                if (DataRate != null)
                {
                    decimal.TryParse(DataRate.Rate, out _rate);
                    _rate = decimal.Round(_rate, 2, MidpointRounding.AwayFromZero);

                    _minRate = DataRate.MinRate;
                    _minRate = decimal.Round(_minRate, 2, MidpointRounding.AwayFromZero);

                    _minWeight = _rate > 0 ? (_minRate / _rate) : 0;
                    model.CurrencyRate = DataRate.CurrRate;
                    model.RA = DataRate.RA;
                    model.LeadTime = DataRate.LeadTime;

                    _costRateIDR = DataRate.CostRateIDR;
                    _costRateIDR = decimal.Round(_costRateIDR, 2, MidpointRounding.AwayFromZero);
                }

                InRateList DataInRate = GetDataInRate(model._Service, model.Origin, model.Destination);
                if (DataInRate != null)
                {
                    _inRate = DataInRate.InRate;
                    model.CurrencyInRate = DataInRate.CurrInRate;
                }

                _dimWeight = _modaFactor > 0 ? ((_lenght * _wide * _height) / _modaFactor) : 0;
                _dimWeight = decimal.Round(_dimWeight, 2, MidpointRounding.AwayFromZero);

                _chargWeight = _dimWeight > _actualWeight ? _dimWeight : _actualWeight;
                _chargWeight = decimal.Round(_chargWeight, 2, MidpointRounding.AwayFromZero);

                _costCBM = (_lenght * _wide * _height) / 1000000;
                _costCBM = decimal.Round(_costCBM, 2, MidpointRounding.AwayFromZero);

                _costPacking = _costCBM < 1 ? Convert.ToDecimal(GetGenericCBM("PackingCost", "0").Value) : _costCBM * Convert.ToDecimal(GetGenericCBM("PackingCost", "1").Value);
                _costPacking = decimal.Round(_costPacking, 2, MidpointRounding.AwayFromZero);

                _costRateUSD = _currency > 0 ? _costRateIDR / _currency : 0;
                _costRateUSD = decimal.Round(_costRateUSD, 2, MidpointRounding.AwayFromZero);

                _costRA = CostRA(model.CurrencyRate, _actualWeight, _costRateUSD, _costRateIDR);
                _costRA = decimal.Round(_costRA, 2, MidpointRounding.AwayFromZero);

                _costSurcharge = CostSurcharge(model._Service, _actualWeight, _cost200, _rate, _chargWeight);
                _costSurcharge = decimal.Round(_costSurcharge, 2, MidpointRounding.AwayFromZero);

                model.Currency = _currency.ToString("n2");
                model.TruckingRate = Convert.ToDecimal(model.TruckingRate).ToString("n2");
                model.Rate = _rate.ToString("n2");
                model.MinRate = _minRate.ToString("n2");
                model.MinWeight = _minWeight.ToString("n2");
                model.InRate = _inRate.ToString("n2");
                model.DimWeight = _dimWeight.ToString("n2");
                model.ChargWeight = _chargWeight.ToString("n2");
                model.CostCBM = _costCBM.ToString("n2");
                model.CostPacking = _costPacking.ToString("n2");
                model.CostSurcharge = _costSurcharge.ToString("n2");
                model.CostRA = _costRA.ToString("n2");
                model.InboundUSD = (_rate < 0 ? 0 : _inRate * _chargWeight).ToString("n2");
                model.InboundIDR = (Convert.ToDecimal(model.InboundUSD) * _currency).ToString("n2");

                if (_chargWeight < _minWeight)
                    model.TotalDomestic = (_minRate + _costSurcharge + _costRA + _costPacking).ToString("n2");
                else
                    model.TotalDomestic = ((_rate * _chargWeight) + _costSurcharge + _costRA + _costPacking).ToString("n2");

                model.TotalInternational = (Convert.ToDecimal(model.InboundUSD) + Convert.ToDecimal(model.TotalDomestic)).ToString("n2");
                if (!string.IsNullOrWhiteSpace(model.CurrencyRate))
                {
                    if (model.CurrencyRate.Trim().ToUpper() != "USD")
                        model.TotalInternational = (Convert.ToDecimal(model.InboundIDR) + Convert.ToDecimal(model.TotalDomestic)).ToString("n2");
                }

                return model;
            }
            catch (Exception)
            {
                return model;
            }
        }

        public static RateList GetDataRate(string ServiceCode, string Origin, string Destination, decimal ActualWeight)
        {
            return (from p in GetRate(ServiceCode, Origin, Destination, ActualWeight) select p).FirstOrDefault();
        }

        public static InRateList GetDataInRate(string ServiceCode, string Origin, string Destination)
        {
            return (from p in GetInRate(ServiceCode, Origin, Destination) select p).FirstOrDefault();
        }

        public static SurchargeList GetDataSurcharge(string ServiceCode, string Origin, string Destination, string ModaFactor)
        {
            return (from p in GetSurcharge(ServiceCode, Origin, Destination, ModaFactor) select p).FirstOrDefault();
        }

        public static Data.Domain.MasterGeneric GetGenericCBM(string code, string CBM)
        {
            return (from p in Service.Master.MasterGeneric.getGenericCBM(code, CBM) select p).FirstOrDefault();
        }

        private static decimal CostSurcharge(string service, decimal ActualWeight, decimal Cost200, decimal rate, decimal ChargWeight)
        {
            try
            {
                decimal surcharge = Cost200;

                if (ActualWeight >= 70 && ActualWeight < 149) surcharge = Convert.ToDecimal(50) / Convert.ToDecimal(100);
                else if (ActualWeight >= 149) surcharge = Convert.ToDecimal(100) / Convert.ToDecimal(100);

                return service != "ED" ? rate * ChargWeight * surcharge : 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private static decimal CostRA(string CurrencyRate, decimal ActualWeight, decimal CostRateUSD, decimal CostRateIDR)
        {
            try
            {
                decimal ret = 0;

                if (CurrencyRate.Trim().ToUpper() == "USD") ret = CostRateUSD * ActualWeight;
                else if (CurrencyRate.Trim().ToUpper() == "IDR") ret = CostRateIDR * ActualWeight;

                return ret;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
