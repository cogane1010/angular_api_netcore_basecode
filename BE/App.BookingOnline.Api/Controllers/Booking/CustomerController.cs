using App.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using App.BookingOnline.Service;
using App.BookingOnline.Service.DTO;
using App.BookingOnline.Data.Paging;
using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using App.Core.Configs;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;

namespace App.BookingOnline.Api.Controllers
{

    public class CustomerController : BaseGridController<CustomerDTO, CustomerPagingModel, ICustomerService>
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public IConfiguration Configuration { get; }
        public CustomerController(ICustomerService service, IHostingEnvironment hostingEnvironment, IConfiguration configuration) : base(service)
        {
            _hostingEnvironment = hostingEnvironment;
            Configuration = configuration;
        }

        public override RespondData Get(Guid Id)
        {
            try
            {
                return Success(_service.GetExtend(Id));
            }
            catch (Exception e)
            {
                return Failure("",e.Message);
            }
        }

        [HttpPost("AddOrEditCustomer")]
        public async Task<RespondData> AddOrEditCustomer(CustomerDTO entityDTO)
        {
            try
            {
                CustomerDTO res = entityDTO;
                if (entityDTO.Id == null || entityDTO.Id == Guid.Empty)
                {
                    entityDTO.CreatedDate = DateTime.Now;
                    entityDTO.CreatedUser = UserName;
                    res = await _service.AddCustomer(entityDTO);

                }
                else
                {
                    entityDTO.UpdatedDate = DateTime.Now;
                    entityDTO.UpdatedUser = UserName;
                    await _service.UpdateCustomer(entityDTO);

                }

                return Success(res);
            }
            catch (Exception e)
            {
                return Failure("",e.Message);
            }

        }

        [HttpPost("ResetPassword")]
        public async Task<RespondData> ResetPassword(CustomerDTO entityDTO)
        {
            try
            {
                await _service.ResetPassword(entityDTO);
                return Success();
            }
            catch (Exception e)
            {
                return Failure("",e.Message);
            }

        }


        [HttpPost("ChangeStatus")]
        public RespondData ChangeStatus(CustomerDTO entityDTO)
        {
            try
            {
                _service.ChangeStatus(entityDTO);
                return Success();
            }
            catch (Exception e)
            {
                return Failure("",e.Message);
            }
        }



        #region Upload /Download File
        private string GetPath(DateTime date)
        {
            var rootPath = _hostingEnvironment.ContentRootPath;
            var path = Path.Combine(rootPath, AppConfigs.UPLOAD_PATH + "/" + date.Date.ToString("MMyyyy"));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        [HttpPost("UploadFile")]
        public async Task<RespondData> UploadFile()
        {
            try
            {
                var files = Request.Form.Files;
                var rootPath = _hostingEnvironment.ContentRootPath;
                var path = Path.Combine(rootPath, AppConfigs.UPLOAD_PATH);

                var postedFile = files[0];
                var fName = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss-fff") + Path.GetExtension(postedFile.FileName);
                var filePath = path + "\\" + fName;

                //using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                //{
                //    await postedFile.CopyToAsync(fileStream);
                //};
                string fileUrl = Configuration.GetSection("fileUpload").GetValue<string>("fileUrl");
                var path2 = fileUrl;
                if (!Directory.Exists(path2))
                {
                    Directory.CreateDirectory(path2);
                }
                var filePath2 = path2 + "/" + fName;
                using (Stream fileStream = new FileStream(filePath2, FileMode.Create))
                {
                    await postedFile.CopyToAsync(fileStream);
                };
                // save db
                var dbFile = new UploadFileDTO()
                {
                    FileName = postedFile.FileName,
                    FilePath = "\\" + fName,
                    FileType = Path.GetExtension(postedFile.FileName),
                    FileSize = (int)postedFile.Length
                };

                var resId = this._service.SaveUploadFile(dbFile);

                return Success(resId);
            }
            catch (Exception ex)
            {
                return Failure(ex.Message);
            }
        }

        [HttpPost("DownloadFile")]
        public async Task<IActionResult> DownloadFile(UploadFileDTO file)
        {
            UploadFileDTO dbFile = _service.GetFile(file);
            if (dbFile != null)
            {
                var rootPath = _hostingEnvironment.ContentRootPath;
                //var filePath = Path.Combine(rootPath, dbFile.FilePath);
                string fileUrl = Configuration.GetSection("fileUpload").GetValue<string>("fileUrl");

                var filePath = fileUrl + dbFile.FilePath;
                if (!string.IsNullOrEmpty(filePath))
                {
                    try
                    {
                        var fileName = Path.GetFileName(filePath);
                        var content = await System.IO.File.ReadAllBytesAsync(filePath);
                        new FileExtensionContentTypeProvider()
                            .TryGetContentType(fileName, out string contentType);
                        return File(content, contentType, fileName);
                    }
                    catch (Exception e)
                    {
                        return BadRequest();
                    }
                }


            }

            return BadRequest();


        }

        #endregion
    }



}