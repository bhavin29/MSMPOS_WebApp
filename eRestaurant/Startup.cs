using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RocketPOS.Models;
using RocketPOS.Repository;
using RocketPOS.Services;
using RocketPOS.Repository.Reports;
using RocketPOS.Services.Reports;
using RocketPOS.Interface.Services;
using RocketPOS.Interface.Repository;
using RocketPOS.Interface.Services.Reports;
using RocketPOS.Interface.Repository.Reports;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using System.Reflection;
using RocketPOS.Resources;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace RocketPOS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<LocService>();
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddControllersWithViews();
            services.Configure<ReadConfig>(Configuration.GetSection("Data"));

            services.AddMvc(option => option.EnableEndpointRouting = false).AddViewLocalization()
           .AddDataAnnotationsLocalization(options =>
           {
               options.DataAnnotationLocalizerProvider = (type, factory) =>
               {
                   var assemblyName = new AssemblyName(typeof(RocketPOSResources).GetTypeInfo().Assembly.FullName);
                   return factory.Create("RocketPOSResources", assemblyName.Name);
               };
           });

            services.AddMvc().AddRazorPagesOptions(options =>
            {
                options.Conventions.AddPageRoute("/Login/Index", "");
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    var supportedCultures = new List<CultureInfo>
                        {
                    new CultureInfo("en-US"),
                    new CultureInfo("de-CH"),
                    new CultureInfo("fr-CH"),
                    new CultureInfo("it-CH"),
                     new CultureInfo("en")
                        };

                    options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;

                    options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
                });

            //services.AddMvc().AddViewOptions(options =>
            //{
            //    options.HtmlHelperOptions.ClientValidationEnabled = false;
            //});

            services.Configure<HtmlHelperOptions>(o => o.ClientValidationEnabled = false);


            services.AddRazorPages();
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            services.AddOptions<IngredientModel>().ValidateDataAnnotations();
            services.AddControllersWithViews();

            services.AddControllers(options =>
            {
                options.EnableEndpointRouting = false;

            });

            services.AddScoped<IIngredientService, IngredientService>();
            services.AddScoped<IIngredientRepository, IngredientRepository>();
            services.AddScoped<IDropDownService, DropDownService>();
            services.AddScoped<IDropDownRepository, DropDownRepository>();
            services.AddScoped<ICommonService, CommonService>();
            services.AddScoped<ICommonRepository, CommonRepository>();
            services.AddScoped<IAddonsService, AddonsService>();
            services.AddScoped<IAddonsService, AddonsService>();
            services.AddScoped<IAddonsRepository, AddonsRepository>();
            services.AddScoped<IIngredientUnitService, IngredientUnitService>();
            services.AddScoped<IIngredientUnitRepository, IngredientUnitRepository>();
            services.AddScoped<IIngredientCategoryService, IngredientCategoryService>();
            services.AddScoped<IIngredientCategoryRepository, IngredientCategoryRepository>();
            services.AddScoped<IExpsenseCategoryRepository, ExpsenseCategoryRepository>();
            services.AddScoped<IExpsenseCategoryService, ExpsenseCategoryService>();
            services.AddScoped<IFoodMenuCatagoryRepository, FoodMenuCatagoryRepository>();
            services.AddScoped<IFoodMenuCatagoryService, FoodMenuCatagoryService>();
            services.AddScoped<IBankRepository, BankRepository>();
            services.AddScoped<IBankService, BankService>();
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IOutletRepository, OutletRepository>();
            services.AddScoped<IOutletService, OutletService>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ICardTerminalService, CardTerminalService>();
            services.AddScoped<ICardTerminalRepository, CardTerminalRepository>();
            services.AddScoped<IPurchaseService, PurchaseService>();
            services.AddScoped<IPurchaseRepository, PurchaseRepository>();
            services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
            services.AddScoped<IPaymentMethodService, PaymentMethodService>();
            services.AddScoped<IVarientService, VarientService>();
            services.AddScoped<IVarientRepository, VarientRepository>();
            services.AddScoped<ITablesRpository, TablesRepository>();
            services.AddScoped<ITablesService, TablesService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFoodMenuRpository, FoodMenuRepository>();
            services.AddScoped<IFoodMenuService, FoodMenuService>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IEmployeeAttendanceRpository, EmployeeAttendanceRepository>();
            services.AddScoped<IEmployeeAttendanceService, EmployeeAttendanceService>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IInventoryAdjustmentRepository, InventoryAdjustmentRepository>();
            services.AddScoped<IInventoryAdjustmentService, InventoryAdjustmentService>();
            services.AddScoped<IInventoryTransferRepository, InventoryTransferRepository>();
            services.AddScoped<IInventoryTransferService, InventoryTransferService>();
            services.AddScoped<IWasteRepository, WasteRepository>();
            services.AddScoped<IWasteService, WasteService>();
            services.AddScoped<IOutletRegisterRepository, OutletRegisterRepository>();
            services.AddScoped<IOutletRegisterService, OutletRegisterService>();
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IPurchaseGRNRepository, PurchaseGRNRepository>();
            services.AddScoped<IPurchaseGRNService, PurchaseGRNService>();
            services.AddScoped<IPurchaseInvoiceRepository, PurchaseInvoiceRepository>();
            services.AddScoped<IPurchaseInvoiceService, PurchaseInvoiceService>();
            services.AddScoped<IFoodMenuRateRepository, FoodMenuRateRepository>();
            services.AddScoped<IFoodMenuRateService, FoodMenuRateService>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IFoodMenuIngredientRepository, FoodMenuIngredientRepository>();
            services.AddScoped<IFoodMenuIngredientService, FoodMenuIngredientService>();
            services.AddScoped<ISchedulerRepository, SchedulerRepository>();
            services.AddScoped<ISchedulerService , SchedulerService>();
            services.AddScoped<IProductionFormulaRepository, ProductionFormulaRepository>();
            services.AddScoped<IProductionFormulaService, ProductionFormulaService>();
            services.AddScoped<IProductionEntryRepository, ProductionEntryRepository>();
            services.AddScoped<IProductionEntryService, ProductionEntryService>();
            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseMvcWithDefaultRoute();

            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);
            app.UseStaticFiles();


            var cultureInfo = new CultureInfo("en-GB");
            cultureInfo.NumberFormat.CurrencySymbol = "£";

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            var supportedCultures = new[]
                                        {
                                            new CultureInfo("en-GB"),
                                        };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-GB"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseExceptionHandler(
                options =>
                {
                    options.Run(
                        async context =>
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            context.Response.ContentType = "text/html";
                            var exceptionObject = context.Features.Get<IExceptionHandlerFeature>();
                            if (null != exceptionObject)
                            {
                                var errorMessage = $"<b>Exception Error: {exceptionObject.Error.Message} </b> {exceptionObject.Error.StackTrace}";
                                await context.Response.WriteAsync(errorMessage).ConfigureAwait(false);
                            }
                        });
                }
            );

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "default",
                    defaults: new
                    {
                        controller = "Login",
                        action = "Index"
                    });
            });

        }
    }

}