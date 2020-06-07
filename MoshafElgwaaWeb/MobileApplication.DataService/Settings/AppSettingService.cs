using Repository;
using MobileApplication.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileApplication.DataModel;
using AutoMapper;
using DataModel.Enum;

namespace MobileApplication.DataService
{
    public class AppSettingService : BaseService
    {
        private readonly Repository<AppSetting> _AppSettingRepository;
        private readonly CU_LogService _logService;
        public static bool HasNotification;
        public AppSettingService()
        {
            _AppSettingRepository = new Repository<AppSetting>(_unitOfWork);
            _logService = new CU_LogService();
        }
        public AppSetting AppSetting
        {
            get { return AppSettingList.FirstOrDefault(); }
        }

       
        public AppSettingModel GetApplicationSettings()
        {
            var data = Mapper.Map<AppSetting, AppSettingModel>(AppSetting);
            return data;
        }
        public IEnumerable<AppSetting> AppSettingList
        {
            get { return _AppSettingRepository.GetList(); }
        }
        public T GetAppSetting<T>(AppSettingEnum appenum)
        {
           var  value= _AppSettingRepository.GetById((int) appenum).Value;
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public void GetMaximumUploadHeightAndWidth(out int MaxUploadHeight, out int MaxUploadWidth)
        {
            MaxUploadHeight = int.Parse(_AppSettingRepository.GetById((int)AppSettingEnum.MaxUploadHeight).Value);
            MaxUploadWidth = int.Parse(_AppSettingRepository.GetById((int)AppSettingEnum.MaxUploadWidth).Value);
        }

        public void GetMaximumThumbHeightAndWidth(out int MaxThumbHeight, out int MaxThumbWidth)
        {
            MaxThumbHeight = int.Parse(_AppSettingRepository.GetById((int)AppSettingEnum.MaxThumbHeight).Value);
            MaxThumbWidth = int.Parse(_AppSettingRepository.GetById((int)AppSettingEnum.MaxThumbWidth).Value);
        }
        public AppSettingModel GetHasNotificationAppSetting()
        {

            var model = Mapper.Map<AppSetting, AppSettingModel>
                         (_AppSettingRepository.Get(a => a.ID == (int)AppSettingEnum.HasNotification).FirstOrDefault());
            return model;
        }
        public AppConfigurationsModel GetConfigurationsModel()
        {
            AppConfigurationsModel model = new AppConfigurationsModel();

            model.CameraQuality = AppSettingList.FirstOrDefault(x => x.ID == (int)AppSettingEnum.CameraQuality).Value;
            model.MaxUploadHeight = AppSettingList.FirstOrDefault(x => x.ID == (int)AppSettingEnum.MaxUploadHeight).Value;
            model.MaxUploadWidth = AppSettingList.FirstOrDefault(x => x.ID == (int)AppSettingEnum.MaxUploadWidth).Value;

            return model;
        }

        public Dictionary<string, string> GetAllConfigurationsDictionary()
        {
            Dictionary<string, string> config = new Dictionary<string, string>();
            foreach (var item in AppSettingList)
            {
                config.Add(item.Key, item.Value);
            }
            return config;
        }

        public Dictionary<string, string> GetAllMobileNotificationsDictionary()
        {
            Dictionary<string, string> config = new Dictionary<string, string>();
            var mobileSettings = _AppSettingRepository.GetByFilter(a=>a.IsMobile).ToList();
            foreach (var item in mobileSettings)
            {
                config.Add(item.Key, item.Value);
            }
            return config;
        }

     
    }
}
