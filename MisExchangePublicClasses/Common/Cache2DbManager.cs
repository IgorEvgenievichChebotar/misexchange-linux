using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ru.novolabs.ExchangeDTOs;
using ru.novolabs.SuperCore;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;

namespace ru.novolabs.Common
{
    public class Cache2DbManager
    {
        private static Lazy<Cache2DbManager> _instance = new Lazy<Cache2DbManager>(() => new Cache2DbManager() { IsHierarchyPart = false });

        public static Cache2DbManager Instance
        {
            get
            {
                return _instance.Value;
            }
        }
        private Cache2DbManager()
        {
            DBQueryGenerator generator = new DBQueryGenerator();
            ReqStatusIncludeStrs = generator.PreBuildQueryList(typeof(RequestObjectStatus));
            ResStatusIncludeStrs = generator.PreBuildQueryList(typeof(ResultObjectStatus));
        }
        /// <summary>
        /// Get or set if cacheContext is a part of hierarchy and have to set its initializer to null 
        /// </summary>
        public bool IsHierarchyPart { get; set; }
        public List<string> ReqStatusIncludeStrs { get; private set; }
        public List<string> ResStatusIncludeStrs { get; private set; }
        #region Get methods
        public Request GetRequestByCode(string requestCode, StatusObjectCache statusId)
        {
            Request request = null;
            try
            {
                using (var context = new CacheContext(IsHierarchyPart))
                {
                    //var requestStatus = (from requestStatutCtxt in DBQueryGenerator.BuildQueryByAllProps(ReqStatusIncludeStrs, context.RequestObjectStatusSet)
                    //                     where requestStatutCtxt.Request.RequestCode == requestCode && requestStatutCtxt.StatusId == statusId
                    //                     select requestStatutCtxt).OrderByDescending(t => t.Id).FirstOrDefault();
                    //if (requestStatus == null)
                    //    return null;
                    //request = requestStatus.Request;

                    var ids = (from status in context.RequestObjectStatusSet
                               where (status.Request != null) &&
                               (status.Request.RequestCode == requestCode) &&
                               (status.StatusId == statusId)
                               select new { satusId = status.Id, requestId = status.Request.Id }).ToList();

                    if (ids.Count == 0)
                        return null;

                    long requestId = ids.First(j => j.satusId == ids.Max(i => i.satusId)).requestId;
                    Log.WriteText("GetRequestByCode. requestId=" + requestId.ToString());
                    //request = context.Requests.Include("Samples").Single(r => r.Id == requestId);
                    request = context.Requests.Include("Patient").Include("Samples").Include("Samples.Targets").Include("UserFields").Single(r => r.Id == requestId);
                    return request;
                }
            }
            catch (Exception ex)
            {
                ThrowCacheExcepion("Ошибка получения объекта Request:\r\n {0}", ex);
            }
            return request;


        }
        /// <summary>
        /// Get only RequestStatusInfo without related full requset info
        /// </summary>
        /// <param name="requestCode"></param>
        /// <returns></returns>
        public RequestObjectStatus GetReqObjStatusInfoByRequestCode(string requestCode)
        {
            RequestObjectStatus reqObjS = null;
            try
            {
                using (var context = new CacheContext(IsHierarchyPart))
                {
                    reqObjS = (from reqS in context.RequestObjectStatusSet.Include("Request")
                               where reqS.Request.RequestCode == requestCode
                               select reqS).OrderByDescending(r => r.Id).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                ThrowCacheExcepion("Ошибка получения объекта RequestObjectStatus:\r\n {0}", ex);
            }
            return reqObjS;

        }

        public Result GetResultByCode(string requestCode, StatusObjectCache statusId)
        {
            Result result = null;
            try
            {
                using (var context = new CacheContext(IsHierarchyPart))
                {

                    var resultStatus = (from resultStatutCtxt in DBQueryGenerator.BuildQueryByAllProps(ResStatusIncludeStrs, context.ResultObjectStatusSet)
                                        where resultStatutCtxt.Result.RequestCode == requestCode && resultStatutCtxt.StatusId == statusId
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
        public List<Result> GetResultListByCode(string requestCode, StatusObjectCache statusId)
        {
            List<Result> results = null;
            try
            {
                using (var context = new CacheContext(IsHierarchyPart))
                {
                    results = (from resultStatutCtxt in DBQueryGenerator.BuildQueryByAllProps(ResStatusIncludeStrs, context.ResultObjectStatusSet)
                               where resultStatutCtxt.Result.RequestCode == requestCode && resultStatutCtxt.StatusId == statusId
                               select resultStatutCtxt).ToList().Select(t => t.Result).ToList();
                }
            }
            catch (Exception ex)
            {
                ThrowCacheExcepion("Ошибка получения объектов Result:\r\n {0}", ex);
            }
            return results;

        }
        /// <summary>
        /// Get only ResultStatusInfo without related full result info
        /// </summary>
        /// <param name="requestCode"></param>
        /// <returns></returns>
        public ResultObjectStatus GetResObjStatusInfoByRequestCode(string requestCode)
        {
            ResultObjectStatus resObjS = null;
            try
            {
                using (var context = new CacheContext(IsHierarchyPart))
                {
                    resObjS = (from res in context.ResultObjectStatusSet.Include("Result")
                               where res.Result.RequestCode == requestCode
                               select res).OrderByDescending(r => r.Id).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                ThrowCacheExcepion("Ошибка получения объекта ResulttObjectStatus:\r\n {0}", ex);
            }
            return resObjS;
        }
        #endregion

        #region SaveMethods
        public void SaveRequest(RequestObjectStatus reqObjS)
        {
            using (var context = new CacheContext(IsHierarchyPart))
            {
                try
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    context.RequestObjectStatusSet.Add(reqObjS);
                    //context.Requests.Add(request);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    ThrowCacheExcepion("Ошибка кэширования объектов Request и RequestObjectStatus:\r\n {0}", ex);
                }
            }
        }

        public void SaveResult(ResultObjectStatus resObjS)
        {
            using (var context = new CacheContext(IsHierarchyPart))
            {
                try
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    context.ResultObjectStatusSet.Add(resObjS);
                    // context.Results.Add(result);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    ThrowCacheExcepion("Ошибка кэширования объекта ResultObjectStatus:\r\n {0}", ex);
                }
            }

        }
        #endregion

        private void ThrowCacheExcepion(string errorStr, Exception ex)
        {
            Log.WriteError(errorStr, ex.ToString());
            CacheException cex = new CacheException(errorStr, ex);
            throw cex;
        }
    }

    public class CacheException : Exception
    {
        public CacheException(string message) : base(message) { }
        public CacheException(string message, Exception innerException) : base(message, innerException) { }

    }
}
