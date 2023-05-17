//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Reflection;
////using ru.novolabs.SuperCore.BusinessObjectsBlood;

//namespace ru.novolabs.SuperCore.HemBusinessObjects
//{
//    public class StageResults: BaseObject
//    {

//        protected void GetVisitStageValues(VisitStage stage)
//        {
//            foreach (ParameterValue paramValue in stage.Values)
//            {
//                if ((paramValue.State == ParameterValueState.STATE_APPROVED) && paramValue.Parameter.isValidObject())
//                {
//                    String paramName = paramValue.Parameter.ExternalCode;
//                    PropertyInfo propInfo = GetType().GetProperty(paramName);
//                    if (propInfo != null)
//                    {
//                        String value = string.Empty;
//                        if (paramValue.Reference.isValidObject())
//                        {
//                            value = paramValue.Reference.ExternalCode;
//                        }
//                        else
//                        {
//                            value = paramValue.Value;
//                        }
//                        if (!String.Empty.Equals(value))
//                        {
//                            SetParamValue(propInfo, value);
//                        }
//                    }
//                }
//            }
//        }

//        protected void SetParamValue(PropertyInfo propInfo, string value)
//        {
//            try
//            {
//                Type pType = propInfo.PropertyType;
//                // Если свойство имеет тип дженерика
//                if (pType.IsGenericType && pType.GetGenericTypeDefinition() == typeof(Nullable<>))
//                {
//                    pType = pType.GetGenericArguments()[0];
//                }


//                if (pType.Equals(typeof(Int32)))
//                {
//                    propInfo.SetValue(this, Int32.Parse(value), null);
//                }
//                else if (pType.Equals(typeof(float)))
//                {
//                    propInfo.SetValue(this, float.Parse(value, ProgramContext.Settings.NumberFormatInfo), null);
//                }
//                else
//                {
//                    propInfo.SetValue(this, value, null);
//                }

//            }
//            catch
//            {
//                Log.WriteError(String.Format("Не удалось преобразовать значение {0} к типу {1} для параметра {2}",
//                    value, propInfo.PropertyType.Name, propInfo.Name));
//            }
//        }     

//    }
//}
