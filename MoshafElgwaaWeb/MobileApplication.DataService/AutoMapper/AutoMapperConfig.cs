using AutoMapper;
using GenericEngine.Service.AutoMapper;
using MobileApplication.Context;
using QvLib.QVUtil;
using QvLib.Security;
using Service.Contracts;
using MobileApplication.DataModel;
using DataModel.Enum;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using DataModel.Extention;
using MobileApplication.DataModel.ControlPanel;
using MobileApplication.DataModel.ControlPanel.DoaaModels;
using MobileApplication.DataModel.ControlPanel.ArticleModels;
using MobileApplication.DataModel.ControlPanel.NamesOfAllahModels;

namespace MobileApplication.DataService.AutoMapper
{
    public class AutoMapperConfig : Generic_Configurations
    {
        public static void RegisterMappers()
        {
            //---------------------------for Role------------------------------------//
            //    Mapper.Configuration.
            Mapper.Initialize(
                cfg =>
                {

                    cfg.CreateMap<CU_Role, RoleModel>();
                    cfg.CreateMap<RoleModel, CU_Role>();
                    //---------------------------for Role---------------------------//
                    cfg.CreateMap<CU_Role, RoleModelLookup>()
                        .ForMember(dest => dest.CanDelete, opt => opt.MapFrom(src => (src.CU_Role_Program != null ? src.CU_Role_Program.Where(t => t.CU_Employee_RoleProgram.Count() == 0).Count() != 0 : true)))
                        .ForMember(des => des.CanEdit, opt => opt.MapFrom(src => true))
                        .ForMember(dest => dest.RoleProgramId, opt => opt.MapFrom(src => src.CU_Role_Program.Where(x => x.IdProgram == int.Parse(System.Configuration.ConfigurationManager.AppSettings["ProgramID"])).FirstOrDefault().ID));

                    cfg.CreateMap<RoleModelLookup, CU_Role>();

                    ////-------------------------for Role_Page ---------------------//
                    cfg.CreateMap<CU_Role_Page, Role_PageModel>();
                    cfg.CreateMap<Role_PageModel, CU_Role_Page>();
                    //---------------------------for Page--------------------------------//  
                    cfg.CreateMap<CU_Page, PageModel>();
                    cfg.CreateMap<PageModel, CU_Page>();
                    //---------------------------for Employee--------------------------------//

                    cfg.CreateMap<CU_Employee, UserModel>();
                    Mapper.CreateMap<UserModel, CU_Employee>();
                    //---------------------------for ResetPasswordRequest---------------------//
                    cfg.CreateMap<ResetPasswordRequest, ResetPasswordModel>();
                    cfg.CreateMap<ResetPasswordModel, ResetPasswordRequest>();

                    //---------------------------For Role Program-------------------//
                    cfg.CreateMap<CU_Role_Program, RoleProgramModel>();
                    cfg.CreateMap<RoleProgramModel, CU_Role_Program>();
                    //---------------------------For Employee Role Reogram--------//
                    cfg.CreateMap<CU_Employee_RoleProgram, EmployeeRoleProgramModel>();
                    cfg.CreateMap<EmployeeRoleProgramModel, CU_Employee_RoleProgram>();

                    ////-------------------------for Role_Security ---------------------//
                    cfg.CreateMap<CU_Role, Role_SecModel>()
                        .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.CU_Role_Program.Where(v => v.IdProgram == int.Parse(ConfigurationManager.AppSettings["ProgramID"])).FirstOrDefault().ID))
                        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                        .ForMember(dest => dest.RoleID, opt => opt.MapFrom(src => src.ID))
                                   ;
                    cfg.CreateMap<Role_SecModel, CU_Role>();

                    cfg.CreateMap<Role_SecModel, CU_Role>();
                    //--------------------------------------------------------------//
                    cfg.CreateMap<CU_Employee, EmployeeModel>()
                        .ForMember(dest => dest.CanDelete, opt => opt.MapFrom(src => src.LastVisitTime == null))
                         .ForMember(dest => dest.CanEdit, opt => opt.MapFrom(src => true));

                    cfg.CreateMap<EmployeeModel, CU_Employee>();

                    //---------------------------for Pages----------------------------//
                    cfg.CreateMap<CU_Page, PageModelLookup>();
                    cfg.CreateMap<PageModelLookup, CU_Page>();
                    //---------------------------for Actions-------------------------//
                    cfg.CreateMap<CU_Action, ActionModel>();
                    cfg.CreateMap<ActionModel, CU_Action>();


                    //------------------------------Notification-----------------------------------------//

                    Mapper.CreateMap<Notification, NotificationModel>();
                    Mapper.CreateMap<NotificationModel, Notification>();


                    //------------------------------NotificationUser-----------------------------------------//

                    Mapper.CreateMap<NotificationUser, NotificationUserModel>();
                    Mapper.CreateMap<NotificationUserModel, NotificationUser>();
                    //---------------------------Nationality------------------------------------//
                    cfg.CreateMap<Nationality, NationalityModel>()
                   .BeforeMap((s, d) => d.CanDelete = true)
                   .BeforeMap((s, d) => d.CanEdit = true);

                    cfg.CreateMap<NationalityModel, Nationality>();
                    //------------------------------Application settings-----------------------------------------//
                    Mapper.CreateMap<MobileApplication.Context.AppSetting, AppSettingModel>();
                    Mapper.CreateMap<AppSettingModel, MobileApplication.Context.AppSetting>();

                    //----------------------------------Doaa-------------------------------------------------------//
                    Mapper.CreateMap<Doaa, DoaaModel>()
                        .ForMember(dest => dest.StrDoaaMainCategory, opt=>opt.MapFrom(src => src.DoaaMainCategory.Name))
                        //.ForMember(dest => dest.StrDoaaCategory, opt => opt.MapFrom(src => src.DoaaCategory.Name))
                        //.ForMember(dest => dest.DoaaMainCategoryID, opt => opt.MapFrom(src => src.DoaaMainCategory.ID))
                        ;
                    Mapper.CreateMap<DoaaModel, Doaa>()
                        .ForMember(dest => dest.DoaaContent, opt => opt.MapFrom(src => src.DoaaContent.Replace("href", "ng-click")))
                        ;
                    //----------------------------------DoaaItemSource-------------------------------------------------------//
                    Mapper.CreateMap<DoaaItemSource, DoaaItemSourceModel>()
                        .ForMember(dest => dest.DoaaName, opt => opt.MapFrom(src => src.Doaa.Name))
                        ;
                    Mapper.CreateMap<DoaaItemSourceModel, DoaaItemSource>();
                    //----------------------------------Doaa Main Category-------------------------------------------------------//
                    Mapper.CreateMap<DoaaMainCategory, DoaaMainCategoryModel>();
                    Mapper.CreateMap<DoaaMainCategoryModel, DoaaMainCategory>();


                    //----------------------------------ArticleItemExplanationModel-------------------------------------------------------//
                    Mapper.CreateMap<ArticleItemExplanation, ArticleItemExplanationModel>()
                    .ForMember(dest => dest.ArticleName, opt => opt.MapFrom(src => src.Article.Name))
                        ;
                    Mapper.CreateMap<ArticleItemExplanationModel, ArticleItemExplanation>();
                    //----------------------------------Article-------------------------------------------------------//
                    Mapper.CreateMap<Article, ArticleModel>();
                    Mapper.CreateMap<ArticleModel, Article>()
                   .ForMember(dest => dest.ArticleContent, opt => opt.MapFrom(src => src.ArticleContent.Replace("href", "ng-click")))

                        ;

                    //----------------------------------NameOfAllah-------------------------------------------------------//
                    Mapper.CreateMap<NamesOfAllah, NameOfAllahModel>();
                    Mapper.CreateMap<NameOfAllahModel, NamesOfAllah>()
                   .ForMember(dest => dest.NameOfAllahMeaning, opt => opt.MapFrom(src => src.NameOfAllahMeaning.Replace("href", "ng-click")))

                        ;
              
                });



        }

    }
}