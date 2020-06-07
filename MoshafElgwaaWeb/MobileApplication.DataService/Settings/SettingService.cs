using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using System.Data.Entity;
using QvLib;
using Service;
using AutoMapper;
using System.Text.RegularExpressions;
using MobileApplication.Context;
using MobileApplication.DataModel;
namespace MobileApplication.DataService
{
    public class SettingService : BaseService
    {
        //private readonly Repository<Settings> _SettingRepository;
        //private readonly CU_LogService _logService;

        //public SettingService()
        //{
        //    _SettingRepository = new Repository<Settings>(_unitOfWork);
        //    _logService = new CU_LogService();

        //}

        //public Settings Setting
        //{

        //    get
        //    {
        //        return _SettingRepository.GetById(1);
        //    }
        //}

        //public SettingModel SettingModel
        //{

        //    get
        //    {
        //        var temp = _SettingRepository.GetList().FirstOrDefault();
        //        var data = Mapper.Map<Settings, SettingModel>(temp);
        //        return data;
        //    }
        //}

        //public int Save(SettingModel obj)
        //{

        //    var oldObj = new Settings();
        //    if (obj.SettingID > 0)
        //    {
        //        oldObj = Setting.Clone();
        //    }
        //    var data = Mapper.Map<SettingModel, Settings>(obj);
        //    _SettingRepository.Save(data);
        //    _unitOfWork.Submit();
        //    if (obj.SettingID > 0)
        //    {
        //        _logService.Update(data.SettingID, DataServiceArabicResource.ManageSettings, oldObj, data, obj.UserId, obj.ActionName);
        //    }
        //    else
        //    {
        //        _logService.Insert(data.SettingID, DataServiceArabicResource.ManageSettings, obj, obj.UserId, obj.ActionName);
        //    }
        //    return (int)data.SettingID;

        //}

       
        //public List<SettingModel> getAllSettings()
        //{
        //    var settings = _SettingRepository.GetList().ToList();
        //    return  Mapper.Map< List<SettingModel>>(settings);
        //}


    }
}
