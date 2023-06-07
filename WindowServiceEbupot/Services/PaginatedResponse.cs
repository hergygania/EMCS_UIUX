using WindowsServiceEbupot.Services.ExceptionHandler;
using IdentityModel.Client;
using System.Collections.Generic;
using System.Linq;

namespace WindowsServiceEbupot.Services
{
    public class PaginatedResponse<TModel> : ProtocolResponse
    {
        public PaginatedResult<TModel> Data
        {
            get
            {
                return Json?.ToObject<PaginatedResult<TModel>>();
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

    public class PaginatedResult<T>
    {
        public int totalCount { get; set; }
        public int pageSize { get; set; }
        public int currentPage { get; set; }
        public int totalPages { get; set; }
        public bool hasNext => currentPage < totalPages;
        public bool hasPrevious => currentPage > 1;
        public string orderBy => sorts != null ? string.Join(",", sorts.Select(s => s.orderBy).ToArray()) : "";
        public List<PaginationSort> sorts { get; set; }
        public List<T> result { get; set; }
    }
    public class PaginationSort
    {
        public string selector { get; set; }
        public bool desc { get; set; }
        public string orderBy => desc ? $"{selector} desc" : $"{selector} asc";
    }
}