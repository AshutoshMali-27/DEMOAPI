using IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.ViewModel;
using System.Net;

namespace SBM2_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WorkOrderApiController : Controller
    {
        private readonly IWorkOrder objWO;
        public WorkOrderApiController(IWorkOrder _objWO)
        {
            objWO = _objWO;
        }
        [HttpGet("GetFinancialYears")]
        public async Task<IActionResult> GetFinancialYears()
        {
            var financialYears =await objWO.lstFinancialYear();
            return Ok(financialYears);
        }
        [HttpGet("GetScheme")]
        public async Task<IActionResult> GetScheme()
        {
            var financialYears =await objWO.lstScheme();
            return Ok(financialYears);
        }
        [HttpGet("GetComponent")]
        public async Task<IActionResult> GetComponent(int SchemeId)
        {
            var Component = await objWO.lstComponentIdWise(SchemeId);
            return Ok(Component);
        }
        [HttpGet("GetAdminApprovalNumber")]
        public async Task<IActionResult> GetAdminApprovalNumber(int ZPID, int SchemeId, int HeadCodeID, int userID, int DivID, int ULBID)
        {
            var Component = await objWO.lstAdminApprovalNo(ZPID, SchemeId, HeadCodeID, userID, DivID, ULBID);
            return Ok(Component);
        }
        [HttpGet("GetLastBillSequences")]
        public async Task<IActionResult> GetLastBillSequences()
        {
            var lstBillSequences = await objWO.GetLastBillSequences();
            return Ok(lstBillSequences);
        }
        [HttpGet("GetWorkStatus")]
        public async Task<IActionResult> GetWorkStatus()
        {
            var lstWorkStatus = await objWO.Get_WorkStatus();
            return Ok(lstWorkStatus);
        }
        [Authorize]
        [HttpGet("CheckAAUpdateApproval")]
        public async Task<IActionResult> CheckAAUpdateApproval(int AdminApprovalNo)
        {
            try
            {
                HttpResponseMessage responseMessage = await objWO.CheckAAUpdateApproval(AdminApprovalNo);
                if (responseMessage.StatusCode == HttpStatusCode.Conflict)
                    return Ok(false);
                else if (responseMessage.StatusCode == HttpStatusCode.OK)
                    return Ok(true);
                else
                    return StatusCode((int)responseMessage.StatusCode);
            }
            catch (Exception ex) {
                throw ex;
            }
        }
        [HttpGet("GetAdminApproveNumberDetails")]
        public async Task<IActionResult> GetAdminApproveNumberDetails(int ZPID, int AdminApprovalNo, int HeadCodeID, int userID, int DivID, int ULBID, int Deptid)
        {
            var AADetails = await objWO.Get_AdminApproveNumberDetails(ZPID, AdminApprovalNo, HeadCodeID, userID, DivID, ULBID, Deptid);
            return Ok(AADetails);
        }
        [HttpGet("GetVendorInformation")]
        public async Task<IActionResult> GetVendorInformation(int ZPID, string SearchBy, string Input, int DivID, int ULBID, int SchemeID, int ComponentID)
        {
            var VendorInformation = await objWO.Get_VendorInformation(ZPID, SearchBy, Input, DivID, ULBID, SchemeID, ComponentID);
            return Ok(VendorInformation);
        }
        [HttpPost("SaveWorkDetails")]
        public async Task<IActionResult> SaveWorkDetails([FromBody] WorkDetailsDummy data)
        {
            if (data == null)
            {
                return BadRequest("Invalid data");
            }
            try
            {
                // Call the Interface method to insert the receipt data and get the output parameters
                var response = await objWO.SaveWorkDetails(data);
                return Ok(new { workDetailsUID = response});
            }
            catch (Exception ex)
            {
                throw ex;   
            }
        }
         [HttpGet("GetWorkDetailsInbox")]
        public async Task<IActionResult> GetWorkDetailsInbox(int UserID, int ZPID)
        {
            try
            {
                var WorkDetilsList = await objWO.Get_WorkDetilsInbox(UserID, ZPID);
                return Json(WorkDetilsList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("GetWorkDetailsInboxViewSlip")]
        public async Task<IActionResult> GetWorkDetailsInboxViewSlip(int ZPID, int RecordId, int MasterID, int ApprovalBy, int ApprovalMasterID)
        {
            try
            {
                var WorkDetilsSlip = await objWO.WorkDetilsInboxViewSlip(ZPID,  RecordId,  MasterID,  ApprovalBy,  ApprovalMasterID);
                return Json(WorkDetilsSlip);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost("ActionOnWorkSlip")]
        public async Task<IActionResult> ActionOnWorkSlip([FromForm] WorkDetailsApprovalViewModel data)
        {
            if (data == null)
            {
                return BadRequest("Invalid data");
            }
            try
            {
                // Call the Interface method to insert the receipt data and get the output parameters
                var Response = await objWO.ActionOnWorkDetilsSlip(data);
                // Optionally, you can return the created budget ID and demand number
                return Ok(Response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("GetWorkDetailsOutbox")]
        public async Task<IActionResult> GetWorkDetailsOutbox(int UserID, int ZPID)
        {
            try
            {
                var WorkDetilsList = await objWO.Get_WorkDetilsOutbox(UserID, ZPID);
                return Json(WorkDetilsList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("GetWorkDetailsOutboxViewSlip")]
        public async Task<IActionResult> GetWorkDetailsOutboxViewSlip(int ZPID, int RecordId, int MasterID, int ApprovalBy, int ApprovalMasterID)
        {
            try
            {
                var WorkDetilsSlip = await objWO.WorkDetilsOutboxViewSlip(ZPID, RecordId, MasterID, ApprovalBy, ApprovalMasterID);
                return Json(WorkDetilsSlip);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("GetWrokDetailsByID")]
        public async Task<IActionResult> GetWrokDetailsByID(int ZPID, int HeadCodeID, int DeptID, int UserID, int AdminApprovalID, int DivID, int ULBID)
        {
            try
            {
                var WorkDetilsList = await objWO.GetWorkDetilsByID(ZPID, HeadCodeID, DeptID, UserID, AdminApprovalID, DivID, ULBID);
                return Json(WorkDetilsList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("CheckExistingWorkNoByID")]
        public async Task<IActionResult> CheckExistingWorkNoByID(string AANumber,string WorkOrderNumber)
        {
            bool result =await objWO.CheckExistingWorkNoByID(AANumber,WorkOrderNumber);
            return Ok(new { message=result});
        }
    }
}
