using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeemusAssetWebAPI.Models;
using System;
using System.Linq;

namespace NeemusAssetWebAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class RfidMappingController : ControllerBase
    {
        private readonly AssetSAPDBContext _context;

        public RfidMappingController(AssetSAPDBContext context)
        {
            _context = context;
        }

        //==========================================
        // Get All Assets
        //==========================================
        [HttpGet("GetAssetMaster")]
        public IActionResult GetAssetMaster()
        {
            try
            {
                var data = _context.AssetModels
                    .OrderByDescending(x => x.SLNO)
                    .ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //==========================================
        // RFID Mapping History
        //==========================================
        [HttpGet("RFIDMappingHistory")]
        public IActionResult GetRFIDMappingHistory()
        {
            try
            {
                var data = (from h in _context.RFIDMappingHistories
                            join a in _context.AssetModels
                                on h.SRNO equals a.SLNO
                            orderby h.RFIDMappinghistoryID descending
                            select new
                            {
                                a.SLNO,
                                a.AssetID,
                                a.MainAssetNumber,
                                a.SerialNumber,
                                a.AssetSubNumber,
                                a.AssetClass,
                                a.AssetDesc,
                                a.Assetstatus,
                                a.CustodianDepartment,
                                a.Location,
                                a.YearofPurchase,

                                h.RFIDCardNumber,
                                h.RFIDHistoryDate,
                                h.RFIDStatus
                            }).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        //==========================================
        // Mapped Assets
        //==========================================
        [HttpGet("GetMappedAssets")]
        public IActionResult GetMappedAssets()
        {
            try
            {
                var data = _context.AssetModels
                    .Where(x => !string.IsNullOrEmpty(x.RFIDCardNumber))
                    .OrderByDescending(x => x.SLNO)
                    .ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = ex.Message,
                    InnerException = ex.InnerException?.Message,
                    BaseException = ex.GetBaseException().Message
                });
            }
        }

        //==========================================
        // Map RFID
        //==========================================
        [HttpPost("MapRFID")]
        public IActionResult MapRFID([FromBody] MapRFIDRequest model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.RFIDCardNumber))
                    return BadRequest("RFID Card Number is required.");

                var currentAsset = _context.AssetModels
                    .FirstOrDefault(x => x.SLNO == model.SRNO);

                if (currentAsset == null)
                    return NotFound("Asset not found.");

                // Remove RFID from another asset
                var oldAsset = _context.AssetModels
                    .FirstOrDefault(x =>
                        x.RFIDCardNumber == model.RFIDCardNumber &&
                        x.SLNO != model.SRNO);

                if (oldAsset != null)
                {
                    oldAsset.RFIDCardNumber = null;
                    oldAsset.RFIDMAPDATE = null;
                }

                var previousHistory = _context.RFIDMappingHistories
                    .Where(x => x.SRNO == model.SRNO && x.RFIDStatus == "Active")
                    .ToList();

                foreach (var item in previousHistory)
                {
                    item.RFIDStatus = "Inactive";
                }

                var utcNow = DateTime.UtcNow;

                _context.RFIDMappingHistories.Add(new RfidMapping
                {
                    SRNO = model.SRNO,
                    RFIDCardNumber = model.RFIDCardNumber,
                    RFIDoldMAPDATE = ToUtc(currentAsset.RFIDMAPDATE), // <-- normalized

                    RFIDHistoryDate = utcNow.Date.ToString("yyyy-MM-dd"), // stored as string in your model
                    RFIDStatus = "Active"
                });

                currentAsset.RFIDCardNumber = model.RFIDCardNumber;
                currentAsset.RFIDMAPDATE = utcNow; // DateTime.UtcNow is already Kind=Utc

                _context.SaveChanges();

                return Ok(new { success = true, message = "RFID mapped successfully.", data = currentAsset });
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new { success = false, message = ex.Message, inner = ex.InnerException?.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // Helper: force any DateTime to Kind=Utc before EF/Npgsql tries to write it
        private static DateTime? ToUtc(DateTime? value)
        {
            if (value == null) return null;

            return value.Value.Kind switch
            {
                DateTimeKind.Utc => value,
                DateTimeKind.Local => value.Value.ToUniversalTime(),
                _ => DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) // Unspecified -> assume it was already UTC
            };
        }
        //==========================================
        // Request Model
        //==========================================
        public class MapRFIDRequest
        {
            public int SRNO { get; set; }

            public string RFIDCardNumber { get; set; } = string.Empty;
        }
    }
}
