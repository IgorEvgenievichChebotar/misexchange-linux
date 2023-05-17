using ru.novolabs.ExchangeDTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;

namespace ru.novolabs.Common
{
    public static class Cache2DbManagerExtended
    {
        public static Result GetNextResultByStatus(this Cache2DbManager cacher ,List<StatusObjectCache> statusIds)
        {
            Result result = null;
            try
            {
                using (var context = new CacheContext())
                {

                    string requestCode = (from resultStatusCtxt in context.ResultObjectStatusSet.Include(r=>r.Result)
                              where statusIds.Contains(resultStatusCtxt.StatusId)
                              orderby resultStatusCtxt.Id
                              select resultStatusCtxt.Result.RequestCode).FirstOrDefault();
                    if (requestCode == null)
                        return null;

                    result = (from resultStatusCtxt in DBQueryGenerator.BuildQueryByAllProps(cacher.ResStatusIncludeStrs, context.ResultObjectStatusSet)
                              where statusIds.Contains(resultStatusCtxt.StatusId)
                              where resultStatusCtxt.Result.RequestCode == requestCode
                              orderby resultStatusCtxt.Id descending
                              select resultStatusCtxt).ToList().Select(t=>t.Result).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                ThrowCacheExcepion("Ошибка получения объектов Result:\r\n {0}", ex);
            }
            return result;
        
        }

        public static Result GetResultByCode(this Cache2DbManager cacher, string requestCode, List<StatusObjectCache> statusIds)
        {
            Result result = null;
            try
            {
                using (var context = new CacheContext())
                {
                    var resultStatus = (from resultStatutCtxt in DBQueryGenerator.BuildQueryByAllProps(cacher.ResStatusIncludeStrs, context.ResultObjectStatusSet)
                                        where resultStatutCtxt.Result.RequestCode == requestCode &&  statusIds.Contains(resultStatutCtxt.StatusId)
                                        select resultStatutCtxt).OrderByDescending(t => t.Id).FirstOrDefault();

                    if (resultStatus == null)
                        return null;
                    result = resultStatus.Result;
                }
            }
            catch (Exception ex)
            {
                ThrowCacheExcepion("Ошибка получения объекта Result:\r\n {0}", ex);
            }
            return result;
        }

        public static bool IsResultPresent(this Cache2DbManager cacher, string requestCode, StatusObjectCache statusId)
        {
            try
            {
                using (var context = new CacheContext())
                {
                    var resultStatus = (from result in context.Results
                                        join ros in context.ResultObjectStatusSet.Include(st => st.Result) on result.Id equals ros.Result.Id
                                        where result.RequestCode == requestCode && statusId == ros.StatusId
                                        select ros.Id).OrderByDescending(t => t).FirstOrDefault();

                    if (resultStatus == 0)
                        return false;
                    return true;
                }
            }
            catch (Exception ex)
            {
                ThrowCacheExcepion("Ошибка получения объекта Result:\r\n {0}", ex);
                return false;
            }
        
        }
        public static bool IsRequestPresent(this Cache2DbManager cacher, string requestCode, StatusObjectCache statusId)
        {
            try
            {
                using (var context = new CacheContext())
                {
                    var requestStatus = (from request in context.Requests
                                         join ros in context.RequestObjectStatusSet.Include(st => st.Request) on request.Id equals ros.Request.Id
                                         where request.RequestCode == requestCode && statusId == ros.StatusId
                                         select ros.Id).OrderByDescending(t => t).FirstOrDefault();

                    if (requestStatus == 0)
                        return false;
                    return true;
                }
            }
            catch (Exception ex)
            {
                ThrowCacheExcepion("Ошибка получения объекта Request:\r\n {0}", ex);
                return false;
            }

        }

        public static T SqlQuery<T>(this Cache2DbManager cacher, string queryStr, params object[] parameters) where T:class
        {
            try
            {
                using (var context = new CacheContext())
                {
                    T result = context.Database.SqlQuery<T>(queryStr, parameters).FirstOrDefault();
                    return result;
                }
            }
            catch (Exception ex)
            {
                ThrowCacheExcepion("Ошибка получения объекта Request:\r\n {0}", ex);
                return null;
            }
        
        }
        #region Update Methods
        public static  void UpdateAllRequestsStatusesByCode(this Cache2DbManager cacher, string requestCode, StatusObjectCache statusFrom, StatusObjectCache statusTo)
        {
            try
            {
                using (var context = new CacheContext())
                {
                    var statuses = (from status in context.RequestObjectStatusSet.Include("Request")
                                    where status.Request.RequestCode == requestCode && status.StatusId == statusFrom
                                    select status).ToList();


                    //  statuses.ForEach(st => st.StatusId = statusTo);
                    foreach (var status in statuses)
                    {
                        status.StatusId = statusTo;
                        ((IObjectContextAdapter)context).ObjectContext.ObjectStateManager.ChangeObjectState(status.Request, EntityState.Detached);
                    }
                    context.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                string errorStr = String.Format("Cannot update all requests by code [{0}] and status_id [{1}]:\r\n", requestCode,
                    Enum.GetName(typeof(StatusObjectCache), statusFrom));
                ThrowCacheExcepion(errorStr + "{0}", ex);
            
            }

        }

        public static  void UpdateAllResultsStatusesByCode(this Cache2DbManager cacher, string requestCode, StatusObjectCache statusFrom, StatusObjectCache statusTo)
        {
            try
            {
                using (var context = new CacheContext())
                {
                    var statuses = (from status in context.ResultObjectStatusSet.Include("Result")
                                    where status.Result.RequestCode == requestCode && status.StatusId == statusFrom
                                    select status).ToList();


                    //  statuses.ForEach(st => st.StatusId = statusTo);
                    foreach (var status in statuses)
                    {
                        status.StatusId = statusTo;
                        ((IObjectContextAdapter)context).ObjectContext.ObjectStateManager.ChangeObjectState(status.Result, EntityState.Detached);
                    }
                    context.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                string errorStr = String.Format("Cannot update all results by code [{0}] and status_id [{1}]:\r\n", requestCode, 
                    Enum.GetName(typeof(StatusObjectCache), statusFrom));
                ThrowCacheExcepion(errorStr + "{0}", ex);
            }

        }
        #endregion

        private static void ThrowCacheExcepion(string errorStr, Exception ex)
        {
            Log.WriteError(errorStr, ex.ToString());
            CacheException cex = new CacheException(errorStr, ex);
            throw cex;
        }
    }
}
