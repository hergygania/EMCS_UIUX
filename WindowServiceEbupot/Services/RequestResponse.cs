using WindowsServiceEbupot.Services.ExceptionHandler;
using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WindowsServiceEbupot.Services
{
    public class RequestResponse<TModel> : ProtocolResponse
    {
        public RequestResponseDTO<TModel> Data
        {
            get
            {
                return (Json != null) ? Json.ToObject<RequestResponseDTO<TModel>>() : default(RequestResponseDTO<TModel>);
            }
        }

        public TModel Model
        {
            get
            {
                try
                {
                    if (Json != null)
                    {
                        return Json.Children().First().Children().First().ToObject<TModel>();
                    }

                    return default(TModel);
                }
                catch (System.Exception)
                {
                    return (Json != null) ? Json.ToObject<TModel>() : default(TModel);
                }
            }
        }
        public List<ValidationModelError> ModelErrors
        {
            get
            {
                if (IsError)
                {
                    List<ValidationModelError> errList = new List<ValidationModelError>();

                    if (Exception.Data != null && Exception.Data.Count > 0)
                    {
                        foreach (var key in Exception.Data.Keys)
                        {
                            var err = Exception.Data[key.ToString()].ToString();
                            errList.Add(new ValidationModelError { Name = key.ToString(), Reason = err });
                        }

                        return errList;
                    }
                }

                return null;
            }
        }
    }
}
