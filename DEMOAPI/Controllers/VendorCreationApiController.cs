using IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.ViewModel;
using System.Collections.ObjectModel;
using System.Net;

namespace SBM2_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VendorCreationApiController : Controller
    {
        private readonly IVendorCreation objVC;
        public VendorCreationApiController(IVendorCreation _obvVC)
        {
            this.objVC = _obvVC;
        }
        [HttpGet("GetMainVendorType")]
        public async Task<IActionResult> GetMainVendorType()
        {
            List<MstVendorType> mstMainVendorTypes = new List<MstVendorType>();
            mstMainVendorTypes = await objVC.GetMainVendorType();
            return Ok(mstMainVendorTypes);
        }
        [HttpGet("GetVendorType")]
        public async Task<IActionResult> GetVendorType()
        {
            List<MstVendorType> lstVendorType = new List<MstVendorType>();
            lstVendorType = await objVC.GetVendorType();
            return Ok(lstVendorType);
        }
        [HttpGet("GetAllState")]
        public async Task<IActionResult> GetAllState()
        {
            List<MstState> lstState = new List<MstState>();
            lstState = await objVC.Get_AllState();
            return Ok(lstState);
        }
        [HttpGet("GetDistrictByStateID")]
        public async Task<IActionResult> GetDistrictByStateID(int StateId)
        {
            List<MstDistrict> lstDistrict = new List<MstDistrict>();
            lstDistrict = await objVC.GetDistrictbyStateID(StateId);
            return Ok(lstDistrict);
        }
        //[HttpGet("CheckPartyExistByPAN")]
        //public async Task<IActionResult> CheckPartyExistByPAN(string PanNo)
        //{
        //    try
        //    {
        //        HttpResponseMessage responseMessage = await objVC.CheckPartyExistByPAN(PanNo);
        //        if (responseMessage.StatusCode == HttpStatusCode.Conflict)
        //            return Ok(false);
        //        else if (responseMessage.StatusCode == HttpStatusCode.OK)
        //            return Ok(true);
        //        else
        //            return StatusCode((int)responseMessage.StatusCode);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        [HttpGet("CheckPartyExistByAAdhar")]
        public async Task<IActionResult> CheckPartyExistByAAdhar(string AAdharNo)
        {
            try
            {
                HttpResponseMessage responseMessage = await objVC.CheckPartyExistByAAdharNo(AAdharNo);
                if (responseMessage.StatusCode == HttpStatusCode.Conflict)
                    return Ok(false);
                else if (responseMessage.StatusCode == HttpStatusCode.OK)
                    return Ok(true);
                else
                    return StatusCode((int)responseMessage.StatusCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("CheckPartyExistByAccount")]
        public async Task<IActionResult> CheckPartyExistByAccount(string AccountNo, string PANNO)
        {
            try
            {
                HttpResponseMessage responseMessage = await objVC.CheckPartyExistByAccountNo(AccountNo, PANNO);
                if (responseMessage.StatusCode == HttpStatusCode.Conflict)
                    return Ok(false);
                else if (responseMessage.StatusCode == HttpStatusCode.OK)
                    return Ok(true);
                else
                    return StatusCode((int)responseMessage.StatusCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("GetBankDetailsByIFSCCode")]
        public async Task<IActionResult> GetBankDetailsByIFSCCode(string IFSCCode)
        {
            try
            {
                List<BankDetail> lstBankDetail = new List<BankDetail>();
                lstBankDetail = await objVC.GetBankDetailsByIFSCCode(IFSCCode);
                return Ok(lstBankDetail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost("SaveVendorDetails")]
        public async Task<IActionResult> SaveVendorDetails(MstPartyDummy data)
        {
            if (data == null)
            {
                return BadRequest("Invalid data");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }
            try
            {
                if (data.BankId == 0)
                {
                    BankDetail bankDetail = new BankDetail();
                    bankDetail.IfscCode = data.Ifsc;
                    bankDetail.MicrCode = data.Micrcode;
                    bankDetail.BankName = data.BankName;
                    bankDetail.BranchName = data.BankBranch;
                    bankDetail.BankAddress = "NA";
                    bankDetail.StdCode = 27;
                    bankDetail.BankContactNo = "123";
                    bankDetail.BankCity = "NA";
                    bankDetail.District = "NA";
                    bankDetail.State = "MAHARASHTRA";
                    bankDetail.CreatedBy = data.CreatedBy;
                    int BankID =await objVC.InsertNewBank(bankDetail);
                    data.BankId = BankID;
                }
                int PartyID = await objVC.SaveVendorDetails(data);
                if (PartyID > 0)
                {
                    return Ok(new { message = "Success", partyid= PartyID });
                }
                else
                {
                    return StatusCode(500, $"An error occurred while saving receipt data");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("VendorDeclarationSlip")]
        public async Task<IActionResult> VendorDeclarationSlip(int PartyId)
        {
            List<NewPartyViewModel> lst = new List<NewPartyViewModel>();
            lst =await objVC.VendorDeclarationSlip(PartyId);
            return Ok(lst);
        }

    }
}
